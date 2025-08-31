<script setup lang="ts">
import { ref, watch, computed } from 'vue'
import { useSongStore } from '@/stores/useSongsStore'
import { defineProps, defineEmits } from 'vue'
import type { Song } from '@/types/songs';
import { useI18n } from 'vue-i18n';
import { useNotificationStore } from '@/stores/useNotificationStore';

const { t } = useI18n()
const notificationStore = useNotificationStore()

// Props y emits
const props = defineProps<{
    modelValue: boolean,
    albumId: string,
    artistId: number,
    song: Song | undefined
}>()

const emit = defineEmits(['update:modelValue', 'update:song'])

// Proxy para usar v-model en el <v-dialog>
const dialogModel = computed({
    get: () => props.modelValue,
    set: (value: boolean) => emit('update:modelValue', value)
})

// Campos reactivos del formulario
const idSong = ref<string | undefined>(undefined)
const name = ref("")
const duration = ref(props.song?.duration?.toString() || "")
const releaseDate = ref<Date | null>(props.song?.releaseDate ? new Date(props.song.releaseDate) : new Date())
const artistId = ref(props.song?.artistId || props.artistId)

const nameRules = [
    (value: string | any[]) => value?.length >= 3 || t('alertname'),
]

const durationRules = [
    (value: string) => !/[^0-9]/.test(value) || t('alertDuration'),
]

const releaseDateRules = [
    (value: Date | null) => value instanceof Date && !isNaN(value.getTime()) || t('alertReleaseDate'),
]

// Watch para sincronizar cuando cambia la canción
watch(() => props.song, (newSong) => {
    idSong.value = newSong?.id?.toString() || undefined
    name.value = newSong?.name || ''
    duration.value = newSong?.duration?.toString() || ''
    releaseDate.value = newSong?.releaseDate ? new Date(newSong.releaseDate) : new Date()
    artistId.value = newSong?.artistId || props.artistId
}, { immediate: true })

// Store
const songStore = useSongStore()

function closeDialog() {
    emit('update:modelValue', false)
    emit('update:song', undefined)
    idSong.value = undefined
    name.value = ''
    duration.value = ''
    releaseDate.value = new Date()
}

async function deleteSongs(song: Song) {
    try {
        await songStore.deleteSongs(song)
        notificationStore.showSuccess('Canción eliminada del álbum exitosamente')
        closeDialog()
    } catch (error) {
        notificationStore.showError('Error al eliminar la canción del álbum')
        console.error('Error:', error)
    }
}

async function submit() {
    if (name.value && duration.value && releaseDate.value) {
        const payload = {
            id: idSong.value ? parseInt(idSong.value) : undefined,
            name: name.value,
            duration: parseInt(duration.value),
            albumId: parseInt(props.albumId),
            artistId: artistId.value,
            plays: 0,
            releaseDate: releaseDate.value,
            softDelete: false
        }

        try {
            if (!payload.id) {
                await songStore.addSongs(payload)
                notificationStore.showSuccess('Canción agregada exitosamente 🎵')
            } else {
                await songStore.updateSongs(payload)
                notificationStore.showSuccess('Canción actualizada exitosamente 🎵')
            }
            closeDialog()
        } catch (error) {
            notificationStore.showError('Error al ' + (!payload.id ? 'agregar' : 'actualizar') + ' la canción')
            console.error('Error:', error)
        }
    }
}
</script>

<template>
    <v-dialog v-model="dialogModel" max-width="500">
        <v-card :title="$t('songForm')">
            <v-form fast-fail @submit.prevent="submit">
                <v-card-item>
                    <v-text-field v-model="name" :rules="nameRules" :label="$t('nameSong')" />
                    <v-text-field v-model="duration" :rules="durationRules" :label="$t('duration')" />

                    <!-- Date Picker -->
                    <v-menu v-slot:activator="{ props: menuProps }" transition="scale-transition" offset-y>
                        <v-text-field v-bind="menuProps" v-model="releaseDate" :label="$t('releaseDate')"
                            :rules="releaseDateRules" readonly
                            :value="releaseDate ? releaseDate.toLocaleDateString() : ''" />
                        <v-date-picker v-model="releaseDate" />
                    </v-menu>
                </v-card-item>

                <v-card-actions>
                    <v-spacer></v-spacer>

                    <v-btn :text="$t('close')" @click="closeDialog"></v-btn>
                    <v-btn :text="$t('delete')" color="error" v-if="props.song"
                        @click="deleteSongs(props.song)"></v-btn>
                    <v-btn type="submit" color="success">
                        {{ idSong ? $t('update') : $t('save') }}
                    </v-btn>
                </v-card-actions>
            </v-form>
        </v-card>
    </v-dialog>
</template>
