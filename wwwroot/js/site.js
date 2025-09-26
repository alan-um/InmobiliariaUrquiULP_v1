// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//Filtrado de Propietarios
function filtrarPropietarios() {
    let HTML = "";
    const expFiltro = document.getElementById("expFiltro").value;
    const isAdmin = document.getElementById("isAdmin").classList.contains("isAdmin");


    fetch(`./Propietario/Filtro/?exp=${expFiltro}`)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            if (data.msgError != null) {
                toastNotifyError(data.msgError);
                HTML += `
                        <tr>
                            <td colspan="100" style="text-align:center">
                                <h5>No se encontraron coincidencias.</h5>
                            </td>
                        </tr>`;
            } else {
                if (data.length == 0) {
                    HTML += `
                        <tr>
                            <td colspan="100" style="text-align:center">
                                <h5>No se encontraron coincidencias.</h5>
                            </td>
                        </tr>`;
                } else {
                    data.forEach(item => {
                        HTML += `<tr>
                                <td>
                                    ${item.idPropietario}
                                </td>
                                <td>
                                    ${item.dni}
                                </td>
                                <td>
                                    ${item.nombre}
                                </td>
                                <td>
                                    ${item.apellido}
                                </td>
                                <td>
                                    ${item.telefono}
                                </td>
                                <td id="acciones">
                                    <div class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups" style="justify-content:center">
                                        <div class="btn-group" role="group" aria-label="First group">
                                            <a class="btn btn-outline-dark" title="Editar" href="/Propietario/FormAM/${item.idPropietario}">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16">
                                                    <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"></path>
                                                    <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"></path>
                                                </svg>
                                            </a>
                                            <a class="btn btn-outline-dark" title="Detalles" href="/Propietario/Detalles/${item.idPropietario}">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-journal-richtext" viewBox="0 0 16 16">
                                                    <path d="M7.5 3.75a.75.75 0 1 1-1.5 0 .75.75 0 0 1 1.5 0m-.861 1.542 1.33.886 1.854-1.855a.25.25 0 0 1 .289-.047L11 4.75V7a.5.5 0 0 1-.5.5h-5A.5.5 0 0 1 5 7v-.5s1.54-1.274 1.639-1.208M5 9.5a.5.5 0 0 1 .5-.5h5a.5.5 0 0 1 0 1h-5a.5.5 0 0 1-.5-.5m0 2a.5.5 0 0 1 .5-.5h2a.5.5 0 0 1 0 1h-2a.5.5 0 0 1-.5-.5"></path>
                                                    <path d="M3 0h10a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2v-1h1v1a1 1 0 0 0 1 1h10a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H3a1 1 0 0 0-1 1v1H1V2a2 2 0 0 1 2-2"></path>
                                                    <path d="M1 5v-.5a.5.5 0 0 1 1 0V5h.5a.5.5 0 0 1 0 1h-2a.5.5 0 0 1 0-1zm0 3v-.5a.5.5 0 0 1 1 0V8h.5a.5.5 0 0 1 0 1h-2a.5.5 0 0 1 0-1zm0 3v-.5a.5.5 0 0 1 1 0v.5h.5a.5.5 0 0 1 0 1h-2a.5.5 0 0 1 0-1z"></path>
                                                </svg>
                                            </a>`
                        if (isAdmin) {
                            HTML += `
                                            <a onclick="confirmaEliminar(${item.idPropietario}, 'Propietario')" class="btn btn-outline-danger" title="Eliminar">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                                                    <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"></path>
                                                    <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"></path>
                                                </svg>
                                            </a>`;
                        }
                        HTML += `
                                        </div>
                                    </div>
                                </td>
                            </tr>`;
                    });
                }
            }
            document.getElementById("tbody").innerHTML = HTML;
        });
}

//Filtrado de Inquilinos
function filtrarInquilinos() {
    let HTML = "";
    const expFiltro = document.getElementById("expFiltro").value;
    const isAdmin = document.getElementById("isAdmin").classList.contains("isAdmin");


    fetch(`./Inquilino/Filtro/?exp=${expFiltro}`)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            if (data.msgError != null) {
                toastNotifyError(data.msgError);
                HTML += `
                        <tr>
                            <td colspan="100" style="text-align:center">
                                <h5>No se encontraron coincidencias.</h5>
                            </td>
                        </tr>`;
            } else {
                if (data.length == 0) {
                    HTML += `
                        <tr>
                            <td colspan="100" style="text-align:center">
                                <h5>No se encontraron coincidencias.</h5>
                            </td>
                        </tr>`;
                } else {
                    data.forEach(item => {
                        HTML += `<tr>
                                <td>
                                    ${item.idInquilino}
                                </td>
                                <td>
                                    ${item.dni}
                                </td>
                                <td>
                                    ${item.nombre}
                                </td>
                                <td>
                                    ${item.apellido}
                                </td>
                                <td>
                                    ${item.telefono}
                                </td>
                                <td id="acciones">
                                    <div class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups" style="justify-content:center">
                                        <div class="btn-group" role="group" aria-label="First group">
                                            <a class="btn btn-outline-dark" title="Editar" href="/Inquilino/FormAM/${item.idInquilino}">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16">
                                                    <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"></path>
                                                    <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"></path>
                                                </svg>
                                            </a>
                                            <a class="btn btn-outline-dark" title="Detalles" href="/Inquilino/Detalles/${item.idInquilino}">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-journal-richtext" viewBox="0 0 16 16">
                                                    <path d="M7.5 3.75a.75.75 0 1 1-1.5 0 .75.75 0 0 1 1.5 0m-.861 1.542 1.33.886 1.854-1.855a.25.25 0 0 1 .289-.047L11 4.75V7a.5.5 0 0 1-.5.5h-5A.5.5 0 0 1 5 7v-.5s1.54-1.274 1.639-1.208M5 9.5a.5.5 0 0 1 .5-.5h5a.5.5 0 0 1 0 1h-5a.5.5 0 0 1-.5-.5m0 2a.5.5 0 0 1 .5-.5h2a.5.5 0 0 1 0 1h-2a.5.5 0 0 1-.5-.5"></path>
                                                    <path d="M3 0h10a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2v-1h1v1a1 1 0 0 0 1 1h10a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H3a1 1 0 0 0-1 1v1H1V2a2 2 0 0 1 2-2"></path>
                                                    <path d="M1 5v-.5a.5.5 0 0 1 1 0V5h.5a.5.5 0 0 1 0 1h-2a.5.5 0 0 1 0-1zm0 3v-.5a.5.5 0 0 1 1 0V8h.5a.5.5 0 0 1 0 1h-2a.5.5 0 0 1 0-1zm0 3v-.5a.5.5 0 0 1 1 0v.5h.5a.5.5 0 0 1 0 1h-2a.5.5 0 0 1 0-1z"></path>
                                                </svg>
                                            </a>`
                        if (isAdmin) {
                            HTML += `
                                            <a onclick="confirmaEliminar(${item.idInquilino}, 'Inquilino')" class="btn btn-outline-danger" title="Eliminar">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                                                    <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"></path>
                                                    <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"></path>
                                                </svg>
                                            </a>`;
                        }
                        HTML += `
                                        </div>
                                    </div>
                                </td>
                            </tr>`;
                    });
                }
            }
            document.getElementById("tbody").innerHTML = HTML;
        });
}

//Filtrado de inmuebles disponibles para alquilar entre unas fechas
function filtrarInmueblesPorFecha() {
    const Stdesde = document.getElementById("desde").value;
    const Sthasta = document.getElementById("hasta").value;
    if (Stdesde != "" && Sthasta != "") {
        desde = new Date(Stdesde);
        hasta = new Date(Sthasta);
        if (desde > hasta) {
            document.getElementById("errorFechas").innerHTML = "La fecha hasta debe ser posterior.";
        } else {
            const a = document.createElement('a');
            a.setAttribute("href", `/Alquilar/?desde=${Stdesde}&hasta=${Sthasta}`);
            document.getElementById("buscar").appendChild(a);
            a.click();
        }
    } else {
        document.getElementById("errorFechas").innerHTML = "Debe ingresar fechas desde y hasta.";
    }
}

//
function confirmaEliminarTipoInmueble(id) {
    //console.log("Modal cargado");
    fetch(`/TipoInmueble/Id/${id}`)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            if (data.msgError != null) {
                toastNotifyError(data.msgError);
            } else {
                document.getElementById("modal-title").innerText = "¿Desea eliminar?";
                document.getElementById("modal-body").innerHTML = `
                    <p>Esta por eliminar el tipo de inmueble con la siguiente descripción:</p>
                    <h5 style="text-align: center;">\"${data.descripcion}\"</h5>
                    <p style="margin-right: 25px; text-align: right;">¿Desea continuar?</p>`;
                document.getElementById("modal-footer").innerHTML = `
                        <form action="TipoInmueble/Eliminar" method="post" novalidate="true">
                            <input type="hidden" name="id" value="${data.idTipoInmueble}" />
                            <input type="submit" value="Eliminar" class="btn btn-danger" />
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        </form>`;
                const openModal = document.createElement('a');
                openModal.setAttribute("data-bs-toggle", "modal");
                openModal.setAttribute("data-bs-target", "#modal");
                document.getElementById("acciones").appendChild(openModal);
                openModal.click();
            }
        });
}

//
function confirmaEliminarInmueble(id) {
    fetch(`/Inmueble/Id/${id}`)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            if (data.msgError != null) {
                toastNotifyError(data.msgError);
            } else {
                document.getElementById("modal-title").innerText = "¿Desea eliminar?";
                document.getElementById("modal-body").innerHTML = `
                    <p>Esta por eliminar el inmueble con la siguiente descripción:</p>
                    <h5 style="text-align: center;">\"${data.nombre} - ${data.direccion}\"</h5>
                    <p style="margin-right: 25px; text-align: right;">¿Desea continuar?</p>`;
                document.getElementById("modal-footer").innerHTML = `
                        <form action="Inmueble/Eliminar" method="post" novalidate="true">
                            <input type="hidden" name="id" value="${data.idInmueble}" />
                            <input type = "submit" value = "Eliminar" class="btn btn-danger" />
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        </form > `;
                const openModal = document.createElement('a');
                openModal.setAttribute("data-bs-toggle", "modal");
                openModal.setAttribute("data-bs-target", "#modal");
                document.getElementById("acciones").appendChild(openModal);
                openModal.click();
            }
        });
}

function confirmaEliminarContrato(id) {
    fetch(`/Contrato/Id/${id}`)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            if (data.msgError != null) {
                toastNotifyError(data.msgError);
            } else {
                document.getElementById("modal-title").innerText = "¿Desea eliminar?";
                document.getElementById("modal-body").innerHTML = `
                    <p>Esta por eliminar el contrato de ${data.inquilino.nombre} ${data.inquilino.apellido} sobre el inmueble:</p>
                    <h5 style="text-align: center;">\"${data.inmueble.nombre}\"</h5>
                    <p style="margin-right: 25px; text-align: right;">¿Desea continuar?</p>`;
                document.getElementById("modal-footer").innerHTML = `
                        <form action="Contrato/Eliminar" method="post" novalidate="true">
                            <input type="hidden" name="id" value="${data.idContrato}" />
                            <input type = "submit" value = "Eliminar" class="btn btn-danger" />
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        </form > `;
                const openModal = document.createElement('a');
                openModal.setAttribute("data-bs-toggle", "modal");
                openModal.setAttribute("data-bs-target", "#modal");
                document.getElementById("acciones").appendChild(openModal);
                openModal.click();
            }
        });
}

//----Cargar y resetear el avatar-------
const inputAvatar = document.getElementById('inputAvatar');
const inputAvatarFile = document.getElementById('inputAvatarFile');
const imgAvatar = document.getElementById('imgAvatar');
function cargarAvatar() {
    inputAvatarFile.click();
}
function borrarAvatar() {
    inputAvatar.value = "";
    inputAvatarFile.value = "";
    imgAvatar.src = "/Uploads/noAvatar.png";
}
inputAvatarFile.addEventListener('change', function (event) {
    const file = event.target.files[0]; // Obtiene el primer archivo seleccionado
    if (file) {
        const reader = new FileReader(); // Permite leer el contenido del archivo
        reader.onload = function (e) {
            imgAvatar.src = e.target.result; // Muestra la imagen (previsualización)
        };
        reader.readAsDataURL(file); // Lee el archivo como una URL de datos
    }
});