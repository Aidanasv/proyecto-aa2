namespace Controllers;

using Models;
using Microsoft.AspNetCore.Mvc;
using Services;

[Route("albums")]
[ApiController]

public class AlbumController : ControllerBase
{
    private readonly IAlbumService _albumService;

    public AlbumController(IAlbumService service)
    {
        _albumService = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<AlbumRead>>> GetAlbums()
    {
        var albums = await _albumService.GetAllAsync();
        return Ok(albums);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AlbumRead>> GetAlbumById(int id)
    {
        var album = await _albumService.GetByIdAsync(id);
        if (album == null)
        {
            return NoContent();
        }
        return Ok(album);
    }

    [HttpPost]
    public async Task<ActionResult<AlbumRead>> CreateAlbum(AlbumCreate album)
    {
        await _albumService.AddAsync(album);
        return CreatedAtAction(nameof(GetAlbums), album);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAlbum(int id, AlbumCreate updatedAlbum)
    {
        await _albumService.UpdateAsync(updatedAlbum, id);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlbum(int id)
    {
        var album = await _albumService.GetByIdAsync(id);
        if (album == null)
        {
            return NotFound();
        }
        await _albumService.DeleteAsync(id);
        return NoContent();
    }

}