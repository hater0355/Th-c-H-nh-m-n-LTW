document.addEventListener("DOMContentLoaded", () => {
    const toggle = document.querySelector(".menu-toggle");
    const menu = document.querySelector(".sidenav");

    toggle?.addEventListener("click", () => {
        const isOpen = menu?.classList.toggle("open") ?? false;
        toggle.setAttribute("aria-expanded", String(isOpen));
    });
});
