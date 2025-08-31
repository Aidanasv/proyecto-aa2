namespace Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Models;

public class ArtistEfRepository : IArtistEfRepository
{
    private readonly MusicDbContext _dbContext;

    public ArtistEfRepository(MusicDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Artist>> GetAllAsync(ArtistDtoParameters artistDtoParameters)
    {
        var query = _dbContext.Artists.Where(
            artist => artist.SoftDelete == false
        );

        if (artistDtoParameters.Name != null)
        {
            query = query.Where(artist => artist.Name.ToLower().Contains(artistDtoParameters.Name.ToLower()));
        }
        if (artistDtoParameters.Followers != null)
        {
            query = query.Where(artist => artist.Followers >= artistDtoParameters.Followers);
        }

        IOrderedQueryable<Artist>? orderedQuery = null;

        if (artistDtoParameters.NameOrder == true)
        {
            orderedQuery = query.OrderBy(artist => artist.Name);
        }
        else if (artistDtoParameters.NameOrder == false)
        {
            orderedQuery = query.OrderByDescending(artist => artist.Name);
        }

        if (artistDtoParameters.FollowersOrder == true)
        {
            orderedQuery = orderedQuery == null
                ? query.OrderBy(artist => artist.Followers)
                : orderedQuery.ThenBy(artist => artist.Followers);
        }
        else if (artistDtoParameters.FollowersOrder == false)
        {
            orderedQuery = orderedQuery == null
                ? query.OrderByDescending(artist => artist.Followers)
                : orderedQuery.ThenByDescending(artist => artist.Followers);
        }
        return await  (orderedQuery ?? query).ToListAsync();
    }

    public async Task<Artist?> GetByIdAsync(int id)
    {
        return await _dbContext.Artists.FindAsync(id);
    }

    public async Task AddAsync(Artist artist)
    {
        await _dbContext.Artists.AddAsync(artist);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Artist artist)
    {
        _dbContext.Artists.Update(artist);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var artist = await _dbContext.Artists.FindAsync(id);
        if (artist == null) return false;

        _dbContext.Artists.Remove(artist);
        await _dbContext.SaveChangesAsync();
        return true;
    }
    public async Task<Artist?> GetAlbumsByArtist(int id)
    {
        return _dbContext.Artists.Include(artist => artist.Albums.Where(album => !album.SoftDelete)).FirstOrDefault(artist => artist.Id == id);
    }

}