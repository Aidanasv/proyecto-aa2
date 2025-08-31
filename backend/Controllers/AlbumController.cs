namespace Controllers;

using Models;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;

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

    [Authorize(Roles = Role.Admin)]
    [HttpPost]
    public async Task<ActionResult<AlbumRead>> CreateAlbum(AlbumCreate album)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newAlbum = await _albumService.AddAsync(album);
            return CreatedAtAction(
                nameof(GetAlbumById),
                new { id = newAlbum.Id },
                newAlbum
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAlbum(int id, AlbumCreate updatedAlbum)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            updatedAlbum = await _albumService.UpdateAsync(updatedAlbum, id);
            return Ok(updatedAlbum);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = Role.Admin)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlbum(int id)
    {
        var album = await _albumService.GetByIdAsync(id);
        if (album == null)
        {
            return NotFound();
        }
        await _albumService.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("{id}/tracks")]
    public async Task<ActionResult<AlbumTrackDTO>> GetTracksByAlbum(int id)
    {
        var albums = await _albumService.GetTracksByAlbum(id);
        if (albums == null)
        {
            return NoContent();
        }
        return Ok(albums);
    }

}