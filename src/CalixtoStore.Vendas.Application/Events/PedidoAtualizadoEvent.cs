using CalixtoStore.Core.Messages;
using System;

namespace CalixtoStore.Vendas.Application.Events
{
    public class PedidoAtualizadoEvent : Event
    {
        public PedidoAtualizadoEvent(Guid clienteId, Guid pedidoId, decimal valorTotal)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
            ValorTotal = valorTotal;
        }

        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public decimal ValorTotal { get; private set; }
    }
}