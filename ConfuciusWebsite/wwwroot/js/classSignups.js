document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.contacted-checkbox').forEach(function (cb) {
        cb.addEventListener('change', onContactedChange);
    });

    document.querySelectorAll('.delete-signup-btn').forEach(function (btn) {
        btn.addEventListener('click', onDeleteClick);
    });
});

function onContactedChange(e) {
    var checkbox = e.target;
    var id = checkbox.dataset.id;
    var contacted = checkbox.checked;

    fetch('/Admin/Classes/ToggleContacted/' + id, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(contacted)
    })
        .then(function (res) {
            if (!res.ok) throw new Error('Request failed');
            moveRow(id, contacted);
        })
        .catch(function () {
            checkbox.checked = !contacted;
            alert('Could not update this request. Please try again.');
        });
}

function moveRow(id, contacted) {
    var row = document.querySelector('tr[data-id="' + id + '"]');
    var detailsRow = document.getElementById('details-row-' + id);
    if (!row) return;

    var targetBody = document.getElementById(contacted ? 'old-requests-body' : 'new-requests-body');

    targetBody.prepend(row);
    if (detailsRow) {
        row.after(detailsRow);
    }

    updateEmptyMessages();
}

function onDeleteClick(e) {
    var btn = e.target.closest('.delete-signup-btn');
    var id = btn.dataset.id;

    if (!confirm('Delete this request? This cannot be undone.')) return;

    fetch('/Admin/Classes/DeleteSignup/' + id, { method: 'POST' })
        .then(function (res) {
            if (!res.ok) throw new Error('Request failed');
            var row = document.querySelector('tr[data-id="' + id + '"]');
            var detailsRow = document.getElementById('details-row-' + id);
            if (row) row.remove();
            if (detailsRow) detailsRow.remove();
            updateEmptyMessages();
        })
        .catch(function () {
            alert('Could not delete this request. Please try again.');
        });
}

function updateEmptyMessages() {
    var newBody = document.getElementById('new-requests-body');
    var oldBody = document.getElementById('old-requests-body');
    var newEmpty = document.getElementById('new-requests-empty');
    var oldEmpty = document.getElementById('old-requests-empty');

    if (newBody && newEmpty) {
        newEmpty.style.display = newBody.children.length ? 'none' : '';
    }
    if (oldBody && oldEmpty) {
        oldEmpty.style.display = oldBody.children.length ? 'none' : '';
    }
}
