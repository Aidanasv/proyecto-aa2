using Data;
using Microsoft.EntityFrameworkCore;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MusicDbContext>(options =>
    options.UseSqlite("Data Source=music.db")
);

builder.Services.AddScoped<IArtistEfRepository, ArtistEfRepository>();
builder.Services.AddScoped<IAlbumEfRepository, AlbumEfRepository>();
builder.Services.AddScoped<ITrackEfRepository, TrackEfRepository>();
builder.Services.AddScoped<IUserEfRepository, UserEfRepository>();
builder.Services.AddScoped<IPlaylistEfRepository, PlaylistEfRepository>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
