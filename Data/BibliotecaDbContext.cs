using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Data;
    using BibliotecaAPI.Models;

    public class BibliotecaDbContext : DbContext
{

    /*
     El DBContext es la clase principal que nos permite interactuar con la base de datos sin escribir SQL directamente,
    proporcionando los métodos para realizar operaciones de lectura, escritura, actualización y eliminación de datos.
     */

    public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options) : base(options)
    {
    }

    /*
     Cada DbSet<T> permite interactuar con la tabla asociada, donde T es el tipo de la entidad
     */

    public DbSet<Author> Authors { get; set; }

    public DbSet<Book> Books { get; set; }

    public DbSet<Category> Categories { get; set; }




}
