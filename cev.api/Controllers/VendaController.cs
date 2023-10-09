using cev.api.Domain.Interfaces;
using cev.api.Domain.ModelsApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace cev.api.Controllers
{
    [ApiController]
    [Route("venda")]
    public class VendaController : ApiBaseController
    {
        private readonly IVendaApplication _vendaApplication;

        public VendaController(IVendaApplication vendaApplication)
        {
            _vendaApplication = vendaApplication;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(VendaCriar), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult Inserir([FromBody] VendaCriar vendaCriar)
        {
            var resultado = _vendaApplication.Inserir(vendaCriar);

            if (resultado.Invalid)
                return BadRequest(resultado.Notifications);

            return Created("", resultado);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(List<VendaLeitura>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult Listar(string startDate, string endDate)
        {
            var resultado = _vendaApplication.Listar(startDate, endDate);

            if (resultado.Invalid)
                return BadRequest(resultado.Notifications);

            return Ok(resultado.Object);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VendaLeitura), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult RecuperarPorId(int id)
        {
            var resultado = _vendaApplication.RecuperarPorId(id);

            if (resultado == null)
                return NoContent();

            if (resultado.Invalid)
                return BadRequest(resultado.Notifications);

            return Ok(resultado.Object);
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult Excluir(int id)
        {
            var resultado = _vendaApplication.Excluir(id);

            if (resultado.Invalid)
                return BadRequest(resultado.Notifications);

            return NoContent();
        }
    }
}
