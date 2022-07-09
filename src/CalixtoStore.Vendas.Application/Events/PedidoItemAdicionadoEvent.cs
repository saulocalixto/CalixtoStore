using CalixtoStore.Core.Messages;
using System;

namespace CalixtoStore.Vendas.Application.Events
{
    public class PedidoItemAdicionadoEvent : Event
    {
        public PedidoItemAdicionadoEvent(Guid clienteId, Guid produtoId, Guid pedidoId, decimal valorUnitario, int quantidade, string produtoNome)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            PedidoId = pedidoId;
            ValorUnitario = valorUnitario;
            Quantidade = quantidade;
            AggregateId = PedidoId;
            ProdutoNome = produtoNome;
        }

        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public Guid PedidoId { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public int Quantidade { get; private set; }
        public string ProdutoNome { get; set; }
    }
}