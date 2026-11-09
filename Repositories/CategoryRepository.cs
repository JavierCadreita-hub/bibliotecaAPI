using BibliotecaAPI.Data;
using BibliotecaAPI.DTO;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.Repositories
{
    /*
     Repositorio de categorías: la unión entre la API (CategoryDto) y la BBDD (Category).
     Valida lo que necesita consultar la BBDD: que no se repita el nombre (error 5).
     */
    public class CategoryRepository
    {
        // El DbContext es la conexión con la BBDD: lo recibe el constructor (inyección de dependencias)
        private readonly BibliotecaDbContext _context;

        public CategoryRepository(BibliotecaDbContext context)
        {
            _context = context;
        }

        // GET /categories: todas las categorías, pasadas de modelo a DTO con Select
        public IEnumerable<CategoryDto> GetAll()
        {
            return _context.Categories
                .Select(c => new CategoryDto { Id = c.Id, Name = c.Name, Description = c.Description })
                .ToList();
        }

        // Busca una categoría por nombre sin distinguir mayúsculas (null si no existe)
        public CategoryDto? GetByName(string name)
        {
            string nameLower = name.ToLower();
            return _context.Categories
                .Where(c => c.Name.ToLower() == nameLower)
                .Select(c => new CategoryDto { Id = c.Id, Name = c.Name, Description = c.Description })
                .FirstOrDefault();
        }

        // POST /categories: comprueba que el nombre no exista y la guarda
        public CategoryDto? Add(CategoryDto dto, out ErrorResponse? error)
        {
            error = null;

            if (GetByName(dto.Name) != null)
            {
                error = new ErrorResponse(5, "Ya existe una categoría con ese nombre");
                return null;
            }

            // Del DTO al modelo. El Id no se pone: lo genera SQL Server
            Category category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _context.Categories.Add(category);
            _context.SaveChanges();   // INSERT en Categories

            return new CategoryDto { Id = category.Id, Name = category.Name, Description = category.Description };
        }
    }
}
