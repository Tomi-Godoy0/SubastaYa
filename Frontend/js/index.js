document.addEventListener("DOMContentLoaded", () => {

cargarCategorias();
cargarSubastas();

});

/* =========================
CATEGORÍAS
========================= */

function cargarCategorias() {

const container = document.getElementById("categorias-container");

const categorias = [
    {
        nombre: "Electrónica",
        descripcion: "Celulares, computadoras y tecnología",
        icono: "💻"
    },
    {
        nombre: "Hogar",
        descripcion: "Productos para tu hogar",
        icono: "🏠"
    },
    {
        nombre: "Vehículos",
        descripcion: "Autos, motos y vehículos",
        icono: "🚗"
    },
    {
        nombre: "Deportes",
        descripcion: "Artículos deportivos",
        icono: "⚽"
    }
];

container.innerHTML = "";

categorias.forEach(categoria => {

    const card = document.createElement("article");

    card.classList.add("category-card");

    card.innerHTML = `
        <div class="category-icon">
            ${categoria.icono}
        </div>

        <h3>
            ${categoria.nombre}
        </h3>

        <p>
            ${categoria.descripcion}
        </p>
    `;

    container.appendChild(card);

});

}

/* =========================
SUBASTAS
========================= */

function cargarSubastas() {

const container = document.getElementById("subastas-container");

const subastas = [
    {
        titulo: "Notebook",
        precio: "$450.000",
        estado: "Finaliza en 2 días"
    },
    {
        titulo: "Bicicleta Mountain Bike",
        precio: "$180.000",
        estado: "Finaliza en 1 día"
    },
    {
        titulo: "Smart TV 55 pulgadas",
        precio: "$320.000",
        estado: "Finaliza en 3 días"
    }
];

container.innerHTML = "";

subastas.forEach(subasta => {

    const card = document.createElement("article");

    card.classList.add("auction-card");

    card.innerHTML = `
        <div class="auction-image">
            Imagen del producto
        </div>

        <div class="auction-content">

            <h3>
                ${subasta.titulo}
            </h3>

            <div class="auction-price">
                ${subasta.precio}
            </div>

            <div class="auction-info">
                ${subasta.estado}
            </div>

            <button class="btn btn-primary">
                Ver subasta
            </button>

        </div>
    `;

    container.appendChild(card);

});

}