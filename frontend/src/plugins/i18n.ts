import SongForm from '@/components/SongForm.vue'
import { createI18n } from 'vue-i18n'

const messages = {
    en: {
        login: 'Login',
        logout: 'Logout',
        home: 'Home',
        addNewSong: 'Add new song',
        songForm: 'Song Form',
        titleSong: 'Song title',
        alertTitle: 'The song title is a required field',
        alertDuration: 'The duration is a required field',
        duration: 'Duration',
        close: 'Close',
        save: 'Save',
        update: 'Update',
        delete: 'Delete',
        buttonNewSong: 'Add New Song',
        artists: 'Artists',
        search: 'Search',
        email : 'Email',
        password: 'Password',
    },
    es: {
        login: 'Iniciar sesión',
        logout: 'Cerrar sesión',
        home: 'Inicio',
        addNewSong: 'Agregar nueva canción',
        songForm: 'Formulario de canciones',
        titleSong: 'Título de la canción',
        alertTitle: 'El título de la canción es un campo obligatorio',
        alertDuration: 'La duración es un campo obligatorio',
        duration: 'Duración',
        close: 'Cerrar',
        save: 'Guardar',
        update: 'Actualizar',
        delete: 'Eliminar',
        buttonNewSong: 'Agregar nueva canción',
        artists: 'Artistas',
        search: 'Buscar',
        email : 'Correo',
        password: 'Contraseña',


    }
}

export const i18n = createI18n({
    legacy: false,
    locale: 'es',
    fallbackLocale: 'en',
    messages
})
