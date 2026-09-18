document.addEventListener("DOMContentLoaded", () => {
    const usuario = localStorage.getItem("userId");
    const usuarioNombre = document.getElementById("usuario-nombre");
    const btnLogout = document.getElementById("btn-logout");
    const btnLogin = document.getElementById("btn-login");
    const btnMiCuenta = document.querySelector(".header__nav a[href*='dashboard']");

    if (usuario) {
        usuarioNombre.style.display = "inline";
        btnLogout.style.display = "inline";
        btnLogin.style.display = "none";
        btnMiCuenta.style.display = "inline";
    } else {
        usuarioNombre.style.display = "none";
        btnLogout.style.display = "none";
        btnLogin.style.display = "inline";
        btnMiCuenta.style.display = "none";
    }
});

const btnMenu = document.getElementById("btn-menu");
const headerNav = document.getElementById("header-nav");

if (btnMenu && headerNav) {
    btnMenu.addEventListener("click", () => {
        const abierto = headerNav.classList.toggle("is-open");
        btnMenu.classList.toggle("is-open", abierto);
    });
}