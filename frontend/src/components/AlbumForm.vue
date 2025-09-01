<template>
    <v-dialog v-model="show" persistent max-width="600px">
        <v-card>
            <v-card-title>
                <span class="text-h5">Nuevo Álbum</span>
            </v-card-title>
            <v-card-text>
                <v-container>
                    <v-form @submit.prevent="submitForm">
                        <v-row>
                            <v-col cols="12">
                                <v-text-field
                                    v-model="album.name"
                                    label="Nombre del álbum"
                                    required
                                    variant="outlined"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                                <v-text-field
                                    v-model="album.imagen"
                                    label="URL de la imagen"
                                    variant="outlined"
                                ></v-text-field>
                            </v-col>
                            <v-col cols="12">
                                <v-text-field
                                    v-model="album.releaseDate"
                                    label="Fecha de lanzamiento"
                                    type="date"
                                    variant="outlined"
                                ></v-text-field>
                            </v-col>
                        </v-row>
                    </v-form>
                </v-container>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue-darken-1" variant="text" @click="close">
                    Cancelar
                </v-btn>
                <v-btn color="blue-darken-1" variant="text" @click="submitForm">
                    Guardar
                </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useAlbumStore } from '@/stores/useAlbumsStore';
import { useNotificationStore } from '@/stores/useNotificationStore';
import type { Album } from '@/types/albums';

const props = defineProps({
    modelValue: Boolean,
    artistId: {
        type: String,
        required: true
    }
});

const emit = defineEmits(['update:modelValue']);

const show = computed({
    get: () => props.modelValue,
    set: (value) => emit('update:modelValue', value)
});

const albumStore = useAlbumStore();
const notificationStore = useNotificationStore();

const album = ref({
    name: '',
    releaseDate: new Date().toISOString().split('T')[0],
    imagen: ''
});

const submitForm = async () => {
    try {
        await albumStore.addAlbum({
            ...album.value,
            artistId: props.artistId
        });
        notificationStore.showSuccess('Álbum creado exitosamente');
        close();
    } catch (error) {
        notificationStore.showError('Error al crear el álbum');
        console.error('Error al crear el álbum:', error);
    }
};

const close = () => {
    show.value = false;
    album.value = {
        name: '',
        releaseDate: new Date().toISOString().split('T')[0],
        imagen: ''
    };
};
</script>
