import { defineStore } from 'pinia'
import { ref } from 'vue'
import axios from 'axios'
import type { User } from '@/types/user'
import {jwtDecode} from "jwt-decode"
import { buildApiUrl, API_ENDPOINTS } from '@/config/api'

export const useAuthStore = defineStore('auth', () => {
    const user = ref<User | undefined>(undefined)
    const token = ref('')
    const isAuthenticated = ref(false)
    const error = ref('')


    function decodeToken(token: string) {

        try {
            return jwtDecode<User>(token)
        } catch (e) {
            return undefined
        }
    }

    async function login(email: string, password: string) {
        try {
            const response = await axios.post(buildApiUrl(API_ENDPOINTS.AUTH.LOGIN), {
                email,
                password,
            })

            
            token.value = response.data
            user.value = decodeToken(token.value)
            isAuthenticated.value = true
            error.value = ''
        } catch (err: any) {
            error.value = 'Credenciales incorrectas'
            isAuthenticated.value = false
        }
    }

    function logout() {
        user.value = undefined
        token.value = ''
        isAuthenticated.value = false
    }

    async function register(userData: {
        email: string;
        password: string;
        name: string;
        username: string;
        birthDate: string;
    }) {
        try {
            const response = await axios.post(buildApiUrl(API_ENDPOINTS.AUTH.REGISTER), userData);
            token.value = response.data;
            user.value = decodeToken(token.value);
            isAuthenticated.value = true;
            error.value = '';
        } catch (err: any) {
            error.value = err.response?.data?.message || 'Error al registrar usuario';
            isAuthenticated.value = false;
            throw new Error(error.value);
        }
    }

    return { user, token, isAuthenticated, error, login, logout, register }
})
