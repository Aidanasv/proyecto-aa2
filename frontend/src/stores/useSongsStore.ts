import { defineStore } from "pinia";
import { ref } from "vue";
import axios from "axios";
import type { AlbumSong, createSong, Song } from "@/types/songs";
import { useAuthStore } from "./useAuthStore";

export const useSongStore = defineStore("songStore", () => {

    const songs = ref<AlbumSong | null>(null);
    const isloading = ref<boolean>(false);
    const error = ref<string | null>(null);
    const authStore = useAuthStore();

    async function fetchSongsByAlbum(id: string): Promise<void> {
        isloading.value = true;
        error.value = null;

        try {
            const response = await axios.get<AlbumSong>(`http://localhost:5053/albums/${id}/tracks`,)
            songs.value = response.data
        } catch (err: any) {
            error.value = err.message || "Error al obtener canciones";
        } finally {
            isloading.value = false;
        }  
    }

    async function addSongs(song: createSong) {
        isloading.value = true;
        error.value = null;

        try {
            const response = await axios.post("http://localhost:5053/tracks",
                song, { headers: { authorization: `Bearer ${authStore.token}` } })
        }catch (err: any) {
            error.value = err.message || "Error al añadir canciones"
        } finally {
            isloading.value = false;
            fetchSongsByAlbum(song.albumId.toString());
        }        
    }

    async function deleteSongs(song: Song) {
        isloading.value = true;
        error.value = null;

        try{
            const response = await axios.delete(`http://localhost:5053/tracks/${song.id}`,
                 { headers: { authorization: `Bearer ${authStore.token}` } })
        } catch (err: any) {
            error.value = err.message || "Error al eliminar canción"
        } finally {
            isloading.value = false;
            fetchSongsByAlbum(song.albumId.toString());
        }
    }

    async function updateSongs(song:Song) {
        isloading.value = true;
        error.value = null;

        try {
            const response = await axios.put(`http://localhost:5053/tracks/${song.id}`, song, { headers: { authorization: `Bearer ${authStore.token}` } })
        }catch (err: any) {
            error.value = err.message || "Error al modificar canción"
        } finally {
            isloading.value = false;
            fetchSongsByAlbum(song.albumId.toString());
        }
        
    }

    function resetSongs(): void {
        songs.value = null
    }

    return {
        songs,
        isloading,
        error,
        fetchSongsByAlbum,
        resetSongs,
        addSongs,
        deleteSongs,
        updateSongs,
    }
})