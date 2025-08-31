import { defineStore } from "pinia";
import { ref, computed } from "vue";
import axios from "axios";
import type { Playlist, NewPlaylist } from "@/types/playlist";
import { useAuthStore } from "./useAuthStore";

export const usePlaylistStore = defineStore("playlistStore", () => {
    const playlists = ref<Playlist[]>([]);
    const activePlaylistId = ref<number | null>(null);
    const isLoading = ref<boolean>(false);
    const error = ref<string | null>(null);
    const authStore = useAuthStore();

    async function fetchUserPlaylists(): Promise<void> {
        isLoading.value = true;
        error.value = null;

        try {
            const response = await axios.get<Playlist[]>('http://localhost:5053/playlists/user', {
                headers: { authorization: `Bearer ${authStore.token}` }
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
            const response = await axios.post('http://localhost:5053/playlists', playlist, {
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
            const response = await axios.put(`http://localhost:5053/playlists/${id}`, playlist, {
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
            await axios.delete(`http://localhost:5053/playlists/${id}`, {
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
            await axios.post(`http://localhost:5053/playlists/${playlistId}/add/${trackId}`, {}, {
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
            await axios.delete(`http://localhost:5053/playlists/${playlistId}/remove/${trackId}`, {
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
