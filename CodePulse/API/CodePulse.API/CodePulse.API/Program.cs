using System.Text;
using CodePulse.API.Data;
using CodePulse.API.Repository.Implementation;
using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// injecting DbContext. 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CodePulseConnectionString"));
});

// injecting AuthDbContext. 
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CodePulseConnectionString"));
});



builder.Services.AddScoped<ICategoryRepository, CategoyRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();


//injecting the identityUser
builder.Services.AddIdentityCore<IdentityUser>() //what type of user we are using
    .AddRoles<IdentityRole>() //what type of role we are going to use
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("CodePulse") 
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

// we this we set how the password can come to our api
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// creando la autenticacion de la aplicacion
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => //agregamos el jwt bearer 
    {
        options.TokenValidationParameters =
            new TokenValidationParameters //aqui configuramos los parametros que tendra el token
            {
                AuthenticationType = "Jwt", //aqui le decimos del tipoque es
                ValidateIssuer = true, //aqui le decimos que valide quien firma el token que somos nosotros
                ValidateAudience = true, // aqui validamos a la persona que va, que en este caso es el frontend
                ValidateLifetime = true, //aqui le ponemos que valide si el token expiro por el tiempo
                ValidateIssuerSigningKey = true, // aqui que valide la llave definida en el appsettings
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});

app.UseAuthentication();
app.UseAuthorization();


//De esta manera la api puede usar archivos estaticos!
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});

app.MapControllers();

app.Run();
