using BibliotecaAPI.DTO;
using BibliotecaAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("books")]
    public class BooksController : ControllerBase
    {
        private readonly BookRepository _repository;

        // Inyección de dependencias: .NET crea el repositorio (con su DbContext) y lo pasa aquí
        public BooksController(BookRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// POST /books
        /// Crea un libro con su categoría y sus autores.
        /// Las validaciones de formato (20-24) las hacen las DataAnnotations de BookDto
        /// antes de entrar aquí; las que necesitan la BBDD (34-36) las hace el repositorio.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(BookDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<ErrorResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public ActionResult<BookDto> CreateBook([FromBody] BookDto dto)
        {
            try
            {
                BookDto? created = _repository.Add(dto, out List<ErrorResponse> errors);
                if (created == null)
                {
                    return BadRequest(errors);   // todos los errores a la vez
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
