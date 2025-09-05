$(document).ready(function () {

    var url = window.location.search;

    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else if (url.includes("pending")) {
        loadDataTable("pending");
    }
    else if (url.includes("completed")) {
        loadDataTable("completed");
    }
    else if (url.includes("pending")) {
        loadDataTable("pending");
    }
    else if (url.includes("approved")) {
        loadDataTable("approved");
    } else {
        loadDataTable("all");
    }


});

function loadDataTable(status) {
    $('#myTable').DataTable({
        "ajax": {
            "url": "/Admin/order/GetAll?status=" + status
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "name", "width": "10%" },
            { "data": "phoneNumber", "width": "20%" },
            { "data": "applicationUser.email", "width": "15%" },
            { "data": "orderStatus", "width": "10%" },
            { "data": "orderTotal", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group text-center" role="group">
                       <a href="/Admin/order/details/${data}" class="btn btn-dark mx-1 ">
                           <i class="bi bi-pencil-square"></i>
                            </a>
                           
                             </div>`;
                },
                "width": "20%"
            }
        ]
    });
}
