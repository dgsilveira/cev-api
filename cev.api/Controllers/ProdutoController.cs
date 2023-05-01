using cev.api.Domain.Interfaces;
using cev.api.Domain.ModelsApi;
using Microsoft.AspNetCore.Mvc;

namespace cev.api.Controllers
{
    [ApiController]
    [Route("produto")]
    public class ProdutoController : ApiBaseController
    {
        private readonly IProdutoApplication _produtoApplication;

        public ProdutoController(IProdutoApplication produtoApplication)
        {
            _produtoApplication = produtoApplication;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProdutoCriar), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult Inserir([FromBody] ProdutoCriar produtoCriar)
        {
            var resultado = _produtoApplication.Inserir(produtoCriar);

            if (resultado.Invalid)
                return BadRequest(resultado.Notifications);

            return Created("", resultado);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProdutoLeitura>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult Listar()
        {
            var resultado = _produtoApplication.Listar();

            if (resultado.Invalid)
                return BadRequest(resultado.Notifications);

            if (resultado.Object.Count == 0)
                return NoContent();

            return Ok(resultado.Object);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProdutoLeitura), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult RecuperarPorId(int id)
        {
            var resultado = _produtoApplication.RecuperarPorId(id);

            if (resultado == null)
                return NoContent();

            if (resultado.Invalid)
                return BadRequest(resultado.Notifications);

            return Ok(resultado.Object);
        }

        [HttpPatch]
        [Route("/atualizar-descricao/{id}")]
        [ProducesResponseType(typeof(ProdutoCriar), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult AtualizarDescricao([FromRoute] int id, [FromBody] string descricao)
        {
            var resultado = _produtoApplication.AtualizarDescricao(id, descricao);

            if (resultado.Invalid)
                return BadRequest(resultado.Notifications);

            return Ok(resultado.Object);
        }

        [HttpPatch]
        [Route("/atualizar-valor/{id}")]
        [ProducesResponseType(typeof(ProdutoCriar), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult AtualizarValor(int id, [FromBody] double valor)
        {
            var resultado = _produtoApplication.AtualizarValor(id, valor);

            if (resultado.Invalid)
                return BadRequest(resultado.Notifications);

            return Ok(resultado.Object);
        }

        [HttpPatch]
        [Route("/atualizar-estoque/{id}/{estoque}")]
        [ProducesResponseType(typeof(ProdutoCriar), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult AtualizarValor([FromRoute] int id, [FromRoute] int estoque)
        {
            var resultado = _produtoApplication.AtualizarEstoque(id, estoque);

            if (resultado.Invalid)
                return BadRequest(resultado.Notifications);

            return Ok(resultado.Object);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult Excluir(int id)
        {
            var resultado = _produtoApplication.Excluir(id);

            if (resultado.Invalid)
                return BadRequest(resultado.Notifications);

            return NoContent();
        }
    }
}
