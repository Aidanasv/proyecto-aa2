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

    //Ontener artistas
    [HttpGet]
    public async Task<ActionResult<List<ArtistRead>>> GetArtists([FromQuery] ArtistDtoParameters artistDtoParameters)
    {
        var artists = await _artistService.GetAllAsync(artistDtoParameters);
        var artistsRead = artists.Select(artist => new ArtistRead
        {
            Name = artist.Name,
            Followers = artist.Followers,
            Biography = artist.Biography,
            CreateDate = artist.CreateDate,
            Imagen = artist.Imagen,
            SoftDelete = artist.SoftDelete,
            Id = artist.Id
        });
        return Ok(artistsRead);
    }

    //Obtener artista por id
    [HttpGet("{id}")]
    public async Task<ActionResult<ArtistRead>> GetArtistById(int id)
    {
        var artist = await _artistService.GetByIdAsync(id);
        if (artist == null)
        {
            return NotFound(new { message = "No se encontró el artista" });
        }

        var artistsRead = new ArtistRead
        {
            Name = artist.Name,
            Followers = artist.Followers,
            Biography = artist.Biography,
            CreateDate = artist.CreateDate,
            Imagen = artist.Imagen,
            SoftDelete = artist.SoftDelete,
            Id = artist.Id
        };
        return Ok(artistsRead);
    }

    //Crear artista - admin
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
            var artistsRead = new ArtistRead
            {
                Name = newArtist.Name,
                Followers = newArtist.Followers,
                Biography = newArtist.Biography,
                CreateDate = newArtist.CreateDate,
                Imagen = newArtist.Imagen,
                SoftDelete = newArtist.SoftDelete,
                Id = newArtist.Id
            };
            return CreatedAtAction(
                nameof(GetArtistById),
                new { id = artistsRead.Id },
                artistsRead
            );
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Error al crear artista" });
        }

    }

    //Modificar album - Admin
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

            var artistsRead = new ArtistRead
            {
                Name = updatedArtist.Name,
                Followers = updatedArtist.Followers,
                Biography = updatedArtist.Biography,
                CreateDate = updatedArtist.CreateDate,
                Imagen = updatedArtist.Imagen,
                SoftDelete = updatedArtist.SoftDelete,
                Id = id
            };
            return Ok(artistsRead);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //Elliminar album - Admin
    [Authorize(Roles = Role.Admin)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArtist(int id)
    {
        var artist = await _artistService.GetByIdAsync(id);
        if (artist == null)
        {
            return NotFound(new { message = "Error al eliminar artista" });
        }
        await _artistService.DeleteAsync(id);
        return Ok(id);
    }

    [HttpGet("{id}/albums")]
    public async Task<ActionResult<ArtistDto>> GetAlbumsByArtists(int id)
    {
        var albums = await _artistService.GetAlbumsByArtist(id);
        if (albums == null)
        {
            return NotFound(new { message = "No se encontraron tracks para el album" });
        }
        return Ok(albums);
    }

}