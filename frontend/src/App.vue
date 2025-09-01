<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import HeaderComponent from './components/HeaderComponent.vue'
import FooterComponent from './components/FooterComponent.vue'
import { useNotificationStore } from '@/stores/useNotificationStore'

const notificationStore = useNotificationStore()
</script>

<template>
  <v-app>
    <HeaderComponent />
    <v-main scrollable>
      <v-container fluid>
        <RouterView />
      </v-container>
    </v-main>
    <FooterComponent />

    <!-- Snackbar para notificaciones -->
    <v-snackbar
      v-model="notificationStore.show"
      :color="notificationStore.color"
      :timeout="3000"
      location="top"
    >
      {{ notificationStore.message }}
      <template v-slot:actions>
        <v-btn variant="text" @click="notificationStore.show = false">
          Cerrar
        </v-btn>
      </template>
    </v-snackbar>
  </v-app>
</template>
