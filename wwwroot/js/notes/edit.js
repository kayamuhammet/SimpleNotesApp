// To be executed when the page is loaded
document.addEventListener('DOMContentLoaded', function() {
    // Syntax highlighting configuration
    hljs.configure({
        languages: ['javascript', 'html', 'css', 'csharp', 'python', 'sql']
    });

    // Quill editor initialization
    var quill = new Quill('#editor', {
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
                ['link', 'image', 'video'],
                ['clean']
            ],
            syntax: {
                highlight: function(text) {
                    return hljs.highlightAuto(text).value;
                }
            }
        }
    });

    // Load existing content into editor
    var hiddenContextElement = document.getElementById('hiddenContext');
    if (hiddenContextElement) {
        quill.root.innerHTML = hiddenContextElement.value;
    }

    // Transfer content when form is submitted
    if (document.getElementById('editForm')) {
        document.getElementById('editForm').onsubmit = function() {
            document.getElementById('hiddenContext').value = quill.root.innerHTML;
            return true;
        };
    }

    // Highlight code blocks
    setTimeout(function() {
        var codeBlocks = document.querySelectorAll('.ql-syntax');
        codeBlocks.forEach((block) => {
            hljs.highlightElement(block);
        });
    }, 100);
});