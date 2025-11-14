function mostrarOcultar(id) {
    if (document.getElementById) {
        var menu = document.getElementById(id);
        menu.style.display = (menu.style.display == 'none') ? 'block' : 'none';
    }
}
window.onload = function () {
    mostrarOcultar('mostrar');
}
// Con esto haremos que al dar click en la imagen "menu.png" muestre u oculte el contenido.