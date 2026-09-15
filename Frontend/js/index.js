const API_URL = "https://localhost:7204";

document.addEventListener("DOMContentLoaded", async () => {
    await cargarCategorias();
    await cargarSubastas();
});

/* =========================
   CATEGORÍAS
========================= */

async function cargarCategorias() {
    const filtroCategoria = document.getElementById("filtro-categoria");
    const categoriaFormulario = document.getElementById("categoria");

    try {
        const response = await fetch(`${API_URL}/api/categories`);

        if (!response.ok) {
            throw new Error(`Error HTTP: ${response.status}`);
        }

        const categorias = await response.json();

        console.log("Categorías:", categorias);

        // Limpiamos las opciones existentes,
        // conservando la opción inicial.
        filtroCategoria.innerHTML =
            `<option value="">Todas las categorías</option>`;

        categoriaFormulario.innerHTML =
            `<option value="">Seleccionar categoría</option>`;

        categorias.forEach(categoria => {
            const optionFiltro = document.createElement("option");
            optionFiltro.value = categoria.id;
            optionFiltro.textContent = categoria.name;

            filtroCategoria.appendChild(optionFiltro);

            const optionFormulario = document.createElement("option");
            optionFormulario.value = categoria.id;
            optionFormulario.textContent = categoria.name;

            categoriaFormulario.appendChild(optionFormulario);
        });

    } catch (error) {
        console.error("Error cargando categorías:", error);
        mostrarToast("No se pudieron cargar las categorías.", "error");
    }
}


/* =========================
   SUBASTAS
========================= */

async function cargarSubastas() {

    const container = document.getElementById("subastas-container");

    try {

        const response = await fetch(`${API_URL}/api/auctions`);

        if (!response.ok) {
            throw new Error(`Error HTTP: ${response.status}`);
        }

        const resultado = await response.json();

        console.log("Subastas:", resultado);

        const subastas = resultado.items ?? resultado.Items ?? resultado;

        renderizarSubastas(subastas);

    } catch (error) {

        console.error("Error cargando subastas:", error);

        container.innerHTML = `
            <p>
                No se pudieron cargar las subastas.
            </p>
        `;

        mostrarToast("No se pudieron cargar las subastas.", "error");
    }
}


/* =========================
   RENDERIZAR SUBASTAS
========================= */

function renderizarSubastas(subastas) {

    const container = document.getElementById("subastas-container");

    container.innerHTML = "";

    if (!subastas || subastas.length === 0) {

        container.innerHTML = `
            <p>No hay subastas disponibles.</p>
        `;

        return;
    }

    subastas.forEach(subasta => {

        const card = document.createElement("article");

        card.className = "auction-card";

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

        card.innerHTML = `
            <img
                class="auction-card__image"
                src="${subasta.imageUrl || ""}"
                alt="${subasta.title}"
            >

            <div class="auction-card__body">

                <span class="auction-card__category">
                    ${categoryName}
                </span>

                <h3>${subasta.title}</h3>

                <p>
                    ${subasta.description ?? ""}
                </p>

                <div class="auction-card__price">
                    ${formatearPrecio(currentBid)}
                </div>

                <div class="auction-card__bids">
                    Pujas: ${totalBids}
                </div>

                <div class="auction-card__timer">
                    ${subasta.status ?? ""}
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
            throw new Error(`Error HTTP: ${response.status}`);
        }

        const subasta = await response.json();

        console.log("Subasta:", subasta);

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

        console.error("Error obteniendo subasta:", error);

        mostrarToast(
            "No se pudo obtener la información de la subasta.",
            "error"
        );
    }
}


/* =========================
   FORMATEAR PRECIO
========================= */

function formatearPrecio(valor) {

    return new Intl.NumberFormat("es-AR", {
        style: "currency",
        currency: "ARS",
        minimumFractionDigits: 0
    }).format(valor ?? 0);
}


/* =========================
   FORMATEAR FECHA
========================= */

function formatearFecha(fecha) {

    if (!fecha) {
        return "Sin fecha";
    }

    return new Date(fecha).toLocaleString("es-AR", {
        dateStyle: "short",
        timeStyle: "short"
    });
}


/* =========================
   TOAST
========================= */

function mostrarToast(mensaje, tipo = "success") {

    const container = document.getElementById("toast-container");

    if (!container) {
        return;
    }

    const toast = document.createElement("div");

    toast.className = `toast ${tipo}`;

    toast.textContent = mensaje;

    container.appendChild(toast);

    setTimeout(() => {
        toast.remove();
    }, 4000);
}
