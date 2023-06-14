
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
            <div class="bg-dark-subtle w-100 h-100 rounded-circle d-flex justify-content-center align-items-center" id="mesa_${i + i}">
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

function CargarMesasSeleccion(){
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
                CargarMesasSeleccion()
            });
        } else {
            document.getElementById(`mesa_${j + i}`).addEventListener("click", () => {
                document.getElementById(`mesa_${j + i}`).classList.toggle("bg-dark-subtle");
                document.getElementById(`mesa_${j + i}`).classList.toggle("bg-warning");
                numeroMesasGuardasArray[j] = j + 1;
                CargarMesasSeleccion()

            });
        }
    }

}

for (let i = 0; i < MESAS; i++) {
    document.getElementById(`ddMesa_${i + 1}`).addEventListener("click", () => {

        CargarMesasSeleccion()
    });
}

btnGuardarMesas.addEventListener('click', () => {
    CargarMesasGuardas()
    btnGuardarMesas.classList.add("invisible")

    fetch('Main.aspx/GuardarMesas', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ array: numeroMesasGuardasArray })
    })
        .then(function (response) {
            if (response.ok) {
                // La llamada AJAX se completó con éxito
                console.log('Datos enviados al código detrás');
            } else {
                // Ocurrió un error en la llamada AJAX
                console.error('Error al enviar los datos al código detrás');
            }
        })
        .catch(function (error) {
            // Ocurrió un error en la llamada AJAX
            console.error('Error al enviar los datos al código detrás:', error);
        });
});