<script setup lang="ts">
import { onMounted, ref, computed } from 'vue';
import { useSongStore } from '@/stores/useSongsStore';
import { usePlaylistStore } from '@/stores/usePlaylistStore';
import { useNotificationStore } from '@/stores/useNotificationStore';
import SongForm from '@/components/SongForm.vue';
import type { Song, Track } from '@/types/songs';
import { formatTime } from '@/utils/TimeFormat';
import { formatDate } from '@/utils/FormatUtils';
import { useAuthStore } from '@/stores/useAuthStore';
import { buildApiUrl, API_ENDPOINTS } from '@/config/api';

const songStore = useSongStore();
const playlistStore = usePlaylistStore();
const notificationStore = useNotificationStore();
const props = defineProps<{ id: string }>()
const authStore = useAuthStore()

const canAddToPlaylist = computed(() => 
    authStore.isAuthenticated && 
    authStore.user?.role === 'client' && 
    playlistStore.activePlaylistId !== null
);

onMounted(() => {
    songStore.fetchSongsByAlbum(props.id);
})

const showDialog = ref(false);
const selectSong = ref<Song | undefined>(undefined);
const isAddingTrack = ref<number | null>(null);

async function addToPlaylist(trackId: number) {
    if (!playlistStore.activePlaylistId) return;
    
    try {
        isAddingTrack.value = trackId;
        await playlistStore.addTrackToPlaylist(playlistStore.activePlaylistId, trackId);
        notificationStore.showSuccess('Canción añadida a la playlist exitosamente');
    } catch (error) {
        notificationStore.showError('Error al añadir la canción a la playlist');
        console.error('Error al añadir la canción a la playlist:', error);
    } finally {
        isAddingTrack.value = null;
    }
}

function changeSong(song: Track) {
    if (authStore.isAuthenticated && authStore.user && authStore.user.role === 'admin') {
        // Convertir Track a Song
        selectSong.value = {
            id: song.id,
            name: song.name,
            duration: song.duration,
            artistId: songStore.songs?.artistId ?? 0,
            albumId: Number(props.id),
            releaseDate: new Date(),
            plays: 0,
            softDelete: false
        };
        showDialog.value = true;
    }
}

function addNewSong() {
    selectSong.value = {
        id: 0,
        name: '',
        duration: 0,
        artistId: songStore.songs?.artistId ?? 0,
        albumId: Number(props.id),
        releaseDate: new Date(),
        plays: 0,
        softDelete: false
    };
    showDialog.value = true;
}

function onSongUpdate(song: Song | undefined) {
    selectSong.value = song;
}
</script>

<template>
    <v-row align="center" style="height: 150px" no-gutters>
        <v-col>
            <v-card class="mx-auto" max-width="400" color="primary">
                <v-card-title>{{ songStore.songs?.name }}</v-card-title>
                <v-img :src="songStore.songs?.imagen" height="200" cover></v-img>
                <v-card-text>
                    <div>{{ formatDate(songStore.songs?.releaseDate ? new Date(songStore.songs.releaseDate) : '') }}</div>
                </v-card-text>
            </v-card>
        </v-col>

        <v-col>
            <v-list width="80%" class="rounded-lg">
                <!-- Botón solo visible para admin -->
                <v-list-item v-if="authStore.isAuthenticated && authStore.user && authStore.user.role === 'admin'"
                    height="80%">
                    <v-btn @click="addNewSong" color="primary">
                        {{ $t('buttonNewSong') }}
                    </v-btn>
                </v-list-item>

                <!-- Lista de canciones -->
                <v-list-item v-for="(item, i) in songStore.songs?.tracks" :key="i" :value="item" rounded="shaped"
                    @click="changeSong(item)">
                    <template v-slot:prepend>
                        <v-icon color="primary" icon="mdi-music-note"></v-icon>
                    </template>

                    <v-list-item-title v-text="item.name"></v-list-item-title>
                    <v-list-item-subtitle v-text="formatTime(item.duration)"></v-list-item-subtitle>

                    <template v-slot:append>
                        <div class="d-flex align-center">
                            <v-btn
                                v-if="canAddToPlaylist"
                                icon="mdi-playlist-plus"
                                size="small"
                                color="primary"
                                class="mr-2"
                                @click.stop="addToPlaylist(item.id)"
                                :loading="isAddingTrack === item.id"
                            ></v-btn>
                            <audio :src="`${buildApiUrl(API_ENDPOINTS.AUDIOSONGS)}/${item.id}`" controls></audio>
                        </div>
                    </template>
                </v-list-item>
            </v-list>
        </v-col>
    </v-row>

    <!-- SongForm: artistId nunca será undefined -->
    <SongForm v-model="showDialog" :album-id="props.id" :artist-id="songStore.songs?.artistId ?? 0" :song="selectSong"
        @update:song="onSongUpdate" />
</template>

<style scoped></style>
