namespace Controllers;

using Microsoft.AspNetCore.Authorization;
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
    public async Task<ActionResult<ArtistRead>> GetArtistById(int id)
    {
        var artist = await _artistService.GetByIdAsync(id);
        if (artist == null)
        {
            return NoContent();
        }
        return Ok(artist);
    }

    [Authorize(Roles = Role.Admin)]
    [HttpPost]
    public async Task<ActionResult<ArtistRead>> CreateArtist(ArtistCreate artist)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newArtist = await _artistService.AddAsync(artist);
            return CreatedAtAction(
                nameof(GetArtistById),
                new { id = newArtist.Id },
                newArtist
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [Authorize(Roles = Role.Admin)]
    [HttpPut("{id}")]
    public async Task<ActionResult<ArtistRead>> UpdateArtist(int id, ArtistCreate updatedArtist)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            updatedArtist = await _artistService.UpdateAsync(updatedArtist, id);
            return Ok(updatedArtist);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = Role.Admin)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArtist(int id)
    {
        var artist = await _artistService.GetByIdAsync(id);
        if (artist == null)
        {
            return NotFound();
        }
        await _artistService.DeleteAsync(id);
        return Ok();
    }
    
     [HttpGet("{id}/albums")]
    public async Task<ActionResult<ArtistDto>> GetAlbumsByArtists(int id)
    {
        var albums = await _artistService.GetAlbumsByArtist(id);
        if (albums == null)
        {
            return NoContent();
        }
        return Ok(albums);
    }

}