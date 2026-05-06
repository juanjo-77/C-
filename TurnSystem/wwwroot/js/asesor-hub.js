let turnoEnAtencionId = null;

async function cargarTodosLosTurnos() {
    const res  = await fetch("/Asesor/ObtenerTurnos");
    const data = await res.json();
    if (!data.ok) return;

    renderizarLista("listaPendientes",  data.data["pendiente"]  || [], "pendiente");
    renderizarLista("listaAtencion",    data.data["enAtencion"] || [], "atencion");
    renderizarLista("listaFinalizados", data.data["finalizado"] || [], "finalizado");

    document.getElementById("countPendientes").textContent  = (data.data["pendiente"]  || []).length;
    document.getElementById("countAtencion").textContent    = (data.data["enAtencion"] || []).length;
    document.getElementById("countFinalizados").textContent = (data.data["finalizado"] || []).length;

    const enAtencion = data.data["enAtencion"] || [];
    if (enAtencion.length > 0) {
        turnoEnAtencionId = enAtencion[0].id;
        document.getElementById("currentTicketNumber").textContent = enAtencion[0].ticket;
        document.getElementById("attentionForm").style.cssText = "display:flex; flex-direction:column; gap:10px;";
    } else {
        turnoEnAtencionId = null;
        document.getElementById("currentTicketNumber").textContent = "—";
        document.getElementById("attentionForm").style.cssText = "display:none";
    }
}

function renderizarLista(listaId, turnos, tipo) {
    const lista = document.getElementById(listaId);
    const mensajes = { pendiente: "Sin turnos pendientes", atencion: "Ningún turno en atención", finalizado: "Sin turnos finalizados" };

    if (turnos.length === 0) {
        lista.innerHTML = `<li class="cola-empty">${mensajes[tipo]}</li>`;
        return;
    }

    lista.innerHTML = turnos.map(t => `
        <li class="turno-item turno-${tipo}">
            <span class="turno-ticket">${t.ticket}</span>
            <span class="turno-usuario">${t.nombreUsuario || "Usuario"}</span>
            <span class="turno-hora">${new Date(t.fechaCreacion).toLocaleTimeString("es-CO", { hour: "2-digit", minute: "2-digit" })}</span>
        </li>
    `).join("");
}

async function llamarSiguiente() {
    const btn = document.getElementById("btnLlamar");
    btn.disabled = true;
    btn.textContent = "Llamando...";

    const res  = await fetch("/Asesor/LlamarSiguiente", { method: "POST" });
    const data = await res.json();

    if (data.ok) {
        mostrarToast(`Turno ${data.turno.ticket} en atención`, "success");
        await cargarTodosLosTurnos();
    } else {
        mostrarToast(data.mensaje, "warning");
    }

    btn.disabled = false;
    btn.textContent = "▶ Llamar siguiente turno";
}

async function finalizarAtencion() {
    if (!turnoEnAtencionId) return;
    const comentario = document.getElementById("comentarioInput").value.trim();

    const res  = await fetch("/Asesor/Finalizar", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ turnoId: turnoEnAtencionId, comentario })
    });
    const data = await res.json();

    if (data.ok) {
        mostrarToast("Atención finalizada", "success");
        await cargarTodosLosTurnos();
    } else {
        mostrarToast(data.mensaje, "error");
    }
}

function mostrarToast(mensaje, tipo = "info") {
    const container = document.getElementById("toastContainer");
    const toast = document.createElement("div");
    toast.className = `toast-msg toast-${tipo}`;
    toast.textContent = mensaje;
    container.appendChild(toast);
    setTimeout(() => toast.remove(), 3000);
}

cargarTodosLosTurnos();