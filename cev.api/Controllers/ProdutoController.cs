using cev.api.Domain.Interfaces;
using cev.api.Domain.ModelsApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ProdutoCriar), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult Atualizar([FromRoute] int id, [FromBody] ProdutoAtualizar produtoAtualizar)
        {
            var resultado = _produtoApplication.Atualizar(id, produtoAtualizar.Descricao, produtoAtualizar.Valor);

            if (resultado.Invalid)
                return BadRequest(resultado.Notifications);

            return Ok(resultado.Object);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPatch]
        [Route("/atualizar-estoque/{id}")]
        [ProducesResponseType(typeof(ProdutoCriar), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult AtualizarValor([FromRoute] int id, [FromBody] ProdutoAtualizarEstoque produtoAtualizarEstoque)
        {
            var resultado = _produtoApplication.AtualizarEstoque(id, produtoAtualizarEstoque.TipoAtualizacao, produtoAtualizarEstoque.Valor);

            if (resultado.Invalid)
                return BadRequest(resultado.Notifications);

            return Ok(resultado.Object);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
