<script setup lang="ts">
import { useSearchStore } from '@/stores/useSearchStore';
import { useRouter } from 'vue-router';
import SearchBarComponent from '@/components/SearchBarComponent.vue'

const searchStore = useSearchStore();
const router = useRouter();
searchStore.fetchAll("");

async function goToPath(id : number, tag: string) {
    if (tag=="artists") {
        router.push({ name: "albums", params: { id } })
    } else if (tag == "song") {
        router.push({ name: "songs", params: { id } })

    }else if (tag== "album") {
        router.push({ name: "songs", params: { id } })
    }
}

function handleSearch(query: string) {
  console.log('🔍 Resultado de búsqueda:', query)
  searchStore.fetchAll(query);
}
</script>

<template>
  <v-container>
    <v-row class="mb-4">
      <v-col cols="12" md="4" offset-md="4">
        <SearchBarComponent @update:query="handleSearch" />
      </v-col>
    </v-row>

    <v-row align="center" no-gutters>
      <v-col
        class="pa-2"
        v-for="search in searchStore.search"
        :key="search.id"
        cols="12"
        sm="6"
        md="4"
      >
        <v-card
          class="mx-auto"
          max-width="300"
          color="primary"
          @click="goToPath(search.id, search.tag)"
        >
          <v-card-title>
            <v-avatar size="28" class="ma-3" color="black">
              <v-icon size="16" v-if="search.tag === 'artists'">mdi-account-music</v-icon>
              <v-icon size="16" v-else-if="search.tag === 'album'">mdi-album</v-icon>
              <v-icon size="16" v-else-if="search.tag === 'song'">mdi-music-note</v-icon>
            </v-avatar>
            {{ search.name }}
          </v-card-title>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

