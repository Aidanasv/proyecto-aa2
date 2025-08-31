namespace Services;

using Data;
using Models;

public class ArtistService : IArtistService
{
    private readonly IArtistEfRepository _artistRepository;

    public ArtistService(IArtistEfRepository artistRepository)
    {
        _artistRepository = artistRepository;
    }

    public async Task<List<Artist>> GetAllAsync(ArtistDtoParameters artistDtoParameters)
    {
        return await _artistRepository.GetAllAsync(artistDtoParameters);
    }

    public async Task<Artist?> GetByIdAsync(int id)
    {
        return await _artistRepository.GetByIdAsync(id);
    }

    public async Task<Artist> AddAsync(ArtistCreate artistCreate)
    {
        var artist = new Artist
        {
            Name = artistCreate.Name,
            Followers = artistCreate.Followers,
            Biography = artistCreate.Biography,
            CreateDate = artistCreate.CreateDate,
            Imagen = artistCreate.Imagen,
            SoftDelete = artistCreate.SoftDelete
        };

        await _artistRepository.AddAsync(artist);
        return artist;
    }

    public async Task<Artist> UpdateAsync(ArtistCreate artist, int id)
    {
        var updatedArtist = await _artistRepository.GetByIdAsync(id);
        if (updatedArtist == null)
        {
            throw KeyNotFoundException("Artista no encontrado");
        }

        updatedArtist.Name = artist.Name;
        updatedArtist.Biography = artist.Biography;
        updatedArtist.Followers = artist.Followers;
        updatedArtist.Imagen = artist.Imagen;

        await _artistRepository.UpdateAsync(updatedArtist);
        return updatedArtist;
    }

    public async Task DeleteAsync(int id)
    {
        var artist = await _artistRepository.GetByIdAsync(id);
        if (artist == null)
        {
            throw KeyNotFoundException("Artista no encontrado");
        }
        artist.SoftDelete = true;

        await _artistRepository.UpdateAsync(artist);
    }

    
    public async Task<ArtistDto?> GetAlbumsByArtist(int id)
    {
        var artist = await _artistRepository.GetAlbumsByArtist(id);
        if (artist == null)
        {
            throw KeyNotFoundException("Artista no encontrado");
        }
        var artistDto = new ArtistDto
        {
            Name = artist.Name,
            Biography = artist.Biography,
            Id = artist.Id,
            Followers = artist.Followers,
            Imagen = artist.Imagen,
            Albums = artist.Albums.Select(album => new AlbumDto
            {
                Id = album.Id,
                Name = album.Name,
                ReleaseDate = album.ReleaseDate,
                Imagen = album.Imagen,
                ArtistId = album.ArtistId

            }).ToList(),
        };
        return artistDto;
    }

    private Exception KeyNotFoundException(string v)
    {
        throw new NotImplementedException();
    }
}