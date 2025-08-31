export interface Artist {
    id: number;
    name: string;
    followers: number;
    biography: string;
    imagen: string;
    createDate: string;
    softDelete: boolean;
}

export interface NewArtist {
  name: string;
  followers: number;
  biography: string;
  createDate: string;
  imagen: string;
  softDelete: boolean;
}