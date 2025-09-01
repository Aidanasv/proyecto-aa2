export interface Album {
    id: number;
    name: string;
    releaseDate: number;
    imagen : string;

}

export interface ArtistAlbum{
    	
  id: number,
  name: string,
  biography: string,
  imagen: string,
  followers: number,
  albums: Album[];
}