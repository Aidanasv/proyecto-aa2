namespace Services;

using Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Models;

public class PlaylistService : IPlaylistService
{
    private readonly IPlaylistEfRepository _playlistRepository;
    private readonly ITrackEfRepository _trackRepository;

    public PlaylistService(IPlaylistEfRepository playlistRepository, ITrackEfRepository trackRepository)
    {
        _playlistRepository = playlistRepository;
        _trackRepository = trackRepository;

    }

    public async Task<List<Playlist>> GetAllAsync()
    {
        return await _playlistRepository.GetAllAsync();
        
    }

    public async Task<PlaylistDto?> GetByIdAsync(int id)
    {
        var playlist = await _playlistRepository.GetByIdAsync(id);
        if (playlist == null)
        {
            throw KeyNotFoundException("Playlist no encontrada");
        }
        var playlistDto = new PlaylistDto
        {
            Name = playlist.Name,
            Tracks = playlist.Tracks.Select(track => new TrackDto
            {
                Id = track.Id,
                Name = track.Name
            }).ToList(),
        };
        return playlistDto;
    }

    public async Task<PlaylistRead> AddAsync(PlaylistCreate playlistCreate, int UserId)
    {
        var playlist = new Playlist
        {
            Name = playlistCreate.Name,
            Description = playlistCreate.Description,
            UserId = UserId,
            SoftDelete = playlistCreate.SoftDelete
        };

        await _playlistRepository.AddAsync(playlist);
        return playlist;
    }

    public async Task<PlaylistRead> UpdateAsync(PlaylistCreate playlist, int id)
    {
        var updatedPlaylist = await _playlistRepository.GetByIdAsync(id);
        if (updatedPlaylist == null)
        {
            throw KeyNotFoundException("Playlist no encontrada");
        }

        updatedPlaylist.Name = playlist.Name;
        updatedPlaylist.Description = playlist.Description;


        await _playlistRepository.UpdateAsync(updatedPlaylist);
        return updatedPlaylist;
    }

    public async Task DeleteAsync(int id)
    {
        var playlist = await _playlistRepository.GetByIdAsync(id);
        if (playlist == null)
        {
            throw KeyNotFoundException("Playlist no encontrado");
        }
        playlist.SoftDelete = true;

        await _playlistRepository.UpdateAsync(playlist);
    }

    public async Task<PlaylistDto> AddTrackToPlaylist(int id, int idTrack)
    {
        var playlist = await _playlistRepository.GetByIdAsync(id);
        var track = await _trackRepository.GetByIdAsync(idTrack);
        if (playlist.Tracks == null)
        {
            playlist.Tracks = new List<Track>();
        }
        playlist.Tracks.Add(track);

        await _playlistRepository.UpdateAsync(playlist);

        var playlistDto = new PlaylistDto
        {
            Name = playlist.Name,
            Tracks = playlist.Tracks.Select(track => new TrackDto
            {
                Id = track.Id,
                Name = track.Name
            }).ToList(),
        };
        return playlistDto;

    }

    public async Task<PlaylistDto> DeleteTrackToPlaylist(int id, int idTrack)
    {
        var playlist = await _playlistRepository.GetByIdAsync(id);
        var track = await _trackRepository.GetByIdAsync(idTrack);
        if (playlist.Tracks == null)
        {
            playlist.Tracks = new List<Track>();
        }
        playlist.Tracks.Remove(track);

        await _playlistRepository.UpdateAsync(playlist);

        var playlistDto = new PlaylistDto
        {
            Name = playlist.Name,
            Tracks = playlist.Tracks.Select(track => new TrackDto
            {
                Id = track.Id,
                Name = track.Name
            }).ToList(),
        };
        return playlistDto;

    }
    private Exception KeyNotFoundException(string v)
    {
        throw new NotImplementedException();
    }
    

}