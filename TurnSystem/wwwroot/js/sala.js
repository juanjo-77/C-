let sonidoActivo = true;
let audioContext = null;

function obtenerAudioContext() {
    if (!audioContext) {
        audioContext = new (window.AudioContext || window.webkitAudioContext)();
    }
    return audioContext;
}

function tocarTono(freq, startTime, duration = 0.18) {
    const ctx = obtenerAudioContext();
    const osc = ctx.createOscillator();
    const gain = ctx.createGain();

    osc.type = 'sine';
    osc.frequency.setValueAtTime(freq, startTime);

    gain.gain.setValueAtTime(0.0001, startTime);
    gain.gain.exponentialRampToValueAtTime(0.08, startTime + 0.02);
    gain.gain.exponentialRampToValueAtTime(0.0001, startTime + duration);

    osc.connect(gain);
    gain.connect(ctx.destination);

    osc.start(startTime);
    osc.stop(startTime + duration + 0.05);
}

async function reproducirChime() {
    const ctx = obtenerAudioContext();
    if (ctx.state === 'suspended') {
        await ctx.resume();
    }

    const now = ctx.currentTime;
    tocarTono(880, now);
    tocarTono(1175, now + 0.22);
    tocarTono(1568, now + 0.44);
}

function hablarTurno(turno) {
    if (!('speechSynthesis' in window)) return;

    const texto = `Turno ${turno.ticketNumber}, por favor dirigirse a ${turno.service}`;
    const utterance = new SpeechSynthesisUtterance(texto);
    utterance.lang = 'es-CO';
    utterance.rate = 1;
    utterance.pitch = 1;

    window.speechSynthesis.cancel();
    window.speechSynthesis.speak(utterance);
}

async function anunciarTurno(turno) {
    if (!sonidoActivo) return;

    await reproducirChime();
    setTimeout(() => hablarTurno(turno), 900);
}

document.getElementById('btnSonido').addEventListener('click', () => {
    sonidoActivo = !sonidoActivo;
    document.getElementById('btnSonido').innerText = sonidoActivo ? 'Sonido: ON' : 'Sonido: OFF';
});