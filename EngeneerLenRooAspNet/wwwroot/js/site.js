window.addEventListener('DOMContentLoaded', event => {
    const datatablesSimple = document.getElementById('datatablesSimple');
    if (datatablesSimple) {
        new simpleDatatables.DataTable(datatablesSimple, {
            labels: {
                placeholder: "Поиск...",
                searchTitle: "Поиск в таблице",
                perPage: "кол-во элементов",
                noRows: "Не найдено элементов",
                noResults: "Нет результатов, соответствующих вашему поисковому запросу",
                info: "Отображение записей от {start} до {end} из {rows}.",
            },
            perPage: 25
        });
    }
});