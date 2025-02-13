using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data;

public class AuthDbContext : IdentityDbContext
{

    //aqui le pasamos la base de datos para que haga conexion directa
    public AuthDbContext(DbContextOptions<AuthDbContext> options): base(options)
    {
        
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //Creando los roles

        var readerRoleId = "81495368-62A1-434C-7268-08DB615CCDFB";
        var writerRoleId = "133CABD9-E0DD-44B8-7DE5-08DD46ED604B";
        
        var roles = new List<IdentityRole>
        {
            new IdentityRole()
            {
                Id = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper(),
                ConcurrencyStamp = readerRoleId
            },

            new IdentityRole()
            {
                Id = writerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper(),
                ConcurrencyStamp = writerRoleId
            },

        };
        
        //alimentando los roles
        //esto se hace para cuando se corra la migracion se alimente una tabla con los roles ya credados.
        modelBuilder.Entity<IdentityRole>().HasData(roles);
        
        //creando el rol de admin
        var adminId = "9449DC77-3FC4-4B72-8C8C-08DB6CAB4598"; //definimos el id del admin
        var admin = new IdentityUser() // asi como usamos identityRoles para crear los roles podemos usar identityUser para crear los usuarios
                {
                    Id = adminId,
                    UserName = "admin@codepulse.com",
                    Email = "admin@codepulse.com",
                    NormalizedEmail = "admin@codepulse.com".ToUpper(),
                    NormalizedUserName = "admin@codepulse.com".ToUpper(),
                };
        
        admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@code1"); //aqui estamos creando el hash de la contraseña del admin
        modelBuilder.Entity<IdentityUser>().HasData(admin); // y aqui estamos insertando el usuario nuevo en la base de datos.
        
        //dandole el rol al admin

        var adminRoles = new List<IdentityUserRole<string>>()
        {
            new() // new IdentityUserRole ()
            {
                UserId = adminId,
                RoleId = readerRoleId
            },
            new()
            {
                UserId = adminId,
                RoleId = writerRoleId
            }
        };
        
        //guardando los nuevos roles que tiene el usuario admin
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

    }
    
}