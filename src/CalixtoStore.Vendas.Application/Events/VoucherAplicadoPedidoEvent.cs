using CalixtoStore.Core.Messages;
using System;

namespace CalixtoStore.Vendas.Application.Events
{
    public class VoucherAplicadoPedidoEvent : Event
    {
        public VoucherAplicadoPedidoEvent(Guid clienteId, Guid pedidoId, Guid voucherId)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            VoucherId = voucherId;
        }

        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }
        public Guid VoucherId { get; set; }
    }
}