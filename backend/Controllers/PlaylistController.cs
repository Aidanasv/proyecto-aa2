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

    
    [HttpGet]
    public async Task<ActionResult<List<Playlist>>> GetPlaylists()
    {
        var playlists = await _playlistService.GetAllAsync();
        return Ok(playlists);
    }

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
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = Role.Client)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlaylist(int id)
    {
        var playlist = await _playlistService.GetByIdAsync(id);
        if (playlist == null)
        {
            return NotFound();
        }
        await _playlistService.DeleteAsync(id);
        return Ok();
    }

    [Authorize(Roles = Role.Client)]
    [HttpPost("{id}/add/{idTrack}")]
    public async Task<IActionResult> AddTrackToPlaylist(int id, int idTrack)
    {
        var playlist = await _playlistService.AddTrackToPlaylist(id, idTrack);
        if (playlist == null)
        {
            return NotFound();
        }
        return Ok(playlist);
    }

    [Authorize(Roles = Role.Client)]
    [HttpDelete("{id}/remove/{idTrack}")]
    public async Task<IActionResult> DeleteTrackToPlaylist(int id, int idTrack)
    {
        var playlist = await _playlistService.DeleteTrackToPlaylist(id, idTrack);
        if (playlist == null)
        {
            return NotFound();
        }
        return Ok(playlist);
    }
    [Authorize(Roles = Role.Client)]
    [HttpGet("user")]
    public async Task<ActionResult<List<PlaylistDto>>> GetPlaylistsByUser()
    {
        var currentUser = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var playlist = await _playlistService.GetPlaylistsByUser(int.Parse(currentUser));
        if (playlist == null)
        {
            return NoContent();
        }
        return Ok(playlist);
    }

}