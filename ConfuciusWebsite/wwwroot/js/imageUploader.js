// wwwroot/js/imageUploader.js
export function initImageUploader(options) {
    const { dropzoneId, fileInputId, previewId, formSelector, deleteUrlBase } = options;

    let selectedFiles = [];

    const dropzone = document.getElementById(dropzoneId);
    const fileInput = document.getElementById(fileInputId);
    const preview = document.getElementById(previewId);

    // Dropzone click
    dropzone.addEventListener("click", () => fileInput.click());

    // Drag & drop
    dropzone.addEventListener("dragover", e => {
        e.preventDefault();
        dropzone.classList.add("bg-light");
    });
    dropzone.addEventListener("dragleave", () => dropzone.classList.remove("bg-light"));
    dropzone.addEventListener("drop", e => {
        e.preventDefault();
        dropzone.classList.remove("bg-light");
        addFiles(e.dataTransfer.files);
    });

    // File input change
    fileInput.addEventListener("change", () => addFiles(fileInput.files));

    function addFiles(files) {
        for (const file of files) {
            if (file.type.startsWith("image/")) {
                selectedFiles.push(file);
            }
        }
        showPreview();
    }

    function showPreview() {
        preview.innerHTML = "";
        selectedFiles.forEach((file, index) => {
            const reader = new FileReader();
            reader.onload = e => {
                const wrapper = document.createElement("div");
                wrapper.style.position = "relative";

                const img = document.createElement("img");
                img.src = e.target.result;
                img.style.width = "120px";
                img.style.height = "120px";
                img.style.objectFit = "cover";
                img.classList.add("rounded", "shadow-sm");

                const removeBtn = document.createElement("button");
                removeBtn.type = "button";
                removeBtn.innerHTML = "×";
                removeBtn.classList.add("btn", "btn-danger", "btn-sm");
                removeBtn.style.position = "absolute";
                removeBtn.style.top = "0";
                removeBtn.style.right = "0";
                removeBtn.style.borderRadius = "50%";

                removeBtn.addEventListener("click", () => {
                    selectedFiles.splice(index, 1);
                    const dt = new DataTransfer();
                    selectedFiles.forEach(f => dt.items.add(f));
                    fileInput.files = dt.files;
                    showPreview();
                });

                wrapper.appendChild(img);
                wrapper.appendChild(removeBtn);
                preview.appendChild(wrapper);
            };
            reader.readAsDataURL(file);
        });
    }

    // Ensure server receives selected files
    document.querySelector(formSelector).addEventListener("submit", () => {
        const dt = new DataTransfer();
        selectedFiles.forEach(file => dt.items.add(file));
        fileInput.files = dt.files;
    });

    // Optional: existing image deletion (only if deleteUrlBase is provided)
    if (deleteUrlBase) {
        document.addEventListener("click", e => {
            if (e.target.classList.contains("delete-existing-image")) {
                const imageId = e.target.getAttribute("data-image-id");
                fetch(`${deleteUrlBase}/${imageId}`, { method: "POST" })
                    .then(res => {
                        if (res.ok) {
                            e.target.closest("div").remove();
                        }
                    });
            }
        });
    }
}