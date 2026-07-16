using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>();
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

app.MapGet("/artistas", ([FromServices] DAL<Artista> dal) => {
    return Results.Ok(dal.Listar());
});

app.MapGet("/artistas/{nome}", ([FromServices] DAL <Artista> dal, string nome) => {
    var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
    if (artista is null)
    {
        return Results.NotFound();
    }
        return Results.Ok(new
        {
            artista.Id,
            artista.Nome,
            artista.FotoPerfil,
            artista.Bio,
            Musicas = artista.Musicas.Select(m => new
            {
                m.Id,
                m.Nome,
                m.AnoLancamento
            })
        });
});

app.MapPost("/artistas", ([FromServices] DAL<Artista> dal, [FromBody] Artista artista) => {
    dal.Adicionar(artista);
    return Results.Ok();
});

app.MapDelete("/artistas/{id}", ([FromServices] DAL<Artista> dal, int id) => {
    var artista = dal.RecuperarPor(a => a.Id == id);
    if (artista is null)
    {
        return Results.NotFound();
    }
    dal.Deletar(artista);
    return Results.NoContent();
});

app.Run();
