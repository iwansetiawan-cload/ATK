﻿var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "order": [[0, "desc"]],
        "ajax": {
            "url": "/Users/RequestItems/GetAll"
        },
        "columns": [
            { "data": "transaction_number", "autoWidth": true },
            { "data": "project_name", "autoWidth": true },
            { "data": "request_date", "autoWidth": true },
            { "data": "notes", "autoWidth": true },
            { "data": "status", "autoWidth": true },
            { "data": "id", "autoWidth": true },
            //{
            //    "data": "id",
            //    "render": function (data) {
            //        return `
            //                <div class="text-center">
            //                    <a href="/Users/RequestItems/Upsert/${data}" class="btn btn-primary btn-mini b-none" style="cursor:pointer" title="edit">
            //                        <i class="fa fa-pencil-square-o m-0"></i>
            //                    </a>
            //                    <a onclick=Delete("/Users/RequestItems/Delete/${data}") class="btn btn-danger btn-mini b-none text-white" style="cursor:pointer" title="delete">
            //                       <i class="fa fa-trash m-0"></i>
            //                    </a>
            //                    <a href="/Users/RequestItems/ViewApproval/${data}" class="btn btn-info btn-mini b-none text-white" style="cursor:pointer">
            //                        <i class="icofont icofont-eye m-0"></i>
            //                    </a>
            //                </div>
            //                `;
            //    }, "autoWidth": true
            //}
        ],
        "createdRow": function (row, data, index) {
            var href = "";
            if (data["status"] === 'Approve' || data["status"] === 'Rejected' || data["status"] === 'Complete' || data["status"] === 'Process') {
                href += '<a href="/Users/RequestItems/ViewApproval/ ' + data["id"] + '" class="btn btn-info btn-mini b-none text-white" style="cursor:pointer"><i class="icofont icofont-eye m-0"></i> </a>';
                $('td', row).eq(5).html(href);
            } else {
                href += '<a href="/Users/RequestItems/Upsert/' + data["id"] + '" class="btn btn-primary btn-mini b-none" style="cursor:pointer" title="edit"><i class="icofont icofont-ui-edit m-0"></i></a>&nbsp;';
                href += '<a onclick=Delete("/Users/RequestItems/Delete/' + data["id"] + '") class="btn btn-danger btn-mini b-none text-white" style="cursor:pointer" title="delete"><i class="icofont icofont-delete-alt m-0"></i></a>';
               
                $('td', row).eq(5).html(href);
            }

        }
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
function ValAdd() {
   
    $.ajax({
        type: "GET",
        url: "/Users/RequestItems/ValidationCreateNew",
        success: function (data) {
            if (data.success) {
                new PNotify({
                    title: 'Information ',
                    text: data.message,
                    icon: 'icofont icofont-info-square',
                    type: 'info'
                });
            }
            else {
               
                window.location.href = "/Users/RequestItems/Upsert";
            }
        }
    });
}