using BibliotecaAPI.DTO;
using BibliotecaAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorRepository _repository;

        // Inyección de dependencias: .NET crea el repositorio (con su DbContext) y lo pasa aquí
        public AuthorsController(AuthorRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// GET /authors, /authors?id=1, /authors?nationality=británica
        /// Devuelve los autores: todos, uno por Id o los de una nacionalidad.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AuthorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery] int? id, [FromQuery] string? nationality)
        {
            try
            {
                // 1. Por Id: devuelve una lista de un solo autor (o 404)
                if (id.HasValue)
                {
                    AuthorDto? author = _repository.GetById(id.Value);
                    if (author == null)
                    {
                        return NotFound(new ErrorResponse(2, $"No existe el autor con ID {id.Value}"));
                    }
                    return Ok(new[] { author });
                }

                // 2. Por nacionalidad
                if (!string.IsNullOrWhiteSpace(nationality))
                {
                    return Ok(_repository.GetByNationality(nationality));
                }

                // 3. Sin filtros: todos
                IEnumerable<AuthorDto> authors = _repository.GetAll();
                if (!authors.Any())
                {
                    return BadRequest(new ErrorResponse(2, "No hay autores"));
                }
                return Ok(authors);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(1, "Error interno del servidor"));
            }
        }

        /// <summary>
        /// POST /authors
        /// Crea un autor.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public ActionResult<AuthorDto> CreateAuthor([FromBody] AuthorDto dto)
        {
            try
            {
                // Condiciones de barrera: no necesitan la BBDD
                if (dto == null || (string.IsNullOrWhiteSpace(dto.Name) && string.IsNullOrWhiteSpace(dto.Nationality) && dto.BirthYear == 0))
                {
                    return BadRequest(new ErrorResponse(3, "Datos del autor vacíos o no válidos"));
                }
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest(new ErrorResponse(4, "El nombre del autor es obligatorio"));
                }

                // El repositorio comprueba en la BBDD que no se repita el nombre (33) y lo guarda
                AuthorDto? created = _repository.Add(dto, out ErrorResponse? error);
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
