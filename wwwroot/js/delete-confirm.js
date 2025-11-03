// Reusable delete modal handler (requires Bootstrap 5)
(() => {
    document.addEventListener('DOMContentLoaded', () => {
        const modalEl = document.getElementById('confirmDeleteModal');
        if (!modalEl) return;

        // Ensure modal is a direct child of body to avoid stacking context issues
        if (modalEl.parentElement !== document.body) {
            document.body.appendChild(modalEl);
        }

        const deleteForm = document.getElementById('deleteConfirmForm');
        const deleteEntityName = document.getElementById('deleteEntityName');
        const deleteEntityId = document.getElementById('deleteEntityId');
        let bsModal;

        function getModal() {
            if (!bsModal) bsModal = new bootstrap.Modal(modalEl, { backdrop: true, keyboard: true });
            return bsModal;
        }

        // Buttons must have: class .btn-delete and attributes:
        // data-id, data-name (optional), data-delete-url (POST endpoint)
        document.body.addEventListener('click', (e) => {
            const btn = e.target.closest('.btn-delete');
            if (!btn) return;

            const id = btn.getAttribute('data-id') || '';
            const name = btn.getAttribute('data-name') || '';
            const url = btn.getAttribute('data-delete-url') || '';

            deleteEntityName.textContent = name;
            deleteEntityId.value = id;

            if (url) {
                deleteForm.setAttribute('action', url);
            } else {
                // fallback: keep existing action
            }

            getModal().show();
        });
    });
})();