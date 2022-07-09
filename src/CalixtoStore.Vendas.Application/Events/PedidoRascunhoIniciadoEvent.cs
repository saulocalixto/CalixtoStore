using CalixtoStore.Core.Messages;
using System;

namespace CalixtoStore.Vendas.Application.Events
{
    public class PedidoRascunhoIniciadoEvent : Event
    {
        public PedidoRascunhoIniciadoEvent(Guid clienteId, Guid pedidoId)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
            AggregateId = pedidoId;
        }

        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
    }
}