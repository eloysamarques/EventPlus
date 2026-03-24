using Azure.AI.ContentSafety;
using EventPlu.WebAPI.Repositories;
using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

var endpoint = "https://moderatorservices-eloysa.cognitiveservices.azure.com/";
var apiKey = "";

var client = new ContentSafetyClient(new Uri
    (endpoint), new Azure.AzureKeyCredential
    (apiKey));

builder.Services.AddSingleton(client);

// 1. Configurar o Contexto do Banco de Dados
builder.Services.AddDbContext<EventContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//2. Registrar as Repositories (Injeção de Dependência)
builder.Services.AddScoped<ITipoEventoRepository, TipoEventoRepository>();
builder.Services.AddScoped<IInstituicaoRepository, InstituicaoRepository>();
builder.Services.AddScoped<ITipoUsuarioRepository, TipoUsuarioRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IPresencaRepository, PresencaRepository>();
builder.Services.AddScoped<IComentarioEventoRepository, ComentarioEventoRepository>();
builder.Services.AddScoped<IPresencaRepository, PresencaRepository>();


//Adiciona Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer"; 
    options.DefaultChallengeScheme = "JwtBearer"; 
})

.AddJwtBearer("JwtBearer", options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {

         //Valida quem esta solicitando
         ValidateIssuer = true,

         //Valida quem esta recebendo
         ValidateAudience = true,

         //Define se o tempo de expiração do token deve ser validado
         ValidateLifetime = true,

         //Forma de cripotrografia e valida a chave de autenticacao
         IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("event+-chave-autenticacao-webapi-dev")),

         //Valida o tempo de expiração do token
         ClockSkew = TimeSpan.FromMinutes(5),

         //Nome do issuer (de onde esta vindo)
         ValidIssuer = "api_eventplus",

         //Nome do audience (para onde vai)
         ValidAudience = "api_eventplus"
     };
 });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Api de Eventos",
        Description = "Aplicação para gerenciamento de eventos",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Eloysa Marques",
            Url = new Uri("https://www.linkedin.com/in/eloysa-marques-53ba39377/")
        },
        License = new OpenApiLicense
        {
            Name = "Licensa de Exemplo",
            Url = new Uri("https://example.com/license")
        }
    });

    //Usando a autenticação no Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT:"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = Array.Empty<string>().ToList()
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger(options => { });

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();