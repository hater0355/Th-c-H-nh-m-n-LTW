(() => {
    const toggle = document.getElementById("sidebarToggle");
    if (!toggle) return;

    if (window.matchMedia("(max-width: 700px)").matches) {
        document.body.classList.add("sidenav-collapsed");
        toggle.setAttribute("aria-expanded", "false");
    }

    toggle.addEventListener("click", () => {
        const collapsed = document.body.classList.toggle("sidenav-collapsed");
        toggle.setAttribute("aria-expanded", String(!collapsed));
    });
})();
