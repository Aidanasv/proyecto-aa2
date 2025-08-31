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
