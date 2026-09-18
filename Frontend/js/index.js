const API_URL = "https://localhost:7204";
let paginaActual = 1;
const PAGE_SIZE = 9;
let intervaloCountdown = null;

/* =========================================================
   INICIO
========================================================= */

document.addEventListener("DOMContentLoaded", () => {
    configurarEventos();
    cargarUsuario();
    cargarCategorias();
    cargarSubastas();
});

/* =========================================================
   EVENTOS
========================================================= */

function configurarEventos() {
    /* =====================================================
       BOTÓN BUSCAR
    ===================================================== */

    const boton = document.getElementById("btn-filtrar");

    if (boton) {
        boton.addEventListener("click", async () => {
            paginaActual = 1;
            await cargarSubastas();

            document.getElementById("subastas").scrollIntoView({
                behavior: "smooth",
                block: "start"
            });
        });
    }

    /* =====================================================
       BUSCAR CON ENTER
    ===================================================== */

    const busqueda = document.getElementById("filtro-busqueda");

    if (busqueda) {
        busqueda.addEventListener("keydown", async event => {
            if (event.key === "Enter") {
                paginaActual = 1;
                await cargarSubastas();

                document.getElementById("subastas").scrollIntoView({
                    behavior: "smooth",
                    block: "start"
                });
            }
        });
    }

    /* =====================================================
       ESTADO
    ===================================================== */

    const estado = document.getElementById("filtro-estado");

    if (estado) {
        estado.addEventListener("change", () => {
            paginaActual = 1;
            cargarSubastas();
        });
    }

    /* =====================================================
       CATEGORÍA
    ===================================================== */

    const categoria = document.getElementById("filtro-categoria");

    if (categoria) {
        categoria.addEventListener("change", () => {
            paginaActual = 1;
            cargarSubastas();
        });
    }

    /* =====================================================
       ORDENAMIENTO
    ===================================================== */

    const orden = document.getElementById("filtro-orden");

    if (orden) {
        orden.addEventListener("change", () => {
            paginaActual = 1;
            cargarSubastas();
        });
    }

    /* =====================================================
       PRECIO MÍNIMO
    ===================================================== */

    const precioMin = document.getElementById("filtro-precio-min");

    if (precioMin) {
        precioMin.addEventListener("keydown", event => {
            if (event.key === "Enter") {
                paginaActual = 1;
                cargarSubastas();
            }
        });
    }

    /* =====================================================
       PRECIO MÁXIMO
    ===================================================== */

    const precioMax = document.getElementById("filtro-precio-max");

    if (precioMax) {
        precioMax.addEventListener("keydown", event => {
            if (event.key === "Enter") {
                paginaActual = 1;
                cargarSubastas();
            }
        });
    }

    /* =====================================================
       LIMPIAR FILTROS
    ===================================================== */

    const limpiar = document.getElementById("btn-limpiar-filtros");

    if (limpiar) {
        limpiar.addEventListener("click", () => {

            const busquedaInput = document.getElementById("filtro-busqueda");

            const categoriaInput = document.getElementById("filtro-categoria");

            const estadoInput = document.getElementById("filtro-estado");

            const precioMinInput = document.getElementById("filtro-precio-min");

            const precioMaxInput = document.getElementById("filtro-precio-max");

            const ordenInput = document.getElementById("filtro-orden");

            if (busquedaInput) {
                busquedaInput.value = "";
            }

            if (categoriaInput) {
                categoriaInput.value = "";
            }

            if (estadoInput) {
                estadoInput.value = "";
            }

            if (precioMinInput) {
                precioMinInput.value = "";
            }

            if (precioMaxInput) {
                precioMaxInput.value = "";
            }

            if (ordenInput) {
                ordenInput.value = "";
            }

            paginaActual = 1;
            cargarSubastas();
        });
    }

    /* =====================================================
       LOGOUT
    ===================================================== */

    const logout = document.getElementById("btn-logout");

    if (logout) {
        logout.addEventListener(
            "click",
            cerrarSesion
        );
    }
}

/* =========================================================
   USUARIO
========================================================= */

async function cargarUsuario() {

    const userId = localStorage.getItem("userId");

    if (!userId) {
        return;
    }

    const element = document.getElementById("usuario-nombre");

    if (!element) {
        return;
    }

    try {
        const response = 
        await fetch(
            `${API_URL}/api/users/${userId}`
        );

        if (!response.ok) {
            console.warn( `No se pudo obtener el usuario. HTTP ${response.status}`);
            return;
        }

        const usuario = await response.json();

        element.textContent =
            usuario.name ||
            usuario.nombre ||
            usuario.email ||
            "Usuario";
    }
    catch (error) {
        console.error("Error al cargar usuario:", error);
    }
}

/* =========================================================
   CATEGORÍAS
========================================================= */

async function cargarCategorias() {

    const select = document.getElementById("filtro-categoria");

    if (!select) {
        return;
    }

    try {
        const response =
            await fetch(
                `${API_URL}/api/categories`
            );


        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }

        const data = await response.json();

        const categorias =
            Array.isArray(data)
                ? data
                : Array.isArray(data.items)
                    ? data.items
                    : Array.isArray(data.data)
                        ? data.data
                        : [];

        categorias.forEach(categoria => {
            const option = document.createElement("option");

            option.value = categoria.id;

            option.textContent =
                categoria.name ||
                categoria.nombre ||
                "Categoría";

            select.appendChild(option);
        });
    }
    catch (error) {
        console.error(
            "Error al cargar categorías:",
            error
        );
    }
}

/* =========================================================
   SUBASTAS
========================================================= */
async function cargarSubastas() {

    const container = document.getElementById("catalogo-subastas");

    if (!container) {
        return;
    }
    container.classList.add("is-loading");

    /* =====================================================
       OBTENER FILTROS
    ===================================================== */
    const search = document
            .getElementById("filtro-busqueda")
            ?.value
            .trim();

    const categoryId = document
            .getElementById("filtro-categoria")
            ?.value;

    const status = document
            .getElementById("filtro-estado")
            ?.value
            ?.trim() || "";

    const precioMinInput = document
            .getElementById("filtro-precio-min")
            ?.value;

    const precioMaxInput = document
            .getElementById("filtro-precio-max")
            ?.value;

    const orden = document
        .getElementById("filtro-orden")
        ?.value || "";

    const precioMin = precioMinInput !== ""
            ? Number(precioMinInput)
            : null;

    const precioMax = precioMaxInput !== ""
            ? Number(precioMaxInput)
            : null;

    /* =====================================================
       VALIDAR PRECIO
    ===================================================== */

    if (precioMin !== null && precioMax !== null &&
        (
            !Number.isFinite(precioMin) ||
            !Number.isFinite(precioMax) ||
            precioMin > precioMax
        )
    ) {
        container.innerHTML = `
            <p class="error-message">
                El precio mínimo no puede ser
                mayor que el precio máximo.
            </p>
        `
        return;
    }
    /* =====================================================
       PARÁMETROS
    ===================================================== */
    const params = new URLSearchParams();

    params.set("PageNumber", paginaActual);

    params.set("PageSize", PAGE_SIZE);

    if (search) {
        params.set("Title", search);
    }

    if (categoryId) {
        params.set("CategoryId", categoryId);
    }

    if (status) {
        params.set("Status", status);
    }

    try {
        /* =================================================
           OBTENER LISTADO
        ================================================= */
        const response =
            await fetch(
                `${API_URL}/api/auctions?${params.toString()}`
            );

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }

        const data = await response.json();

        const subastas = Array.isArray(data)
                ? data
                : Array.isArray(data.items)
                    ? data.items
                    : Array.isArray(data.data)
                        ? data.data
                        : [];

        const totalCount =
            data.totalCount ??
            data.total ??
            data.count ??
            subastas.length;

        /* =================================================
           OBTENER DETALLE Y PUJAS
        ================================================= */
        const subastasConPrecio = await Promise.all(
                subastas.map(
                    async subasta => {
                        const id =
                            subasta.id;

                        let basePrice = Number(
                                subasta.basePrice ??
                                subasta.precioBase ??
                                0
                            );

                        let mayorPuja = 0;

                        /* =================================
                           DETALLE
                        ================================= */
                        try {
                            const detalleResponse =
                                await fetch(
                                    `${API_URL}/api/auctions/${id}`
                                );

                            if (detalleResponse.ok) {
                                const detalle = await detalleResponse.json();

                                if (
                                    detalle.basePrice !==
                                    undefined &&
                                    detalle.basePrice !==
                                    null
                                ) {
                                    basePrice = Number(detalle.basePrice);
                                }
                                else if (
                                    detalle.precioBase !==
                                    undefined &&
                                    detalle.precioBase !==
                                    null
                                ) {
                                    basePrice = Number(detalle.precioBase);
                                }
                            }
                        }
                        catch (error) {
                            console.error(
                                `Error obteniendo detalle ${id}:`,
                                error
                            );
                        }
                        /* =================================
                           PUJAS
                        ================================= */
                        try {

                            const bidsResponse =
                                await fetch(
                                    `${API_URL}/api/auctions/${id}/bids?pageNumber=1&pageSize=100`
                                );

                            if (bidsResponse.ok) {
                                const bidsData = await bidsResponse.json();

                                const bids = Array.isArray(bidsData)
                                        ? bidsData
                                        : Array.isArray(
                                            bidsData.items
                                        )
                                            ? bidsData.items
                                            : Array.isArray(
                                                bidsData.data
                                            )
                                                ? bidsData.data
                                                : [];
                                for (
                                    const bid of bids
                                ) {
                                    const amount = Number(
                                            bid.amount ??
                                            bid.Amount ??
                                            bid.monto ??
                                            0
                                        );
                                    if (Number.isFinite(amount) && amount > mayorPuja) {
                                        mayorPuja = amount;
                                    }
                                }
                            }
                        }
                        catch (error) {
                            console.error(
                                `Error obteniendo bids ${id}:`,
                                error
                            );
                        }

                        /* =================================
                           PRECIO REAL
                        ================================= */

                        const precioReal = mayorPuja > 0
                                ? mayorPuja
                                : basePrice;

                        return {
                            ...subasta,

                            basePrice:
                                basePrice,

                            mayorPuja:
                                mayorPuja,

                            precioReal:
                                precioReal
                        };
                    }
                )
            );
        /* =================================================
           FILTRAR POR PRECIO
        ================================================= */
        let subastasFiltradas =
            subastasConPrecio.filter(
                subasta => {

                    const precio = Number(
                            subasta.precioReal ?? 0
                        );

                    if (precioMin !== null && precio < precioMin) {
                        return false;
                    }

                    if (precioMax !== null && precio > precioMax) {
                        return false;
                    }

                    return true;
                }
            );

        /* =================================================
           ORDENAR POR TIEMPO
        ================================================= */
if (orden === "tiempo") {

    const ahora = Date.now();

    subastasFiltradas = subastasFiltradas.filter(subasta => {

        const fechaFin = obtenerFechaFinalizacion(subasta);

        if (fechaFin === null) {
            return false;
        }

        return fechaFin > ahora;
    });

    subastasFiltradas.sort((a, b) => {

        const fechaA = obtenerFechaFinalizacion(a);

        const fechaB = obtenerFechaFinalizacion(b);

        return fechaA - fechaB;
    });
}

/* =================================================
   ORDENAR POR MAYOR PUJA
================================================= */
else if (orden === "puja") {

    subastasFiltradas.sort((a, b) => {

        const pujaA = Number(a.mayorPuja ?? 0);

        const pujaB = Number(b.mayorPuja ?? 0);

        if (pujaA > 0 && pujaB > 0) {
            return pujaB - pujaA;
        }

        if (pujaA > 0 && pujaB === 0) {
            return -1;
        }

        if (pujaA === 0 && pujaB > 0) {
            return 1;
        }

        const precioA = Number(a.basePrice ?? 0);

        const precioB = Number(b.basePrice ?? 0);

        return precioB - precioA;
    });
}
        /* =================================================
           RENDERIZAR
        ================================================= */
        detenerCountdownCards();
        container.classList.remove("is-loading");

        renderizarSubastas(subastasFiltradas);
        renderizarPaginacion(totalCount);

        iniciarCountdownCards();
    }
    catch (error) {
        console.error(
            "Error al cargar subastas:",
            error
        );
        
        container.innerHTML = `
        <p class="error-message">
        No se pudieron cargar las subastas.
        </p>
        `;

        container.classList.remove("is-loading");
        return;
    }
}

/* =========================================================
   OBTENER FECHA DE FINALIZACIÓN
========================================================= */
function obtenerFechaFinalizacion(subasta) {

    const fecha =
        subasta.endDate ??
        subasta.endTime ??
        subasta.fechaFinalizacion ??
        subasta.fechaFin;

    if (!fecha) {
        return null;
    }

    if (typeof fecha === "number") {
        return Number.isFinite(fecha)
            ? fecha
            : null;
    }

    const timestamp = new Date(fecha).getTime();

    return Number.isNaN(timestamp)
        ? null
        : timestamp;
}

/* =========================================================
   RENDER SUBASTAS
========================================================= */
function renderizarSubastas(subastas) {
    const container = document.getElementById("catalogo-subastas");

    if (!container) {
        return;
    }

    container.innerHTML = "";

    if (!subastas.length) {
        container.innerHTML = `
            <p class="empty-message">
                No se encontraron subastas.
            </p>
        `;
        return;
    }

    subastas.forEach(
        subasta => {
            const card = document.createElement("article");

            card.className = "auction-card";

            const id = subasta.id;

            const titulo = subasta.title || "Subasta";

            const categoria = subasta.categoryName || "Sin categoría";

            const totalPujas = Number(subasta.totalBids ?? 0);


            const imagen = typeof subasta.imageUrl === "string" && subasta.imageUrl.startsWith("http")
                    ? subasta.imageUrl
                    : "assets/placeholder.jpg";

            const precioReal = Number(subasta.precioReal);
            const precioBase = Number(subasta.basePrice ?? 0);

            const precio = Number.isFinite(precioReal)
                    ? precioReal
                    : precioBase;

            const estado = subasta.status || "Sin estado";
            const fecha = subasta.endDate;


            card.innerHTML = `
                <div class="auction-card__image">
                
                    <img
                        src="${escapeAttribute(imagen)}"
                        alt="${escapeAttribute(titulo)}"
                        onerror="this.style.display='none'"
                    >
                </div>

                <div class="auction-card__body">

                    <span class="auction-card__category">
                        ${escapeHtml(categoria)}
                    </span>

                    <h3>
                        ${escapeHtml(titulo)}
                    </h3>

                    <strong class="auction-card__price">
                        ${formatearPrecio(precio)}
                    </strong>

                    <div class="auction-card__meta">

                        <span class="auction-card__status status-${estado.toLowerCase()}"> 
                            ${escapeHtml(estado)}
                        </span>

                        <span class="auction-card__bids">
                            ${totalPujas} ${totalPujas === 1 ? "puja" : "pujas"}
                        </span>
                    </div>

                    <div class="auction-card__countdown"
                        ${estado === "ACTIVA" && fecha
                            ? `data-end-date="${escapeAttribute(fecha)}"`
                            : ""}
                    >
                        ${obtenerTextoCountdown(fecha, estado)}
                    </div>

                    <button
                        type="button"
                        class="btn-ver-subasta">

                        Ver subasta

                    </button>
                </div>
            `;

            const boton = card.querySelector(".btn-ver-subasta");

            if (boton) {
                boton.addEventListener(
                    "click",
                    () => {
                        window.location.href = `pages/subasta.html?id=${id}`;
                    }
                );
            }
            container.appendChild(card);
        }
    );
}

function obtenerTextoCountdown(fecha, estado) {

    if (!fecha) {
        return "Sin fecha";
    }

    if (estado === "FINALIZADA" || estado === "DESIERTA") {
        return "Subasta finalizada";
    }

    if (estado === "PROGRAMADA") {
        return "Próximamente";
    }

    const fechaFin = convertirFecha(fecha);

    if (!fechaFin) {
        return "Sin fecha";
    }

    const diferencia = fechaFin.getTime() - Date.now();

    if (diferencia <= 0) {
        return "Subasta finalizada";
    }

    const segundosTotales = Math.floor(diferencia / 1000);

    const dias = Math.floor(segundosTotales / 86400);
    const horas = Math.floor((segundosTotales % 86400) / 3600);
    const minutos = Math.floor((segundosTotales % 3600) / 60);
    const segundos = segundosTotales % 60;

    if (dias > 0) {
        return `Termina en ${dias}d ${horas}h ${minutos}m`;
    }

    return `Termina en ${String(horas).padStart(2, "0")}:${String(minutos).padStart(2, "0")}:${String(segundos).padStart(2, "0")}`;
}

function convertirFecha(fecha){
    if (!fecha) {
        return null;
    }

    if (typeof fecha === "number") {
        const timestamp = fecha < 10000000000
            ? fecha * 1000
            : fecha;

        return new Date(timestamp);
    }

    const fechaUtc = String(fecha).endsWith("Z")
        ? fecha
        : `${fecha}Z`;

    const date = new Date(fechaUtc);

    return Number.isNaN(date.getTime())
        ? null
        : date;
}

function actualizarCountdownCards() {

    const countdowns = document.querySelectorAll(".auction-card__countdown[data-end-date]");

    countdowns.forEach(countdown => {

        const fecha = countdown.dataset.endDate;

        const fechaFin = convertirFecha(fecha);

        if (!fechaFin) {
            countdown.textContent = "Sin fecha";
            return;
        }

        const diferencia = fechaFin.getTime() - Date.now();

        countdown.classList.remove(
            "countdown-warning",
            "countdown-critical"
        );

        if (diferencia <= 0) {
            countdown.textContent = "Subasta finalizada";
            return;
        }

        const segundosRestantes = Math.floor(diferencia / 1000);

        if (segundosRestantes <= 60) {
            countdown.classList.add("countdown-critical");
        }
        else if (segundosRestantes <= 300) {
            countdown.classList.add("countdown-warning");
        }

        const segundosTotales = Math.floor(diferencia / 1000);

        const dias = Math.floor(segundosTotales / 86400);

        const horas = Math.floor((segundosTotales % 86400) / 3600);

        const minutos = Math.floor((segundosTotales % 3600) / 60);

        const segundos = segundosTotales % 60;

        if (dias > 0) {
            countdown.textContent = `Termina en ${dias}d ${horas}h ${minutos}m`;
        }
        else {
            countdown.textContent =
                `Termina en ${
                    String(horas).padStart(2, "0")
                }:${
                    String(minutos).padStart(2, "0")
                }:${
                    String(segundos).padStart(2, "0")
                }`;
        }
    });
}

function iniciarCountdownCards() {

    detenerCountdownCards();

    actualizarCountdownCards();

    intervaloCountdown = setInterval(actualizarCountdownCards, 1000);
}

function detenerCountdownCards() {

    if (intervaloCountdown) {
        clearInterval(intervaloCountdown);
        intervaloCountdown = null;
    }
}
/* =========================================================
   PAGINACIÓN
========================================================= */

function renderizarPaginacion(totalCount) {

    const container = document.getElementById("paginacion");

    if (!container) {
        return;
    }

    container.innerHTML = "";

    const totalPages = Math.ceil(totalCount / PAGE_SIZE);

    if (totalPages <= 1) {
        return;
    }

    const anterior = document.createElement("button");

    anterior.textContent = "Anterior";

    anterior.disabled = paginaActual <= 1;

    anterior.addEventListener(
        "click",
        () => {
            if (paginaActual > 1) {
                paginaActual--;
                cargarSubastas();
            }
        }
    );

    container.appendChild(anterior);

    const indicador = document.createElement("span");

    indicador.id = "pagina-actual";

    indicador.textContent = `Página ${paginaActual} de ${totalPages}`;

    container.appendChild(indicador);

    const siguiente = document.createElement("button");

    siguiente.textContent = "Siguiente";

    siguiente.disabled = paginaActual >= totalPages;

    siguiente.addEventListener(
        "click",
        () => {
            if (paginaActual < totalPages) {
                paginaActual++;
                cargarSubastas();
            }
        }
    );

    container.appendChild(siguiente);
}

/* =========================================================
   SESIÓN
========================================================= */

function cerrarSesion() {
    localStorage.removeItem("userId");
    window.location.href = "login.html";
}

/* =========================================================
   FORMATEAR PRECIO
========================================================= */
function formatearPrecio(valor) {

    const numero = Number(valor);

    if (!Number.isFinite(numero)) {
        return "$ 0,00";
    }

    return new Intl.NumberFormat(
        "es-AR",
        {
            style: "currency",
            currency: "ARS"
        }
    ).format(numero);
}

/* =========================================================
   FORMATEAR FECHA
========================================================= */

function formatearFecha(fecha) {

    const date = new Date(fecha);

    if (Number.isNaN(date.getTime())) {
        return String(fecha);
    }

    return date.toLocaleString(
        "es-AR"
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
   ESCAPE ATRIBUTO
========================================================= */
function escapeAttribute(texto) {
    return String(texto ?? "")
        .replace(
            /&/g,
            "&amp;"
        )
        .replace(
            /"/g,
            "&quot;"
        )
        .replace(
            /</g,
            "&lt;"
        )
        .replace(
            />/g,
            "&gt;"
        );
}