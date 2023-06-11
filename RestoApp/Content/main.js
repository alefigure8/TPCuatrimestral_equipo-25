const ddMesa = document.getElementById("dropdown-mesa");
const MESAS = 20;

//Dropdown de mesas
for (i = 0; i < MESAS; i++) {
    ddMesa.innerHTML += `<li><a class="dropdown-item" id="ddMesa_${i + 1}">${i + 1
        }</a></li>`;
}

const mesas = document.getElementById("mesas");
const inputMesa = document.getElementById("txb_guardar_mesa");

for (let i = 0; i < MESAS; i++) {
    document.getElementById(`ddMesa_${i + 1}`).addEventListener("click", () => {
        mesas.innerHTML = "";
        for (let j = 0; j < i + 1; j++) {
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

        for (let j = 0; j < i + 1; j++) {
            document.getElementById(`mesa_${j + i}`).addEventListener("click", () => {
                document.getElementById(`mesa_${j + i}`).classList.toggle("bg-dark-subtle");
                document.getElementById(`mesa_${j + i}`).classList.toggle("bg-success");
            });
        }

        inputMesa.value = i + 1;
    });
}
