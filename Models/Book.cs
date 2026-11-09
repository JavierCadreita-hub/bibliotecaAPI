using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaAPI.Models
{
    public class Book
    {
       public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        //El isbn no se puede repetir.
        public string Isbn { get; set; } = string.Empty;
        public int Pages { get; set; }
        public int PublicationYear { get; set; }
        public int CategoryId { get; set; }

        //Relacion m-n autores y libros
        public ICollection<Author> Authors { get; set; } = new List<Author>();

        [ForeignKey(nameof(CategoryId))]

        public Category? Category { get; set; }









    }
}
