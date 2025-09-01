namespace Services;

using System;
using Data;
using Models;

public class AlbumService : IAlbumService
{
    private readonly IAlbumEfRepository _albumRepository;

    public AlbumService(IAlbumEfRepository albumRepository)
    {
        _albumRepository = albumRepository;
    }

    public async Task<List<Album>> GetAllAsync()
    {
        return await _albumRepository.GetAllAsync();
    }

    public async Task<Album?> GetByIdAsync(int id)
    {
        return await _albumRepository.GetByIdAsync(id);
    }

    public async Task<Album> AddAsync(AlbumCreate albumCreate)
    {
        var album = new Album
        {
            Name = albumCreate.Name,
            ArtistId = albumCreate.ArtistId,
            ReleaseDate = albumCreate.ReleaseDate,
            Duration = albumCreate.Duration,
            Imagen = albumCreate.Imagen,
            SoftDelete = albumCreate.SoftDelete

        };
        await _albumRepository.AddAsync(album);
        return album;
    }

    public async Task<Album> UpdateAsync(AlbumCreate album, int id)
    {
        var updatedAlbum = await _albumRepository.GetByIdAsync(id);
        if (updatedAlbum == null)
        {
            throw KeyNotFoundException("Albúm no encontrado");
        }

        updatedAlbum.Name = album.Name;
        updatedAlbum.Duration = album.Duration;
        updatedAlbum.ReleaseDate = album.ReleaseDate;
        updatedAlbum.Imagen = album.Imagen;

        await _albumRepository.UpdateAsync(updatedAlbum);
        return updatedAlbum;
    }

    public async Task DeleteAsync(int id)
    {
        var album = await _albumRepository.GetByIdAsync(id);
        if (album == null)
        {
            throw KeyNotFoundException("Albúm no encontrado");
        }
        album.SoftDelete = true;

        await _albumRepository.UpdateAsync(album);
    }

    public async Task<AlbumTrackDTO?> GetTracksByAlbum(int id)
    {
        var album = await _albumRepository.GetTracksByAlbum(id);
        if (album == null)
        {
            throw KeyNotFoundException("Album no encontrado");
        }
        var trackDTO = new AlbumTrackDTO
        {
            Name = album.Name,
            Imagen = album.Imagen,
            Tracks = album.Tracks.Select(track => new TrackDto
            {
                Id = track.Id,
                Name = track.Name,
                Duration = track.Duration

            }).ToList(),
            Id = album.Id,
            ReleaseDate = album.ReleaseDate,
            ArtistId = album.ArtistId
        };
        return trackDTO;
    }

    private Exception KeyNotFoundException(string v)
    {
        throw new NotImplementedException();
    }
}