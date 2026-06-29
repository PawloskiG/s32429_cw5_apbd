var builder = WebApplication.CreateBuilder(args);

// Rejestracja kontrolerów (Web API oparte na kontrolerach).
builder.Services.AddControllers();

// Swagger / OpenAPI - przydatne do podglądu API obok testów w Postmanie.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Mapowanie tras do kontrolerów.
app.MapControllers();

app.Run();
