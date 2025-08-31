<script setup lang="ts">
import { useAuthStore } from '@/stores/useAuthStore'
import { useRouter } from 'vue-router'
import MenuApp from '@/components/MenuApp.vue'
import logo from '@/assets/logo.png'
import { useI18n } from 'vue-i18n'

const authStore = useAuthStore()
const router = useRouter()
const { t, locale } = useI18n()

function goToLogin() {
  router.push({ name: 'login' })
}

function logout() {
  authStore.logout()
  router.push({ name: 'login' })
}

const languages = [
  { code: 'es', label: 'ES' },
  { code: 'en', label: 'EN' }
]
</script>

<template>
  <v-app-bar app flat height="80" class="px-4">
    <v-img :src="logo" height="64" width="auto" max-width="200" class="mr-4" cover />

    <MenuApp />

    <v-spacer />

    <v-btn v-if="authStore.isAuthenticated" @click="logout">
      <v-icon>mdi-logout</v-icon>
      {{ t('logout')}}
    </v-btn>

    <v-btn v-else @click="goToLogin">
      <v-icon>mdi-login</v-icon>
      {{ t('login')}}
    </v-btn>

    <v-select v-model="locale" :items="languages" item-title="label" item-value="code" variant="outlined"
      density="compact" hide-details style="max-width: 80px" />
  </v-app-bar>
</template>
