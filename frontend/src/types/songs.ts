import type { Album } from "@/types/albums";

export interface createSong {
    name: string,
    artistId: number,
    albumId: number,
    duration: number,
    releaseDate: Date,
    plays: number,
    softDelete: boolean
}

export interface Song extends createSong {
    id?: number
}

export interface Track {
    name : string;
    id: number;
    duration: number;
    

}

export interface AlbumSong extends Album {
    artistId: number;
    tracks: Track[];
}