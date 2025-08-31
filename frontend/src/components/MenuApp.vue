<script setup lang="ts">
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { usePlaylistStore } from '@/stores/usePlaylistStore'
import { useAuthStore } from '@/stores/useAuthStore'
import { storeToRefs } from 'pinia'
import { ref, onMounted, watch } from 'vue'

const { t } = useI18n()
const router = useRouter()
const playlistStore = usePlaylistStore()
const authStore = useAuthStore()

const selectedPlaylistId = ref<number | null>(null)

function goTo(route: string) {
  router.push({ name: route })
}

onMounted(async () => {
  if (authStore.isAuthenticated && authStore.user?.role === 'client') {
    await playlistStore.fetchUserPlaylists()
  }
})

watch(selectedPlaylistId, (newId) => {
  if (newId) {
    playlistStore.setActivePlaylist(newId)
  }
})

</script>

<template>
  <div class="d-flex align-center flex-grow-1" style="gap: 16px;">
    <v-btn @click="goTo('home')">
      <v-icon>mdi-home</v-icon>
      {{ t('home') }}
    </v-btn>

    <v-btn @click="goTo('artists')">
      <v-icon>mdi-account-music</v-icon>
      {{ t('artists') }}
    </v-btn>

    <v-btn 
      v-if="authStore.isAuthenticated && authStore.user?.role === 'client'"
      @click="goTo('playlists')"
    >
      <v-icon>mdi-playlist-music</v-icon>
      {{ t('playlists') }}
    </v-btn>

    <v-select
      v-if="authStore.isAuthenticated && authStore.user?.role === 'client'"
      v-model="selectedPlaylistId"
      :items="playlistStore.playlists"
      item-title="name"
      item-value="id"
      label="Playlist activa"
      variant="outlined"
      density="compact"
      hide-details
      class="mx-4"
      style="max-width: 200px;"
      persistent-placeholder
    ></v-select>

  </div>
</template>
