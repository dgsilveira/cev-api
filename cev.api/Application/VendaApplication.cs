using cev.api.Data;
using cev.api.Domain.Entidades;
using cev.api.Domain.Interfaces;
using cev.api.Domain.ModelsApi;
using cev.api.Domain.ModelsDb;
using cev.api.Uteis;
using cev.api.Uteis.Results;
using Flunt.Notifications;

namespace cev.api.Application
{
    public class VendaApplication : IVendaApplication
    {
        private readonly AppDbContext _appDbContext;

        public VendaApplication(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Result Excluir(int id)
        {
            var modelsDb = _appDbContext.Vendas.Where(v => v.IdVenda == id).ToList();

            if (!modelsDb.Any())
                return Result.Error(new Notification(nameof(id), Constantes.Vendas.VENDA_NAO_ENCONTRADA));

            _appDbContext.Vendas.RemoveRange(modelsDb);
            _appDbContext.SaveChanges();

            return Result.Ok();
        }

        //TODO: Estou nesse
        public Result<VendaLeitura> Inserir(VendaCriar vendaCriar)
        {
            var produtos = verificarQuantidadeProdutos(vendaCriar.Produtos);


            var entidade = VendaCriarParaVenda(vendaCriar);

            if (entidade.Invalid)
                return Result<VendaLeitura>.Error(entidade.Notifications);

            var modelsDb = VendaParaListVendaDb(entidade);

            foreach (var model in modelsDb)
            {
                _appDbContext.Vendas.Add(model);
                _appDbContext.SaveChanges();
            }

            return Result<VendaLeitura>.Ok(ListVendaDbParaVendaLeitura(modelsDb));
        }

        public Result<List<VendaLeitura>> Listar()
        {
            var modelsDb = _appDbContext.Vendas.ToList();

            if (!modelsDb.Any())
                return Result<List<VendaLeitura>>.Error(new Notification(nameof(VendaLeitura), Constantes.Vendas.VENDA_NAO_ENCONTRADA));

            List<VendaLeitura> modelsLeitura = ListVendaDbParaListVendaLeitura(modelsDb);

            return Result<List<VendaLeitura>>.Ok(modelsLeitura);
        }

        public Result<VendaLeitura> RecuperarPorId(int id)
        {
            var modelsDb = _appDbContext.Vendas.Where(v => v.IdVenda == id).ToList();

            if (!modelsDb.Any())
                return Result<VendaLeitura>.Error(new Notification(nameof(VendaLeitura), Constantes.Vendas.VENDA_NAO_ENCONTRADA));

            return Result<VendaLeitura>.Ok(ListVendaDbParaVendaLeitura(modelsDb));
        }

        #region Métodos Privados

        private bool verificarQuantidadeProdutos(IReadOnlyCollection<VendaProdutoCriar> produtos)
        {
            return null;
        }
        private Venda VendaCriarParaVenda(VendaCriar vendaCriar)
        {
            return new Venda
                (
                    buscarUltimoValor(),
                    vendaCriar.DataVenda,
                    vendaCriar.FormaPagamento,
                    VendedorDbParaVendedor(_appDbContext.Vendedores.Find(vendaCriar.VendedorId)),
                    ListVendaProdutoCriarParaListVendaProduto(vendaCriar.Produtos).AsReadOnly()
                );
        }

        private VendaLeitura ListVendaDbParaVendaLeitura(List<VendaDb> vendasDb)
        {
            List<VendaProdutoLeitura> produtos = new List<VendaProdutoLeitura>();

            foreach (var item in vendasDb)
            {
                produtos.Add(new VendaProdutoLeitura
                {
                    ProdutoId = item.ProdutoId,
                    ProdutoValor = item.ProdutoValor,
                    Quantidade = item.Quantidade
                });
            }

            VendedorLeitura vendedorLeitura = new VendedorLeitura
            {
                Id = vendasDb[0].VendedorId,
                Nome = _appDbContext.Vendedores.Find(vendasDb[0].VendedorId).Nome
            };

            return new VendaLeitura
            {
                Id = vendasDb[0].IdVenda,
                DataVenda = vendasDb[0].DataVenda,
                FormaPagamento = vendasDb[0].FormaPagamento,
                Vendedor = vendedorLeitura,
                Produtos = produtos
            };
        }


        private Vendedor VendedorDbParaVendedor(VendedorDb vendedorDb)
        {
            return new Vendedor(vendedorDb.Id, vendedorDb.Nome);
        }

        private List<VendaDb> VendaParaListVendaDb(Venda venda)
        {
            List<VendaDb> vendasDb = new List<VendaDb>();

            VendedorDb vendedorDb = new VendedorDb
            {
                Id = venda.Vendedor.Id,
                Nome = venda.Vendedor.Nome
            };

            foreach (var item in venda.Produtos)
            {
                VendaDb vendaDb = new VendaDb
                {
                    IdDb = default,
                    IdVenda = venda.Id,
                    DataVenda = venda.DataVenda,
                    FormaPagamento = venda.FormaPagamento,
                    VendedorId = venda.Vendedor.Id,
                    ProdutoId = item.ProdutoId,
                    ProdutoValor = item.ProdutoValor,
                    Quantidade = item.Quantidade
                };

                vendasDb.Add(vendaDb);
            }

            return vendasDb;
        }

        private List<VendaProduto> ListVendaProdutoCriarParaListVendaProduto(IReadOnlyCollection<VendaProdutoCriar> produtos)
        {
            List<VendaProduto> vendaProdutos = new List<VendaProduto>();

            foreach (var produto in produtos)
            {
                var produtoDb = _appDbContext.Produtos.Find(produto.ProdutoId);
                VendaProduto vendaProduto;

                if (produtoDb == null)
                {
                    vendaProduto = new VendaProduto(produto.ProdutoId, 0, produto.Quantidade);
                    vendaProduto
                        .AddNotification(nameof(produto.ProdutoId), Constantes.Entidades.ID_PRODUTO_INVALIDO);
                    vendaProdutos.Add(vendaProduto);
                }
                else
                {
                    vendaProdutos.Add(new VendaProduto(produto.ProdutoId, produtoDb.Valor, produto.Quantidade));
                }
            }

            return vendaProdutos;
        }

        private List<VendaLeitura> ListVendaDbParaListVendaLeitura(List<VendaDb> vendasDb)
        {
            List<VendaLeitura> vendas = new List<VendaLeitura>();

            var listas = vendasDb.GroupBy(v => v.IdVenda);

            foreach (var group in listas)
            {
                List<VendaDb> vendasDbGroup = new List<VendaDb>();
                vendasDbGroup.AddRange(group);
                vendas.Add(ListVendaDbParaVendaLeitura(vendasDbGroup));
            }

            return vendas;
        }

        private int buscarUltimoValor()
        {
            var controle = _appDbContext.ControlesDiversos.FirstOrDefault(c => c.Id == 1);

            if (controle == null)
            {
                controle = new ControlesDiversos
                {
                    CodigoVenda = 0
                };
                _appDbContext.ControlesDiversos.Add(controle);
                _appDbContext.SaveChanges();

                return controle.CodigoVenda;
            }

            controle.CodigoVenda++;
            _appDbContext.SaveChanges();
            return controle.CodigoVenda;
        }
        #endregion
    }
}
