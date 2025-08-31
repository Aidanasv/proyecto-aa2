<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/useAuthStore'
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const email = ref('')
const password = ref('')
const authStore = useAuthStore()
const router = useRouter()

async function handleLogin() {
  await authStore.login(email.value, password.value)
  if (authStore.isAuthenticated) {
    router.push({ name: 'artists' })
  }
}
</script>

<template>
  <v-container class="mt-5" style="max-width: 400px">
    <v-card>
      <v-card-title>{{ t('login')}}</v-card-title>
      <v-card-text>
        <v-text-field :label= "$t('email')" v-model="email" />
        <v-text-field :label= "$t('password')" v-model="password" type="password" />
        <v-alert v-if="authStore.error" type="error">{{ authStore.error }}</v-alert>
      </v-card-text>
      <v-card-actions>
        <v-btn color="primary" block @click="handleLogin">{{ t('login')}}</v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
</template>
