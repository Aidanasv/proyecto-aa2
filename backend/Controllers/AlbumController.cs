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

    //Obtener Albumes
    [HttpGet]
    public async Task<ActionResult<List<AlbumRead>>> GetAlbums()
    {
        var albums = await _albumService.GetAllAsync();
        var albumsRead = albums.Select(album => new AlbumRead
        {
            Name = album.Name,
            ArtistId = album.ArtistId,
            ReleaseDate = album.ReleaseDate,
            Duration = album.Duration,
            Imagen = album.Imagen,
            SoftDelete = album.SoftDelete,
            Id = album.Id
        });

        return Ok(albumsRead);
    }

    //Obtener album por Id
    [HttpGet("{id}")]
    public async Task<ActionResult<AlbumRead>> GetAlbumById(int id)
    {
        var album = await _albumService.GetByIdAsync(id);
        if (album == null)
        {
            return NotFound(new { message = "No se encontró el albúm" });
        }

        var albumRead = new AlbumRead
        {
            Name = album.Name,
            ArtistId = album.ArtistId,
            ReleaseDate = album.ReleaseDate,
            Duration = album.Duration,
            Imagen = album.Imagen,
            SoftDelete = album.SoftDelete,
            Id = album.Id
        };
        return Ok(albumRead);
    }

    //Crear album - Admin
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
            var albumRead = new AlbumRead
            {
                Name = newAlbum.Name,
                ArtistId = newAlbum.ArtistId,
                ReleaseDate = newAlbum.ReleaseDate,
                Duration = newAlbum.Duration,
                Imagen = newAlbum.Imagen,
                SoftDelete = newAlbum.SoftDelete,
                Id = newAlbum.Id
            };
            return CreatedAtAction(
                nameof(GetAlbumById),
                new { id = albumRead.Id },
                albumRead
            );
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Error al crear album" });
        }
    }

    //Modificar album - Admin
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

            var albumRead = new AlbumRead
            {
                Name = updatedAlbum.Name,
                ArtistId = updatedAlbum.ArtistId,
                ReleaseDate = updatedAlbum.ReleaseDate,
                Duration = updatedAlbum.Duration,
                Imagen = updatedAlbum.Imagen,
                SoftDelete = updatedAlbum.SoftDelete,
                Id = id
            };
            return Ok(albumRead);
        }
        catch
        {
            return BadRequest(new { message = "Error al modificar album" });
        }
    }

    //Eliminar album - admin
    [Authorize(Roles = Role.Admin)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlbum(int id)
    {
        var album = await _albumService.GetByIdAsync(id);
        if (album == null)
        {
            return NotFound(new { message = "Error al eliminar album" });
        }
        await _albumService.DeleteAsync(id);
        return Ok(id);
    }

    //Obtener tracks de album
    [HttpGet("{id}/tracks")]
    public async Task<ActionResult<AlbumTrackDTO>> GetTracksByAlbum(int id)
    {
        var albums = await _albumService.GetTracksByAlbum(id);
        if (albums == null)
        {
            return NotFound(new { message = "No se encontraron tracks para el album" });
        }
        return Ok(albums);
    }

}