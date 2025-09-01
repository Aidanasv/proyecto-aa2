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

            // Verificar si la respuesta es exitosa
            if (response.status === 200) {
                token.value = response.data
                user.value = decodeToken(token.value)
                isAuthenticated.value = true
                error.value = ''
            } else {
                // Si no es 200, tratar como error
                error.value = `Error del servidor: ${response.status}`
                isAuthenticated.value = false
            }
        } catch (err: any) {
            // Manejar diferentes tipos de errores
            if (err.response) {
                // El servidor respondió con un código de estado que no está en el rango 2xx
                switch (err.response.status) {
                    case 400:
                        error.value = 'Datos de inicio de sesión inválidos'
                        break
                    case 401:
                        error.value = 'Credenciales incorrectas'
                        break
                    case 404:
                        error.value = 'Servicio no encontrado'
                        break
                    case 500:
                        error.value = 'Error interno del servidor'
                        break
                    default:
                        error.value = `Error del servidor: ${err.response.status}`
                }
            } else if (err.request) {
                // La petición fue hecha pero no se recibió respuesta
                error.value = 'No se pudo conectar con el servidor'
            } else {
                // Algo pasó al configurar la petición
                error.value = 'Error al procesar la solicitud'
            }
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
        passwordValidate: string
        name: string;
        username: string;
        birthDate: string;
    }) {
        try {
            const response = await axios.post(buildApiUrl(API_ENDPOINTS.AUTH.REGISTER), userData);
            
            if (response.status === 200 || response.status === 201) {
                token.value = response.data;
                user.value = decodeToken(token.value);
                isAuthenticated.value = true;
                error.value = '';
            } else {
                error.value = `Error del servidor: ${response.status}`;
                isAuthenticated.value = false;
                throw new Error(error.value);
            }
        } catch (err: any) {
            if (err.response) {
                switch (err.response.status) {
                    case 400:
                        error.value = err.response.data?.message || 'Datos de registro inválidos';
                        break;
                    case 409:
                        error.value = 'El email ya está registrado';
                        break;
                    case 422:
                        error.value = 'Datos de registro incorrectos';
                        break;
                    case 500:
                        error.value = 'Error interno del servidor';
                        break;
                    default:
                        error.value = err.response.data?.message || `Error del servidor: ${err.response.status}`;
                }
            } else if (err.request) {
                error.value = 'No se pudo conectar con el servidor';
            } else {
                error.value = 'Error al procesar la solicitud de registro';
            }
            isAuthenticated.value = false;
            throw new Error(error.value);
        }
    }

    return { user, token, isAuthenticated, error, login, logout, register }
})
