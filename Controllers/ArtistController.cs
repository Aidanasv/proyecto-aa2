namespace Controllers;

using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

[Route("artists")]
[ApiController]

public class ArtistController : ControllerBase
{
    private readonly IArtistService _artistService;

    public ArtistController(IArtistService service)
    {
        _artistService = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Artist>>> GetArtists()
    {
        var artists = await _artistService.GetAllAsync();
        return Ok(artists);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Artist>> GetArtistById(int id)
    {
        var artist = await _artistService.GetByIdAsync(id);
        if (artist == null)
        {
            return NoContent();
        }
        return Ok(artist);
    }

    [HttpPost]
    public async Task<ActionResult<Artist>> CreateArtist(Artist artist)
    {
        await _artistService.AddAsync(artist);
        return CreatedAtAction(nameof(GetArtists), new { id = artist.Id }, artist);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateArtist(int id, Artist updatedArtist)
    {
        await _artistService.UpdateAsync(updatedArtist, id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArtist(int id)
    {
        var artist = await _artistService.GetByIdAsync(id);
        if (artist == null)
        {
            return NotFound();
        }
        await _artistService.DeleteAsync(id);
        return NoContent();
    }

}