"use strict";

/* =========================================================
   CONFIGURACIÓN
========================================================= */

const API_URL = "https://localhost:7204";
const TAMANO_PAGINA_MOVIMIENTOS = 10;

/* =========================================================
   ESTADO
========================================================= */

let paginaMovimientos = 1;
let totalPaginasMovimientos = 1;

/* =========================================================
   USUARIO LOGUEADO
========================================================= */

function obtenerUserId() {

    const valor = localStorage.getItem("userId");

    if (!valor) {
        return null;
    }

    const id = Number(valor);

    if (!Number.isInteger(id) || id <= 0) {
        return null;
    }

    return id;
}

/* =========================================================
   INICIO
========================================================= */

document.addEventListener("DOMContentLoaded", () => {

    const userId = obtenerUserId();

    if (!userId) {

        console.error("No existe un userId válido.");

        window.location.href = "../login.html";

        return;
    }

    console.log("Usuario logueado:", userId);

    configurarTabs();
    configurarEventos();
    configurarPaginacion();

    cargarUsuario();
    cargarMisPujas();
    cargarMisPublicaciones();
    cargarBilletera();
    cargarHistorialMovimientos();

});

/* =========================================================
   EVENTOS
========================================================= */

function configurarEventos() {

    const logout = document.getElementById("btn-logout");

    if (logout) {
        logout.addEventListener("click", cerrarSesion);
    }

    const deposito = document.getElementById("form-deposito");

    if (deposito) {
        deposito.addEventListener("submit", realizarDeposito);
    }
}

/* =========================================================
   TABS
========================================================= */

function configurarTabs() {

    const tabs = document.querySelectorAll(".dashboard-tab");
    const panels = document.querySelectorAll(".dashboard-panel");

    tabs.forEach(tab => {
        tab.addEventListener("click", () => {

            const tabName = tab.dataset.tab;
            const panelId = `panel-${tabName}`;

            tabs.forEach(item => {
                item.classList.remove("is-active");
            });

            tab.classList.add("is-active");

            panels.forEach(panel => {
                panel.hidden = panel.id !== panelId;
            });

            if (tabName === "billetera") {
                paginaMovimientos = 1;

                cargarBilletera();
                cargarHistorialMovimientos();
            }
        });
    });
}

/* =========================================================
   USUARIO
========================================================= */
async function cargarUsuario() {

    const element =
        document.getElementById("usuario-nombre");

    if (!element) {
        console.warn("No existe #usuario-nombre");
        return;
    }

    const userId = obtenerUserId();

    if (!userId) {
        element.textContent = "Usuario";
        return;
    }

    try {
        const response =
            await fetch(
                `${API_URL}/api/users/${userId}`,
                {
                    method: "GET",
                    headers: {
                        "Accept": "application/json"
                    }
                }
            );

        if (!response.ok) {
            throw new Error(
                `HTTP ${response.status}`
            );
        }

        const usuario =
            await response.json();

        console.log("Usuario recibido:", usuario);
        /*
         * Adaptamos posibles nombres
         * del DTO del usuario.
         */

        element.textContent =
            usuario.name ??
            usuario.nombre ??
            usuario.alias ??
            usuario.email ??
            "Usuario";
    }
    catch (error) {
        console.error(
            "Error al cargar usuario:",
            error
        );
        element.textContent = "Usuario";
    }
}

/* =========================================================
   MIS PUJAS
========================================================= */
async function cargarMisPujas() {
    const tbody = document.getElementById("tabla-mis-pujas");

    if (!tbody) {
        return;
    }
    const userId = obtenerUserId();

    if (!userId) {

        tbody.innerHTML = `
            <tr>
                <td colspan="5">
                    No se pudo identificar al usuario.
                </td>
            </tr>
        `;
        return;
    }

    tbody.innerHTML = `
        <tr>
            <td colspan="5">
                Cargando...
            </td>
        </tr>
    `;

    try {
        const response =
            await fetch(
                `${API_URL}/api/users/${userId}/bids`,
                {
                    method: "GET",
                    headers: {
                        "Accept": "application/json"
                    }
                }
            );

        if (!response.ok) {
            throw new Error(
                `HTTP ${response.status}`
            );
        }

        const data = await response.json();

        console.log("Mis pujas:", data);

        const pujas =
            Array.isArray(data)
                ? data
                : data.items ??
                  data.data ??
                  data.items?.items ??
                  [];

        renderizarMisPujas(pujas);
    }
    catch (error) {
        console.error(
            "Error cargando pujas:",
            error
        );

        tbody.innerHTML = `
            <tr>
                <td colspan="5">
                    No se pudieron cargar tus pujas.
                </td>
            </tr>
        `;
    }
}

/* =========================================================
   RENDER PUJAS
========================================================= */
function renderizarMisPujas(pujas) {

    const tbody = document.getElementById("tabla-mis-pujas");

    if (!tbody) {
        return;
    }

    tbody.innerHTML = "";

    if (!Array.isArray(pujas) || pujas.length === 0) {

        tbody.innerHTML = `
            <tr>
                <td colspan="5">
                    Todavía no participaste
                    en ninguna subasta.
                </td>
            </tr>
        `;
        return;
    }

    pujas.forEach(puja => {
        const tr = document.createElement("tr");
        /*
         * UserBidsResponse
         */

        const auctionId = puja.id;

        const titulo = puja.title || "Subasta";

        const miPuja = puja.myBidAmount ?? 0;

        const actual = puja.currentBidAmount ?? 0;

        const estado = puja.status || "Sin estado";

        const fecha = puja.endDate;

        tr.innerHTML = `
            <td>
                ${escapeHtml(titulo)}
            </td>

            <td>
                ${formatearPrecio(miPuja)}
            </td>

            <td>
                ${formatearPrecio(actual)}
            </td>

            <td>
                ${escapeHtml(estado)}
            </td>

            <td>
                ${fecha ? formatearFecha(fecha) : "-"}
            </td>
        `;

        if (auctionId) {
            tr.style.cursor = "pointer";

            tr.addEventListener("click", () => {
                window.location.href = `subasta.html?id=${auctionId}`;
            });
        }
        tbody.appendChild(tr);
    });
}

/* =========================================================
   MIS PUBLICACIONES
========================================================= */

async function cargarMisPublicaciones() {

    const tbody = document.getElementById("tabla-mis-publicaciones");

    if (!tbody) {
        return;
    }

    const userId = obtenerUserId();

    if (!userId) {
        tbody.innerHTML = `
            <tr>
                <td colspan="5">
                    No se pudo identificar al usuario.
                </td>
            </tr>
        `;
        return;
    }

    tbody.innerHTML = `
        <tr>
            <td colspan="5">
                Cargando...
            </td>
        </tr>
    `;

    try {
        const response =
            await fetch(
                `${API_URL}/api/users/${userId}/auctions`,
                {
                    method: "GET",
                    headers: {
                        "Accept": "application/json"
                    }
                }
            );

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }

        const data = await response.json();

        console.log("Mis publicaciones:", data);

        const publicaciones =
            Array.isArray(data)
                ? data
                : data.items ??
                  data.data ??
                  [];

        renderizarPublicaciones(publicaciones);
    }
    catch (error) {
        console.error("Error cargando publicaciones:",error);

        tbody.innerHTML = `
            <tr>
                <td colspan="5">
                    No se pudieron cargar tus publicaciones.
                </td>
            </tr>
        `;
    }
}

/* =========================================================
   RENDER PUBLICACIONES
========================================================= */

function renderizarPublicaciones(publicaciones) {

    const tbody = document.getElementById("tabla-mis-publicaciones");

    if (!tbody) {
        return;
    }

    tbody.innerHTML = "";

    if (!Array.isArray(publicaciones) || publicaciones.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="5">
                    No tenés publicaciones.
                </td>
            </tr>
        `;
        return;
    }

    publicaciones.forEach(subasta => {

        const tr = document.createElement("tr");
        /*
         * UserAuctionsResponse
         */

        const id = subasta.id;

        const titulo = subasta.title || "Subasta";

        const precioInicial = subasta.basePrice ?? 0;
        
        const precioActual = subasta.currentBidAmount ?? 0;

        const estado = subasta.status || "Sin estado";

        const fechaFin = subasta.endDate;

        tr.innerHTML = `
            <td>
                ${escapeHtml(titulo)}
            </td>

            <td>
                ${formatearPrecio(precioInicial)}
            </td>

            <td>
                ${formatearPrecio(precioActual)}
            </td>

            <td>
                ${escapeHtml(estado)}
            </td>

            <td>
                ${fechaFin ? formatearFecha(fechaFin) : "-"}
            </td>
        `;

        if (id) {
            tr.style.cursor = "pointer";

            tr.addEventListener("click",
                () => {
                    window.location.href = `subasta.html?id=${id}`;
                }
            );
        }
        tbody.appendChild(tr);
    });
}

/* =========================================================
   BILLETERA
========================================================= */
async function cargarBilletera() {

    const userId = obtenerUserId();

    if (!userId) {
        return;
    }

    try {

        const response =
            await fetch(
                `${API_URL}/api/users/${userId}/wallet/balance`,
                {
                    method: "GET",
                    headers: {
                        "Accept": "application/json"
                    }
                }
            );

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }

        const wallet = await response.json();

        console.log(
            "Billetera:",
            wallet
        );

        /*
         * WalletResponse
         */

        const saldoTotal = Number(wallet.totalBalance ?? 0);
        const disponible = Number(wallet.availableBalance ?? 0);
        const retenido = Number(wallet.heldBalance ?? 0);

        const elementoTotal = document.getElementById("saldo-total");
        const elementoDisponible = document.getElementById("saldo-disponible");
        const elementoRetenido = document.getElementById("saldo-retenido");

        if (elementoTotal) {
            elementoTotal.textContent = formatearPrecio(saldoTotal);
        }

        if (elementoDisponible) {
            elementoDisponible.textContent = formatearPrecio(disponible);
        }

        if (elementoRetenido) {
            elementoRetenido.textContent = formatearPrecio(retenido);
        }
    }
    catch (error) {
        console.error(
            "Error al cargar billetera:",
            error
        );
    }
}

/* =========================================================
   HISTORIAL DE MOVIMIENTOS
========================================================= */

async function cargarHistorialMovimientos() {

    const tbody = document.getElementById("tabla-movimientos");

    if (!tbody) {
        console.warn("No existe #tabla-movimientos en el HTML.");
        return;
    }

    const userId = obtenerUserId();

    if (!userId) {
        tbody.innerHTML = `
            <tr>
                <td colspan="4">
                    No se pudo identificar al usuario.
                </td>
            </tr>
        `;
        return;
    }

    tbody.innerHTML = `
        <tr>
            <td colspan="4">
                Cargando movimientos...
            </td>
        </tr>
    `;

    const walletId = userId;

    const url =
        `${API_URL}/api/transactions/wallet/${walletId}` +
        `?pageNumber=${paginaMovimientos}` +
        `&pageSize=${TAMANO_PAGINA_MOVIMIENTOS}`;

    console.log(
        "Cargando historial:",
        url
    );

    try {
        const response =
            await fetch(
                url,
                {
                    method: "GET",
                    headers: {
                        "Accept": "application/json"
                    }
                }
            );

        const texto = await response.text();

        console.log("Transactions status:", response.status);

        console.log("Transactions response:", texto);

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}: ${texto}`);
        }

        let data = null;

        if (texto) {
            data = JSON.parse(texto);
        }

        console.log("Movimientos recibidos:", data);

        const movimientos =
            Array.isArray(data)
                ? data
                : Array.isArray(data?.items)
                    ? data.items
                    : Array.isArray(data?.data)
                        ? data.data
                        : [];


        const totalCount = Number(
                data?.totalCount ??
                data?.totalItems ??
                movimientos.length
            );

        const pageSize = Number(data?.pageSize ?? TAMANO_PAGINA_MOVIMIENTOS);

        totalPaginasMovimientos = Math.max(1, Math.ceil(totalCount / pageSize));

        renderizarHistorialMovimientos(movimientos);
        actualizarPaginacionMovimientos();
    }
    catch (error) {

        console.error(
            "Error cargando historial:",
            error
        );


        tbody.innerHTML = `
            <tr>
                <td colspan="4">
                    No se pudo cargar el historial de movimientos.
                </td>
            </tr>
        `;

        actualizarPaginacionMovimientos();
    }
}

/* =========================================================
   RENDER HISTORIAL
========================================================= */
function renderizarHistorialMovimientos(movimientos) {
    const tbody = document.getElementById("tabla-movimientos");

    if (!tbody) {
        return;
    }

    tbody.innerHTML = "";

    if (!Array.isArray(movimientos) || movimientos.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="4">
                    No hay movimientos registrados.
                </td>
            </tr>
        `;
        return;
    }

    movimientos.forEach(movimiento => {

        const tr = document.createElement("tr");

        const tipo =
            movimiento.type ??
            movimiento.transactionType ??
            movimiento.tipo ??
            movimiento.transactionTypeName ??
            "Movimiento";

        const monto = Number(movimiento.amount ?? movimiento.monto ?? 0);

        const fecha =
            movimiento.createdAt ??
            movimiento.date ??
            movimiento.fecha;

        const auctionId =
            movimiento.auctionId ??
            movimiento.auctionID ??
            null;

        const descripcion = obtenerDescripcionMovimiento(movimiento);
        const tipoTexto = obtenerTextoMovimiento(tipo);

        tr.innerHTML = `
            <td>
                ${fecha ? formatearFechaMovimiento(fecha) : "-"}
            </td>

            <td>
                ${escapeHtml(tipoTexto)}
            </td>

            <td>
                ${escapeHtml(descripcion)}

                ${
                    auctionId
                        ? `
                            <br>
                            <small>
                                Subasta #${escapeHtml(
                                    auctionId
                                )}
                            </small>
                        `
                        : ""
                }
            </td>

            <td class="${monto >= 0
                        ? "transaction-amount--positive"
                        : "transaction-amount--negative"
                }">

                ${monto >= 0
                        ? "+"
                        : ""
                }

                ${formatearPrecioMovimiento(monto)}
            </td>
        `;

        tbody.appendChild(tr);
    });
}

/* =========================================================
   DESCRIPCIÓN MOVIMIENTO
========================================================= */
function obtenerDescripcionMovimiento(movimiento) {
    const tipo =String(
            movimiento.type ??
            movimiento.transactionType ??
            movimiento.tipo ??
            ""
        ).toLowerCase();

    if (tipo.includes("deposit") || tipo.includes("deposito")) {
        return "Depósito de fondos";
    }

    if (tipo.includes("bid") || tipo.includes("puja")) {
        if (movimiento.auctionId) {
            return (
                `Puja en subasta #` +
                `${movimiento.auctionId}`
            );
        }
        return "Puja";
    }

    if (tipo.includes("payment") || tipo.includes("pago")) {
        return "Pago";
    }

    if (tipo.includes("refund") || tipo.includes("reintegro")) {
        return "Reintegro";
    }

    if (tipo.includes("withdraw") || tipo.includes("retiro")) {
        return "Retiro";
    }

    if (tipo.includes("charge") || tipo.includes("cargo")) {
        return "Cargo de billetera";
    }

    return "Movimiento de billetera";
}

/* =========================================================
   TEXTO MOVIMIENTO
========================================================= */

function obtenerTextoMovimiento(tipo) {
    const valor = String(tipo)
            .trim()
            .toUpperCase();

    switch (valor) {
        case "DEPOSIT":
        case "DEPOSITO":
            return "Depósito";

        case "BID":
        case "PUJA":
            return "Puja";

        case "PAYMENT":
        case "PAGO":
            return "Pago";

        case "REFUND":
        case "REINTEGRO":
            return "Reintegro";

        case "WITHDRAW":
        case "RETIRO":
            return "Retiro";

        case "CHARGE":
        case "CARGO":
            return "Cargo";

        default:
            return tipo || "Movimiento";
    }
}

/* =========================================================
   PAGINACIÓN
========================================================= */

function actualizarPaginacionMovimientos() {

    const pagina = document.getElementById("pagina-movimientos");

    const anterior = document.getElementById("btn-movimientos-anterior");
    const siguiente = document.getElementById("btn-movimientos-siguiente");

    if (pagina) {
        pagina.textContent = `Página ${paginaMovimientos} de ${totalPaginasMovimientos}`;
    }

    if (anterior) {
        anterior.disabled = paginaMovimientos <= 1;
    }

    if (siguiente) {
        siguiente.disabled = paginaMovimientos >= totalPaginasMovimientos;
    }
}

/* =========================================================
   CONFIGURAR PAGINACIÓN
========================================================= */

function configurarPaginacion() {
    const anterior = document.getElementById("btn-movimientos-anterior");
    const siguiente = document.getElementById("btn-movimientos-siguiente");

    if (anterior) {
        anterior.addEventListener("click",
            () => {
                if (paginaMovimientos <= 1) {
                    return;
                }
                paginaMovimientos--;
                cargarHistorialMovimientos();
            }
        );
    }

    if (siguiente) {
        siguiente.addEventListener("click",
            () => {
                if (paginaMovimientos >= totalPaginasMovimientos) {
                    return;
                }
                paginaMovimientos++;
                cargarHistorialMovimientos();
            }
        );
    }
}

/* =========================================================
   DEPÓSITO
========================================================= */
async function realizarDeposito(event) {
    event.preventDefault();

    const userId = obtenerUserId();

    if (!userId) {
        mostrarToast(
            "No se pudo identificar al usuario.",
            "error"
        );
        return;
    }

    const input = document.getElementById("monto-deposito");

    if (!input) {
        return;
    }

    const monto = Number(input.value);

    if (!monto || monto <= 0) {
        mostrarToast(
            "Ingresá un monto válido.",
            "error"
        );
        return;
    }

    try {
        const response = await fetch(`${API_URL}/api/users/${userId}/wallet/deposit`,
                {
                    method: "POST",
                    headers: {
                        "Content-Type":
                            "application/json",

                        "Accept":
                            "application/json"
                    },
                    body:
                        JSON.stringify({
                            amount: monto
                        })
                }
            );

        const texto = await response.text();
        let data = null;

        try {
            data = texto
                    ? JSON.parse(texto)
                    : null;
        }
        catch {
            data = null;
        }

        if (!response.ok) {
            throw new Error(
                data?.message ||
                data?.title ||
                `HTTP ${response.status}`
            );
        }
        input.value = "";

        mostrarToast(
            "Depósito realizado correctamente.",
            "success"
        );

        await cargarBilletera();
        paginaMovimientos = 1;
        await cargarHistorialMovimientos();
    }
    catch (error) {
        console.error(
            "Error al realizar depósito:",
            error
        );

        mostrarToast(
            error.message ||
            "No se pudo realizar el depósito.",
            "error"
        );
    }
}

/* =========================================================
   SESIÓN
========================================================= */
function cerrarSesion() {

    localStorage.removeItem("userId");
    localStorage.removeItem("token");
    localStorage.removeItem("accessToken");
    localStorage.removeItem("user");

    window.location.href = "../login.html";
}

/* =========================================================
   FECHAS
========================================================= */
function formatearFecha(fecha) {

    const date = new Date(fecha);

    if (Number.isNaN(date.getTime())) {
        return String(
            fecha ?? "-"
        );
    }

    return date.toLocaleString("es-AR");
}

function formatearFechaMovimiento(fecha) {
    const date = new Date(fecha);

    if (Number.isNaN(date.getTime())) {
        return "Fecha inválida";
    }

    return date.toLocaleString(
        "es-AR",
        {
            day: "2-digit",
            month: "2-digit",
            year: "numeric",
            hour: "2-digit",
            minute: "2-digit"
        }
    );
}

/* =========================================================
   PRECIOS
========================================================= */
function formatearPrecio(valor) {
    return new Intl.NumberFormat(
        "es-AR",
        {
            style: "currency",
            currency: "ARS"
        }
    ).format(
        Number(valor) || 0
    );
}


function formatearPrecioMovimiento(valor) {
    return Number(
        valor || 0
    ).toLocaleString(
        "es-AR",
        {
            style: "currency",
            currency: "ARS",
            minimumFractionDigits: 0
        }
    );
}

/* =========================================================
   ESCAPE HTML
========================================================= */
function escapeHtml(texto) {
    const div = document.createElement("div");

    div.textContent = texto ?? "";

    return div.innerHTML;
}

/* =========================================================
   TOAST
========================================================= */
function mostrarToast(mensaje, tipo = "") {
    const container = document.getElementById("toast-container");

    if (!container) {
        console.log(mensaje);
        return;
    }

    const toast = document.createElement("div");

    toast.className = `toast ${tipo}`;
    toast.textContent = mensaje;

    container.appendChild(toast);

    setTimeout(
        () => toast.remove(),
        4000
    );
}

/* =========================================================
   ERROR MOVIMIENTOS
========================================================= */
function mostrarErrorMovimientos(mensaje) {
    const container = document.getElementById("historial-movimientos");

    if (!container) {
        console.error(mensaje);
        return;
    }

    container.innerHTML = `
        <div class="empty-message">
            ${escapeHtml(mensaje)}
        </div>
    `;
}