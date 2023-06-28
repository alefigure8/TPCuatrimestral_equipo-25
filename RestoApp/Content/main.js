const currentPagePath = document.location.pathname.toLowerCase();

//****** FUNCIONES *******/

//Función para pasar numero de mesero a color
function convertirAHexadecimal(numero) {
    let r = (numero * 4321) % 256; // rojo
    let g = (numero * 1234) % 256; // verde
    let b = (numero * 9876) % 256; // azul
    let opacity = 0.9; // Valor de opacidad deseado (por ejemplo, 0.5 para 50%)

    let colorHexadecimal = '#' + toDosDigitosHex(r) + toDosDigitosHex(g) + toDosDigitosHex(b);

    return colorHexadecimal + toOpacityHex(opacity);
}

//Armamos digitos hexadecimal
function toDosDigitosHex(numero) {
    let hex = numero.toString(16);
    return hex.length === 1 ? '0' + hex : hex;
}

//armamos opacidad
function toOpacityHex(opacity) {
    let opacityDecimal = Math.floor(opacity * 255);
    return toDosDigitosHex(opacityDecimal);
}


//******* /Mesas.aspx *******/
if (currentPagePath.toLowerCase().indexOf('/mesas.aspx') !== -1) {

    //Botón
    const guardarMesa = document.querySelectorAll("#guardarMesa");
    const asignarMesas = document.querySelectorAll("#asignarMesa");
    const btncancelar = document.querySelectorAll("#cancelarMesa");

    //Funciones para modificar estados de los botones
    function botonCancelar(opcion) {
        btncancelar.forEach(btn4 => {
            btn4.disabled = opcion;
        });
    }

    function botonAsignarMesa(opcion) {
        asignarMesas.forEach(btn3 => {
            btn3.disabled = opcion;
        });
    }

    function botonGuardar(opcion) {
        guardarMesa.forEach(btn2 => {
            if (btn2.getAttribute("id-mesero") == idMesero)
                btn2.disabled = opcion;
        });
    }

    let idMesero;
    let idMeseroPorDia;

    //CARGAMOS MESAS GUARDADAS
    const mesas = document.getElementById("mesas");
    let numeroMesasGuardasArray = JSON.parse(numeroMesasGuardasJSON).filter(x => x != 0);
    const cantidadMesasGuardas = numeroMesasGuardasArray.length;

    //CARGAMOS MESAS POR DIA GUARDADAS
    let numeroMesasPorDiaArray = JSON.parse(numeroMesasPorDiaJSON)
    let numeroMesasPorDiaArrayCopy = structuredClone(numeroMesasPorDiaArray)

    //FUNCION QUE CARGA LAS MESAS ASIGNADAS Y GUARDADAS EN BASE DE DATOS QUE SIGUEN ABIERTAS
    function CargarMesasGuardas() {

        mesas.innerHTML = "";


        for (let i = 0; i < cantidadMesasGuardas; i++) {

            if (numeroMesasPorDiaArray.some(el => el.mesa === i + 1)) {

                let numeroIdMesero = numeroMesasPorDiaArray.find(el => el.mesa === i + 1).mesero;

                mesas.innerHTML += `
                <style>
                    .bg-mesa${numeroIdMesero}{
                        background-color: ${convertirAHexadecimal(numeroIdMesero)};
                    }
                 </style>`

                mesas.innerHTML += `
            <div class="col-6 col-sm-3 d-flex justify-content-center flex-column m-4" style="height: 150px; width: 150px;">
                <div class="w-100 h-100 border rounded-circle border-dark-subtle p-1 btn">
                    <div class="bg-mesa${numeroIdMesero} w-100 h-100 rounded-circle d-flex justify-content-center align-items-center" id-mesero="${numeroIdMesero}" id="mesa_${i + i}">
                        <i class="fa-solid fa-utensils fs-4"></i>
                    </div>
                </div>
                <div class=" w-100 text-light d-flex justify-content-center">
                    <div class="w-50 bg-black rounded-4 text-center">
                        <small class="fw-bold">${numeroMesasGuardasArray[i]}</small>
                    </div>
                </div>
            </div>
            `;
            } else {
                mesas.innerHTML += `
            <div class="col-6 col-sm-3 d-flex justify-content-center flex-column m-4" style="height: 150px; width: 150px;">
                <div class="w-100 h-100 border rounded-circle border-dark-subtle p-1 btn">
                    <div class="bg-dark-subtle  w-100 h-100 rounded-circle d-flex justify-content-center align-items-center" id="mesa_${i + i}">
                        <i class="fa-solid fa-utensils fs-4"></i>
                    </div>
                </div>
                <div class=" w-100 text-light d-flex justify-content-center">
                    <div class="w-50 bg-black rounded-4 text-center">
                        <small class="fw-bold">${numeroMesasGuardasArray[i]}</small>
                    </div>
                </div>
            </div>
            `;

            }

        };
    }

    (function () {
        CargarMesasGuardas()
    })();


    //CARGAMOS MESAS PARA SIGNAR
    function CargarMesasSeleccion(i) {
        
        mesas.innerHTML = "";


        for (let j = 0; j < i; j++) {
            
            if (numeroMesasPorDiaArray.some(el => el.mesa === j + 1 && el.abierta == 1)) {
                let numeroIdMesero = numeroMesasPorDiaArray.find(el => el.mesa === j + 1 && el.abierta == 1).mesero;

                mesas.innerHTML += `
                <style>
                    .bg-mesa${numeroIdMesero}{
                        background-color: ${convertirAHexadecimal(numeroIdMesero)};
                    }
                 </style>`

                mesas.innerHTML += `
            <div class="col-6 col-sm-3 d-flex justify-content-center flex-column m-4" style="height: 150px; width: 150px;">
                <div class="bg-mesa${numeroIdMesero} w-100 h-100 border rounded-circle border-dark-subtle p-1 btn">
                    <div class="background-color w-100 h-100 rounded-circle d-flex justify-content-center align-items-center" id-mesero="${numeroIdMesero}" id="mesa_${j + i}">
                        <i class="fa-solid fa-utensils fs-4"></i>
                    </div>
                </div>
                <div class=" w-100 text-light d-flex justify-content-center">
                    <div class="w-50 bg-black rounded-4 text-center">
                        <small class="fw-bold">${numeroMesasGuardasArray[j]}</small>
                    </div>
                </div>
            </div>
            `;
            } else {
                mesas.innerHTML += `
            <div class="col-6 col-sm-3 d-flex justify-content-center flex-column m-4" style="height: 150px; width: 150px;">
                <div class="w-100 h-100 border rounded-circle border-dark-subtle p-1 btn">
                    <div class="bg-dark-subtle  w-100 h-100 rounded-circle d-flex justify-content-center align-items-center" id="mesa_${j + i}">
                        <i class="fa-solid fa-utensils fs-4"></i>
                    </div>
                </div>
                <div class=" w-100 text-light d-flex justify-content-center">
                    <div class="w-50 bg-black rounded-4 text-center">
                        <small class="fw-bold">${numeroMesasGuardasArray[j]}</small>
                    </div>
                </div>
            </div>
            `;

            }
        }

        for (let j = 0; j < i; j++) {

            let numeroIdMesero = numeroMesasPorDiaArray.find(el => el.mesa === j + 1)?.mesero;
            
            if (numeroMesasPorDiaArray.some(el => el.mesa === j + 1)) {

                document.getElementById(`mesa_${j + i}`).addEventListener("click", () => {
                    
                    //Si el mesero elegido no es el mismo que tiene la mesa asignada, no se puede cambiar
                    if (numeroIdMesero == idMesero) {
                        console.log("Desasigna mesa")
                        document.getElementById(`mesa_${j + i}`).classList.toggle("bg-dark-subtle");
                        numeroMesasPorDiaArray.find(el => el.mesa === j + 1).abierta = false;

                        CargarMesasSeleccion(i);
                    }
                });
            } else {

                document.getElementById(`mesa_${j + i}`).addEventListener("click", () => {
                    console.log("Asigna mesa")
                    document.getElementById(`mesa_${j + i}`).classList.toggle("bg-dark-subtle");
                    document.getElementById(`mesa_${j + i}`).style.backgroundColor = convertirAHexadecimal(idMesero);
                    numeroMesasPorDiaArray.push({ mesa: j + 1, mesero: parseInt(idMesero), idmeseropordia: parseInt(idMeseroPorDia), abierta: true });

                    CargarMesasSeleccion(i);
                });
            }
        }

    }


    //Eventos del boton cancelar
    btncancelar.forEach(btnc => {
        btnc.addEventListener('click', btn => {

            //Copiamos el array original que no fue modificado
            numeroMesasPorDiaArray = structuredClone(numeroMesasPorDiaArrayCopy)

            //Cargamos las mesas al estado anterior
            CargarMesasGuardas()

            //Modificamos estados de las mesas
            botonGuardar(true)
            botonAsignarMesa(false)
            botonCancelar(true)
        })
    })

    //Evento del botón Asignar mesas
    asignarMesas.forEach(btn => {
        btn.addEventListener('click', () => {
            //Id del mesero
            idMesero = btn.getAttribute("id-mesero");
            idMeseroPorDia = btn.getAttribute("id-meseropordia");

            //Modificamos estados de botones
            botonGuardar(false)
            botonAsignarMesa(true)
            botonCancelar(false)

            //Cargamos los eventos de la mesa
            CargarMesasSeleccion(cantidadMesasGuardas)
        })
    })


    //CAMBIAR COLORES DE MESEROS
    let colorMesero = document.querySelectorAll("#colorMesero")

    colorMesero.forEach(bg => {
        idMeseroPorDia = bg.getAttribute("id-mesero");
        bg.style.backgroundColor = convertirAHexadecimal(idMeseroPorDia)
    })

    guardarMesa.forEach(btn => {

        //Copiamos array a guardar en copia
        numeroMesasPorDiaArrayCopy = structuredClone(numeroMesasPorDiaArray)

        btn.addEventListener('click', () => {
            //Modificamos estados de los botones
            botonGuardar(true)
            botonAsignarMesa(false)
            botonGuardar(true)

            let EnviarnumeroMesasPorDiaArray = numeroMesasPorDiaArray.map((el) => {

                return { mesa: el.mesa, mesero: el.mesero, idmeseropordia: el.idmeseropordia, abierta: el.abierta ? 1 : 0 }

            });

            //Sacamos eventos a las masas
            CargarMesasSeleccion(cantidadMesasGuardas)

            //Enviamos datos a Mesas.aspx
            fetch('Mesas.aspx/GuardarMesas', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ array: EnviarnumeroMesasPorDiaArray })
            })
                .then(function (response) {
                    if (response.ok) {
                        location.reload();
                    } else {
                        console.error('Error al enviar los datos al código detrás');
                    }
                })
                .catch(function (error) {
                    console.error('Error al enviar los datos al código detrás:', error);
                });
        });
    })
};
//******* FIN /Mesas.aspx *******/


//******* /MesaHabilitar.aspx *******/
if (currentPagePath.toLowerCase().indexOf('/mesahabilitar.aspx') !== -1) {

    //CARGAMOS MESAS GUARDADAS
    const mesas = document.getElementById("mesas");
    let numeroMesasGuardasArray = JSON.parse(numeroMesasGuardasJSON)


    //RENDERIZAMOS LAS MESAS
    function CargarMesasSeleccion(i, events) {

        mesas.innerHTML = "";

        for (let j = 0; j < i + 1; j++) {
            if (numeroMesasGuardasArray[j] != '0') {
                mesas.innerHTML += `
            <div class="col-6 col-sm-3 d-flex justify-content-center flex-column m-4" style="height: 150px; width: 150px;">
                <div class="w-100 h-100 border rounded-circle border-dark-subtle p-1 btn">
                    <div class="${events ? "bg-warning" : "bg-success"} w-100 h-100 rounded-circle d-flex justify-content-center align-items-center" id="mesa_${j + i}">
                        <i class="fa-solid fa-utensils fs-4"></i>
                    </div>
                </div>
                <div class=" w-100 text-light d-flex justify-content-center">
                    <div class="w-50 bg-black rounded-4 text-center">
                        <small class="fw-bold">${j + 1}</small>
                    </div>
                </div>
            </div>
            `;
            } else {
                mesas.innerHTML += `
            <div class="col-6 col-sm-3 d-flex justify-content-center flex-column m-4" style="height: 150px; width: 150px;">
                <div class="w-100 h-100 border rounded-circle border-dark-subtle p-1 btn">
                    <div class="bg-dark-subtle w-100 h-100 rounded-circle d-flex justify-content-center align-items-center" id="mesa_${j + i}">
                        <i class="fa-solid fa-utensils fs-4"></i>
                    </div>
                </div>
                <div class=" w-100 text-light d-flex justify-content-center">
                    <div class="w-50 bg-black rounded-4 text-center">
                        <small class="fw-bold">${j + 1}</small>
                    </div>
                </div>
            </div>
            `;

            }
        }

        if (events) {
            for (let j = 0; j < i + 1; j++) {

                if (numeroMesasGuardasArray[j] != 0) {
                    document.getElementById(`mesa_${j + i}`).addEventListener("click", () => {
                        document.getElementById(`mesa_${j + i}`).classList.toggle("bg-warning");
                        document.getElementById(`mesa_${j + i}`).classList.toggle("bg-dark-subtle");
                        numeroMesasGuardasArray[j] = 0;
                    });
                } else {
                    document.getElementById(`mesa_${j + i}`).addEventListener("click", () => {
                        document.getElementById(`mesa_${j + i}`).classList.toggle("bg-dark-subtle");
                        document.getElementById(`mesa_${j + i}`).classList.toggle("bg-warning");
                        numeroMesasGuardasArray[j] = 1;
                    });
                }
            }
        }

    }

    const btnHabilitarMesas = document.getElementById("btnHabilitarMesas");
    const btnGuardarMesas = document.getElementById("btnGuardarMesas");

    //Cargamos Mesas con eventos
    btnHabilitarMesas.addEventListener('click', () => {
        btnGuardarMesas.disabled = false;
        CargarMesasSeleccion(cantidadMesas - 1, true)
    })

    //Cargamos Mesas sin eventos al inicio de la página
    CargarMesasSeleccion(cantidadMesas - 1, false)

    // GUARDAMOS MESAS
    btnGuardarMesas.addEventListener('click', () => {
        btnGuardarMesas.disabled = true;

        //Sacamos eventos a las masas
        CargarMesasSeleccion(cantidadMesas - 1, false)

        //Enviamos datos a Mesas.aspx
        fetch('MesaHabilitar.aspx/GuardarMesas', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ array: numeroMesasGuardasArray })
        })
            .then(function (response) {
                if (response.ok) {
                    location.reload();
                } else {
                    console.error('Error al enviar los datos al código detrás');
                }
            })
            .catch(function (error) {
                console.error('Error al enviar los datos al código detrás:', error);
            });
    });

}
