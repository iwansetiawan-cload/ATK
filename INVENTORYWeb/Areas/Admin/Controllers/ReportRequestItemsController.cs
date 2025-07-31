using INVENTORYWeb.DataAccess.Repository.IRepository;
using INVENTORYWeb.Models;
using INVENTORYWeb.Models.ViewModels;
using INVENTORYWeb.Utility;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using NPOI.Util;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.UserModel.Extensions;
using System.Data;
using NPOI.SS.Formula.Functions;
using System.Reflection.PortableExecutable;

namespace INVENTORYWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = OI.Role_SuperAdmin + "," + OI.Role_Admin)]
    public class ReportRequestItemsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReportRequestItemsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public static ReportRequestItemsViewModel getSearchModel = new ReportRequestItemsViewModel();
        public IActionResult Index(ReportRequestItemsViewModel vm)
        {           
            ViewBag.listStatus = _unitOfWork.MSUDC.GetAll().OrderBy(x=>x.MNUM1).Where(z => z.ENTRY_KEY == "status").Select(x => new SelectListItem { Value = x.TEXT1, Text = x.TEXT1 });           
            vm.listData = _unitOfWork.RequestItemHeader.GetAll();
            if(!string.IsNullOrEmpty(vm.request_number))
                vm.listData = vm.listData.Where(z=> vm.request_number.Contains(z.REQUEST_ORDER_NO ?? string.Empty)).ToList();
            if (!string.IsNullOrEmpty(vm.project_name))
                vm.listData = vm.listData.Where(z => vm.project_name.Contains(z.PROJECT_NAME)).ToList();
            if (!string.IsNullOrEmpty(vm.status))
                vm.listData = vm.listData.Where(z => z.STATUS == vm.status).ToList();
            if (vm.year != null)
                vm.listData = vm.listData.Where(z => z.REQUEST_DATE.Value.Year == vm.year).ToList();
            if (vm.month != null)
                vm.listData = vm.listData.Where(z => z.REQUEST_DATE.Value.Month == vm.month).ToList();

            getSearchModel.request_number = vm.request_number;
            getSearchModel.project_name = vm.project_name;
            getSearchModel.status = vm.status;
            getSearchModel.year = vm.year;
            getSearchModel.month = vm.month;

            return View(vm);
        }
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            var allObj = (from z in _unitOfWork.RequestItemHeader.GetAll()
                          select new
                          {
                              id = z.ID,
                              transaction_number = z.REQUEST_ORDER_NO,
                              project_name = z.PROJECT_NAME,
                              request_date = z.REQUEST_DATE.HasValue ? z.REQUEST_DATE.Value.ToString("yyyy-MM-dd") : "",
                              notes = z.NOTES,
                              status = z.STATUS,
                              status_id = z.STATUS_ID
                          });
          
            return Json(new { data = allObj });
        }
        public FileResult ExportData()
        {
            
           
            List<REQUEST_ITEM_DETAIL> getRequestDetailList = new List<REQUEST_ITEM_DETAIL>();
            getRequestDetailList = _unitOfWork.RequestItemDetail.GetAll(includeProperties: "REQUEST_ITEM_HEADER").ToList();
            if (!string.IsNullOrEmpty(getSearchModel.request_number))
                getRequestDetailList = getRequestDetailList.Where(z => getSearchModel.request_number.Contains(z.REQUEST_ITEM_HEADER.REQUEST_ORDER_NO ?? string.Empty)).ToList();
            if (!string.IsNullOrEmpty(getSearchModel.project_name))
                getRequestDetailList = getRequestDetailList.Where(z => getSearchModel.project_name.Contains(z.REQUEST_ITEM_HEADER.PROJECT_NAME)).ToList();
            if (!string.IsNullOrEmpty(getSearchModel.status))
                getRequestDetailList = getRequestDetailList.Where(z => z.STATUS == getSearchModel.status).ToList();
            else
                getRequestDetailList = getRequestDetailList.Where(z => z.STATUS_ID != -1 && z.STATUS_ID != 2).ToList();
            if (getSearchModel.year != null)
                getRequestDetailList = getRequestDetailList.Where(z => z.REQUEST_ITEM_HEADER.REQUEST_DATE.Value.Year == getSearchModel.year).ToList();
            if (getSearchModel.month != null)
                getRequestDetailList = getRequestDetailList.Where(z => z.REQUEST_ITEM_HEADER.REQUEST_DATE.Value.Month == getSearchModel.month).ToList();

            int Number = 1;
            var datalist = (from z in getRequestDetailList
                            select new ExportReportPR
                            {
                                Number = Number++,
                                Id = z.ID,
                                RequestNumber = z.REQUEST_ITEM_HEADER.REQUEST_ORDER_NO,
                                ProjectName = z.REQUEST_ITEM_HEADER.PROJECT_NAME,
                                RequestDate = z.REQUEST_ITEM_HEADER.REQUEST_DATE.HasValue ? z.REQUEST_ITEM_HEADER.REQUEST_DATE.Value.ToString("yyyy-MM-dd") : "",
                                ItemName = z.ITEM_NAME,
                                Catagory = z.CATEGORY,
                                Piece = z.PIECE.ToString() ?? string.Empty,
                                Qty = z.QTY.ToString() ?? string.Empty,
                                Status = z.STATUS
                            }).ToList();

            var file = CreateFile(datalist);
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export_Data_PR" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx");


        }
        public byte[] CreateFile<T>(List<T> source)
        {

            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("Sheet1");
            var rowHeader = sheet.CreateRow(0);

            var properties = typeof(T).GetProperties();

            //header
            var font = workbook.CreateFont();
            font.IsBold = true;
            var style = workbook.CreateCellStyle();
            style.SetFont(font);

            var cell = rowHeader.CreateCell(0);
            cell.SetCellValue("No");
            cell.CellStyle = style;
            //cell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;      

            cell = rowHeader.CreateCell(1);
            cell.SetCellValue("No Transaksi");
            cell.CellStyle = style;

            cell = rowHeader.CreateCell(2);
            cell.SetCellValue("Nama Proyek");
            cell.CellStyle = style;

            cell = rowHeader.CreateCell(3);
            cell.SetCellValue("Tanggal Permintaan");
            cell.CellStyle = style;

            cell = rowHeader.CreateCell(4);
            cell.SetCellValue("Nama Barang");
            cell.CellStyle = style;

            cell = rowHeader.CreateCell(5);
            cell.SetCellValue("Satuan");
            cell.CellStyle = style;

            cell = rowHeader.CreateCell(6);
            cell.SetCellValue("Isi");
            cell.CellStyle = style;

            cell = rowHeader.CreateCell(7);
            cell.SetCellValue("Jumlah");
            cell.CellStyle = style;

            cell = rowHeader.CreateCell(8);
            cell.SetCellValue("Status");
            cell.CellStyle = style;

            var rowNum = 1;
            foreach (var item in source)
            {
                var rowContent = sheet.CreateRow(rowNum);
                var colContentIndex = 0;
                foreach (var property in properties)
                {

                    if (property.Name == "Id")
                    {
                        continue;
                    }

                    var cellContent = rowContent.CreateCell(colContentIndex);
                    var value = property.GetValue(item, null);

                    if (value == null)
                    {
                        cellContent.SetCellValue("");
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        cellContent.SetCellValue(value.ToString());
                    }
                    else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                    {
                        cellContent.SetCellValue(Convert.ToInt32(value));
                    }
                    else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                    {
                        cellContent.SetCellValue(Convert.ToDouble(value));
                    }
                    else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                    {
                        var dateValue = (DateTime)value;
                        cellContent.SetCellValue(dateValue.ToString("yyyy-MM-dd"));
                    }
                    else cellContent.SetCellValue(value.ToString());

                    sheet.AutoSizeColumn(colContentIndex);

                    colContentIndex++;
                }               
                rowNum++;
            }

            var stream = new MemoryStream();
            workbook.Write(stream);
            var content = stream.ToArray();

            return content;
        }

        #region Export Data PDF
        public FileResult ExportPDF()
        {
            //if (string.IsNullOrEmpty(getSearchModel.request_number) &&
            //    string.IsNullOrEmpty(getSearchModel.project_name) &&
            //    string.IsNullOrEmpty(getSearchModel.status) &&
            //    getSearchModel.year == null && getSearchModel.month == null)
            //{
            //    TempData["Information"] = "Pilih pencarian data terlebih dahulu..! ";
            //    RedirectToAction("Index");
            //}
            List<REQUEST_ITEM_DETAIL> getRequestDetailList = new List<REQUEST_ITEM_DETAIL>();
            getRequestDetailList = _unitOfWork.RequestItemDetail.GetAll(includeProperties: "REQUEST_ITEM_HEADER").ToList();
            if (!string.IsNullOrEmpty(getSearchModel.request_number))
                getRequestDetailList = getRequestDetailList.Where(z => getSearchModel.request_number.Contains(z.REQUEST_ITEM_HEADER.REQUEST_ORDER_NO ?? string.Empty)).ToList();
            if (!string.IsNullOrEmpty(getSearchModel.project_name))
                getRequestDetailList = getRequestDetailList.Where(z => getSearchModel.project_name.Contains(z.REQUEST_ITEM_HEADER.PROJECT_NAME)).ToList();
            if (!string.IsNullOrEmpty(getSearchModel.status))
                getRequestDetailList = getRequestDetailList.Where(z => z.STATUS == getSearchModel.status).ToList();
            else
                getRequestDetailList = getRequestDetailList.Where(z => z.STATUS_ID != -1 && z.STATUS_ID != 2).ToList();
            if (getSearchModel.year != null)
                getRequestDetailList = getRequestDetailList.Where(z => z.REQUEST_ITEM_HEADER.REQUEST_DATE.Value.Year == getSearchModel.year).ToList();
            if (getSearchModel.month != null)
                getRequestDetailList = getRequestDetailList.Where(z => z.REQUEST_ITEM_HEADER.REQUEST_DATE.Value.Month == getSearchModel.month).ToList();

            int Number = 1;
            var datalist = (from z in getRequestDetailList
                            select new 
                            {
                                Number = Number++,                             
                                RequestNumber = z.REQUEST_ITEM_HEADER.REQUEST_ORDER_NO,
                                ProjectName = z.REQUEST_ITEM_HEADER.PROJECT_NAME,
                                RequestDate = z.REQUEST_ITEM_HEADER.REQUEST_DATE.HasValue ? z.REQUEST_ITEM_HEADER.REQUEST_DATE.Value.ToString("yyyy-MM-dd") : "",
                                ItemName = z.ITEM_NAME,
                                Catagory = z.CATEGORY,
                                Piece = z.PIECE.ToString() ?? string.Empty,
                                Qty = z.QTY_ADJUST != null ? z.QTY_ADJUST.ToString() : z.QTY.ToString(),
                                Status = z.STATUS
                            }).ToList();

            var datalist_distinct = (from z in datalist
                            select new
                            {
                                itemName = z.ItemName,
                                qty = datalist.Where(x=> x.ItemName == z.ItemName && x.Status == z.Status).Sum(i=> Convert.ToInt32(i.Qty)).ToString() ?? string.Empty,
                                status = z.Status
                            }).Distinct().ToList();

            DataTable dttbl = CreateDataTable(datalist);
            DataTable dttbl_distinct = CreateDataTable(datalist_distinct);
            string physicalPath = "wwwroot\\files\\DaftarPermintaanBarang.pdf";
            ExportDataTableToPdf(dttbl, physicalPath, dttbl_distinct, "Daftar Permintaan Barang");


            byte[] pdfBytes = System.IO.File.ReadAllBytes(physicalPath);
            MemoryStream ms = new MemoryStream(pdfBytes);
            return new FileStreamResult(ms, "application/pdf");

        }
        public static DataTable CreateDataTable<T>(IEnumerable<T> entities)
        {
            var dt = new DataTable();

            //creating columns
            foreach (var prop in typeof(T).GetProperties())
            {
                dt.Columns.Add(prop.Name, prop.PropertyType);
            }

            //creating rows
            foreach (var entity in entities)
            {
                var values = GetObjectValues(entity);
                dt.Rows.Add(values);
            }


            return dt;
        }
        public static object[] GetObjectValues<T>(T entity)
        {
            var values = new List<object>();
            foreach (var prop in typeof(T).GetProperties())
            {
                values.Add(prop.GetValue(entity));
            }

            return values.ToArray();
        }

        void ExportDataTableToPdf(DataTable dtblTable, String strPdfPath, DataTable dtblTable_distict, string strHeader)
        {

            System.IO.FileStream fs = new FileStream(strPdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            //Report Header
            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fntHead = new iTextSharp.text.Font(bfntHead, 14, 1, BaseColor.DarkGray);
            Paragraph prgHeading = new Paragraph();
            prgHeading.Alignment = Element.ALIGN_CENTER;
            prgHeading.Add(new Chunk(strHeader.ToUpper(), fntHead));
            document.Add(prgHeading);

            //Add line break
            document.Add(new Chunk("\n", fntHead));

            //Write the table         
            PdfPTable table = new PdfPTable(9);
            table.HorizontalAlignment = 0;
            table.TotalWidth = 520f;
            table.LockedWidth = true;
            float[] widths = new float[] { 20f, 55f, 55f, 35f, 60f, 30f, 20f, 20f, 40f };
            table.SetWidths(widths);
            //Table header
            BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fntColumnHeader = new iTextSharp.text.Font(btnColumnHeader, 8, 1, BaseColor.White);

            addCell(table, "No", 1);
            addCell(table, "No Transaksi", 1);
            addCell(table, "Nama Proyek", 1);
            addCell(table, "Tanggal Permintaan", 1);
            addCell(table, "Nama Barang", 1);
            addCell(table, "Satuan", 1);
            addCell(table, "Isi", 1);
            addCell(table, "Jumlah", 1);
            addCell(table, "Status", 1);


            //table Data
            for (int i = 0; i < dtblTable.Rows.Count; i++)
            {
                for (int j = 0; j < dtblTable.Columns.Count; j++)
                {
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.Black);

                    PdfPCell cell = new PdfPCell(new Phrase(dtblTable.Rows[i][j].ToString(), times));
                    cell.Rowspan = 1;
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    table.AddCell(cell);
                }
            }
            document.Add(table);
            document.Add(new Chunk("\n", fntHead));
           

            PdfPTable table_total = new PdfPTable(3);
            table_total.HorizontalAlignment = 0;
            table_total.TotalWidth = 250f;
            table_total.LockedWidth = true;
            float[] widths_total = new float[] { 50f, 20f, 30f };
            table_total.SetWidths(widths_total);
            //Table header
            BaseFont btnColumnHeader_total = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fntColumnHeader_total = new iTextSharp.text.Font(btnColumnHeader, 8, 1, BaseColor.White);

            addCell(table_total, "Nama Barang", 1);
            addCell(table_total, "Total", 1);
            addCell(table_total, "Status", 1);

            for (int i = 0; i < dtblTable_distict.Rows.Count; i++)
            {
                for (int j = 0; j < dtblTable_distict.Columns.Count; j++)
                {
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.Black);

                    PdfPCell cell = new PdfPCell(new Phrase(dtblTable_distict.Rows[i][j].ToString(), times));
                    cell.Rowspan = 1;
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    table_total.AddCell(cell);
                }
            }

            document.Add(table_total);

            document.Close();
            writer.Close();
            fs.Close();

        }

        private static void addCell(PdfPTable table, string text, int rowspan)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.Black);

            PdfPCell cell = new PdfPCell(new Phrase(text, times));
            cell.BackgroundColor = BaseColor.White;
            cell.Rowspan = rowspan;
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            table.AddCell(cell);
        }
        #endregion
        //public byte[] CreateFile<T>(List<T> source)
        //{

        //    var workbook = new XSSFWorkbook();         
        //    var sheet = workbook.CreateSheet("Sheet1");
        //    var rowHeader = sheet.CreateRow(0);

        //    var properties = typeof(T).GetProperties();

        //    //header
        //    var font = workbook.CreateFont();
        //    font.IsBold = true;
        //    var style = workbook.CreateCellStyle();
        //    style.SetFont(font);

        //    var cell = rowHeader.CreateCell(0);
        //    cell.SetCellValue("No");
        //    cell.CellStyle = style;        
        //    //cell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;      

        //    cell = rowHeader.CreateCell(1);
        //    cell.SetCellValue("No Transaksi");
        //    cell.CellStyle = style;            

        //    cell = rowHeader.CreateCell(2);
        //    cell.SetCellValue("Nama Proyek");
        //    cell.CellStyle = style;

        //    cell = rowHeader.CreateCell(3);
        //    cell.SetCellValue("Tanggal Permintaan");
        //    cell.CellStyle = style;

        //    cell = rowHeader.CreateCell(4);
        //    cell.SetCellValue("Keterangan");
        //    cell.CellStyle = style;

        //    cell = rowHeader.CreateCell(5);
        //    cell.SetCellValue("Status");
        //    cell.CellStyle = style;


        //    var rowNum = 1;
        //    foreach (var item in source)
        //    {
        //        var rowContent = sheet.CreateRow(rowNum);
        //        var colContentIndex = 0;
        //        IList<REQUEST_ITEM_DETAIL> rEQUEST_ITEM_DETAILs = new List<REQUEST_ITEM_DETAIL>();

        //        foreach (var property in properties)
        //        {

        //            if (property.Name == "Id")
        //            {
        //                var idHeader = property.GetValue(item, null);
        //                if (idHeader != null)
        //                {
        //                    rEQUEST_ITEM_DETAILs = getRequestDetailList((long)idHeader);                            
        //                }

        //                continue;
        //            }


        //            var cellContent = rowContent.CreateCell(colContentIndex);
        //            var value = property.GetValue(item, null);

        //            if (value == null)
        //            {
        //                cellContent.SetCellValue("");
        //            }
        //            else if (property.PropertyType == typeof(string))
        //            {
        //                cellContent.SetCellValue(value.ToString());
        //            }
        //            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
        //            {
        //                cellContent.SetCellValue(Convert.ToInt32(value));
        //            }
        //            else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
        //            {
        //                cellContent.SetCellValue(Convert.ToDouble(value));
        //            }
        //            else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
        //            {
        //                var dateValue = (DateTime)value;
        //                cellContent.SetCellValue(dateValue.ToString("yyyy-MM-dd"));
        //            }
        //            else cellContent.SetCellValue(value.ToString());



        //            colContentIndex++;
        //        }
        //        int rowNumDetail = rowNum + 1;

        //        var rowDetail = sheet.CreateRow(rowNumDetail);
        //        cell = rowDetail.CreateCell(0);
        //        cell.SetCellValue("No");
        //        cell.CellStyle = style;

        //        cell = rowDetail.CreateCell(1);
        //        cell.SetCellValue("Nama Barang");
        //        cell.CellStyle = style;

        //        int numberDetail = 1;

        //        foreach (var detail in rEQUEST_ITEM_DETAILs)
        //        {
        //            var rowDetail_ = sheet.CreateRow(rowNumDetail + 1);
        //            var colContentIndexDetail = 0;

        //            var cellContentNo = rowDetail_.CreateCell(colContentIndexDetail);
        //            cellContentNo.SetCellValue(numberDetail);

        //            var cellContentDetail = rowDetail_.CreateCell(colContentIndexDetail + 1);
        //            cellContentDetail.SetCellValue(detail.ITEM_NAME.ToString());

        //            rowNumDetail++;
        //            numberDetail++;
        //            //colContentIndexDetail++;
        //        }
        //        rowNum++;
        //    }


        //    for (int i = 0; i < 5; i++)
        //    {
        //        sheet.AutoSizeColumn(i);
        //    }
        //    //end content


        //    var stream = new MemoryStream();
        //    workbook.Write(stream);
        //    var content = stream.ToArray();

        //    return content;
        //}

        public IList<REQUEST_ITEM_DETAIL> getRequestDetailList(long idHeader) {


            return _unitOfWork.RequestItemDetail.GetAll().Where(z => z.HEADER_ID == idHeader).ToList();
        }
    }
}
