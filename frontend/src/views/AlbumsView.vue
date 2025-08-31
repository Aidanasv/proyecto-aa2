<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useAlbumStore } from '@/stores/useAlbumsStore';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/useAuthStore';
import AlbumForm from '@/components/AlbumForm.vue';

const albumsStore = useAlbumStore();
const router = useRouter();
const authStore = useAuthStore();
const props = defineProps<{ id: string }>()
const showDialog = ref(false);

onMounted(() => {
    albumsStore.fetchAlbumsByArtists(props.id);
})

function goToSongs(id: number) {
    router.push({ name: "songs", params: { id } })
}

function showAddAlbumDialog() {
    showDialog.value = true;
}
</script>

<template>
    <!-- Información del artista -->
    <v-row class="mb-6" justify="center" align="center" no-gutters>
        <v-col cols="12" md="6" class="text-center">
            <v-card class="mx-auto" max-width="600" color="primary">
                <v-img :src="albumsStore.artist?.imagen || 'https://via.placeholder.com/400'" height="300"
                    cover></v-img>
                <v-card-title class="text-h4 justify-center">{{ albumsStore.artist?.name }}</v-card-title>
                <v-card-text class="text-center">
                    <p v-if="albumsStore.artist?.biography" class="text-body-1">
                        {{ albumsStore.artist.biography }}
                    </p>
                    <v-chip class="ma-2" variant="outlined">
                        {{ albumsStore.artist?.followers || 0 }} seguidores
                    </v-chip>
                </v-card-text>
                <v-card-actions class="justify-end">
                    <v-btn
                        v-if="authStore.isAuthenticated && authStore.user?.role === 'admin'"
                        color="secundary" class="mb-4"
                        prepend-icon="mdi-plus"
                        @click.stop="showAddAlbumDialog"
                    >
                        Añadir Álbum
                    </v-btn>
                </v-card-actions>
            </v-card>
        </v-col>
    </v-row>

    <!-- Lista de álbumes -->
    <v-row align="center" no-gutters>
        <v-col class="pa-2" v-for="album in albumsStore.artist?.albums" :key="album?.id" cols="12" sm="6" md="3">
            <v-card class="mx-auto" width="250" color="primary" @click="goToSongs(album.id)">
                <v-card-title>{{ album.name }}</v-card-title>
                <v-img :src="album.imagen || 'https://via.placeholder.com/200'" height="200" cover></v-img>
                <v-card-text>
                    <div>{{ new Date(album.releaseDate).toLocaleDateString() }}</div>
                </v-card-text>
            </v-card>
        </v-col>
    </v-row>

    <AlbumForm v-model="showDialog" :artist-id="props.id" />
</template>
