var dataTable;
$(document).ready(function () {
    loadDataTable();
});
/*var url = Url.Action("GetAll", "ReportRequestItems", new { Area = "Admin" }) ;*/
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "order": [[0, "desc"]],
        "ajax": {
            "url": "/Admin/ReportRequestItems/GetAll"
            
        },
        "columns": [
            { "data": "transaction_number", "autoWidth": true },
            { "data": "project_name", "autoWidth": true },
            { "data": "request_date", "autoWidth": true },
            { "data": "notes", "autoWidth": true },
            { "data": "status", "autoWidth": true }
        ]
    });
}
