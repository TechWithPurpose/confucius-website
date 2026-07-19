document.addEventListener('DOMContentLoaded', function () {
    var list = document.getElementById('nav-order-list');
    if (!list) return;

    var draggedItem = null;

    list.querySelectorAll('li').forEach(function (item) {
        item.addEventListener('dragstart', function () {
            draggedItem = item;
            setTimeout(function () { item.classList.add('dragging'); }, 0);
        });

        item.addEventListener('dragend', function () {
            item.classList.remove('dragging');
            draggedItem = null;
        });
    });

    list.addEventListener('dragover', function (e) {
        e.preventDefault();
        if (!draggedItem) return;

        var afterElement = getDragAfterElement(list, e.clientY);
        if (afterElement == null) {
            list.appendChild(draggedItem);
        } else {
            list.insertBefore(draggedItem, afterElement);
        }
    });

    function getDragAfterElement(container, y) {
        var items = Array.from(container.querySelectorAll('li:not(.dragging)'));

        return items.reduce(function (closest, child) {
            var box = child.getBoundingClientRect();
            var offset = y - box.top - box.height / 2;

            if (offset < 0 && offset > closest.offset) {
                return { offset: offset, element: child };
            } else {
                return closest;
            }
        }, { offset: Number.NEGATIVE_INFINITY }).element;
    }

    var saveBtn = document.getElementById('save-order-btn');
    if (saveBtn) {
        saveBtn.addEventListener('click', function () {
            var orderedIds = Array.from(list.querySelectorAll('li')).map(function (li) {
                return parseInt(li.dataset.id, 10);
            });

            fetch('/Admin/Pages/SaveNavigationOrder', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(orderedIds)
            })
                .then(function (res) {
                    if (!res.ok) throw new Error('Request failed');
                    var status = document.getElementById('save-order-status');
                    if (status) {
                        status.style.display = 'inline';
                        setTimeout(function () { status.style.display = 'none'; }, 2000);
                    }
                })
                .catch(function () {
                    alert('Could not save the new order. Please try again.');
                });
        });
    }
});
