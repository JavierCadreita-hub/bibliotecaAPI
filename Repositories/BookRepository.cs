using BibliotecaAPI.Data;
using BibliotecaAPI.DTO;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.Repositories
{
    /*
     Repositorio de libros: la unión entre la API (BookDto) y la BBDD (Book).
     Valida lo que necesita consultar la BBDD:
       34 - que exista la categoría
       35 - que existan todos los autores (un error por cada autor que falte)
       36 - que no se repita el ISBN
     Los errores se ACUMULAN en la lista, para devolverlos todos a la vez.
     */
    public class BookRepository
    {
        // El DbContext es la conexión con la BBDD: lo recibe el constructor (inyección de dependencias)
        private readonly BibliotecaDbContext _context;

        public BookRepository(BibliotecaDbContext context)
        {
            _context = context;
        }

        // POST /books: valida y guarda el libro con su categoría (1:N) y sus autores (M:N)
        public BookDto? Add(BookDto dto, out List<ErrorResponse> errors)
        {
            errors = new List<ErrorResponse>();

            // 34: que exista la categoría
            bool categoryExists = _context.Categories.Any(c => c.Id == dto.CategoryId);
            if (!categoryExists)
            {
                errors.Add(new ErrorResponse(34, "La categoría del libro no existe"));
            }

            // 35: que existan los autores. Una sola consulta: WHERE Id IN (...)
            List<Author> authors = _context.Authors
                .Where(a => dto.AuthorsId.Contains(a.Id))
                .ToList();

            // Los ids que no han salido de la BBDD son autores que no existen
            foreach (int authorId in dto.AuthorsId.Distinct())
            {
                if (!authors.Any(a => a.Id == authorId))
                {
                    errors.Add(new ErrorResponse(35, "El autor " + authorId + " no existe"));
                }
            }

            // 36: que no se repita el ISBN
            bool isbnExists = _context.Books.Any(b => b.Isbn == dto.Isbn);
            if (isbnExists)
            {
                errors.Add(new ErrorResponse(36, "Ya existe un libro con ese ISBN"));
            }

            if (errors.Count > 0)
            {
                return null;
            }

            // Del DTO al modelo: la categoría va por su clave ajena y los autores como OBJETOS
            Book book = new Book
            {
                Title = dto.Title,
                Isbn = dto.Isbn,
                Pages = dto.Pages,
                PublicationYear = dto.PublicationYear,
                CategoryId = dto.CategoryId,
                Authors = authors          // EF rellena la tabla intermedia AuthorBook él solo
            };

            _context.Books.Add(book);
            _context.SaveChanges();        // INSERT en Books + una fila en AuthorBook por autor

            // Devolvemos el DTO con el Id que ha generado SQL Server
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn,
                Pages = book.Pages,
                PublicationYear = book.PublicationYear,
                CategoryId = book.CategoryId,
                AuthorsId = authors.Select(a => a.Id).ToList()
            };
        }
    }
}
