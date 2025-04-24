hljs.configure({
    languages: ['javascript', 'html', 'css', 'csharp', 'python', 'sql']
});


var quill = new Quill('#noteEditor', {
    theme: 'snow',
    modules: {
        toolbar: [
            [{ 'font': [] }, { 'size': ['small', false, 'large', 'huge'] }],
            ['bold', 'italic', 'underline', 'strike'],
            [{ 'color': [] }, { 'background': [] }],
            [{ 'align': [] }, { 'indent': '-1' }, { 'indent': '+1' }],
            [{ 'direction': 'rtl' }],
            [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
            [{ 'list': 'ordered' }, { 'list': 'bullet' }],
            ['blockquote', 'code-block'],
            ['link', 'image'],
            ['clean']
        ],
        syntax: {
            highlight: function(text) {
                return hljs.highlightAuto(text).value;
            }
        }
    },
    placeholder: 'Not içeriğini buraya yazın...'
});

// Transfer Quill content to textarea before the form is submitted
document.getElementById('addNoteForm').onsubmit = function() {
    document.getElementById('noteContext').value = quill.root.innerHTML;
    return true;
};

// Hide/show sidebars
document.getElementById('toggleSidebars').addEventListener('click', function() {
    var sidebar = document.getElementById('sidebar');
    var noteList = document.getElementById('noteList');
    var noteDetail = document.getElementById('noteDetail');
    var toggleIcon = document.getElementById('toggleIcon');
    
    sidebar.classList.toggle('d-none');
    noteList.classList.toggle('d-none');
    
    
    if (sidebar.classList.contains('d-none') && noteList.classList.contains('d-none')) {
        noteDetail.classList.remove('col-md-7');
        noteDetail.classList.add('col-md-12');
        toggleIcon.classList.remove('bi-layout-sidebar');
        toggleIcon.classList.add('bi-layout-sidebar-inset-reverse');
        document.getElementById('toggleSidebars').classList.add('active');
    } else {
        noteDetail.classList.remove('col-md-12');
        noteDetail.classList.add('col-md-7');
        toggleIcon.classList.remove('bi-layout-sidebar-inset-reverse');
        toggleIcon.classList.add('bi-layout-sidebar');
        document.getElementById('toggleSidebars').classList.remove('active');
    }
});


document.addEventListener('DOMContentLoaded', function() {
    
    var noteDetailCodeBlocks = document.querySelectorAll('.note-text pre code');
    noteDetailCodeBlocks.forEach((block) => {
        hljs.highlightBlock(block);
    });
    
    
    var noteDetailSyntaxBlocks = document.querySelectorAll('.note-text .ql-syntax');
    noteDetailSyntaxBlocks.forEach((block) => {
        hljs.highlightBlock(block);
    });
    
    
    if (window.innerWidth < 992 && new URLSearchParams(window.location.search).has('noteId')) {
        var toggleButton = document.getElementById('toggleSidebars');
        toggleButton.click();
    }

    const searchNotes = document.getElementById('searchNotes');
    const itemsPerPage = 6;
    let currentPage = 1;
    let allNoteItems = Array.from(document.querySelectorAll('.note-list .list-group-item'));
    let filteredNoteItems = allNoteItems;
    
    const prevButton = document.getElementById('prevPage');
    const nextButton = document.getElementById('nextPage');
    const pageInfo = document.getElementById('pageInfo');

    function updatePagination() {
        const totalPages = Math.ceil(filteredNoteItems.length / itemsPerPage);
        const startIndex = (currentPage - 1) * itemsPerPage;
        const endIndex = startIndex + itemsPerPage;

        // Hide all notes
        allNoteItems.forEach(item => item.style.display = 'none');

        // Show only filtered notes and notes on the current page
        filteredNoteItems.slice(startIndex, endIndex).forEach(item => {
            item.style.display = '';
        });

        // Update page information
        pageInfo.textContent = totalPages > 0 
            ? `Sayfa ${currentPage}/${totalPages}` 
            : 'Sayfa 0/0';

        // Update button states
        prevButton.disabled = currentPage === 1;
        nextButton.disabled = currentPage === totalPages || totalPages === 0;
    }

    // Search function
    function handleSearch(searchText) {
        searchText = searchText.toLowerCase();
        
        if (searchText === '') {
            filteredNoteItems = allNoteItems;
        } else {
            filteredNoteItems = allNoteItems.filter(item => {
                const title = item.querySelector('strong').textContent.toLowerCase();
                const preview = item.querySelector('.note-preview').textContent.toLowerCase();
                return title.includes(searchText) || preview.includes(searchText);
            });
        }

        // Show/hide search results
        const noResults = document.getElementById('noResults');
        noResults.style.display = filteredNoteItems.length === 0 && searchText !== '' ? 'block' : 'none';

        // Reset and update pagination
        currentPage = 1;
        updatePagination();
    }

    if (searchNotes) {
        let searchTimeout = null;
        searchNotes.addEventListener('input', function(e) {
            if (searchTimeout) {
                clearTimeout(searchTimeout);
            }
            searchTimeout = setTimeout(() => {
                handleSearch(e.target.value);
            }, 300);
        });
    }

    // Event listener for paging buttons
    prevButton.addEventListener('click', () => {
        if (currentPage > 1) {
            currentPage--;
            updatePagination();
        }
    });

    nextButton.addEventListener('click', () => {
        const totalPages = Math.ceil(filteredNoteItems.length / itemsPerPage);
        if (currentPage < totalPages) {
            currentPage++;
            updatePagination();
        }
    });

    // Start paging for initial load
    updatePagination();


});


function printNote() {
    const noteTitle = document.querySelector('.note-detail-header h3').innerText;
    const noteContent = document.querySelector('.note-content').innerHTML;
    
    const printWindow = window.open('', '_blank');
    printWindow.document.write(`
        <!DOCTYPE html>
        <html>
        <head>
            <title>${noteTitle}</title>
            <link href="//cdnjs.cloudflare.com/ajax/libs/highlight.js/11.7.0/styles/default.min.css" rel="stylesheet">
            <style>
                body { 
                    font-family: Arial, sans-serif; 
                    padding: 20px; 
                }
                .note-title { 
                    font-size: 24px; 
                    margin-bottom: 20px; 
                }
                .note-content { 
                    font-size: 14px; 
                    line-height: 1.6; 
                }
                /* Code Blocks */
                pre {
                    background:rgb(255, 253, 235);
                    border: 1px solid #ddd;
                    border-radius: 4px;
                    padding: 15px;
                    margin: 15px 0;
                    overflow-x: auto;
                }
                .ql-syntax {
                    font-family: 'Consolas', 'Monaco', monospace;
                    background:rgb(221, 255, 235);
                    border: 1px solid #ddd;
                    border-radius: 4px;
                    padding: 15px;
                    margin: 15px 0;
                    white-space: pre-wrap;
                }
                code {
                    font-family: 'Consolas', 'Monaco', monospace;
                    padding: 2px 5px;
                    background:rgb(255, 253, 235);
                    border-radius: 3px;
                }
                @media print {
                    pre, .ql-syntax {
                        white-space: pre-wrap;
                        word-wrap: break-word;
                        border: 1px solid #ddd;
                        page-break-inside: avoid;
                    }
                }
            </style>
        </head>
        <body>
            <div class="note-title">${noteTitle}</div>
            <div class="note-content">${noteContent}</div>
            <script src="//cdnjs.cloudflare.com/ajax/libs/highlight.js/11.7.0/highlight.min.js"></script>
            <script>
                document.querySelectorAll('pre code, .ql-syntax').forEach((block) => {
                    hljs.highlightBlock(block);
                });
            </script>
        </body>
        </html>
    `);
    
    printWindow.document.close();
    printWindow.focus();
    
    setTimeout(() => {
        printWindow.print();
        printWindow.close();
    }, 500);
}