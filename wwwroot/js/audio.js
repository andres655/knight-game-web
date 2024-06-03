
// Variable global para almacenar el audio de fondo actual
var backgroundAudio = null;
window.playSound = function (audioFilePath) {
    var audio = new Audio(audioFilePath);
    backgroundAudio.loop = false;// El volumen debe ser un valor entre 0.0 (silencio) y 1.0 (volumen máximo)
    audio.play();
}



window.playBackgroundSound = function (audioFilePath, volume) {
    // Comprueba si ya hay un audio de fondo reproduciéndose
    if (backgroundAudio && !backgroundAudio.paused) {
        return; // Si el audio de fondo actual está reproduciéndose, no hace nada
    }

    // Crea un nuevo objeto Audio y almacénalo en backgroundAudio
    backgroundAudio = new Audio(audioFilePath);
    backgroundAudio.volume = volume; // El volumen debe ser un valor entre 0.0 (silencio) y 1.0 (volumen máximo)
    backgroundAudio.loop = true; // Opcional: hace que el audio se repita en bucle
    backgroundAudio.play();
}

// Para detener el audio de fondo si es necesario
window.stopBackgroundSound = function () {
    if (backgroundAudio) {
        backgroundAudio.pause();
        backgroundAudio.currentTime = 0; // Reinicia el audio
        backgroundAudio = null;
    }
}
