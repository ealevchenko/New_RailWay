function LockScreen(message) {
    var lock = document.getElementById('lockPanel');
    if (lock)
        lock.className = 'LockOn';
    lock.innerHTML = message;
}

function LockScreenOff() {
    var lock = document.getElementById('lockPanel');
    if (lock)
        lock.className = 'LockOff';
}


