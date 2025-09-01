<template>
    <v-dialog v-model="show" persistent max-width="600px">
        <v-card>
            <v-card-title>
                <span class="text-h5">{{ isEdit ? 'Editar Artista' : 'Nuevo Artista' }}</span>
            </v-card-title>
            <v-card-text>
                <v-container>
                    <v-form ref="form" @submit.prevent="submitForm">
                        <v-row>
                            <v-col cols="12">
                                <v-text-field 
                                    v-model="artistData.name" 
                                    label="Nombre del artista" 
                                    required
                                    variant="outlined"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                                <v-text-field 
                                    v-model.number="artistData.followers"
                                    label="Seguidores"
                                    type="number"
                                    required
                                    variant="outlined"
                                    :hint="artistData.followers ? formatNumber(artistData.followers) : ''"
                                    persistent-hint
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                                <v-textarea 
                                    v-model="artistData.biography" 
                                    label="Biografía"
                                    variant="outlined"
                                ></v-textarea>
                            </v-col>
                            <v-col cols="12">
                                <v-text-field 
                                    v-model="artistData.imagen" 
                                    label="URL de la imagen"
                                    variant="outlined"
                                ></v-text-field>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-container>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue-darken-1" variant="text" @click="close">Cancelar</v-btn>
                <v-btn color="blue-darken-1" variant="text" @click="submitForm">Guardar</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue';
import { useArtistsStore } from '@/stores/useArtistsStore';
import { useNotificationStore } from '@/stores/useNotificationStore';
import type { Artist } from '@/types/artists';
import { formatNumber } from '@/utils/FormatUtils';

const props = defineProps({
    modelValue: Boolean,
    artist: {
        type: Object as () => Artist | null,
        default: null
    }
});

const emit = defineEmits(['update:modelValue']);

const show = computed({
    get: () => props.modelValue,
    set: (value) => emit('update:modelValue', value)
});

const artistsStore = useArtistsStore();
const notificationStore = useNotificationStore();
const isEdit = computed(() => props.artist !== null);

const artistData = ref<Artist>({
    id: 0,
    name: '',
    followers: 0,
    biography: '',
    imagen: '',
    createDate: new Date().toISOString(),
    softDelete: false
});

const resetForm = () => {
    artistData.value = {
        id: 0,
        name: '',
        followers: 0,
        biography: '',
        createDate: new Date().toISOString(),
        imagen: '',
        softDelete: false
    };
};

watch(() => props.artist, (newArtist) => {
    if (newArtist) {
        artistData.value = { ...newArtist };
    } else {
        resetForm();
    }
}, { immediate: true });

const close = () => {
    show.value = false;
    resetForm();
};

const submitForm = async () => {
    try {
        if (isEdit.value && props.artist) {
            await artistsStore.updateArtist(artistData.value.id, artistData.value);
            notificationStore.showSuccess('Artista actualizado exitosamente');
        } else {
            await artistsStore.addArtist(artistData.value);
            notificationStore.showSuccess('Artista creado exitosamente');
        }
        resetForm();
        close();
    } catch (error) {
        notificationStore.showError('Error al ' + (isEdit.value ? 'actualizar' : 'crear') + ' el artista');
        console.error('Error:', error);
    }
};
</script>
