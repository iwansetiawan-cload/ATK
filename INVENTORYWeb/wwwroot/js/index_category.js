var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Category/GetAll"
        },
        "columns": [
            { "data": "name", "autoWidth": true },
            { "data": "description", "autoWidth": true },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/Category/Upsert/${data}" class="btn btn-primary btn-mini b-none" style="cursor:pointer" title="edit">
                                    <i class="fa fa-pencil-square-o m-0"></i>
                                </a>
                                <a onclick=Delete("/Admin/Category/Delete/${data}") class="btn btn-danger btn-mini b-none text-white" style="cursor:pointer" title="delete">
                                   <i class="fa fa-trash m-0"></i>
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