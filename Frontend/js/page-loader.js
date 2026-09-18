document.addEventListener("DOMContentLoaded", () => {
    const loader = document.getElementById("page-loader");

    if (!loader) return;

    requestAnimationFrame(() => {
        loader.classList.add("page-loader--hidden");
    });
});