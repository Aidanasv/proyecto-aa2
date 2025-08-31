import { defineStore } from "pinia";
import { ref } from "vue";
import axios from "axios";
import type { Search } from "@/types/search";

export const useSearchStore = defineStore("searchStore", () => {

    //Estado
    const search = ref<Search[]>([]);
    const isLoading = ref<boolean>(false);
    const error = ref<string | null>(null);

    //Obtener Artist
    async function fetchAll(key: string): Promise<void> {
        isLoading.value = true;
        error.value = null;

        search.value = []

        try {
            const response = await axios.get<Search[]>(`http://127.0.0.1:3000/search?q=${key}`)
            console.log(response.data);
            search.value = response.data;
        } catch (err: any) {
            error.value = err.message || "Error al obtener busqueda";
        } finally {
            isLoading.value = false;
        }
    }

    return {
        search,
        isLoading,
        error,
        fetchAll,
    }
})