const currentPagePath = document.location.pathname.toLowerCase();


//******* /Mesas.aspx *******/
if (currentPagePath.toLowerCase().indexOf('/mesas.aspx') !== -1) {

    //DROPDOWN
    const ddMesa = document.getElementById("dropdown-mesa");
    let MESAS = cantidadMesas;


    for (i = 0; i < MESAS; i++) {
        ddMesa.innerHTML += `<li><a class="dropdown-item" id="ddMesa_${i + 1}">${i + 1
            }</a></li>`;
    }


    //CARGAMOS MESAS GUARDADAS
    const mesas = document.getElementById("mesas");
    let numeroMesasGuardasArray = JSON.parse(numeroMesasGuardasJSON)

    function CargarMesasGuardas() {

        mesas.innerHTML = "";

        for (let i = 0; i < numeroMesasGuardasArray.length; i++) {

            if (numeroMesasGuardasArray[i] != '0') {
                mesas.innerHTML += `
                    <div class="col-6 col-sm-3 d-flex justify-content-center flex-column m-4" style="height: 150px; width: 150px;">
                        <div class="w-100 h-100 border rounded-circle border-dark-subtle p-1 btn">
                            <div class="bg-dark-subtle w-100 h-100 rounded-circle d-flex justify-content-center align-items-center" id="mesa_${i + 1}">
                                <i class="fa-solid fa-utensils fs-4"></i>
                            </div>
                        </div>
                        <div class=" w-100 text-light d-flex justify-content-center">
                            <div class="w-50 bg-black rounded-4 text-center">
                                <small class="fw-bold">${i + 1}</small>
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


    //CARGAMOS MESAS SELECCIONADAS DESDE DROPDOWN
    const btnGuardarMesas = document.getElementById("btnGuardarMesas");

    function CargarMesasSeleccion(i) {
        btnGuardarMesas.classList.remove("invisible")
        mesas.innerHTML = "";


        for (let j = 0; j < i + 1; j++) {
            if (numeroMesasGuardasArray[j] != '0') {
                mesas.innerHTML += `
            <div class="col-6 col-sm-3 d-flex justify-content-center flex-column m-4" style="height: 150px; width: 150px;">
                <div class="w-100 h-100 border rounded-circle border-dark-subtle p-1 btn">
                    <div class="bg-warning w-100 h-100 rounded-circle d-flex justify-content-center align-items-center" id="mesa_${j + i}">
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

        for (let j = 0; j < i + 1; j++) {

            if (numeroMesasGuardasArray[j] != 0) {
                document.getElementById(`mesa_${j + i}`).addEventListener("click", () => {
                    document.getElementById(`mesa_${j + i}`).classList.toggle("bg-warning");
                    document.getElementById(`mesa_${j + i}`).classList.toggle("bg-dark-subtle");
                    numeroMesasGuardasArray[j] = 0;
                    CargarMesasSeleccion(i);
                });
            } else {
                document.getElementById(`mesa_${j + i}`).addEventListener("click", () => {
                    document.getElementById(`mesa_${j + i}`).classList.toggle("bg-dark-subtle");
                    document.getElementById(`mesa_${j + i}`).classList.toggle("bg-warning");
                    numeroMesasGuardasArray[j] = 1;
                    CargarMesasSeleccion(i);

                });
            }
        }

    }

    // RENDERIZAMOS MESAS
    const tituloGerenteMesas = document.getElementById("titulo_gerente_Mesas");

    for (let i = 0; i < MESAS; i++) {
        document.getElementById(`ddMesa_${i + 1}`).addEventListener("click", () => {
            tituloGerenteMesas.textContent = "Elija las mesas que quiere activar";
            CargarMesasSeleccion(i);
        });
    }


    // GUARDAMOS MESAS
    btnGuardarMesas.addEventListener('click', () => {
        CargarMesasGuardas()
        btnGuardarMesas.classList.add("invisible")
        tituloGerenteMesas.textContent = "Asignar Mesas a Meseros";


        //Enviamos datos a Mesas.aspx
        fetch('Mesas.aspx/GuardarMesas', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ array: numeroMesasGuardasArray })
        })
            .then(function (response) {
                if (response.ok) {
                    console.log('Datos enviados al código detrás');
                } else {
                    console.error('Error al enviar los datos al código detrás');
                }
            })
            .catch(function (error) {
                console.error('Error al enviar los datos al código detrás:', error);
            });
    });

};

//******* FIN /Mesas.aspx *******/



