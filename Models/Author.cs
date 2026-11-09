namespace BibliotecaAPI.Models
{
    public class Author
    {
        public int Id { get; set; }

        //El nombre no se puede repetir.
        public string Name { get; set; } = string.Empty;

        public string Nationality { get; set; } = string.Empty;

        public DateTime BirthYear { get; set; }


        //Relacion m-n autores y libros
        public ICollection<Book> Books { get; set; } = new List<Book>();

        
      
    }
}
