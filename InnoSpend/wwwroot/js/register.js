// Import the functions you need from the SDKs you need
import { initializeApp } from "https://www.gstatic.com/firebasejs/11.3.1/firebase-app.js";
import { getAnalytics } from "https://www.gstatic.com/firebasejs/11.3.1/firebase-analytics.js";
import { getAuth, createUserWithEmailAndPassword } from "firebase/auth";

// Your web app's Firebase configuration
const firebaseConfig = {
    apiKey: "AIzaSyBSgRAguojmSfOHBzvi9eZoqjo78bUVV6A",
    authDomain: "localhost",
    projectId: "inno-pos-d92c5",
    storageBucket: "inno-pos-d92c5.appspot.com",  // "projectId.appspot.com"
    messagingSenderId: "527057648288",
    appId: "1:527057648288:web:b189f5e0aebd9bc565a7c5",
    measurementId: "G-0KLBPTLPE3"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);
const auth = getAuth(app);  // Initialize the auth instance

const submit = document.getElementById('submit');
submit.addEventListener("click", function (event) {
    event.preventDefault();

    // Get inputs from the registration form
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    createUserWithEmailAndPassword(auth, email, password)
        .then((userCredential) => {
            const user = userCredential.user;
            console.log("User created:", user);
            alert(`Account created! UID: ${user.uid}`);
        })
        .catch((error) => {
            console.error("Error creating user:", error);
            alert(error.message);
        });

});
