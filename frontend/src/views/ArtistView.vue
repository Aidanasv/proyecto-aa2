<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useArtistsStore } from '@/stores/useArtistsStore';
import { useRouter } from 'vue-router';
import ArtistForm from '@/components/ArtistForm.vue';
import { useAuthStore } from '@/stores/useAuthStore';
import { useNotificationStore } from '@/stores/useNotificationStore';
import type { Artist } from '@/types/artists';
import { formatNumber } from '@/utils/FormatUtils';

const artistsStore = useArtistsStore();
const router = useRouter();
const authStore = useAuthStore();
const notificationStore = useNotificationStore();
const showDialog = ref(false);
const showConfirmDialog = ref(false);
const artistToDelete = ref<number | null>(null);
const selectedArtist = ref<Artist | null>(null);

// Filtros
const nameFilter = ref('');
const nameOrderAsc = ref<boolean | null>(null);
const followersFilter = ref<number | null>(null);
const followersOrderAsc = ref<boolean | null>(null);

const applyFilters = async () => {
    await artistsStore.fetchArtists({
        name: nameFilter.value || undefined,
        nameOrder: nameOrderAsc.value === undefined ? null : nameOrderAsc.value,
        followers: followersFilter.value || undefined,
        followersOrder: followersOrderAsc.value === undefined ? null : followersOrderAsc.value
    });
};

const toggleNameOrder = () => {
    // Ciclo: null -> true -> false -> null
    if (nameOrderAsc.value === null) nameOrderAsc.value = true;
    else if (nameOrderAsc.value === true) nameOrderAsc.value = false;
    else nameOrderAsc.value = null;
    applyFilters();
};

const toggleFollowersOrder = () => {
    // Ciclo: null -> true -> false -> null
    if (followersOrderAsc.value === null) followersOrderAsc.value = true;
    else if (followersOrderAsc.value === true) followersOrderAsc.value = false;
    else followersOrderAsc.value = null;
    applyFilters();
};

onMounted(() => {
    applyFilters();
})

function editArtist(artist: Artist, event: Event) {
    if (authStore.isAuthenticated && authStore.user?.role === 'admin') {
        event.stopPropagation();
        selectedArtist.value = artist;
        showDialog.value = true;
    }
}

function addNewArtist() {
    selectedArtist.value = null;
    showDialog.value = true;
}

function goToAlbum(id: number) {
    router.push({ name: "albums", params: { id } })
}

function confirmDelete(id: number) {
    artistToDelete.value = id;
    showConfirmDialog.value = true;
}

async function removeArtist() {
    if (artistToDelete.value) {
        try {
            await artistsStore.deleteArtist(artistToDelete.value);
            notificationStore.showSuccess('Artista eliminado exitosamente');
            showConfirmDialog.value = false;
            artistToDelete.value = null;
        } catch (error) {
            notificationStore.showError('Error al eliminar el artista');
            console.error('Error:', error);
        }
    }
}
</script>

<template>
    <v-container>
        <!-- Cabecera -->
        <v-row class="mb-4">
            <v-col cols="12" class="d-flex justify-space-between align-center">
                <h1 class="text-h4">Artistas</h1>
                <v-btn v-if="authStore.isAuthenticated && authStore.user?.role === 'admin'" color="primary"
                    prepend-icon="mdi-plus" @click="addNewArtist">
                    Nuevo Artista
                </v-btn>
            </v-col>
        </v-row>

        <!-- Filtros -->
        <v-row class="mb-4">
            <v-col cols="12" md="6">
                <v-text-field v-model="nameFilter" label="Buscar por nombre" prepend-inner-icon="mdi-magnify" clearable
                    @update:model-value="applyFilters" @clear="nameFilter = ''; applyFilters()">
                    <template v-slot:append>
                        <v-btn icon @click="toggleNameOrder">
                            <v-icon>{{ 
                                nameOrderAsc === null ? 'mdi-sort-variant-remove' :
                                nameOrderAsc ? 'mdi-sort-alphabetical-ascending' : 'mdi-sort-alphabetical-descending' 
                            }}</v-icon>
                        </v-btn>
                    </template>
                </v-text-field>
            </v-col>
            <v-col cols="12" md="6">
                <v-text-field v-model="followersFilter" type="number" label="Filtrar por seguidores"
                    prepend-inner-icon="mdi-account-group" clearable @update:model-value="applyFilters"
                    @clear="followersFilter = null; applyFilters()">
                    <template v-slot:append>
                        <v-btn icon @click="toggleFollowersOrder">
                            <v-icon>{{ 
                                followersOrderAsc === null ? 'mdi-sort-variant-remove' :
                                followersOrderAsc ? 'mdi-sort-numeric-ascending' : 'mdi-sort-numeric-descending' 
                            }}</v-icon>
                        </v-btn>
                    </template>
                </v-text-field>
            </v-col>
        </v-row>

        <!-- Loading indicator -->
        <v-row v-if="artistsStore.isLoading">
            <v-col cols="12" class="text-center">
                <v-progress-circular indeterminate color="primary"></v-progress-circular>
            </v-col>
        </v-row>

        <!-- Error message -->
        <v-row v-else-if="artistsStore.error">
            <v-col cols="12">
                <v-alert type="error">
                    {{ artistsStore.error }}
                </v-alert>
            </v-col>
        </v-row>

        <!-- Artists grid -->
        <v-row v-else>
            <v-col v-for="artist in artistsStore.artists" :key="artist.id" cols="12" sm="6" md="4" lg="3" class="pa-2">
                <v-card class="mx-auto" max-width="250" color="primary" @click="goToAlbum(artist.id)">
                    <div class="d-flex justify-end" style="position: absolute; right: 4px; top: 4px; z-index: 1;">
                        <v-btn v-if="authStore.isAuthenticated && authStore.user?.role === 'admin'" icon="mdi-pencil"
                            density="compact" size="small" variant="text" color="white" style="opacity: 0.7;"
                            @click="editArtist(artist, $event)" class="edit-btn mr-2"></v-btn>
                        <v-btn v-if="authStore.isAuthenticated && authStore.user?.role === 'admin'" icon="mdi-close"
                            density="compact" size="small" variant="text" color="white" style="opacity: 0.7;"
                            @click.stop="confirmDelete(artist.id)" class="delete-btn"></v-btn>
                    </div>

                    <v-card-title>{{ artist.name }}</v-card-title>
                    <v-card-subtitle class="pt-4">
                        <v-icon>mdi-account-group</v-icon>
                        {{ formatNumber(artist.followers) }} seguidores
                    </v-card-subtitle>

                    <v-img :src="artist.imagen" color="surface-variant" height="200" cover></v-img>

                    <v-card-text>
                        <div>{{ artist.biography }}</div>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
    </v-container>

    <!-- Forms and Dialogs -->
    <ArtistForm v-model="showDialog" :artist="selectedArtist" />

    <!-- Confirmation Dialog -->
    <v-dialog v-model="showConfirmDialog" max-width="500px">
        <v-card>
            <v-card-title>Confirmar eliminación</v-card-title>
            <v-card-text>¿Está seguro que desea eliminar este artista?</v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="error" @click="removeArtist">Eliminar</v-btn>
                <v-btn @click="showConfirmDialog = false">Cancelar</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
