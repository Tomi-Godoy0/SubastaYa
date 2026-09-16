const API_URL = "https://localhost:7204";

console.log("🔥 INDEX.JS CARGADO 🔥");

/* =========================
INICIO
========================= */

document.addEventListener("DOMContentLoaded", () => {

    console.log("🚀 DOM CARGADO");

    verificarSesion();

    const logoutButton =
        document.getElementById("btn-logout");

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

    const userId =
        localStorage.getItem("userId");

    console.log("👤 UserId:", userId);

    if (!userId) {

        console.warn("No hay usuario logueado");

        window.location.href = "login.html";

        return;
    }

    // Ocultar login si ya hay sesión
    const loginSection =
        document.getElementById("login");

    if (loginSection) {
        loginSection.style.display = "none";
    }

    cargarUsuario();
    cargarCategorias();
    cargarSubastas();
    cargarBilletera();
    cargarMovimientos();
    cargarAuditorias();
}


/* =========================
USUARIO
========================= */

async function cargarUsuario() {

    const userId =
        localStorage.getItem("userId");

    const nombreElement =
        document.getElementById("usuario-nombre");

    if (!userId) {
        return;
    }

    try {

        const response = await fetch(
            `${API_URL}/api/users/${userId}`
        );

        if (!response.ok) {
            throw new Error(
                "No se pudo obtener el usuario"
            );
        }

        const usuario =
            await response.json();

        if (nombreElement) {

            nombreElement.textContent =
                usuario.name;
        }

        console.log(
            "Usuario cargado:",
            usuario
        );

    } catch (error) {

        console.error(
            "Error cargando usuario:",
            error
        );

        if (nombreElement) {
            nombreElement.textContent =
                "Usuario";
        }
    }
}


/* =========================
CATEGORÍAS
========================= */

async function cargarCategorias() {

    const filtroCategoria =
        document.getElementById("filtro-categoria");

    const categoriaFormulario =
        document.getElementById("categoria");

    try {

        const response = await fetch(
            `${API_URL}/api/categories`
        );

        if (!response.ok) {

            throw new Error(
                `Error HTTP: ${response.status}`
            );
        }

        const categorias =
            await response.json();

        console.log(
            "Categorías:",
            categorias
        );

        if (filtroCategoria) {

            filtroCategoria.innerHTML =
                `<option value="">Todas las categorías</option>`;

            categorias.forEach(categoria => {

                const option =
                    document.createElement("option");

                option.value =
                    categoria.id;

                option.textContent =
                    categoria.name;

                filtroCategoria.appendChild(
                    option
                );
            });
        }

        if (categoriaFormulario) {

            categoriaFormulario.innerHTML =
                `<option value="">Seleccionar categoría</option>`;

            categorias.forEach(categoria => {

                const option =
                    document.createElement("option");

                option.value =
                    categoria.id;

                option.textContent =
                    categoria.name;

                categoriaFormulario.appendChild(
                    option
                );
            });
        }

    } catch (error) {

        console.error(
            "Error cargando categorías:",
            error
        );

        mostrarToast(
            "No se pudieron cargar las categorías.",
            "error"
        );
    }
}


/* =========================
SUBASTAS
========================= */

async function cargarSubastas() {

    const container =
        document.getElementById(
            "subastas-container"
        );

    if (!container) {
        return;
    }

    try {

        const response = await fetch(
            `${API_URL}/api/auctions`
        );

        if (!response.ok) {

            throw new Error(
                `Error HTTP: ${response.status}`
            );
        }

        const resultado =
            await response.json();

        console.log(
            "Subastas recibidas:",
            resultado
        );

        const subastas =
            Array.isArray(resultado)
                ? resultado
                : resultado.items || [];

        renderizarSubastas(
            subastas
        );

    } catch (error) {

        console.error(
            "Error cargando subastas:",
            error
        );

        container.innerHTML = `
            <p class="error-message">
                No se pudieron cargar las subastas.
            </p>
        `;

        mostrarToast(
            "No se pudieron cargar las subastas.",
            "error"
        );
    }
}


/* =========================
RENDERIZAR SUBASTAS
========================= */

function renderizarSubastas(subastas) {

    const container =
        document.getElementById(
            "subastas-container"
        );

    if (!container) {
        return;
    }

    container.innerHTML = "";

    if (!subastas || subastas.length === 0) {

        container.innerHTML = `
            <p class="empty-message">
                No hay subastas disponibles.
            </p>
        `;

        return;
    }

    subastas.forEach(subasta => {

        const card =
            document.createElement("article");

        card.className =
            "auction-card";

        const currentBid =
            subasta.currentBidAmount ??
            subasta.currentBid ??
            subasta.basePrice ??
            0;

        const totalBids =
            subasta.totalBids ??
            subasta.bids?.length ??
            0;

        const categoryName =
            subasta.category?.name ??
            subasta.categoryName ??
            "Sin categoría";

        const imagen =
            subasta.imageUrl ||
            "https://via.placeholder.com/400x250?text=Sin+imagen";

        card.innerHTML = `
            <img
                class="auction-card__image"
                src="${imagen}"
                alt="${subasta.title || "Producto"}"
            >

            <div class="auction-card__body">

                <span class="auction-card__category">
                    ${categoryName}
                </span>

                <h3>
                    ${subasta.title || "Sin título"}
                </h3>

                <p>
                    ${subasta.description || "Sin descripción"}
                </p>

                <div class="auction-card__price">
                    ${formatearPrecio(currentBid)}
                </div>

                <div class="auction-card__bids">
                    Pujas: ${totalBids}
                </div>

                <div class="auction-card__timer">
                    Estado: ${subasta.status || "Sin estado"}
                </div>

                <button
                    type="button"
                    onclick="verSubasta(${subasta.id})"
                >
                    Ver subasta
                </button>

            </div>
        `;

        container.appendChild(card);
    });
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

        const subasta =
            await response.json();

        console.log(
            "Subasta:",
            subasta
        );

        const currentBid =
            subasta.currentBidAmount ??
            subasta.currentBid ??
            subasta.basePrice ??
            0;

        alert(
            `Subasta: ${subasta.title}\n` +
            `Precio actual: ${formatearPrecio(currentBid)}\n` +
            `Pujas: ${subasta.totalBids ?? 0}\n` +
            `Estado: ${subasta.status}`
        );

    } catch (error) {

        console.error(
            "Error obteniendo subasta:",
            error
        );

        mostrarToast(
            "No se pudo obtener la información de la subasta.",
            "error"
        );
    }
}


/* =========================
BILLETERA
========================= */

async function cargarBilletera() {

    const userId =
        localStorage.getItem("userId");

    if (!userId) {
        return;
    }

    try {

        const response = await fetch(
            `${API_URL}/api/users/${userId}/wallet/balance`
        );

        if (!response.ok) {

            throw new Error(
                "No se pudo obtener el saldo"
            );
        }

        const billetera =
            await response.json();

        console.log(
            "Billetera:",
            billetera
        );

        const saldoTotal =
            document.getElementById(
                "saldo-total"
            );

        const saldoRetenido =
            document.getElementById(
                "saldo-retenido"
            );

        const saldoDisponible =
            document.getElementById(
                "saldo-disponible"
            );

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
DEPÓSITO
========================= */

function configurarDeposito() {

    const formDeposito =
        document.getElementById(
            "form-deposito"
        );

    if (!formDeposito) {
        return;
    }

    formDeposito.addEventListener(
        "submit",
        async event => {

            event.preventDefault();

            const userId =
                localStorage.getItem(
                    "userId"
                );

            if (!userId) {

                mostrarToast(
                    "No hay un usuario logueado.",
                    "error"
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

                mostrarToast(
                    "Ingresá un monto válido.",
                    "error"
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

                mostrarToast(
                    "Depósito realizado correctamente.",
                    "success"
                );

                inputMonto.value = "";

                await cargarBilletera();
                await cargarMovimientos();
                await cargarAuditorias();

            } catch (error) {

                console.error(
                    "Error realizando depósito:",
                    error
                );

                mostrarToast(
                    error.message ||
                    "No se pudo realizar el depósito.",
                    "error"
                );
            }
        }
    );
}


/* =========================
MOVIMIENTOS
========================= */


async function cargarMovimientos() {

    const userId =
        localStorage.getItem("userId");

    const container =
        document.getElementById(
            "movimientos-container"
        );

    if (!userId || !container) {
        return;
    }

    try {

        /*
         * Actualmente walletId y userId
         * coinciden en la base de datos.
         */
        const walletId = userId;

        const response = await fetch(
            `${API_URL}/api/transactions/wallet/${walletId}?pageNumber=1&pageSize=10`
        );

        if (!response.ok) {

            throw new Error(
                `Error HTTP: ${response.status}`
            );
        }

        const resultado =
            await response.json();

        console.log(
            "Movimientos recibidos:",
            resultado
        );

        container.innerHTML = "";

        if (
            !resultado.items ||
            resultado.items.length === 0
        ) {

            container.innerHTML = `
                <tr>
                    <td
                        colspan="4"
                        class="transactions-empty"
                    >
                        No hay movimientos registrados.
                    </td>
                </tr>
            `;

            return;
        }

        resultado.items.forEach(movimiento => {

            const fila =
                document.createElement("tr");

            const fecha =
                formatearFecha(
                    movimiento.createdAt
                );

            const tipo =
                movimiento.type || "MOVIMIENTO";

            let descripcion = "";
            let claseTipo = "transaction-type";

            /*
             * Descripción y color según el tipo
             */
            if (tipo === "DEPOSITO") {

                descripcion = "Carga de saldo";

                claseTipo +=
                    " transaction-type--deposit";

            } else if (tipo === "COBRO") {

                descripcion =
                    movimiento.auctionId
                        ? `Cobro de subasta #${movimiento.auctionId}`
                        : "Cobro";

                claseTipo +=
                    " transaction-type--charge";

            } else {

                descripcion = tipo;

                claseTipo +=
                    " transaction-type--other";
            }

            const monto =
                formatearPrecio(
                    movimiento.amount
                );

            /*
             * Signo visual del movimiento
             */
            const signo =
                tipo === "DEPOSITO"
                    ? "+"
                    : tipo === "COBRO"
                        ? "-"
                        : "";

            const claseMonto =
                tipo === "DEPOSITO"
                    ? "transaction-amount transaction-amount--positive"
                    : tipo === "COBRO"
                        ? "transaction-amount transaction-amount--negative"
                        : "transaction-amount";

            fila.innerHTML = `
                <td class="audit-date">
                    ${fecha}
                </td>

                <td>
                    <span class="${claseTipo}">
                        ${tipo}
                    </span>
                </td>

                <td class="transaction-description">
                    ${descripcion}
                </td>

                <td class="${claseMonto}">
                    ${signo}${monto}
                </td>
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
                <td
                    colspan="4"
                    class="transactions-error"
                >
                    No se pudieron cargar los movimientos.
                </td>
            </tr>
        `;
    }
}



/* =========================
AUDITORÍAS
========================= */

async function cargarAuditorias() {

    console.log("🔥 CARGANDO AUDITORIAS 🔥");

    const userId =
        localStorage.getItem("userId");

    const container =
        document.getElementById(
            "auditorias-container"
        );

    console.log(
        "👤 UserId auditorías:",
        userId
    );

    console.log(
        "📋 Container auditorías:",
        container
    );

    if (!userId) {

        console.error(
            "❌ No existe userId"
        );

        return;
    }

    if (!container) {

        console.error(
            "❌ No existe #auditorias-container"
        );

        return;
    }

    try {

        const url =
            `${API_URL}/api/audits?userId=${userId}&pageNumber=1&pageSize=100`;

        console.log(
            "📡 URL auditorías:",
            url
        );

        const response =
            await fetch(url);

        console.log(
            "📡 Status auditorías:",
            response.status
        );

        if (!response.ok) {

            const errorText =
                await response.text();

            console.error(
                "❌ Respuesta del servidor:",
                errorText
            );

            throw new Error(
                `Error HTTP: ${response.status}`
            );
        }

        const resultado =
            await response.json();

        console.log(
            "📦 Resultado auditorías:",
            resultado
        );

        const auditorias =
            Array.isArray(resultado.items)
                ? resultado.items
                : [];

        console.log(
            "📊 Cantidad auditorías:",
            auditorias.length
        );

        container.innerHTML = "";

        if (auditorias.length === 0) {

            container.innerHTML = `
                <tr>
                    <td colspan="5">
                        No hay auditorías registradas.
                    </td>
                </tr>
            `;

            return;
        }

        auditorias.forEach(
            auditoria => {

                console.log(
                    "➡️ Renderizando:",
                    auditoria
                );

                const fila =
                    document.createElement("tr");

                const fecha =
                    formatearFecha(
                        auditoria.createdAt
                    );

                let detalle = "-";

                if (
                    auditoria.detailJson
                ) {

                    try {

                        const datos =
                            JSON.parse(
                                auditoria.detailJson
                            );

                        if (
                            datos.amount !== undefined &&
                            datos.newTotalBalance !== undefined
                        ) {

                            detalle =
                                `Depósito: ${formatearPrecio(datos.amount)} | ` +
                                `Nuevo saldo: ${formatearPrecio(datos.newTotalBalance)}`;

                        } else {

                            detalle =
                                Object.entries(datos)
                                    .map(
                                        ([clave, valor]) =>
                                            `${clave}: ${valor}`
                                    )
                                    .join(" | ");
                        }

                    } catch (error) {

                        console.warn(
                            "⚠️ Error parseando detailJson:",
                            error
                        );

                        detalle =
                            auditoria.detailJson;
                    }
                }

                fila.innerHTML = `
                <td class="transaction-date">
                    ${fecha}
                </td>

                <td class="audit-entity">
                    ${auditoria.entity ?? "-"}
                </td>

                <td class="audit-id">
                    #${auditoria.entityId ?? "-"}
                </td>

                <td>
                    <span class="audit-action">
                        ${auditoria.action ?? "-"}
                    </span>
                </td>

                <td class="audit-detail">
                    ${detalle}
                </td>
            `;

                container.appendChild(
                    fila
                );
            }
        );

        console.log(
            "✅ Auditorías renderizadas:",
            container.children.length
        );

    } catch (error) {

        console.error(
            "❌ Error cargando auditorías:",
            error
        );

        container.innerHTML = `
            <tr>
                <td colspan="5">
                    No se pudieron cargar las auditorías.
                </td>
            </tr>
        `;
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
    ).format(valor ?? 0);
}


/* =========================
FORMATEAR FECHA
========================= */

function formatearFecha(fecha) {

    if (!fecha) {
        return "Sin fecha";
    }

    return new Date(
        fecha
    ).toLocaleString(
        "es-AR",
        {
            dateStyle: "short",
            timeStyle: "short"
        }
    );
}


/* =========================
TOAST
========================= */

function mostrarToast(
    mensaje,
    tipo = "success"
) {

    const container =
        document.getElementById(
            "toast-container"
        );

    if (!container) {

        alert(mensaje);

        return;
    }

    const toast =
        document.createElement(
            "div"
        );

    toast.className =
        `toast ${tipo}`;

    toast.textContent =
        mensaje;

    container.appendChild(
        toast
    );

    setTimeout(() => {

        toast.remove();

    }, 4000);
}
