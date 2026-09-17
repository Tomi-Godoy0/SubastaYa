const API_URL = "https://localhost:7204";

const userId = localStorage.getItem("userId");

const params =
    new URLSearchParams(window.location.search);

const auctionId =
    Number(params.get("id"));


let paginaPujas = 1;

const pageSizePujas = 10;


let subastaActual = null;


/* =========================================================
   TEMPORIZADOR / ACTUALIZACIÓN EN VIVO
========================================================= */

let intervaloTemporizador = null;

let intervaloActualizacion = null;


/*
 * Short-Polling:
 *
 * Cada 2 segundos consultamos nuevamente
 * la subasta y sus pujas.
 *
 * Esto permite que:
 *
 * - se actualice la puja actual
 * - aparezcan nuevas pujas
 * - se actualice el estado
 * - se mantenga sincronizado el temporizador
 */

const INTERVALO_POLLING = 2000;


/* =========================================================
   INICIO
========================================================= */

document.addEventListener("DOMContentLoaded", () => {

    configurarEventos();

    cargarUsuario();


    if (!auctionId || auctionId <= 0) {

        mostrarError(
            "No se especificó una subasta."
        );

        return;

    }


    cargarSubasta();

    cargarHistorialPujas();


    /*
     * Iniciamos la actualización automática.
     */
    iniciarActualizacionEnVivo();

});


/* =========================================================
   EVENTOS
========================================================= */

function configurarEventos() {

    const logout =
        document.getElementById("btn-logout");


    if (logout) {

        logout.addEventListener(
            "click",
            cerrarSesion
        );

    }

}


/* =========================================================
   ACTUALIZACIÓN EN VIVO
========================================================= */

function iniciarActualizacionEnVivo() {

    detenerActualizacionEnVivo();


    /*
     * Actualiza el cronómetro cada segundo.
     */
    intervaloTemporizador =
        setInterval(
            actualizarTemporizador,
            1000
        );


    /*
     * Short-Polling cada 2 segundos.
     *
     * No recargamos toda la página.
     * Solamente actualizamos los datos.
     */
    intervaloActualizacion =
        setInterval(
            actualizarDatosEnVivo,
            INTERVALO_POLLING
        );

}


/* =========================================================
   DETENER ACTUALIZACIÓN EN VIVO
========================================================= */

function detenerActualizacionEnVivo() {

    if (intervaloTemporizador) {

        clearInterval(
            intervaloTemporizador
        );

        intervaloTemporizador = null;

    }


    if (intervaloActualizacion) {

        clearInterval(
            intervaloActualizacion
        );

        intervaloActualizacion = null;

    }

}


/* =========================================================
   ACTUALIZAR DATOS EN VIVO
========================================================= */

async function actualizarDatosEnVivo() {

    /*
     * Si no tenemos la subasta todavía,
     * no hacemos nada.
     */
    if (!auctionId || auctionId <= 0) {
        return;
    }


    try {

        /*
         * Obtenemos nuevamente la subasta.
         */
        const response =
            await fetch(
                `${API_URL}/api/auctions/${auctionId}`
            );


        if (!response.ok) {
            return;
        }


        const subasta =
            await response.json();


        /*
         * Obtenemos las pujas actuales.
         */
        const bids =
            await obtenerTodasLasPujas();


        let mayorPuja = 0;


        for (const puja of bids) {

            const amount =
                Number(
                    puja.amount ??
                    puja.Amount ??
                    puja.monto ??
                    puja.Monto ??
                    0
                );


            if (
                Number.isFinite(amount) &&
                amount > mayorPuja
            ) {

                mayorPuja = amount;

            }

        }


        const basePrice =
            Number(
                subasta.basePrice ??
                subasta.BasePrice ??
                0
            );


        const precioActual =
            mayorPuja > 0
                ? mayorPuja
                : basePrice;


        /*
         * Guardamos la información actualizada.
         */
        subastaActual = {

            ...subasta,

            precioActual,

            mayorPuja,

            totalBids: bids.length

        };


        /*
         * Actualizamos visualmente los datos
         * sin recargar la página completa.
         */
        actualizarDatosVisuales(
            subastaActual
        );


        /*
         * Actualizamos el historial de pujas.
         */
        cargarHistorialPujas();


        /*
         * Actualizamos el temporizador.
         */
        actualizarTemporizador();


    }
    catch (error) {

        console.error(
            "Error actualizando datos en vivo:",
            error
        );

    }

}


/* =========================================================
   USUARIO
========================================================= */

async function cargarUsuario() {

    if (!userId) {
        return;
    }


    const element =
        document.getElementById(
            "usuario-nombre"
        );


    if (!element) {
        return;
    }


    try {

        const response =
            await fetch(
                `${API_URL}/api/users/${userId}`
            );


        if (!response.ok) {
            return;
        }


        const usuario =
            await response.json();


        element.textContent =
            usuario.name ||
            usuario.nombre ||
            usuario.email ||
            "Usuario";

    }
    catch (error) {

        console.error(
            "Error cargando usuario:",
            error
        );

    }

}


/* =========================================================
   CARGAR SUBASTA
========================================================= */

async function cargarSubasta() {

    const container =
        document.getElementById(
            "detalle-subasta"
        );


    if (!container) {
        return;
    }


    try {

        const response =
            await fetch(
                `${API_URL}/api/auctions/${auctionId}`
            );


        if (!response.ok) {

            throw new Error(
                `No se pudo cargar la subasta. HTTP ${response.status}`
            );

        }


        const subasta =
            await response.json();


        console.log(
            "SUBASTA RECIBIDA:",
            subasta
        );


        /*
         * Obtenemos las pujas.
         */
        const bids =
            await obtenerTodasLasPujas();


        let mayorPuja = 0;


        for (const puja of bids) {

            const amount =
                Number(
                    puja.amount ??
                    puja.Amount ??
                    puja.monto ??
                    puja.Monto ??
                    0
                );


            if (
                Number.isFinite(amount) &&
                amount > mayorPuja
            ) {

                mayorPuja = amount;

            }

        }


        const basePrice =
            Number(
                subasta.basePrice ??
                subasta.BasePrice ??
                0
            );


        const precioActual =
            mayorPuja > 0
                ? mayorPuja
                : basePrice;


        subastaActual = {

            ...subasta,

            precioActual,

            mayorPuja,

            totalBids: bids.length

        };


        renderizarSubasta(
            subastaActual
        );


        /*
         * Iniciamos el temporizador inmediatamente.
         */
        actualizarTemporizador();

    }
    catch (error) {

        console.error(
            "Error cargando subasta:",
            error
        );


        mostrarError(
            error.message ||
            "No se pudo cargar la subasta."
        );

    }

}


/* =========================================================
   OBTENER PUJAS
========================================================= */

async function obtenerTodasLasPujas() {

    try {

        const response =
            await fetch(
                `${API_URL}/api/auctions/${auctionId}/bids?PageNumber=1&PageSize=100`
            );


        if (!response.ok) {

            console.warn(
                "No se pudieron obtener las pujas.",
                response.status
            );

            return [];

        }


        const data =
            await response.json();


        if (Array.isArray(data)) {
            return data;
        }


        if (Array.isArray(data.items)) {
            return data.items;
        }


        if (Array.isArray(data.Items)) {
            return data.Items;
        }


        if (Array.isArray(data.data)) {
            return data.data;
        }


        if (Array.isArray(data.Data)) {
            return data.Data;
        }


        if (Array.isArray(data.results)) {
            return data.results;
        }


        return [];

    }
    catch (error) {

        console.error(
            "Error obteniendo pujas:",
            error
        );


        return [];

    }

}


/* =========================================================
   DETALLE SUBASTA
========================================================= */

function renderizarSubasta(subasta) {

    const container =
        document.getElementById(
            "detalle-subasta"
        );


    if (!container) {
        return;
    }


    const titulo =
        subasta.title ??
        subasta.Title ??
        "Subasta";


    const descripcion =
        subasta.description ??
        subasta.Description ??
        "";


    const imagen =
        subasta.imageUrl ??
        subasta.ImageUrl ??
        "";


    const basePrice =
        Number(
            subasta.basePrice ??
            subasta.BasePrice ??
            0
        );


    const increment =
        Number(
            subasta.minimumIncrement ??
            subasta.MinimumIncrement ??
            0
        );


    const currentBid =
        Number(
            subasta.precioActual ??
            subasta.currentBidAmount ??
            subasta.CurrentBidAmount ??
            basePrice
        );


    const totalBids =
        Number(
            subasta.totalBids ??
            subasta.TotalBids ??
            0
        );


    const status =
        subasta.status ??
        subasta.Status ??
        "Sin estado";


    const seller =
        subasta.sellerName ??
        subasta.SellerName ??
        "-";


    const category =
        subasta.categoryName ??
        subasta.CategoryName ??
        "-";


    const endDate =
        obtenerFechaFinalizacion(
            subasta
        );


    const puedePujar =
        esSubastaActiva(status) &&
        endDate !== null &&
        endDate > Date.now();


    container.innerHTML = `

        <article class="auction-detail-card">

            ${
                imagen
                    ? `
                        <div class="auction-detail-card__image">

                            <img
                                src="${escapeAttribute(imagen)}"
                                alt="${escapeAttribute(titulo)}"
                                onerror="this.style.display='none'"
                            >

                        </div>
                    `
                    : ""
            }


            <div class="auction-detail-card__content">

                <h1>
                    ${escapeHtml(titulo)}
                </h1>


                <p>
                    ${escapeHtml(descripcion)}
                </p>


                <!-- =========================================
                     TEMPORIZADOR EN VIVO
                ========================================== -->

                <div
                    id="temporizador-subasta"
                    class="auction-timer">

                    <span class="auction-timer__label">
                        Tiempo restante
                    </span>

                    <strong
                        id="contador-subasta"
                        class="auction-timer__clock">

                        --:--:--

                    </strong>

                    <span
                        id="mensaje-temporizador"
                        class="auction-timer__message">

                    </span>

                </div>


                <div class="auction-info">

                    <div>

                        <span>
                            Precio base
                        </span>

                        <strong
                            id="precio-base">

                            ${formatearPrecio(basePrice)}

                        </strong>

                    </div>


                    <div>

                        <span>
                            Puja actual
                        </span>

                        <strong
                            id="puja-actual">

                            ${formatearPrecio(currentBid)}

                        </strong>

                    </div>


                    <div>

                        <span>
                            Incremento mínimo
                        </span>

                        <strong>

                            ${formatearPrecio(increment)}

                        </strong>

                    </div>


                    <div>

                        <span>
                            Pujas
                        </span>

                        <strong
                            id="cantidad-pujas">

                            ${totalBids}

                        </strong>

                    </div>

                </div>


                <p>

                    <strong>
                        Estado:
                    </strong>

                    <span id="estado-subasta">
                        ${escapeHtml(status)}
                    </span>

                </p>


                <p>

                    <strong>
                        Categoría:
                    </strong>

                    ${escapeHtml(category)}

                </p>


                <p>

                    <strong>
                        Vendedor:
                    </strong>

                    ${escapeHtml(seller)}

                </p>


                <p>

                    <strong>
                        Finaliza:
                    </strong>

                    <span id="fecha-finalizacion">

                        ${
                            endDate
                                ? formatearFecha(endDate)
                                : "-"
                        }

                    </span>

                </p>


                <div id="zona-formulario-puja">

                    ${
                        puedePujar
                            ? renderizarFormularioPuja(
                                currentBid,
                                increment
                            )
                            : `
                                <div class="auction-closed">

                                    Esta subasta no está disponible
                                    para nuevas pujas.

                                </div>
                            `
                    }

                </div>

            </div>

        </article>

    `;


    const form =
        document.getElementById(
            "form-puja"
        );


    if (form) {

        form.addEventListener(
            "submit",
            realizarPuja
        );

    }


    /*
     * Actualizamos inmediatamente
     * el temporizador.
     */
    actualizarTemporizador();

}


/* =========================================================
   ACTUALIZAR DATOS VISUALES
========================================================= */

function actualizarDatosVisuales(subasta) {

    if (!subasta) {
        return;
    }


    const currentBid =
        Number(
            subasta.precioActual ??
            subasta.currentBidAmount ??
            subasta.CurrentBidAmount ??
            subasta.basePrice ??
            0
        );


    const totalBids =
        Number(
            subasta.totalBids ??
            subasta.TotalBids ??
            0
        );


    const status =
        subasta.status ??
        subasta.Status ??
        "Sin estado";


    /*
     * Actualizamos puja actual.
     */
    const pujaActual =
        document.getElementById(
            "puja-actual"
        );


    if (pujaActual) {

        pujaActual.textContent =
            formatearPrecio(
                currentBid
            );

    }


    /*
     * Actualizamos cantidad de pujas.
     */
    const cantidadPujas =
        document.getElementById(
            "cantidad-pujas"
        );


    if (cantidadPujas) {

        cantidadPujas.textContent =
            totalBids;

    }


    /*
     * Actualizamos estado.
     */
    const estado =
        document.getElementById(
            "estado-subasta"
        );


    if (estado) {

        estado.textContent =
            status;

    }


    /*
     * Actualizamos el formulario de puja.
     */
    const zonaFormulario =
        document.getElementById(
            "zona-formulario-puja"
        );


    if (zonaFormulario) {

        const endDate =
            obtenerFechaFinalizacion(
                subasta
            );


        const puedePujar =
            esSubastaActiva(status) &&
            endDate !== null &&
            endDate > Date.now();


        /*
         * Si la subasta sigue activa,
         * mantenemos el formulario.
         */
        if (puedePujar) {

            const formActual =
                document.getElementById(
                    "form-puja"
                );


            if (formActual) {

                actualizarFormularioPuja(
                    currentBid,
                    Number(
                        subasta.minimumIncrement ??
                        subasta.MinimumIncrement ??
                        0
                    )
                );

            }
            else {

                zonaFormulario.innerHTML =
                    renderizarFormularioPuja(
                        currentBid,
                        Number(
                            subasta.minimumIncrement ??
                            subasta.MinimumIncrement ??
                            0
                        )
                    );


                const nuevoForm =
                    document.getElementById(
                        "form-puja"
                    );


                if (nuevoForm) {

                    nuevoForm.addEventListener(
                        "submit",
                        realizarPuja
                    );

                }

            }

        }
        else {

            zonaFormulario.innerHTML = `

                <div class="auction-closed">

                    Esta subasta no está disponible
                    para nuevas pujas.

                </div>

            `;

        }

    }

}


/* =========================================================
   ACTUALIZAR FORMULARIO DE PUJA
========================================================= */

function actualizarFormularioPuja(
    currentBid,
    increment
) {

    const input =
        document.getElementById(
            "monto-puja"
        );


    const minimo =
        calcularMontoMinimo(
            currentBid,
            increment
        );


    if (input) {

        /*
         * No pisamos el valor si el usuario
         * está escribiendo.
         */
        if (
            document.activeElement !== input
        ) {

            input.min =
                minimo;

            input.value =
                minimo;

        }
        else {

            input.min =
                minimo;

        }

    }


    const small =
        document.querySelector(
            "#form-puja small"
        );


    if (small) {

        small.textContent =
            `Monto mínimo: ${formatearPrecio(minimo)}`;

    }

}


/* =========================================================
   CALCULAR MONTO MÍNIMO
========================================================= */

function calcularMontoMinimo(
    currentBid,
    increment
) {

    const basePrice =
        Number(
            subastaActual?.basePrice ??
            subastaActual?.BasePrice ??
            0
        );


    const pujaActual =
        Number(
            currentBid ?? 0
        );


    const incremento =
        Number(increment) || 0;


    if (pujaActual > basePrice) {

        return (
            pujaActual +
            incremento
        );

    }


    return (
        basePrice +
        incremento
    );

}


/* =========================================================
   TEMPORIZADOR
========================================================= */

function actualizarTemporizador() {

    const contador =
        document.getElementById(
            "contador-subasta"
        );


    const temporizador =
        document.getElementById(
            "temporizador-subasta"
        );


    const mensaje =
        document.getElementById(
            "mensaje-temporizador"
        );


    if (
        !contador ||
        !temporizador
    ) {

        return;

    }


    if (!subastaActual) {

        contador.textContent =
            "--:--:--";

        return;

    }


    const fechaFin =
        obtenerFechaFinalizacion(
            subastaActual
        );


    if (fechaFin === null) {

        contador.textContent =
            "Sin fecha";

        temporizador.classList.remove(
            "auction-timer--critical"
        );

        temporizador.classList.remove(
            "auction-timer--warning"
        );

        if (mensaje) {

            mensaje.textContent =
                "";

        }

        return;

    }


    const ahora =
        Date.now();


    let diferencia =
        fechaFin -
        ahora;


    /*
     * ==============================================
     * SUBASTA FINALIZADA
     * ==============================================
     */

    if (diferencia <= 0) {

        contador.textContent =
            "00:00:00";


        temporizador.classList.remove(
            "auction-timer--warning"
        );


        temporizador.classList.add(
            "auction-timer--critical"
        );


        if (mensaje) {

            mensaje.textContent =
                "La subasta ha finalizado.";

        }


        /*
         * Deshabilitamos el formulario.
         */
        const zonaFormulario =
            document.getElementById(
                "zona-formulario-puja"
            );


        if (zonaFormulario) {

            zonaFormulario.innerHTML = `

                <div class="auction-closed">

                    Esta subasta ha finalizado.
                    Ya no se aceptan nuevas pujas.

                </div>

            `;

        }


        /*
         * Ya no necesitamos seguir
         * ejecutando el temporizador.
         */
        if (intervaloTemporizador) {

            clearInterval(
                intervaloTemporizador
            );

            intervaloTemporizador = null;

        }


        return;

    }


    /*
     * ==============================================
     * CALCULAR TIEMPO
     * ==============================================
     */

    const totalSegundos =
        Math.floor(
            diferencia / 1000
        );


    const dias =
        Math.floor(
            totalSegundos / 86400
        );


    const horas =
        Math.floor(
            (totalSegundos % 86400) /
            3600
        );


    const minutos =
        Math.floor(
            (totalSegundos % 3600) /
            60
        );


    const segundos =
        totalSegundos % 60;


    /*
     * ==============================================
     * FORMATO
     * ==============================================
     */

    let texto;


    if (dias > 0) {

        texto =
            `${dias}d ${pad(horas)}:${pad(minutos)}:${pad(segundos)}`;

    }
    else {

        texto =
            `${pad(horas)}:${pad(minutos)}:${pad(segundos)}`;

    }


    contador.textContent =
        texto;


    /*
     * ==============================================
     * ZONA NORMAL
     * ==============================================
     */

    temporizador.classList.remove(
        "auction-timer--warning"
    );


    temporizador.classList.remove(
        "auction-timer--critical"
    );


    /*
     * ==============================================
     * ÚLTIMO MINUTO
     * ==============================================
     */

    if (diferencia <= 60000) {

        temporizador.classList.add(
            "auction-timer--critical"
        );


        if (mensaje) {

            mensaje.textContent =
                "¡Atención! Queda menos de un minuto.";

        }

    }


    /*
     * ==============================================
     * ENTRE 1 Y 5 MINUTOS
     * ==============================================
     */

    else if (diferencia <= 300000) {

        temporizador.classList.add(
            "auction-timer--warning"
        );


        if (mensaje) {

            mensaje.textContent =
                "La subasta está por finalizar.";

        }

    }


    else {

        if (mensaje) {

            mensaje.textContent =
                "";

        }

    }

}


/* =========================================================
   PAD
========================================================= */

function pad(numero) {

    return String(numero)
        .padStart(
            2,
            "0"
        );

}


/* =========================================================
   OBTENER FECHA DE FINALIZACIÓN
========================================================= */

function obtenerFechaFinalizacion(
    subasta
) {

    if (!subasta) {
        return null;
    }


    const fecha =
        subasta.endDate ??
        subasta.EndDate ??
        subasta.endTime ??
        subasta.EndTime ??
        subasta.fechaFinalizacion ??
        subasta.fechaFin ??
        subasta.endingAt ??
        subasta.endDateTime ??
        subasta.EndDateTime;


    if (!fecha) {
        return null;
    }


    if (
        typeof fecha === "number"
    ) {

        /*
         * Si el backend devuelve Unix
         * en segundos.
         */
        if (
            fecha < 10000000000
        ) {

            return fecha * 1000;

        }


        return Number.isFinite(fecha)
            ? fecha
            : null;

    }


    const timestamp =
        new Date(fecha).getTime();


    return Number.isNaN(timestamp)
        ? null
        : timestamp;

}


/* =========================================================
   FORMULARIO PUJA
========================================================= */

function renderizarFormularioPuja(
    currentBid,
    increment
) {

    const minimo =
        calcularMontoMinimo(
            currentBid,
            increment
        );


    return `

        <form
            id="form-puja"
            class="bid-form"
        >

            <h3>
                Realizar una puja
            </h3>


            <label for="monto-puja">
                Monto
            </label>


            <input
                type="number"
                id="monto-puja"
                min="${minimo}"
                step="0.01"
                value="${minimo}"
                required
            >


            <small>

                Monto mínimo:
                ${formatearPrecio(minimo)}

            </small>


            <button
                type="submit">

                Pujar

            </button>

        </form>

    `;

}


/* =========================================================
   REALIZAR PUJA
========================================================= */

async function realizarPuja(event) {

    event.preventDefault();


    if (!userId) {

        mostrarToast(
            "Debés iniciar sesión para pujar.",
            "error"
        );

        return;

    }


    /*
     * Verificación adicional del tiempo
     * antes de enviar la puja.
     */
    const fechaFin =
        obtenerFechaFinalizacion(
            subastaActual
        );


    if (
        fechaFin === null ||
        fechaFin <= Date.now()
    ) {

        mostrarToast(
            "La subasta ya finalizó.",
            "error"
        );


        actualizarTemporizador();

        return;

    }


    const input =
        document.getElementById(
            "monto-puja"
        );


    if (!input) {
        return;
    }


    const amount =
        Number(input.value);


    if (
        !Number.isFinite(amount) ||
        amount <= 0
    ) {

        mostrarToast(
            "Ingresá un monto válido.",
            "error"
        );

        return;

    }


    const command = {

        auctionId: auctionId,

        buyerId: Number(userId),

        amount: amount

    };


    try {

        const response =
            await fetch(
                `${API_URL}/api/auctions/${auctionId}/bids`,
                {
                    method: "POST",

                    headers: {
                        "Content-Type": "application/json"
                    },

                    body: JSON.stringify(command)
                }
            );


        const data =
            await leerRespuesta(
                response
            );


        if (!response.ok) {

            throw new Error(
                data?.message ||
                data?.title ||
                data?.detail ||
                "No se pudo realizar la puja."
            );

        }


        mostrarToast(
            "Puja realizada correctamente.",
            "success"
        );


        paginaPujas = 1;


        /*
         * Actualizamos inmediatamente.
         */
        await actualizarDatosEnVivo();


    }
    catch (error) {

        console.error(
            "Error realizando puja:",
            error
        );


        mostrarToast(
            error.message ||
            "Error realizando la puja.",
            "error"
        );

    }

}


/* =========================================================
   HISTORIAL PUJAS
========================================================= */

async function cargarHistorialPujas() {

    const tbody =
        document.getElementById(
            "tabla-pujas"
        );


    if (!tbody) {
        return;
    }


    if (
        !auctionId ||
        auctionId <= 0
    ) {

        return;

    }


    try {

        const response =
            await fetch(
                `${API_URL}/api/auctions/${auctionId}/bids?PageNumber=${paginaPujas}&PageSize=${pageSizePujas}`
            );


        if (!response.ok) {

            throw new Error(
                `HTTP ${response.status}`
            );

        }


        const data =
            await response.json();


        const pujas =
            obtenerItems(data);


        const totalCount =
            Number(
                data.totalCount ??
                data.TotalCount ??
                data.total ??
                data.Total ??
                pujas.length
            );


        renderizarPujas(
            pujas
        );


        renderizarPaginacionPujas(
            totalCount
        );

    }
    catch (error) {

        console.error(
            "Error cargando historial:",
            error
        );


        /*
         * No borramos el historial anterior
         * si falla una consulta de polling.
         */
        if (
            !tbody.children.length
        ) {

            tbody.innerHTML = `

                <tr>

                    <td colspan="3">

                        No se pudo cargar el historial.

                    </td>

                </tr>

            `;

        }

    }

}


/* =========================================================
   OBTENER ITEMS
========================================================= */

function obtenerItems(data) {

    if (Array.isArray(data)) {
        return data;
    }


    if (Array.isArray(data.items)) {
        return data.items;
    }


    if (Array.isArray(data.Items)) {
        return data.Items;
    }


    if (Array.isArray(data.data)) {
        return data.data;
    }


    if (Array.isArray(data.Data)) {
        return data.Data;
    }


    if (Array.isArray(data.results)) {
        return data.results;
    }


    return [];

}


/* =========================================================
   RENDER PUJAS
========================================================= */

function renderizarPujas(
    pujas
) {

    const tbody =
        document.getElementById(
            "tabla-pujas"
        );


    if (!tbody) {
        return;
    }


    tbody.innerHTML = "";


    if (!pujas.length) {

        tbody.innerHTML = `

            <tr>

                <td colspan="3">

                    Todavía no hay pujas.

                </td>

            </tr>

        `;

        return;

    }


    pujas.forEach(
        puja => {

            const tr =
                document.createElement(
                    "tr"
                );


            const alias =
                puja.alias ??
                puja.Alias ??
                puja.userName ??
                puja.UserName ??
                puja.buyerName ??
                puja.BuyerName ??
                puja.email ??
                puja.Email ??
                "-";


            const amount =
                Number(
                    puja.amount ??
                    puja.Amount ??
                    puja.monto ??
                    puja.Monto ??
                    0
                );


            const createdAt =
                puja.createdAt ??
                puja.CreatedAt ??
                puja.createdDate ??
                puja.CreatedDate ??
                puja.date ??
                puja.Date ??
                null;


            tr.innerHTML = `

                <td>
                    ${escapeHtml(alias)}
                </td>


                <td>
                    ${formatearPrecio(amount)}
                </td>


                <td>

                    ${
                        createdAt
                            ? formatearFecha(
                                createdAt
                            )
                            : "-"
                    }

                </td>

            `;


            tbody.appendChild(
                tr
            );

        }
    );

}


/* =========================================================
   PAGINACIÓN PUJAS
========================================================= */

function renderizarPaginacionPujas(
    totalCount
) {

    const container =
        document.getElementById(
            "paginacion-pujas"
        );


    if (!container) {
        return;
    }


    container.innerHTML = "";


    const totalPages =
        Math.ceil(
            totalCount /
            pageSizePujas
        );


    if (totalPages <= 1) {
        return;
    }


    const anterior =
        document.createElement(
            "button"
        );


    anterior.type =
        "button";


    anterior.textContent =
        "Anterior";


    anterior.disabled =
        paginaPujas <= 1;


    anterior.addEventListener(
        "click",
        () => {

            if (
                paginaPujas > 1
            ) {

                paginaPujas--;

                cargarHistorialPujas();

            }

        }
    );


    container.appendChild(
        anterior
    );


    const pagina =
        document.createElement(
            "span"
        );


    pagina.textContent =
        `Página ${paginaPujas} de ${totalPages}`;


    container.appendChild(
        pagina
    );


    const siguiente =
        document.createElement(
            "button"
        );


    siguiente.type =
        "button";


    siguiente.textContent =
        "Siguiente";


    siguiente.disabled =
        paginaPujas >= totalPages;


    siguiente.addEventListener(
        "click",
        () => {

            if (
                paginaPujas <
                totalPages
            ) {

                paginaPujas++;

                cargarHistorialPujas();

            }

        }
    );


    container.appendChild(
        siguiente
    );

}


/* =========================================================
   ESTADO SUBASTA
========================================================= */

function esSubastaActiva(
    status
) {

    const value =
        String(status)
            .toLowerCase()
            .trim();


    return (

        value === "active" ||

        value === "activa" ||

        value === "activo" ||

        value === "live" ||

        value === "en vivo"

    );

}


/* =========================================================
   SESIÓN
========================================================= */

function cerrarSesion() {

    detenerActualizacionEnVivo();


    localStorage.removeItem(
        "userId"
    );


    window.location.href =
        "login.html";

}


/* =========================================================
   LEER RESPUESTA
========================================================= */

async function leerRespuesta(
    response
) {

    const text =
        await response.text();


    if (!text) {
        return null;
    }


    try {

        return JSON.parse(
            text
        );

    }
    catch {

        return {
            message: text
        };

    }

}


/* =========================================================
   PRECIO
========================================================= */

function formatearPrecio(
    valor
) {

    const numero =
        Number(valor);


    if (
        !Number.isFinite(numero)
    ) {

        return "$ 0,00";

    }


    return new Intl.NumberFormat(
        "es-AR",
        {
            style: "currency",
            currency: "ARS"
        }
    ).format(
        numero
    );

}


/* =========================================================
   FECHA
========================================================= */

function formatearFecha(
    fecha
) {

    const date =
        new Date(fecha);


    if (
        Number.isNaN(
            date.getTime()
        )
    ) {

        return String(fecha);

    }


    return date.toLocaleString(
        "es-AR"
    );

}


/* =========================================================
   ESCAPE HTML
========================================================= */

function escapeHtml(
    texto
) {

    const div =
        document.createElement(
            "div"
        );


    div.textContent =
        texto ?? "";


    return div.innerHTML;

}


/* =========================================================
   ESCAPE ATTRIBUTE
========================================================= */

function escapeAttribute(
    texto
) {

    return String(
        texto ?? ""
    )
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


/* =========================================================
   TOAST
========================================================= */

function mostrarToast(
    mensaje,
    tipo = ""
) {

    const container =
        document.getElementById(
            "toast-container"
        );


    if (!container) {
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


    setTimeout(
        () => toast.remove(),
        4000
    );

}


/* =========================================================
   ERROR
========================================================= */

function mostrarError(
    mensaje
) {

    const container =
        document.getElementById(
            "detalle-subasta"
        );


    if (!container) {
        return;
    }


    container.innerHTML = `

        <div class="auction-closed">

            ${escapeHtml(mensaje)}

        </div>

    `;

}