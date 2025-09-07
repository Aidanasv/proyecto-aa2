// Configuración de la API
const getApiBaseUrl = (): string => {
  // En desarrollo, usar localhost directamente
  if (import.meta.env.DEV) {
    return 'http://localhost:7818';
  }
  
  // En producción (Docker), usar localhost ya que los puertos están expuestos
  return 'http://localhost:7818';
};

export const API_BASE_URL = getApiBaseUrl();

// URLs específicas de la API
export const API_ENDPOINTS = {
  AUTH: {
    LOGIN: '/auth/Login',
    REGISTER: '/auth/Register',
  },
  ARTISTS: '/artists',
  ALBUMS: '/albums',
  TRACKS: '/tracks',
  PLAYLISTS: '/playlists',
  AUDIOSONGS: '/tracks/audio',
  UPDATE_PLAYS: '/tracks/audio',
} as const;

// Helper function para construir URLs completas
export const buildApiUrl = (endpoint: string): string => {
  return `${API_BASE_URL}${endpoint}`;
};
