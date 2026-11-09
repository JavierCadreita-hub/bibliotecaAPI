namespace BibliotecaAPI.Models
{
    public class Category
    {
        public int Id { get; set; }

        //El nombre no se puede repetir.
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        //Una categoria puede tener varios libros (1 categoria - N libros)

        public ICollection<Book> Books { get; set; } =  new List<Book>();










       



       




    }
}
