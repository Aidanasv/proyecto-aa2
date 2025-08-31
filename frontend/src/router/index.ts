import { createRouter, createWebHistory } from 'vue-router'
import 'vue-router'

declare module 'vue-router' {
  interface RouteMeta {
    requiresAuth?: boolean
    roles?: string[]
  }
}
import ArtistView from '../views/ArtistView.vue'
import AlbumsView from '../views/AlbumsView.vue'
import SongsView from '../views/SongsView.vue'
import HomeView from '@/views/HomeView.vue'
import { useAuthStore } from '@/stores/useAuthStore'
import LoginView from '@/views/LoginView.vue'
import PlaylistView from '../views/PlaylistView.vue'


const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/artists',
      name: 'artists',
      component: ArtistView,
    },
    {
      path: '/albums/:id',
      name: 'albums',
      component: () => AlbumsView,
      props: true,
    },
    {
      path: '/playlists',
      name: 'playlists',
      component: PlaylistView,
      meta: { requiresAuth: true, roles: ['client'] }
    },
    {
      path: '/songs/:id',
      name: 'songs',
      component: () => SongsView,
      props: true,

    },
    { path: '/login', 
      name: 'login', 
      component: LoginView },
  ],
})

router.beforeEach((to, from, next) => {
  const auth = useAuthStore()
  
  if (to.meta.requiresAuth) {
    if (!auth.isAuthenticated) {
      next({ name: 'login' })
      return
    }
    
    if (to.meta.roles && auth.user?.role && !to.meta.roles.includes(auth.user.role)) {
      next({ name: 'home' }) // Redirigir a home si no tiene el rol adecuado
      return
    }
  }
  
  next()
})

export default router
