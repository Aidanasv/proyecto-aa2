<template>
    <v-container class="fill-height">
        <v-row align="center" justify="center">
            <v-col cols="12" sm="8" md="6" lg="4">
                <v-card class="elevation-12">
                    <v-toolbar color="primary" dark>
                        <v-toolbar-title>Registro de Usuario</v-toolbar-title>
                    </v-toolbar>
                    <v-card-text>
                        <v-form ref="form" v-model="isFormValid" @submit.prevent="register">
                            <v-text-field
                                v-model="formData.email"
                                label="Email"
                                prepend-inner-icon="mdi-email"
                                type="email"
                                required
                                :rules="[rules.required, rules.email]"
                                variant="outlined"
                                class="mt-4"
                            ></v-text-field>

                            <v-text-field
                                v-model="formData.password"
                                label="Contraseña"
                                prepend-inner-icon="mdi-lock"
                                :type="showPassword ? 'text' : 'password'"
                                required
                                :rules="[rules.required, rules.minLength]"
                                variant="outlined"
                                :append-inner-icon="showPassword ? 'mdi-eye-off' : 'mdi-eye'"
                                @click:append-inner="showPassword = !showPassword"
                            ></v-text-field>

                            <v-text-field
                                v-model="passwordConfirm"
                                label="Confirmar Contraseña"
                                prepend-inner-icon="mdi-lock-check"
                                :type="showPassword ? 'text' : 'password'"
                                required
                                :rules="[rules.required, rules.passwordMatch]"
                                variant="outlined"
                                :append-inner-icon="showPassword ? 'mdi-eye-off' : 'mdi-eye'"
                                @click:append-inner="showPassword = !showPassword"
                            ></v-text-field>

                            <v-text-field
                                v-model="formData.name"
                                label="Nombre"
                                prepend-inner-icon="mdi-account"
                                required
                                :rules="[rules.required]"
                                variant="outlined"
                            ></v-text-field>

                            <v-text-field
                                v-model="formData.username"
                                label="Nombre de usuario"
                                prepend-inner-icon="mdi-account-circle"
                                required
                                :rules="[rules.required]"
                                variant="outlined"
                            ></v-text-field>

                            <v-menu
                                v-model="dateMenu"
                                :close-on-content-click="false"
                                transition="scale-transition"
                                offset-y
                            >
                                <template v-slot:activator="{ props }">
                                    <v-text-field
                                        v-model="formattedDate"
                                        label="Fecha de nacimiento"
                                        prepend-inner-icon="mdi-calendar"
                                        readonly
                                        v-bind="props"
                                        variant="outlined"
                                        :rules="[rules.required]"
                                    ></v-text-field>
                                </template>
                                <v-date-picker
                                    v-model="formData.birthDate"
                                    @update:model-value="dateMenu = false"
                                ></v-date-picker>
                            </v-menu>
                        </v-form>
                    </v-card-text>
                    <v-card-actions class="px-4 pb-4">
                        <v-btn
                            type="submit"
                            color="primary"
                            block
                            :loading="isLoading"
                            :disabled="!isFormValid"
                            @click="register"
                        >
                            Registrarse
                        </v-btn>
                    </v-card-actions>
                    <v-card-text class="text-center pt-0">
                        ¿Ya tienes una cuenta?
                        <router-link to="/login" class="text-decoration-none">
                            Inicia sesión
                        </router-link>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/useAuthStore';
import { useNotificationStore } from '@/stores/useNotificationStore';
import { formatDate } from '@/utils/FormatUtils';

const router = useRouter();
const authStore = useAuthStore();
const notificationStore = useNotificationStore();

const form = ref();
const isFormValid = ref(false);
const isLoading = ref(false);
const showPassword = ref(false);
const dateMenu = ref(false);
const passwordConfirm = ref('');

const formData = ref({
    email: '',
    password: '',
    name: '',
    username: '',
    birthDate: new Date().toISOString().substr(0, 10)
});

const rules = {
    required: (v: string) => !!v || 'Este campo es requerido',
    email: (v: string) => /.+@.+\..+/.test(v) || 'Email debe ser válido',
    minLength: (v: string) => v.length >= 6 || 'La contraseña debe tener al menos 6 caracteres',
    passwordMatch: (v: string) => v === formData.value.password || 'Las contraseñas no coinciden'
};

const formattedDate = computed(() => {
    return formData.value.birthDate ? formatDate(formData.value.birthDate) : '';
});

const register = async () => {
    if (!form.value?.validate()) return;
    
    if (formData.value.password !== passwordConfirm.value) {
        notificationStore.showError('Las contraseñas no coinciden');
        return;
    }
    
    isLoading.value = true;
    try {
        await authStore.register({
            ...formData.value,
            birthDate: new Date(formData.value.birthDate).toISOString()
        });
        notificationStore.showSuccess('Registro exitoso');
        passwordConfirm.value = ''; // Reset password confirm
        router.push('/');
    } catch (error: any) {
        notificationStore.showError(error.message || 'Error al registrar usuario');
    } finally {
        isLoading.value = false;
    }
};
</script>
