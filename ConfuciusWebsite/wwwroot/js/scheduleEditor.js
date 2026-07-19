document.addEventListener('DOMContentLoaded', function () {
    var list = document.getElementById('schedule-list');
    var addBtn = document.getElementById('add-schedule-row');
    var template = document.getElementById('schedule-row-template');

    if (!list || !addBtn || !template) return;

    addBtn.addEventListener('click', function () {
        var clone = template.content.cloneNode(true);
        list.appendChild(clone);
        reindexRows();
    });

    list.addEventListener('click', function (e) {
        var btn = e.target.closest('.remove-schedule-row');
        if (!btn) return;

        var row = btn.closest('.schedule-row');
        if (row) {
            row.remove();
            reindexRows();
        }
    });

    function reindexRows() {
        var rows = list.querySelectorAll('.schedule-row');
        rows.forEach(function (row, index) {
            var idInput = row.querySelector('.schedule-id-input');
            var daySelect = row.querySelector('.schedule-day-select');
            var startInput = row.querySelector('.schedule-start-input');
            var endInput = row.querySelector('.schedule-end-input');

            if (idInput) {
                idInput.name = 'Schedules[' + index + '].Id';
                if (!idInput.value) idInput.value = '0';
            }
            if (daySelect) daySelect.name = 'Schedules[' + index + '].DayOfWeek';
            if (startInput) startInput.name = 'Schedules[' + index + '].StartTime';
            if (endInput) endInput.name = 'Schedules[' + index + '].EndTime';
        });
    }
});
