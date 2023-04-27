using cev.api.Domain.ModelsApi;
using cev.api.Uteis.Results;

namespace cev.api.Domain.Interfaces
{
    public interface IProdutoApplication
    {
        Result<ProdutoLeitura> Inserir(ProdutoCriar produtoCriar);
        Result<List<ProdutoLeitura>> Listar();
        Result<ProdutoLeitura> RecuperarPorId(int id);
        public Result<ProdutoLeitura> AtualizarDescricao(int id, string descricao);
        public Result<ProdutoLeitura> AtualizarValor(int id, double valor);
        public Result<ProdutoLeitura> AtualizarEstoque(int id, int estoque);
        Result Excluir(int id);
    }
}
