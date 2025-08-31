import { defineStore } from "pinia";
import { ref } from "vue";
import axios from "axios";
import type { Album, ArtistAlbum } from "@/types/albums";
import { useAuthStore } from "./useAuthStore";

export const useAlbumStore = defineStore("albumStore", () => {

    const artist = ref<ArtistAlbum | null>(null);
    const isLoading = ref<boolean>(false);
    const error = ref<string | null>(null);
    const authStore = useAuthStore();

    async function fetchAlbumsByArtists(id: string): Promise<void> {
        isLoading.value = true;
        error.value = null;

        try {
            const response = await axios.get<ArtistAlbum>(`http://localhost:5053/artists/${id}/albums`)
            artist.value = response.data
        } catch (err: any) {
            error.value = err.message || "Error al obtener albumes"
        } finally {
            isLoading.value = false
        }
    }

    async function addAlbum(albumData: { name: string, releaseDate: string, imagen: string, artistId: string }) {
        isLoading.value = true;
        error.value = null;
        try {
            const response = await axios.post(
                `http://localhost:5053/albums`, 
                {
                    ...albumData,
                    releaseDate: new Date(albumData.releaseDate).toISOString(),
                    softDelete: false
                },
                {
                    headers: { authorization: `Bearer ${authStore.token}` }
                }
            );
            
            if (artist.value && artist.value.albums) {
                artist.value.albums.push(response.data);
            }
            return response.data;
        } catch (err: any) {
            error.value = err.message || "Error al crear el álbum";
            throw error.value;
        } finally {
            isLoading.value = false;
        }
    }

    function resetAlbums(): void {
        artist.value = null
    }

    return {
        artist,
        isLoading,
        error,
        fetchAlbumsByArtists,
        resetAlbums,
        addAlbum
    }
})