const API_URL = "https://localhost:7204";

document.addEventListener("DOMContentLoaded", () => {

    const form = document.getElementById("form-login");
    const errorElement = document.getElementById("login-error");

    if (!form) {
        console.error("No se encontró el formulario de login");
        return;
    }

    form.addEventListener("submit", async (event) => {
        event.preventDefault();

        const email = document.getElementById("login-email").value;
        const password = document.getElementById("login-password").value;

        // Limpiar mensaje anterior
        errorElement.textContent = "";

        try {
            const response = await fetch(`${API_URL}/api/auth/login`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    email: email,
                    password: password
                })
            });

            const data = await response.json();

            if (!response.ok) {
                throw new Error(data.message || "Credenciales inválidas");
            }

            // Guardar usuario autenticado
            localStorage.setItem("userId", data.id);

            console.log("Login correcto. Usuario:", data.id);

            // Ir a la página principal
            window.location.href = "index.html";

        } catch (error) {
            console.error("Error de login:", error);

            errorElement.textContent = error.message;
        }
    });
});