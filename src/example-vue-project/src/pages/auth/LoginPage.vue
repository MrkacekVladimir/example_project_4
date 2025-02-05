<template>
    <form @submit.prevent="handleLogin" class="login-form">
        <div class="form-group">
            <label for="username">Username</label>
            <input id="username" v-model="username" type="text" required />
        </div>
        <div class="form-group">
            <label for="password">Password</label>
            <input id="password" v-model="password" type="password" required />
        </div>
        <div>
            <button type="submit">Login</button>
            <button type="button" v-on:click="handleMe">Get Me Info</button>
        </div>
    </form>
</template>

<script setup lang="tsx">
import { ref } from 'vue';

const username = ref('');
const password = ref('');

const handleMe = async () => {
         await fetch('https://localhost:7271/api/auth/me', {
            method: 'GET',
            credentials: 'include' // Ensures cookies are sent and received
        });

}

const handleLogin = async () => {
    try {
        const response = await fetch('https://localhost:7271/api/auth/token', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username: username.value, password: password.value }),
            credentials: 'include' // Ensures cookies are sent and received
        });
        
        if (!response.ok) {
            throw new Error('Login failed');
        }
        alert('Login successful!');
    } catch (error) {
        console.error('Error:', error);
        alert('Login failed!');
    }
};
</script>

<style scoped>
.login-form {
    max-width: 300px;
    margin: 0 auto;
    padding: 20px;
    border: 1px solid #ddd;
    border-radius: 8px;
    background-color: #f9f9f9;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}
.form-group {
    margin-bottom: 15px;
}
label {
    display: block;
    margin-bottom: 5px;
    font-weight: bold;
}
input {
    width: 100%;
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 4px;
}
button {
    width: 100%;
    padding: 10px;
    border: none;
    background-color: #007bff;
    color: white;
    font-size: 16px;
    border-radius: 4px;
    cursor: pointer;
}
button:hover {
    background-color: #0056b3;
}

button[type="submit"] {
    margin-bottom: 8px;
}
</style>