$(document).ready(function () {
    console.log('pasa por aqui 0');
    ListaTarea();
    ListaUsuario();
});

function ListaUsuario() {
    $.ajax({
        url: $.MisUrls.url.getListaUsuarioURL,
        type: "GET",
        dataType: "json",
        success: function (data) {
            $("#cboUsuario").html("");
            if (data.data) {
                $.each(data.data, function (i, item) {
                    $("<option>").val(item.IdUsuario).text(item.Nombre).appendTo("#cboUsuario");
                });
            }
        }
    });
}

function ListaTarea() {
    console.log('pasa por aqui');
    jQuery.ajax({
        url: $.MisUrls.url.getListaTareaURL,
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(".card-load").LoadingOverlay("hide");
            const tableBody = document.querySelector('#tbTarea tbody');
            tableBody.innerHTML = '';
            if (data && data.data) {
                console.log(data.data);
                if (data.data.length === 0) {
                    const tr = document.createElement('tr');
                    tr.innerHTML = `<td colspan="5" class="text-center">Ningún dato disponible en esta tabla</td>`;
                    tableBody.appendChild(tr);
                    return;
                }
                data.data.forEach(row => {
                    const btnEditar = `
                    <button class='btn-sm btn-primary' onclick='abrirPopUpFormTarea(${JSON.stringify(row)})'>
                        <i class='bi bi-pencil-square'></i>
                    </button>`;
                    const btnCambiarEstado = `
                        <button class='btn-sm btn-${row.Color}' onclick='cambiarEstado(${JSON.stringify(row)})'>
                            <i class='bi bi-${row.Icono}-fill'></i>
                        </button>`;
                    const btnEliminar = `
                    <button class='btn-sm btn-warning' onclick='EliminarTarea(${JSON.stringify(row)})'>
                        <i class="bi bi-trash-fill"></i>
                    </button>`;
                    const botonesHtml = btnEditar + " " + btnCambiarEstado + " " + btnEliminar;
                    const estadoHtml = row.Estado ? "<span class='badge badge-success'>Activo</span>" : "<span class='badge badge-danger'>Inactivo</span>";
                    const tr = document.createElement('tr');
                    tr.innerHTML = `
                        <td>${row.Nombre}</td>
                        <td>${row.Descripcion}</td>
                        <td>${row._Usuario.Nombre}</td>
                        <td>${estadoHtml}</td>
                        <td>${botonesHtml}</td>`;
                    tableBody.appendChild(tr);
                });
            }
        },
        error: function (error) {
        },
        beforeSend: function () {
            $(".card-load").LoadingOverlay("show");
        },
    });
}


function abrirPopUpFormTarea(json) {
    if (json) {
        $("#txtCodigo").val(json.IdTarea);
        $("#txtNombre").val(json.Nombre);
        $("#txtNombre").prop("readonly", false);
        $("#txtDescripcion").val(json.Descripcion);
        $("#txtDescripcion").prop("readonly", false);
        $("#cboUsuario").val(json.IdUsuario);
    }
    else {
        $("#txtCodigo").val(0);
        $("#cboUsuario").val(1);
        $("#cboUsuario").prop("disabled", false);
        $("#txtNombre").val('');
        $("#txtNombre").prop("readonly", false);
        $("#txtDescripcion").val('');
        $("#txtDescripcion").prop("readonly", false);
    }
    $('#cboRol').trigger('change');
    $('#FormModal').modal('show');
}

function GuardarTarea() {
    var request = {
        tarea: {
            IdUsuario: $("#cboUsuario").val(),
            Nombre: $("#txtNombre").val(),
            Descripcion: $("#txtDescripcion").val()
        }
    }
    jQuery.ajax({
        url: $.MisUrls.url.postGuardarTareaURL,
        type: "POST",
        data: JSON.stringify(request),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.data.Respuesta === true) {
                ListaTarea();
                $('#FormModal').modal('hide');
                Swal.fire({
                    title: data.data.Mensaje,
                    icon: "success"
                });
            } else {
                Swal.fire({
                    title: data.data.Mensaje,
                    icon: "warning"
                });
            }
        },
        error: function (error) {
            Swal.fire({
                title: "Error en la comunicación con el servidor",
                icon: "error"
            });
        }
    });
}

function cambiarEstado(json) {
    var request = {
        tarea: {
            IdTarea: json.IdTarea,
            IdUsuario: json.IdUsuario,
            Nombre: json.Nombre,
            Descripcion: json.Descripcion,
            Estado: !json.Estado
        }
    }
    jQuery.ajax({
        url: $.MisUrls.url.postGuardarTareaURL,
        type: "POST",
        data: JSON.stringify(request),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.data.Respuesta === true) {
                ListaTarea();
            } else {
                Swal.fire({
                    title: data.data.Mensaje,
                    icon: "warning"
                });
            }
        },
        error: function (error) {
            Swal.fire({
                title: "Error en la comunicación con el servidor",
                icon: "error"
            });
        }
    });
}

function EliminarTarea(json) {
    var request = {
        tarea: {
            IdTarea: json.IdTarea
        }
    }
    jQuery.ajax({
        url: $.MisUrls.url.postEliminarTareaURL,
        type: "POST",
        data: JSON.stringify(request),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.data.Respuesta === true) {
                ListaTarea();
            } else {
                Swal.fire({
                    title: data.data.Mensaje,
                    icon: "warning"
                });
            }
        },
        error: function (error) {
            Swal.fire({
                title: "Error en la comunicación con el servidor",
                icon: "error"
            });
        }
    });
}