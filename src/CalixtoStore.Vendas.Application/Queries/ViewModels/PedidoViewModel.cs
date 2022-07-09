using System;

namespace CalixtoStore.Vendas.Application.Queries.ViewModels
{
    public class PedidoViewModel
    {
        public int Codigo { get; set; }
        public decimal ValorTotal { get; set; }
        public int PedidoStatus { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}