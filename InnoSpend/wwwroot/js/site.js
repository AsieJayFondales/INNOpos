function togglePasswordVisibility() {
    var passwordInput = document.getElementById('passwordInput');
    var togglePassword = document.getElementById('togglePassword');
    if (passwordInput.type === 'password') {
        passwordInput.type = 'text';
        togglePassword.src = '/images/eye.svg'; // Show "eye" icon when password is visible
    } else {
        passwordInput.type = 'password';
        togglePassword.src = '/images/eye-off.svg'; // Show "eye-off" icon when password is hidden
    }
}

function togglePasswordVisibility2() {
    var confirmPasswordInput = document.getElementById('confirmPasswordInput');
    var toggleConfirmPassword = document.getElementById('toggleConfirmPassword');
    if (confirmPasswordInput.type === 'password') {
        confirmPasswordInput.type = 'text';
        toggleConfirmPassword.src = '/images/eye.svg';
    } else {
        confirmPasswordInput.type = 'password';
        toggleConfirmPassword.src = '/images/eye-off.svg';
    }
}
 
