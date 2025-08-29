namespace Services;

using System;
using System.Text.Json;
using Data;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.Identity.Client;
using Models;

public class TrackService : ITrackService
{
    private readonly ITrackEfRepository _trackRepository;
    private readonly IArtistEfRepository _artistRepository;

    public TrackService(ITrackEfRepository trackRepository, IArtistEfRepository artistRepository)
    {
        _trackRepository = trackRepository;
        _artistRepository = artistRepository;
    }

    public async Task<List<Track>> GetAllAsync()
    {
        return await _trackRepository.GetAllAsync();
    }

    public async Task<Track?> GetByIdAsync(int id)
    {
        return await _trackRepository.GetByIdAsync(id);
    }

    public async Task<Track> AddAsync(TrackCreate trackCreate)
    {
        var artistService = await _artistRepository.GetByIdAsync(trackCreate.ArtistId);
        if (artistService == null)
        {
            throw KeyNotFoundException("Error al encontrar el artista");
        }
        var audioBytes = await GetTrackFromAPI(trackCreate.Name + " " + artistService.Name);
        if (audioBytes == null)
        {
            throw KeyNotFoundException("Canción no encontrada");
        }
        var track = new Track
        {
            Name = trackCreate.Name,
            ArtistId = trackCreate.ArtistId,
            AlbumId = trackCreate.AlbumId,
            Duration = trackCreate.Duration,
            Plays = trackCreate.Plays,
            ReleaseDate = trackCreate.ReleaseDate
        };

        await _trackRepository.AddAsync(track);
        await SaveTrack(track.Id, audioBytes);
        return track;
    }

    public async Task<Track> UpdateAsync(TrackCreate track, int id)
    {
        var updatedTrack = await _trackRepository.GetByIdAsync(id);
        if (updatedTrack == null)
        {
            throw KeyNotFoundException("Canción no encontrada");
        }

        updatedTrack.Name = track.Name;
        updatedTrack.Duration = track.Duration;
        updatedTrack.ReleaseDate = track.ReleaseDate;

        updatedTrack.Plays = track.Plays;

        await _trackRepository.UpdateAsync(updatedTrack);
        return updatedTrack;
    }

    public async Task DeleteAsync(int id)
    {
        var track = await _trackRepository.GetByIdAsync(id);
        if (track == null)
        {
            throw KeyNotFoundException("Canción no encontrada");
        }
        track.SoftDelete = true;

        await _trackRepository.UpdateAsync(track);
    }

    public async Task<byte[]?> GetTrackFromAPI(string trackName)
    {
        try
        {
            HttpClient client = new HttpClient();

            string url = "https://api.deezer.com/search?q=" + trackName;
            string responseBody = await client.GetStringAsync(url);
            DataApi dataApi = JsonSerializer.Deserialize<DataApi>(responseBody);
            string urlaudio = dataApi.data[0].preview;
            var audiobytes = await client.GetByteArrayAsync(urlaudio);
            return audiobytes;
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    public async Task SaveTrack(int id, byte[] audiobytes)
    {
        string filePath = Path.Combine("Previews", $"{id}.mp3");

        var previewsPath = Environment.GetEnvironmentVariable("DATA_PATH") ?? "/app/data/data";
        Directory.CreateDirectory(previewsPath + "/Previews");

        await File.WriteAllBytesAsync(previewsPath + "/" + filePath, audiobytes);

    }

    public async Task<byte[]> GetAudio(int id)
    {
        string filePath = Path.Combine("Previews", $"{id}.mp3");
        var previewsPath = Environment.GetEnvironmentVariable("DATA_PATH") ?? "/app/data/data";
        filePath = Path.Combine(previewsPath, filePath);

        if (!System.IO.File.Exists(filePath))
        {
            throw KeyNotFoundException("Canción no encontrada");
        }

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return fileBytes;
            
    }

    private Exception KeyNotFoundException(string v)
    {
        throw new NotImplementedException();
    }
}