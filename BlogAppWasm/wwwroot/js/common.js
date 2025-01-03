window.ShowToastr = (type, message) => {
    if (type === "success") {
        toastr.success(message, "Operation Success", { timeOut: 10000});
    }
    if (type === "error") {
        toastr.error(message, "Operation Failed", { timeOut: 10000 });
    }
}

window.ShowSwal = (type, message) => {
    if (type === "success") {
        Swal.fire(
            'Success Notification',
            message,
            'success'
        );
    }
    if (type === "error") {
        Swal.fire(
            'Error Notification',
            message,
            'error'
        );
    }
}

function ShowModal() {
    $('#showModal').modal('show');
}

function HideModal() {
    $('#showModal').modal('hide');
}
