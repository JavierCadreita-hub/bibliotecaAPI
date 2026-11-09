using BibliotecaAPI.DTO;
using BibliotecaAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryRepository _repository;

        // Inyección de dependencias: .NET crea el repositorio (con su DbContext) y lo pasa aquí
        public CategoriesController(CategoryRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// GET /categories
        /// Devuelve todas las categorías.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<CategoryDto>> GetCategories()
        {
            try
            {
                IEnumerable<CategoryDto> categories = _repository.GetAll();
                if (!categories.Any())
                {
                    return BadRequest(new ErrorResponse(2, "No hay categorías"));
                }
                return Ok(categories);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(1, "Error interno del servidor"));
            }
        }

        /// <summary>
        /// POST /categories
        /// Crea una categoría.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public ActionResult<CategoryDto> CreateCategory([FromBody] CategoryDto dto)
        {
            try
            {
                // Condiciones de barrera: no necesitan la BBDD
                if (dto == null || (string.IsNullOrWhiteSpace(dto.Name) && string.IsNullOrWhiteSpace(dto.Description)))
                {
                    return BadRequest(new ErrorResponse(3, "Datos de la categoría vacíos o no válidos"));
                }
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest(new ErrorResponse(4, "El nombre de la categoría es obligatorio"));
                }

                // El repositorio comprueba en la BBDD que no se repita el nombre (5) y la guarda
                CategoryDto? created = _repository.Add(dto, out ErrorResponse? error);
                if (error != null)
                {
                    return BadRequest(error);
                }

                return StatusCode(StatusCodes.Status201Created, created);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(1, "Error interno del servidor"));
            }
        }
    }
}
