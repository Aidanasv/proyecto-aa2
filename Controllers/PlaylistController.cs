namespace Controllers;

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
    public async Task<ActionResult<Playlist>> GetPlaylistById(int id)
    {
        var playlist = await _playlistService.GetByIdAsync(id);
        if (playlist == null)
        {
            return NoContent();
        }
        return Ok(playlist);
    }

    [HttpPost]
    public async Task<ActionResult<Playlist>> CreatePlaylist(Playlist playlist)
    {
        await _playlistService.AddAsync(playlist);
        return CreatedAtAction(nameof(GetPlaylists), new { id = playlist.Id }, playlist);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlaylist(int id, Playlist updatedPlaylist)
    {
        await _playlistService.UpdateAsync(updatedPlaylist, id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlaylist(int id)
    {
        var playlist = await _playlistService.GetByIdAsync(id);
        if (playlist == null)
        {
            return NotFound();
        }
        await _playlistService.DeleteAsync(id);
        return NoContent();
    }

}