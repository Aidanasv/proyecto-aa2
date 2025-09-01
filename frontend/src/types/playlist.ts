import type { Track } from './songs';

export interface Playlist {
    id: number;
    name: string;
    description: string;
    tracks: Track[];
}

export interface NewPlaylist {
    name: string;
    description: string;
}

export interface PlaylistFilters {
    name?: string;
    nameOrder?: boolean | null;
    description?: string;
    descriptionOrder?: boolean | null;
}
