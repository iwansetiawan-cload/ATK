var dataTable;

$(document).ready(function () {

    var url = window.location.search;
    if (url.includes("approval")) {
        loadDataTable("GetAll?status=approval");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("GetAll?status=completed");
        } 
        else if (url.includes("process")){
            loadDataTable("GetAll?status=process");
        }
        else {
            loadDataTable("GetAll?status=waitingApproval");
        }
    }   
   
});
/*var urls = "@Url.Action("GetAll", "ApprovelRequest", new { Area = "Admin" })" ;*/
function loadDataTable(url) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/RequestApproval/" + url
            
        },
        "columns": [
            { "data": "transaction_number", "autoWidth": true },
            { "data": "project_name", "autoWidth": true },
            { "data": "request_date", "autoWidth": true },
            { "data": "notes", "autoWidth": true },
            { "data": "status", "autoWidth": true },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                               
                                <a href="/Admin/RequestApproval/ProcessApproval/${data}" class="btn btn-info btn-mini b-none text-white" style="cursor:pointer">
                                    <i class="icofont icofont-eye m-0"></i>
                                </a>
                            </div>
                            `;
                }, "autoWidth": true
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Apakah Anda yakin ingin Menghapus?",
        text: "Anda tidak akan dapat memulihkan data!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Hapus",
        closeOnConfirm: false
    },
        function () {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        swal("Hapus!", data.message, "success");

                    }
                    else {
                        swal("Hapus!", data.message, "error");
                    }
                    dataTable.ajax.reload();
                }
            });
        });

}