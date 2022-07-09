using CalixtoStore.Core.Messages;
using CalixtoStore.Vendas.Application.Commands.Validacoes;
using System;

namespace CalixtoStore.Vendas.Application.Commands
{
    public class AtualizarItemPedidoCommand : Command
    {
        public AtualizarItemPedidoCommand(Guid clienteId, Guid produtoId, int quantidade)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }

        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public int Quantidade { get; private set; }

        public override bool EValido()
        {
            ValidationResult = new AtualizarItemPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}