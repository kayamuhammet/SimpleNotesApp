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
});