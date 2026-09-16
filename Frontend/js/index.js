const API_URL = "https://localhost:7204";

document.addEventListener("DOMContentLoaded", () => {
    verificarSesion();

    const logoutButton = document.getElementById("btn-logout");

    if (logoutButton) {
        logoutButton.addEventListener("click", () => {
            localStorage.removeItem("userId");
            window.location.href = "login.html";
        });
    }

    configurarDeposito();
});


/* =========================
   VERIFICAR SESIÓN
========================= */

function verificarSesion() {

    const userId = localStorage.getItem("userId");

    const loginSection = document.getElementById("login");

    if (!userId) {

        console.warn("No hay usuario logueado");

        if (loginSection) {
            loginSection.style.display = "block";
        }

        window.location.href = "login.html";

        return;
    }

    // Ocultar login si ya está autenticado
    if (loginSection) {
        loginSection.style.display = "none";
    }

    cargarUsuario();
    cargarCategorias();
    cargarSubastas();
    cargarBilletera();
    cargarMovimientos();
}


/* =========================
   USUARIO LOGUEADO
========================= */

async function cargarUsuario() {

    const userId = localStorage.getItem("userId");
    const nombreElement = document.getElementById("usuario-nombre");

    if (!userId) {
        console.warn("No hay usuario logueado");
        return;
    }

    try {

        const response = await fetch(
            `${API_URL}/api/users/${userId}`
        );

        if (!response.ok) {
            throw new Error("No se pudo obtener el usuario");
        }

        const usuario = await response.json();

        if (nombreElement) {
            nombreElement.textContent = usuario.name;
        }

        console.log("Usuario cargado:", usuario);

    } catch (error) {

        console.error(
            "Error cargando usuario:",
            error
        );

        if (nombreElement) {
            nombreElement.textContent = "Usuario";
        }
    }
}


/* =========================
   CATEGORÍAS
========================= */

async function cargarCategorias() {

    const filtroCategoria =
        document.getElementById("filtro-categoria");

    const categoriaSubasta =
        document.getElementById("categoria");

    try {

        const response = await fetch(
            `${API_URL}/api/categories`
        );

        if (!response.ok) {
            throw new Error(
                "No se pudieron cargar las categorías"
            );
        }

        const categorias = await response.json();

        console.log(
            "Categorías recibidas:",
            categorias
        );


        // Categorías para el filtro del catálogo

        if (filtroCategoria) {

            categorias.forEach(categoria => {

                const option =
                    document.createElement("option");

                option.value = categoria.id;
                option.textContent = categoria.name;

                filtroCategoria.appendChild(option);
            });
        }


        // Categorías para crear una subasta

        if (categoriaSubasta) {

            categorias.forEach(categoria => {

                const option =
                    document.createElement("option");

                option.value = categoria.id;
                option.textContent = categoria.name;

                categoriaSubasta.appendChild(option);
            });
        }

    } catch (error) {

        console.error(
            "Error cargando categorías:",
            error
        );
    }
}


/* =========================
   SUBASTAS
========================= */

async function cargarSubastas() {

    const container =
        document.getElementById("subastas-container");

    try {

        const response = await fetch(
            `${API_URL}/api/auctions`
        );

        if (!response.ok) {
            throw new Error(
                "No se pudieron cargar las subastas"
            );
        }

        const resultado = await response.json();

        console.log(
            "Subastas recibidas:",
            resultado
        );

    } catch (error) {

        console.error(
            "Error cargando subastas:",
            error
        );
    }
}


/* =========================
   VER SUBASTA
========================= */

async function verSubasta(id) {

    try {

        const response = await fetch(
            `${API_URL}/api/auctions/${id}`
        );

        if (!response.ok) {
            throw new Error(
                `Error HTTP: ${response.status}`
            );
        }

        const subasta = await response.json();

        console.log(
            "Subasta:",
            subasta
        );

        alert(
            `Subasta: ${subasta.title}\n` +
            `Precio actual: ${formatearPrecio(subasta.currentBidAmount)}\n` +
            `Pujas: ${subasta.totalBids}\n` +
            `Estado: ${subasta.status}`
        );

    } catch (error) {

        console.error(
            "Error obteniendo subasta:",
            error
        );

        alert(
            "No se pudo obtener la información de la subasta."
        );
    }
}


/* =========================
   FORMATEAR PRECIO
========================= */

function formatearPrecio(valor) {

    return new Intl.NumberFormat(
        "es-AR",
        {
            style: "currency",
            currency: "ARS",
            minimumFractionDigits: 0
        }
    ).format(valor);
}


/* =========================
   FORMATEAR FECHA
========================= */

function formatearFecha(fecha) {

    if (!fecha) {
        return "Sin fecha";
    }

    return new Date(fecha).toLocaleString(
        "es-AR",
        {
            dateStyle: "short",
            timeStyle: "short"
        }
    );
}


/* =========================
   BILLETERA
========================= */

async function cargarBilletera() {

    const userId = localStorage.getItem("userId");

    if (!userId) {
        console.warn(
            "No hay usuario logueado para cargar la billetera"
        );
        return;
    }

    try {

        const response = await fetch(
            `${API_URL}/api/users/${userId}/wallet/balance`
        );

        if (!response.ok) {
            throw new Error(
                "No se pudo obtener el saldo de la billetera"
            );
        }

        const billetera = await response.json();

        console.log(
            "Billetera recibida:",
            billetera
        );


        const saldoTotal =
            document.getElementById("saldo-total");

        const saldoRetenido =
            document.getElementById("saldo-retenido");

        const saldoDisponible =
            document.getElementById("saldo-disponible");


        if (saldoTotal) {
            saldoTotal.textContent =
                formatearPrecio(
                    billetera.totalBalance
                );
        }

        if (saldoRetenido) {
            saldoRetenido.textContent =
                formatearPrecio(
                    billetera.heldBalance
                );
        }

        if (saldoDisponible) {
            saldoDisponible.textContent =
                formatearPrecio(
                    billetera.availableBalance
                );
        }

    } catch (error) {

        console.error(
            "Error cargando billetera:",
            error
        );
    }
}


/* =========================
   CONFIGURAR DEPÓSITO
========================= */

function configurarDeposito() {

    const formDeposito =
        document.getElementById("form-deposito");

    if (!formDeposito) {
        return;
    }

    formDeposito.addEventListener(
        "submit",
        async (event) => {

            event.preventDefault();

            const userId =
                localStorage.getItem("userId");

            if (!userId) {

                alert(
                    "No hay un usuario logueado."
                );

                return;
            }

            const inputMonto =
                document.getElementById(
                    "monto-deposito"
                );

            const monto =
                Number(inputMonto.value);

            if (!monto || monto <= 0) {

                alert(
                    "Ingresá un monto válido."
                );

                return;
            }

            try {

                const response = await fetch(
                    `${API_URL}/api/users/${userId}/wallet/deposit`,
                    {
                        method: "POST",

                        headers: {
                            "Content-Type":
                                "application/json"
                        },

                        body: JSON.stringify({
                            amount: monto
                        })
                    }
                );


                const data =
                    await response.json();


                if (!response.ok) {

                    throw new Error(
                        data.message ||
                        "No se pudo realizar el depósito."
                    );
                }


                console.log(
                    "Depósito realizado:",
                    data
                );


                alert(
                    "Depósito realizado correctamente."
                );


                // Limpiar el formulario
                inputMonto.value = "";


                // Volver a consultar el saldo
                await cargarBilletera();
                await cargarMovimientos();

            } catch (error) {

                console.error(
                    "Error realizando depósito:",
                    error
                );

                alert(
                    error.message
                );
            }
        }
    );
}




/* =========================
   MOVIMIENTOS DE BILLETERA
========================= */

async function cargarMovimientos() {

    const userId = localStorage.getItem("userId");
    const container = document.getElementById("movimientos-container");

    if (!userId) {
        console.warn("No hay usuario logueado para cargar movimientos");
        return;
    }

    if (!container) {
        console.warn("No se encontró el contenedor de movimientos");
        return;
    }

    try {

        // Actualmente userId y walletId coinciden
        const walletId = userId;

        const response = await fetch(
            `${API_URL}/api/transactions/wallet/${walletId}?pageNumber=1&pageSize=10`
        );

        if (!response.ok) {
            throw new Error(
                "No se pudieron cargar los movimientos"
            );
        }

        const resultado = await response.json();

        console.log(
            "Movimientos recibidos:",
            resultado
        );

        // Limpiar tabla
        container.innerHTML = "";

        if (!resultado.items || resultado.items.length === 0) {

            container.innerHTML = `
                <tr>
                    <td colspan="4">
                        No hay movimientos registrados.
                    </td>
                </tr>
            `;

            return;
        }

        resultado.items.forEach(movimiento => {

            const fila = document.createElement("tr");

            const fecha = formatearFecha(
                movimiento.createdAt
            );

            const tipo = movimiento.type;

            let descripcion = "";

            if (tipo === "DEPOSITO") {
                descripcion = "Carga de saldo";
            }
            else if (tipo === "COBRO") {
                descripcion = movimiento.auctionId
                    ? `Cobro de subasta #${movimiento.auctionId}`
                    : "Cobro";
            }
            else {
                descripcion = tipo;
            }

            const monto = formatearPrecio(
                movimiento.amount
            );

            fila.innerHTML = `
                <td>${fecha}</td>
                <td>${tipo}</td>
                <td>${descripcion}</td>
                <td>${monto}</td>
            `;

            container.appendChild(fila);
        });

    } catch (error) {

        console.error(
            "Error cargando movimientos:",
            error
        );

        container.innerHTML = `
            <tr>
                <td colspan="4">
                    No se pudieron cargar los movimientos.
                </td>
            </tr>
        `;
    }
}

