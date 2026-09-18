"use strict";

const API_URL = "https://localhost:7204";

document.addEventListener("DOMContentLoaded", () => {
    document.body.classList.remove("page-loading");
    const userId = obtenerUserId();

    if (!userId) {
        window.location.href = "../login.html";
        return;
    }

    configurarTabs();
    configurarLogout();
    cargarUsuario();
    cargarMisPujas();
    cargarMisPublicaciones();
});

function obtenerUserId() {
    const valor = localStorage.getItem("userId");

    if (!valor) return null;

    const id = Number(valor);

    return Number.isInteger(id) && id > 0 ? id : null;
}

function configurarTabs() {
    const tabs = document.querySelectorAll(".dashboard-tab");
    const panels = document.querySelectorAll(".dashboard-tab-panel");

    tabs.forEach(tab => {
        tab.addEventListener("click", () => {
            const tabName = tab.dataset.tab;
            const panelId = `panel-${tabName}`;

            tabs.forEach(item => {
                item.classList.remove("dashboard-tab--active");
            });

            tab.classList.add("dashboard-tab--active");

            panels.forEach(panel => {
                panel.hidden = panel.id !== panelId;
            });
        });
    });
}

function configurarLogout() {
    const logout = document.getElementById("btn-logout");

    if (logout) {
        logout.addEventListener("click", cerrarSesion);
    }
}

async function cargarUsuario() {
    const element = document.getElementById("usuario-nombre");

    if (!element) return;

    const userId = obtenerUserId();

    if (!userId) {
        element.textContent = "Usuario";
        return;
    }

    try {
        const response = await fetch(`${API_URL}/api/users/${userId}`);

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }

        const usuario = await response.json();

        element.textContent =
            usuario.name ??
            usuario.nombre ??
            usuario.alias ??
            usuario.email ??
            "Usuario";
    } catch (error) {
        console.error("Error al cargar usuario:", error);
        element.textContent = "Usuario";
    }
}

async function cargarMisPujas() {
    const tbody = document.getElementById("tabla-mis-pujas");

    if (!tbody) return;

    const userId = obtenerUserId();

    if (!userId) {
        mostrarMensajeTabla(tbody, 6, "No se pudo identificar al usuario.");
        return;
    }

    mostrarMensajeTabla(tbody, 6, "Cargando...");

    try {
        const response = await fetch(`${API_URL}/api/users/${userId}/bids`);

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }

        const data = await response.json();

        const pujas = Array.isArray(data)
            ? data
            : data.items ?? [];

        renderizarMisPujas(pujas);
    } catch (error) {
        console.error("Error cargando mis pujas:", error);
        mostrarMensajeTabla(tbody, 6, "No se pudieron cargar tus pujas.");
    }
}

function renderizarMisPujas(pujas) {
    const tbody = document.getElementById("tabla-mis-pujas");

    if (!tbody) return;

    tbody.innerHTML = "";

    if (!Array.isArray(pujas) || pujas.length === 0) {
        mostrarMensajeTabla(tbody, 6, "Todavía no participaste en ninguna subasta.");
        return;
    }

    pujas.forEach(puja => {
        const tr = document.createElement("tr");

        const auctionId = puja.id;
        const titulo = puja.title ?? "Subasta";
        const miPuja = Number(puja.myBidAmount ?? 0);
        const pujaActual = Number(puja.currentBidAmount ?? 0);
        const estado = puja.status ?? "Sin estado";
        const fecha = puja.endDate ?? null;

        tr.innerHTML = `
            <td>
                <div class="auction-row">
                    <div class="auction-row__placeholder">SB</div>
                    <div class="auction-row__info">
                        <strong>${escapeHtml(titulo)}</strong>
                        <span>Subasta</span>
                    </div>
                </div>
            </td>
            <td>${formatearPrecio(miPuja)}</td>
            <td><strong>${formatearPrecio(pujaActual)}</strong></td>
            <td>${renderizarEstado(puja, true)}</td>
            <td>${fecha ? formatearFecha(fecha) : "-"}</td>
            <td>
                <a href="subasta.html?id=${auctionId}" class="dashboard-action">
                    Ver detalles <span>→</span>
                </a>
            </td>
        `;

        tbody.appendChild(tr);
    });
}

async function cargarMisPublicaciones() {
    const tbody = document.getElementById("tabla-mis-publicaciones");

    if (!tbody) return;

    const userId = obtenerUserId();

    if (!userId) {
        mostrarMensajeTabla(tbody, 6, "No se pudo identificar al usuario.");
        return;
    }

    mostrarMensajeTabla(tbody, 6, "Cargando...");

    try {
        const response = await fetch(`${API_URL}/api/users/${userId}/auctions`);

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }

        const data = await response.json();

        const publicaciones = Array.isArray(data)
            ? data
            : data.items ?? [];

        renderizarPublicaciones(publicaciones);
    } catch (error) {
        console.error("Error cargando mis publicaciones:", error);
        mostrarMensajeTabla(tbody, 6, "No se pudieron cargar tus publicaciones.");
    }
}

function renderizarPublicaciones(publicaciones) {
    const tbody = document.getElementById("tabla-mis-publicaciones");

    if (!tbody) return;

    tbody.innerHTML = "";

    if (!Array.isArray(publicaciones) || publicaciones.length === 0) {
        mostrarMensajeTabla(tbody, 6, "No tenés publicaciones.");
        return;
    }

    publicaciones.forEach(subasta => {
        const tr = document.createElement("tr");

        const id = subasta.id;
        const titulo = subasta.title ?? "Subasta";
        const precioBase = Number(subasta.basePrice ?? 0);
        const precioActual = Number(subasta.currentBidAmount ?? 0);
        const totalBids = Number(subasta.totalBids ?? 0);
        const estado = subasta.status ?? "Sin estado";
        const fecha = subasta.endDate ?? null;

        tr.innerHTML = `
            <td>
                <div class="auction-row">
                    <div class="auction-row__placeholder">SB</div>
                    <div class="auction-row__info">
                        <strong>${escapeHtml(titulo)}</strong>
                        <span>${totalBids} ${totalBids === 1 ? "puja" : "pujas"}</span>
                    </div>
                </div>
            </td>
            <td>${formatearPrecio(precioBase)}</td>
            <td><strong>${formatearPrecio(precioActual)}</strong></td>
            <td>${renderizarEstado({ status: estado })}</td>
            <td>${fecha ? formatearFecha(fecha) : "-"}</td>
            <td>
                <a href="subasta.html?id=${id}" class="dashboard-action">
                    Ver detalles <span>→</span>
                </a>
            </td>
        `;

        tbody.appendChild(tr);
    });
}

function renderizarEstado(item, esPuja = false) {
    const estado = String(item.status ?? "").toUpperCase();

    if (esPuja) {
        if (estado === "FINALIZADA") {
            return item.won
                ? `<span class="status status--won">Ganada</span>`
                : `<span class="status status--lost">Perdida</span>`;
        }

        if (estado === "ACTIVA") {
            return `<span class="status status--active">En curso</span>`;
        }

        if (estado === "PROGRAMADA") {
            return `<span class="status status--active">Próxima</span>`;
        }

        return `<span class="status">${escapeHtml(item.status ?? "Sin estado")}</span>`;
    }

    switch (estado) {
        case "ACTIVA":
            return `<span class="status status--active">En curso</span>`;
        case "PROGRAMADA":
            return `<span class="status status--active">Próxima</span>`;
        case "FINALIZADA":
            return `<span class="status status--won">Finalizada</span>`;
        case "DESIERTA":
            return `<span class="status status--lost">Desierta</span>`;
        default:
            return `<span class="status">${escapeHtml(item.status ?? "Sin estado")}</span>`;
    }
}

function mostrarMensajeTabla(tbody, columnas, mensaje) {
    tbody.innerHTML = `
        <tr>
            <td colspan="${columnas}">${escapeHtml(mensaje)}</td>
        </tr>
    `;
}

function cerrarSesion() {
    localStorage.removeItem("userId");
    localStorage.removeItem("token");
    localStorage.removeItem("accessToken");
    localStorage.removeItem("user");

    window.location.href = "../login.html";
}

function formatearFecha(fecha) {
    const date = new Date(fecha);

    if (Number.isNaN(date.getTime())) {
        return String(fecha ?? "-");
    }

    return date.toLocaleString("es-AR");
}

function formatearPrecio(valor) {
    return new Intl.NumberFormat("es-AR", {
        style: "currency",
        currency: "ARS"
    }).format(Number(valor) || 0);
}

function escapeHtml(texto) {
    const div = document.createElement("div");
    div.textContent = texto ?? "";
    return div.innerHTML;
}