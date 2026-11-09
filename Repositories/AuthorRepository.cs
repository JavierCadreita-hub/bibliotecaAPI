using BibliotecaAPI.Data;
using BibliotecaAPI.DTO;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.Repositories
{
    /*
     Repositorio de autores: la unión entre la API (AuthorDto) y la BBDD (Author).
     Valida lo que necesita consultar la BBDD: que no se repita el nombre (error 33).
     */
    public class AuthorRepository
    {
        // El DbContext es la conexión con la BBDD: lo recibe el constructor (inyección de dependencias)
        private readonly BibliotecaDbContext _context;

        public AuthorRepository(BibliotecaDbContext context)
        {
            _context = context;
        }

        // GET /authors: todos los autores
        public IEnumerable<AuthorDto> GetAll()
        {
            return _context.Authors
                .Select(a => new AuthorDto { Id = a.Id, Name = a.Name, Nationality = a.Nationality, BirthYear = a.BirthYear })
                .ToList();
        }

        // GET /authors?id=1: un autor por su Id (null si no existe)
        public AuthorDto? GetById(int id)
        {
            return _context.Authors
                .Where(a => a.Id == id)
                .Select(a => new AuthorDto { Id = a.Id, Name = a.Name, Nationality = a.Nationality, BirthYear = a.BirthYear })
                .FirstOrDefault();
        }

        // GET /authors?nationality=británica: los de esa nacionalidad, sin distinguir mayúsculas
        public IEnumerable<AuthorDto> GetByNationality(string nationality)
        {
            string nationalityLower = nationality.ToLower();
            return _context.Authors
                .Where(a => a.Nationality.ToLower() == nationalityLower)
                .Select(a => new AuthorDto { Id = a.Id, Name = a.Name, Nationality = a.Nationality, BirthYear = a.BirthYear })
                .ToList();
        }

        // POST /authors: comprueba que el nombre no exista y lo guarda
        public AuthorDto? Add(AuthorDto dto, out ErrorResponse? error)
        {
            error = null;

            string nameLower = dto.Name.ToLower();
            bool exists = _context.Authors.Any(a => a.Name.ToLower() == nameLower);   // SQL: EXISTS
            if (exists)
            {
                error = new ErrorResponse(33, "Ya existe un autor con ese nombre");
                return null;
            }

            // Del DTO al modelo. El Id no se pone: lo genera SQL Server
            Author author = new Author
            {
                Name = dto.Name,
                Nationality = dto.Nationality,
                BirthYear = dto.BirthYear
            };

            _context.Authors.Add(author);
            _context.SaveChanges();   // INSERT en Authors

            return new AuthorDto { Id = author.Id, Name = author.Name, Nationality = author.Nationality, BirthYear = author.BirthYear };
        }
    }
}
