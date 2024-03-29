﻿using CalixtoStore.Core.Messages;
using CalixtoStore.Vendas.Application.Commands.Validacoes;
using System;

namespace CalixtoStore.Vendas.Application.Commands
{
    public class AdicionarItemPedidoCommand : Command
    {
        public AdicionarItemPedidoCommand(
            Guid clienteId,
            Guid produtoId,
            string nome,
            int quantidade,
            decimal valorUnitario)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            Nome = nome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string Nome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public override bool EValido()
        {
            var validacao = new AdicionarItemPedidoValidation();
            ValidationResult = validacao.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}