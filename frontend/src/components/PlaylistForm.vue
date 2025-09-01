<template>
    <v-dialog v-model="show" persistent max-width="500px">
        <v-card>
            <v-card-title>
                <span class="text-h5">{{ isEdit ? 'Editar Playlist' : 'Nueva Playlist' }}</span>
            </v-card-title>
            <v-card-text>
                <v-container>
                    <v-row>
                        <v-col cols="12">
                            <v-text-field
                                v-model="formData.name"
                                label="Nombre de la playlist"
                                required
                                variant="outlined"
                            ></v-text-field>
                        </v-col>
                        <v-col cols="12">
                            <v-textarea
                                v-model="formData.description"
                                label="Descripción"
                                variant="outlined"
                                rows="3"
                            ></v-textarea>
                        </v-col>
                    </v-row>
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
import { ref, computed } from 'vue';
import { usePlaylistStore } from '@/stores/usePlaylistStore';
import { useNotificationStore } from '@/stores/useNotificationStore';
import type { Playlist, NewPlaylist } from '@/types/playlist';

const props = defineProps({
    modelValue: Boolean,
    playlist: {
        type: Object as () => Playlist | null,
        default: null
    }
});

const emit = defineEmits(['update:modelValue']);

const show = computed({
    get: () => props.modelValue,
    set: (value) => emit('update:modelValue', value)
});

const playlistStore = usePlaylistStore();
const notificationStore = useNotificationStore();
const isEdit = computed(() => props.playlist !== null);

const playlist = ref<NewPlaylist>({
    name: '',
    description: ''
});

const formData = computed(() => {
    if (props.playlist) {
        return {
            name: props.playlist.name,
            description: props.playlist.description ?? ''
        };
    }
    return playlist.value;
});

const submitForm = async () => {
    try {
        const playlistData: NewPlaylist = {
            name: formData.value.name,
            description: formData.value.description
        };

        if (isEdit.value && props.playlist) {
            await playlistStore.updatePlaylist(props.playlist.id, playlistData);
            notificationStore.showSuccess('Playlist actualizada exitosamente');
        } else {
            await playlistStore.createPlaylist(playlistData);
            notificationStore.showSuccess('Playlist creada exitosamente');
        }
        close();
    } catch (error) {
        notificationStore.showError('Error al ' + (isEdit.value ? 'actualizar' : 'crear') + ' la playlist');
        console.error('Error:', error);
    }
};

const close = () => {
    show.value = false;
    playlist.value = {
        name: '',
        description: ''
    };
};
</script>
