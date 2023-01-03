using APIAuth.Models;
using APIAuth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bear", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "Jwt",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with Jwt Token",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bear",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });

});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IMovieService, MovieService>();
builder.Services.AddSingleton<IUserService, UserService>();

var app = builder.Build();

app.UseSwagger();
app.UseAuthorization();
app.UseAuthentication();


app.MapGet("/", () => "Hello World!")
    .ExcludeFromDescription();

app.MapPost("/login",
    (UserLogin user, IUserService service) => Login(user, service))
    .Accepts<UserLogin>("application/json")
    .Produces<string>();

app.MapPost("/create",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    (Movie movie, IMovieService service) => Create(movie, service))
    .Accepts<Movie>("application/json")
    .Produces<string>(statusCode: 200, contentType: "application/json");

app.MapGet("/get",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard, Admin")]
    (int id, IMovieService service) => Get(id, service))
    .Accepts<Movie>("application/json");


app.MapGet("/list",
    (IMovieService service) => List(service))
    .Produces<List<Movie>>(statusCode: 200, contentType: "application/json");


app.MapPut("/update",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    (Movie newMovie, IMovieService service) => Update(newMovie, service))
    .Accepts<Movie>("application/json")
    .Produces<Movie>(statusCode: 200, contentType: "application/json");

app.MapDelete("/delete",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    (int id, IMovieService service) => Delete(id, service));

IResult Login(UserLogin user, IUserService service)
{
    if(!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password))
    {
        var LoggedInUser = service.Get(user);
        if (LoggedInUser == null) return Results.NotFound("User not found");


        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, LoggedInUser.UserName),
            new Claim(ClaimTypes.Email, LoggedInUser.EmailAddress),
            new Claim(ClaimTypes.GivenName, LoggedInUser.GivenName),
            new Claim(ClaimTypes.Surname, LoggedInUser.Surname),
            new Claim(ClaimTypes.Role, LoggedInUser.Role)
        };

        var token = new JwtSecurityToken
        (
            issuer: builder.Configuration["Jwt:Issuer"],
            audience: builder.Configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(60),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256)
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(tokenString);
    }
    return Results.NotFound("User not found");
    
}

IResult Create(Movie movie, IMovieService service)
{
    var result = service.Create(movie);
    return Results.Ok(result);
}

IResult Get(int id, IMovieService service)
{
    var movie = service.Get(id);
    if (movie == null) return Results.NotFound("Movie not found");

    return Results.Ok(movie);
}

IResult List(IMovieService service)
{
    var movies = service.List();
    return Results.Ok(movies);
}

IResult Update(Movie newMovie, IMovieService service)
{
    var updatedMovie = service.Update(newMovie);
    if (updatedMovie == null) return Results.NotFound("Movie not Found");

    return Results.Ok(updatedMovie);
}

IResult Delete(int id, IMovieService service)
{
    var result = service.Delete(id);

    if (!result) return Results.BadRequest("something went wrong");

    return Results.Ok(result);
}

app.UseSwaggerUI();

app.Run();
