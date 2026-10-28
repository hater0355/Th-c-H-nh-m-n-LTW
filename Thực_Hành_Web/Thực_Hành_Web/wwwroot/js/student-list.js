(() => {
    const search = document.getElementById("dashboardSearch");
    const rows = [...document.querySelectorAll(".student-table tbody tr")];
    if (!search || rows.length === 0) return;

    search.addEventListener("input", () => {
        const query = search.value.trim().toLocaleLowerCase("vi");
        for (const row of rows) {
            row.hidden = !row.textContent.toLocaleLowerCase("vi").includes(query);
        }
    });
})();
