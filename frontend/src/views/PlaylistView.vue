<template>
    <v-container>
        <v-row class="mb-4">
            <v-col cols="12" class="d-flex justify-space-between align-center">
                <h1 class="text-h4">Mis Playlists</h1>
                <v-btn
                    v-if="authStore.isAuthenticated && authStore.user?.role === 'client'"
                    color="primary"
                    prepend-icon="mdi-plus"
                    @click="showCreateDialog"
                >
                    Nueva Playlist
                </v-btn>
            </v-col>
        </v-row>

        <!-- Filtros -->
        <v-row class="mb-4">
            <v-col cols="12">
                <v-text-field
                    v-model="searchFilter"
                    label="Buscar en playlists"
                    prepend-inner-icon="mdi-magnify"
                    clearable
                    @update:model-value="applyFilters"
                    @clear="searchFilter = ''; applyFilters()"
                >
                    <template v-slot:append>
                        <v-btn icon @click="toggleOrder">
                            <v-icon>{{ 
                                orderAsc === null ? 'mdi-sort-variant-remove' :
                                orderAsc ? 'mdi-sort-alphabetical-ascending' : 'mdi-sort-alphabetical-descending' 
                            }}</v-icon>
                        </v-btn>
                    </template>
                </v-text-field>
            </v-col>
        </v-row>

        <!-- Loading indicator -->
        <v-row v-if="playlistStore.isLoading">
            <v-col cols="12" class="text-center">
                <v-progress-circular indeterminate color="primary"></v-progress-circular>
            </v-col>
        </v-row>

        <!-- Error message -->
        <v-row v-else-if="playlistStore.error">
            <v-col cols="12">
                <v-alert type="error">
                    {{ playlistStore.error }}
                </v-alert>
            </v-col>
        </v-row>

        <!-- Playlists grid -->
        <v-row v-else>
            <v-col v-for="playlist in playlistStore.playlists" 
                :key="playlist.id" 
                cols="4" sm="4" md="6"
            >
                <v-card max-width="1200" class="mx-auto">
                    <v-card-title class="d-flex justify-space-between align-center px-6 py-4 bg-primary text-white">
                        <span class="text-h5">{{ playlist.name }}</span>
                        <div>
                            <v-btn
                                icon="mdi-pencil"
                                size="small"
                                variant="text"
                                color="white"
                                @click.stop="editPlaylist(playlist)"
                            ></v-btn>
                            <v-btn
                                icon="mdi-delete"
                                size="small"
                                variant="text"
                                color="white"
                                @click.stop="confirmDelete(playlist.id)"
                            ></v-btn>
                        </div>
                    </v-card-title>

                    <v-card-text v-if="playlist.description" class="pt-4 px-6 text-body-1">
                        {{ playlist.description }}
                    </v-card-text>

                    <v-divider></v-divider>

                    <!-- Lista de canciones -->
                    <v-card-text class="pa-6">
                        <div class="text-h6 font-weight-bold mb-4">Canciones</div>
                        <v-list class="bg-grey-lighten-4 rounded-lg">
                            <v-list-item v-for="track in playlist.tracks" 
                                :key="track.id"
                                class="mb-2"
                                rounded="lg"
                            >
                                <div class="d-flex align-center w-100">
                                    <!-- Icono y nombre de la canción -->
                                    <div class="d-flex align-center" style="min-width: 200px;">
                                        <v-icon 
                                            color="primary" 
                                            icon="mdi-music-note"
                                            class="mr-3"
                                        ></v-icon>
                                        <span class="text-subtitle-1">{{ track.name }}</span>
                                    </div>

                                    <!-- Espacio flexible -->
                                    <v-spacer></v-spacer>

                                    <!-- Controles y acciones -->
                                    <div class="d-flex align-center gap-4">
                                        <!-- Reproductor de audio -->
                                        <audio 
                                            :src="`${buildApiUrl(API_ENDPOINTS.AUDIOSONGS)}/${track.id}`" 
                                            controls
                                            class="audio-player"
                                        ></audio>

                                        <!-- Botón eliminar -->
                                        <v-btn
                                            icon="mdi-delete"
                                            size="small"
                                            variant="text"
                                            color="error"
                                            @click.stop="confirmTrackDelete(playlist.id, track.id, track.name)"
                                        ></v-btn>
                                    </div>
                                </div>
                            </v-list-item>

                            <v-list-item v-if="playlist.tracks.length === 0">
                                <v-list-item-title class="text-center text-subtitle-1 pa-4">
                                    No hay canciones en esta playlist
                                </v-list-item-title>
                            </v-list-item>
                        </v-list>
                    </v-card-text>

                    <!-- Estadísticas de la playlist -->
                    <v-card-text class="pt-0">
                        <v-divider class="mb-2"></v-divider>
                        <div class="d-flex justify-space-between text-caption text-grey">
                            <span>{{ playlist.tracks.length }} canciones</span>
                            <span>Duración total: {{ formatTotalDuration(playlist.tracks) }}</span>
                        </div>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>

        <!-- Formulario de playlist -->
        <PlaylistForm 
            v-model="showDialog"
            :playlist="selectedPlaylist"
        />

        <!-- Diálogo de confirmación para eliminar playlist -->
        <v-dialog v-model="showConfirmDialog" max-width="400px">
            <v-card>
                <v-card-title class="text-h5">Confirmar eliminación</v-card-title>
                <v-card-text>
                    ¿Estás seguro de que deseas eliminar esta playlist?
                </v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn color="primary" variant="text" @click="showConfirmDialog = false">
                        Cancelar
                    </v-btn>
                    <v-btn color="error" variant="text" @click="deleteSelectedPlaylist">
                        Eliminar
                    </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>

        <!-- Diálogo de confirmación para eliminar canción -->
        <v-dialog v-model="showDeleteTrackDialog" max-width="400px">
            <v-card>
                <v-card-title class="text-h6">Eliminar canción de la playlist</v-card-title>
                <v-card-text>
                    ¿Estás seguro de que deseas eliminar la canción "{{ trackToDelete?.name }}" de esta playlist?
                </v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn color="primary" variant="text" @click="showDeleteTrackDialog = false">
                        Cancelar
                    </v-btn>
                    <v-btn color="error" variant="text" @click="removeTrackFromPlaylist">
                        Eliminar
                    </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </v-container>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { usePlaylistStore } from '@/stores/usePlaylistStore';
import { useAuthStore } from '@/stores/useAuthStore';
import { useNotificationStore } from '@/stores/useNotificationStore';
import PlaylistForm from '@/components/PlaylistForm.vue';
import { formatTime } from '@/utils/TimeFormat';
import type { Playlist } from '@/types/playlist';
import type { Track } from '@/types/songs';
import { buildApiUrl, API_ENDPOINTS } from '@/config/api';


const playlistStore = usePlaylistStore();
const authStore = useAuthStore();
const notificationStore = useNotificationStore();

// Variables para los filtros
const searchFilter = ref('');
const orderAsc = ref<boolean | null>(null);

const showDialog = ref(false);
const showConfirmDialog = ref(false);
const showAddTracksDialog = ref(false);
const selectedPlaylist = ref<Playlist | null>(null);
const playlistToDelete = ref<number | null>(null);
const selectedPlaylistForTracks = ref<number | null>(null);

// Funciones para los filtros
const applyFilters = async () => {
    if (authStore.isAuthenticated) {
        await playlistStore.fetchUserPlaylists({
            name: searchFilter.value || undefined,
            nameOrder: orderAsc.value === undefined ? null : orderAsc.value,
            description: searchFilter.value || undefined,
            descriptionOrder: orderAsc.value === undefined ? null : orderAsc.value
        });
    }
};

const toggleOrder = () => {
    if (orderAsc.value === null) orderAsc.value = true;
    else if (orderAsc.value === true) orderAsc.value = false;
    else orderAsc.value = null;
    applyFilters();
};

onMounted(async () => {
    if (authStore.isAuthenticated) {
        await applyFilters();
    }
});

function showCreateDialog() {
    selectedPlaylist.value = null;
    showDialog.value = true;
}

function editPlaylist(playlist: Playlist) {
    selectedPlaylist.value = playlist;
    showDialog.value = true;
}

function confirmDelete(id: number) {
    playlistToDelete.value = id;
    showConfirmDialog.value = true;
}

async function deleteSelectedPlaylist() {
    if (playlistToDelete.value) {
        try {
            await playlistStore.deletePlaylist(playlistToDelete.value);
            notificationStore.showSuccess('Playlist eliminada exitosamente');
            showConfirmDialog.value = false;
            playlistToDelete.value = null;
        } catch (error) {
            notificationStore.showError('Error al eliminar la playlist');
        }
    }
}

const showDeleteTrackDialog = ref(false);
const trackToDelete = ref<{ playlistId: number; trackId: number; name: string } | null>(null);

function confirmTrackDelete(playlistId: number, trackId: number, trackName: string) {
    trackToDelete.value = {
        playlistId,
        trackId,
        name: trackName
    };
    showDeleteTrackDialog.value = true;
}

async function removeTrackFromPlaylist() {
    if (trackToDelete.value) {
        try {
            await playlistStore.removeTrackFromPlaylist(
                trackToDelete.value.playlistId,
                trackToDelete.value.trackId
            );
            notificationStore.showSuccess('Canción eliminada de la playlist');
            showDeleteTrackDialog.value = false;
            trackToDelete.value = null;
        } catch (error) {
            notificationStore.showError('Error al eliminar la canción de la playlist');
        }
    }
}

function showAddTracksToPlaylist(playlistId: number) {
    selectedPlaylistForTracks.value = playlistId;
}

function formatTotalDuration(tracks: Track[]): string {
    const totalSeconds = tracks.reduce((total, track) => total + track.duration, 0);
    return formatTime(totalSeconds);
}
</script>

<style scoped>
.audio-player {
    height: 32px;
    width: 250px;
    border-radius: 16px;
}

.audio-player::-webkit-media-controls-panel {
    background-color: transparent;
}

.audio-player::-webkit-media-controls-current-time-display,
.audio-player::-webkit-media-controls-time-remaining-display {
    color: rgba(0, 0, 0, 0.87);
}

.v-list {
    background-color: transparent !important;
}

.v-list-item {
    border-radius: 8px;
    margin-bottom: 4px;
    background-color: rgb(243, 243, 243);
}

.v-list-item:hover {
    background-color: rgb(237, 237, 237);
}
</style>
