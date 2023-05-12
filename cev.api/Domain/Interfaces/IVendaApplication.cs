using cev.api.Domain.ModelsApi;
using cev.api.Uteis.Results;

namespace cev.api.Domain.Interfaces
{
    public interface IVendaApplication
    {
        Result<VendaLeitura> Inserir(VendaCriar vendaCriar);
        Result<List<VendaLeitura>> Listar();
        Result<VendaLeitura> RecuperarPorId(int id);
        Result Excluir(int id);
    }
}
