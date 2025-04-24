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