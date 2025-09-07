namespace Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

[Route("playlists")]
[ApiController]

public class PlaylistController : ControllerBase
{
    private readonly IPlaylistService _playlistService;

    public PlaylistController(IPlaylistService service)
    {
        _playlistService = service;
    }

    //Obtener playlists
    [HttpGet]
    public async Task<ActionResult<List<PlaylistRead>>> GetPlaylists()
    {
        var playlists = await _playlistService.GetAllAsync();
        var playlistsRead = playlists.Select(playlist => new PlaylistRead
        {
            Name = playlist.Name,
            Description = playlist.Description,
            SoftDelete = playlist.SoftDelete,
            Id = playlist.Id
        });

        return Ok(playlistsRead);
    }

    //Obtener playlist por id
    [HttpGet("{id}")]
    public async Task<ActionResult<PlaylistDto>> GetPlaylistById(int id)
    {
        var playlist = await _playlistService.GetByIdAsync(id);
        if (playlist == null)
        {
            return NoContent();
        }
        return Ok(playlist);
    }

    //Crear playlist - Usuario verificado
    [Authorize(Roles = Role.Client)]
    [HttpPost]
    public async Task<ActionResult<PlaylistRead>> CreatePlaylist(PlaylistCreate playlist)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUser = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var newPlaylist = await _playlistService.AddAsync(playlist, int.Parse(currentUser));
            return CreatedAtAction(
                nameof(GetPlaylistById),
                new { id = newPlaylist.Id },
                newPlaylist
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    //Modificar playlist - Usuario verificado
    [Authorize(Roles = Role.Client)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlaylist(int id, PlaylistCreate updatedPlaylist)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            updatedPlaylist = await _playlistService.UpdateAsync(updatedPlaylist, id);

            var playlistRead = new PlaylistRead
            {
                Name = updatedPlaylist.Name,
                Description = updatedPlaylist.Description,
                SoftDelete = updatedPlaylist.SoftDelete,
                Id = id
            };
            return Ok(updatedPlaylist);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //Eliminar playlist - Usuario verificado
    [Authorize(Roles = Role.Client)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlaylist(int id)
    {
        var playlist = await _playlistService.GetByIdAsync(id);
        if (playlist == null)
        {
            return NotFound(new { message = "Error al eliminar album" });
        }
        await _playlistService.DeleteAsync(id);
        return Ok(id);
    }

    [Authorize(Roles = Role.Client)]
    [HttpPost("{id}/add/{idTrack}")]
    public async Task<IActionResult> AddTrackToPlaylist(int id, int idTrack)
    {
        var playlist = await _playlistService.AddTrackToPlaylist(id, idTrack);
        if (playlist == null)
        {
            return NotFound(new { message = "Error al añadir canción a la playlist" });
        }
        return Ok(playlist);
    }

    //Eliminar canción de playlist - Usuario verificado
    [Authorize(Roles = Role.Client)]
    [HttpDelete("{id}/remove/{idTrack}")]
    public async Task<IActionResult> DeleteTrackToPlaylist(int id, int idTrack)
    {
        var playlist = await _playlistService.DeleteTrackToPlaylist(id, idTrack);
        if (playlist == null)
        {
            return NotFound(new { message = "Error al eliminar la canción de la playlist" });
        }
        return Ok(playlist);
    }

    //Obtener playlists de usuarios
    [Authorize(Roles = Role.Client)]
    [HttpGet("user")]
    public async Task<ActionResult<List<PlaylistDto>>> GetPlaylistsByUser([FromQuery] PlaylistDtoParameters playlistDtoParameters)
    {
        var currentUser = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var playlist = await _playlistService.GetPlaylistsByUser(playlistDtoParameters, int.Parse(currentUser));
        if (playlist == null)
        {
            return NotFound(new { message = "No se encontraron playlist para el usuario" });
        }
        return Ok(playlist);
    }

}