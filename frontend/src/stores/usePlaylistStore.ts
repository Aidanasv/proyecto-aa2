import { defineStore } from "pinia";
import { ref, computed } from "vue";
import axios from "axios";
import type { Playlist, NewPlaylist, PlaylistFilters } from "@/types/playlist";
import { useAuthStore } from "./useAuthStore";
import { buildApiUrl, API_ENDPOINTS } from '@/config/api';

export const usePlaylistStore = defineStore("playlistStore", () => {
    const playlists = ref<Playlist[]>([]);
    const activePlaylistId = ref<number | null>(null);
    const isLoading = ref<boolean>(false);
    const error = ref<string | null>(null);
    const authStore = useAuthStore();

    async function fetchUserPlaylists(filters?: PlaylistFilters): Promise<void> {
        isLoading.value = true;
        error.value = null;

        try {
            const response = await axios.get<Playlist[]>(`${buildApiUrl(API_ENDPOINTS.PLAYLISTS)}/user`, {
                headers: { authorization: `Bearer ${authStore.token}` },
                params: filters
            });
            playlists.value = response.data;
        } catch (err: any) {
            error.value = err.message || "Error al obtener las playlists";
        } finally {
            isLoading.value = false;
        }
    }

    async function createPlaylist(playlist: NewPlaylist): Promise<void> {
        isLoading.value = true;
        error.value = null;

        try {
            const response = await axios.post(buildApiUrl(API_ENDPOINTS.PLAYLISTS), playlist, {
                headers: { authorization: `Bearer ${authStore.token}` }
            });
            await fetchUserPlaylists();
        } catch (err: any) {
            error.value = err.message || "Error al crear la playlist";
            throw error.value;
        } finally {
            isLoading.value = false;
        }
    }

    async function updatePlaylist(id: number, playlist: Partial<NewPlaylist>): Promise<void> {
        isLoading.value = true;
        error.value = null;

        try {
            const response = await axios.put(`${buildApiUrl(API_ENDPOINTS.PLAYLISTS)}/${id}`, playlist, {
                headers: { authorization: `Bearer ${authStore.token}` }
            });
            await fetchUserPlaylists();
        } catch (err: any) {
            error.value = err.message || "Error al actualizar la playlist";
            throw error.value;
        } finally {
            isLoading.value = false;
        }
    }

    async function deletePlaylist(id: number): Promise<void> {
        isLoading.value = true;
        error.value = null;

        try {
            await axios.delete(`${buildApiUrl(API_ENDPOINTS.PLAYLISTS)}/${id}`, {
                headers: { authorization: `Bearer ${authStore.token}` }
            });
            await fetchUserPlaylists();
        } catch (err: any) {
            error.value = err.message || "Error al eliminar la playlist";
            throw error.value;
        } finally {
            isLoading.value = false;
        }
    }

    async function addTrackToPlaylist(playlistId: number, trackId: number): Promise<void> {
        isLoading.value = true;
        error.value = null;

        try {
            await axios.post(`${buildApiUrl(API_ENDPOINTS.PLAYLISTS)}/${playlistId}/add/${trackId}`, {}, {
                headers: { authorization: `Bearer ${authStore.token}` }
            });
            // Actualizar la lista después de añadir la canción
            await fetchUserPlaylists();
        } catch (err: any) {
            error.value = err.message || "Error al añadir la canción a la playlist";
            throw error.value;
        } finally {
            isLoading.value = false;
        }
    }

    async function removeTrackFromPlaylist(playlistId: number, trackId: number): Promise<void> {
        isLoading.value = true;
        error.value = null;

        try {
            await axios.delete(`${buildApiUrl(API_ENDPOINTS.PLAYLISTS)}/${playlistId}/remove/${trackId}`, {
                headers: { authorization: `Bearer ${authStore.token}` }
            });
            // Actualizar la lista después de eliminar la canción
            await fetchUserPlaylists();
        } catch (err: any) {
            error.value = err.message || "Error al eliminar la canción de la playlist";
            throw error.value;
        } finally {
            isLoading.value = false;
        }
    }

    function setActivePlaylist(id: number | null) {
        activePlaylistId.value = id;
    }

    const activePlaylist = computed(() => {
        return playlists.value.find(p => p.id === activePlaylistId.value) || null;
    });

    return {
        playlists,
        activePlaylistId,
        activePlaylist,
        isLoading,
        error,
        fetchUserPlaylists,
        createPlaylist,
        updatePlaylist,
        deletePlaylist,
        addTrackToPlaylist,
        removeTrackFromPlaylist,
        setActivePlaylist
    };
});
