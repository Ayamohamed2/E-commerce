$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#myTable').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "10%" },
            { "data": "streetAddress", "width": "17%" },
            { "data": "city", "width": "10%" },
            { "data": "state", "width": "10%" },
            { "data": "postalCode", "width": "15%" },
            { "data": "phoneNumber", "width": "17%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                       <a href="/Admin/Company/Edit/${data}" class="btn btn-dark mx-2">
                           <i class="bi bi-pencil-square"></i> Edit
                            </a>
                           <a onClick="Delete('/Admin/Company/Delete/${data}')" class="btn btn-danger mx-2">
                           <i class="bi bi-trash"></i> Delete
                              </a>
                             </div>`;
                },
                "width": "25%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        $('#myTable').DataTable().ajax.reload();

                        Swal.fire({
                            title: "Deleted!",
                            text: data.message,
                            icon: "success"
                        });
                    } else {
                        Swal.fire({
                            title: "Error!",
                            text: data.message,
                            icon: "error"
                        });
                    }
                }
            });
        }
    });
}
