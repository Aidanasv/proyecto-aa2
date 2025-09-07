namespace Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

[Route("tracks")]
[ApiController]

public class TrackController : ControllerBase
{
    private readonly ITrackService _trackService;

    public TrackController(ITrackService service, IAlbumService albumService)
    {
        _trackService = service;
    }

    //Obtener tracks
    [HttpGet]
    public async Task<ActionResult<List<TrackRead>>> GetTracks()
    {
        var tracks = await _trackService.GetAllAsync();
        var tracksRead = tracks.Select(track => new TrackRead
        {
            Name = track.Name,
            ArtistId = track.ArtistId,
            AlbumId = track.AlbumId,
            Duration = track.Duration,
            ReleaseDate = track.ReleaseDate,
            Plays = track.Plays,
            SoftDelete = track.SoftDelete,
            Id = track.Id

        });
        return Ok(tracksRead);
    }

    //Obtener canción por id
    [HttpGet("{id}")]
    public async Task<ActionResult<TrackRead>> GetTrackById(int id)
    {
        var track = await _trackService.GetByIdAsync(id);
        if (track == null)
        {
            return NotFound(new { message = "No se encontró la canción" });
        }

        var trackRead = new TrackRead
        {
            Name = track.Name,
            ArtistId = track.ArtistId,
            AlbumId = track.AlbumId,
            Duration = track.Duration,
            ReleaseDate = track.ReleaseDate,
            Plays = track.Plays,
            SoftDelete = track.SoftDelete,
            Id = track.Id
        };
        return Ok(trackRead);
    }

    //Crear cancion - Admin
    [Authorize(Roles = Role.Admin)]
    [HttpPost]
    public async Task<ActionResult<TrackRead>> CreateTrack(TrackCreate track)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newTrack = await _trackService.AddAsync(track);
            var trackRead = new TrackRead
            {
                Name = newTrack.Name,
                ArtistId = newTrack.ArtistId,
                AlbumId = newTrack.AlbumId,
                Duration = newTrack.Duration,
                ReleaseDate = newTrack.ReleaseDate,
                Plays = newTrack.Plays,
                SoftDelete = newTrack.SoftDelete,
                Id = newTrack.Id
            };

            return CreatedAtAction(
                nameof(GetTrackById),
                new { id = trackRead.Id },
                trackRead
            );
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Error al crear canción" });
        }
    }

    //Modificar canción - Admin
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrack(int id, TrackCreate updatedTrack)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            updatedTrack = await _trackService.UpdateAsync(updatedTrack, id);

            var trackRead = new TrackRead
            {
                Name = updatedTrack.Name,
                ArtistId = updatedTrack.ArtistId,
                AlbumId = updatedTrack.AlbumId,
                Duration = updatedTrack.Duration,
                ReleaseDate = updatedTrack.ReleaseDate,
                Plays = updatedTrack.Plays,
                SoftDelete = updatedTrack.SoftDelete,
                Id = id
            };

            return Ok(trackRead);
        }
        catch
        {
            return BadRequest(new { message = "Error al modificar canción" });
        }
    }

    //Eliminar canción - Admin
    [Authorize(Roles = Role.Admin)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrack(int id)
    {
        var track = await _trackService.GetByIdAsync(id);
        if (track == null)
        {
            return NotFound();
        }
        await _trackService.DeleteAsync(id);

        return Ok(id);
    }

    [HttpGet("audio/{id}")]
    public async Task<IActionResult> GetAudio(int id)
    {
        var contentType = "audio/mpeg";
        var audio = await _trackService.GetAudio(id);
        return File(audio, contentType);
    }

    [HttpPost("audio/{id}")]
    public async Task<IActionResult> UpdatePlays(int id)
    {
        var audio = await _trackService.GetByIdAsync(id);
        audio.Plays = audio.Plays + 1;

        var track = new TrackCreate
        {
            Name = audio.Name,
            ArtistId = audio.ArtistId,
            AlbumId = audio.AlbumId,
            Duration = audio.Duration,
            ReleaseDate = audio.ReleaseDate,
            Plays = audio.Plays,
            SoftDelete = audio.SoftDelete
        };

        await _trackService.UpdateAsync(track, id);
        return Ok();
    }
}