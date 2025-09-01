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

    public TrackController(ITrackService service)
    {
        _trackService = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<TrackRead>>> GetTracks()
    {
        var tracks = await _trackService.GetAllAsync();
        return Ok(tracks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TrackRead>> GetTrackById(int id)
    {
        var track = await _trackService.GetByIdAsync(id);
        if (track == null)
        {
            return NoContent();
        }
        return Ok(track);
    }

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
            return CreatedAtAction(
                nameof(GetTrackById),
                new { id = newTrack.Id },
                newTrack
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

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
            return Ok(updatedTrack);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

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
        return Ok();
    }

    [HttpGet("audio/{id}")]
    public async Task<IActionResult> GetAudio(int id)
    {
        var contentType = "audio/mpeg";
        var audio = await _trackService.GetAudio(id);
        return File(audio, contentType);
    }
}