// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showSpinner() {
    const spinner = document.getElementById('loadingSpinner');
    const body = document.body;

    spinner.style.display = 'block'; // Show Spinner
    body.classList.add('blur'); // Add the blur class to the background

    
    setTimeout(() => {
        spinner.style.display = 'none';
        body.classList.remove('blur'); // Remove blur in the background
    }, 3000);
}

// Show spinner on page transitions
window.addEventListener('beforeunload', showSpinner);

function showLogoutNotification(event) {
    event.preventDefault(); // Stop sending the form
    
    
    if (document.getElementById('notification').style.display === 'block') {
        return;
    }

    const notification = document.getElementById('notification');
    const countdown = document.getElementById('countdown');
    notification.style.display = 'block';

    let timeLeft = 5;
    countdown.innerText = timeLeft;

    const interval = setInterval(() => {
        timeLeft--;
        countdown.innerText = timeLeft;

        if (timeLeft <= 0) {
            clearInterval(interval);
            notification.style.display = 'none';
            document.getElementById('logoutForm').submit();
        }
    }, 1000);
}

function changeCulture(culture) {
    const currentPath = window.location.pathname.split('/').slice(2).join('/');
    const newPath = `/${culture}/${currentPath}`;
    
    
    document.cookie = `.AspNetCore.Culture=c=${culture}|uic=${culture};path=/;max-age=${365 * 24 * 60 * 60}`;
    
   
    window.location.href = newPath || `/${culture}`;
}