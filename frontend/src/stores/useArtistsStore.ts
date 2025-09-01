import { defineStore } from "pinia";
import { ref } from "vue";
import axios from "axios";
import type { Artist, NewArtist } from "@/types/artists";
import { useAuthStore } from "./useAuthStore";
import { buildApiUrl, API_ENDPOINTS } from '@/config/api';

export const useArtistsStore = defineStore("artistsStore", () => {

    const artists = ref<Artist[]>([]);
    const isLoading = ref<boolean>(false);
    const error = ref<string | null>(null);
    const authStore = useAuthStore();

    interface ArtistFilters {
        name?: string;
        nameOrder?: boolean | null;
        followers?: number;
        followersOrder?: boolean | null;
    }

    async function fetchArtists(filters?: ArtistFilters): Promise<void> {
        isLoading.value = true;
        error.value = null;

        try {
            let url = buildApiUrl(API_ENDPOINTS.ARTISTS);
            if (filters) {
                const params = new URLSearchParams();
                if (filters.name) params.append('Name', filters.name);
                if (filters.nameOrder !== null && filters.nameOrder !== undefined) {
                    params.append('NameOrder', filters.nameOrder.toString());
                }
                if (filters.followers !== undefined) params.append('Followers', filters.followers.toString());
                if (filters.followersOrder !== null && filters.followersOrder !== undefined) {
                    params.append('FollowersOrder', filters.followersOrder.toString());
                }
                url += `?${params.toString()}`;
            }
            const response = await axios.get<Artist[]>(url);
            artists.value = response.data;
        } catch (err: any) {
            error.value = err.message || "Error al obtener artistas";
        } finally {
            isLoading.value = false;
        }
    }

    async function addArtist(artist: NewArtist) {
        isLoading.value = true;
        error.value = null;
        try {
            const response = await axios.post(buildApiUrl(API_ENDPOINTS.ARTISTS), artist,
                { headers: { authorization: `Bearer ${authStore.token}` } }
            );
            artists.value.push(response.data);
        } catch (err: any) {
            error.value = err.message || "Error creating artist";
        } finally {
            isLoading.value = false;
        }
    }

    async function deleteArtist(artistId: number) {
        isLoading.value = true;
        error.value = null;
        try {
            await axios.delete(`${buildApiUrl(API_ENDPOINTS.ARTISTS)}/${artistId}`, {
                headers: { authorization: `Bearer ${authStore.token}` }
            });
            artists.value = artists.value.filter(artist => artist.id !== artistId);
        } catch (err: any) {
            error.value = err.message || "Error deleting artist";
        } finally {
            isLoading.value = false;
        }
    }

    async function updateArtist(artistId: number, artist: NewArtist) {
        isLoading.value = true;
        error.value = null;
        try {
            const response = await axios.put(`${buildApiUrl(API_ENDPOINTS.ARTISTS)}/${artistId}`, artist, {
                headers: { authorization: `Bearer ${authStore.token}` }
            });
            const index = artists.value.findIndex(a => a.id === artistId);
            if (index !== -1) {
                artists.value[index] = response.data;
            }
        } catch (err: any) {
            error.value = err.message || "Error updating artist";
        } finally {
            isLoading.value = false;
        }
    }

    function resetArtists(): void {
        artists.value = [];
    }

    return {
        artists,
        isLoading,
        error,
        fetchArtists,
        resetArtists,
        addArtist,
        deleteArtist,
        updateArtist,
    }
});