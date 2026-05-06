async function cargarStats() {
    const res = await fetch('/Estadisticas/Obtener');
    const data = await res.json();

    if (!data.ok) return;

    document.getElementById('totalTurnos').innerText = data.stats.total;
    document.getElementById('finalizados').innerText = data.stats.finalizados;
    document.getElementById('promedio').innerText = data.stats.promedio + ' min';
    document.getElementById('asesoresActivos').innerText = data.stats.asesores;
}

window.onload = cargarStats;