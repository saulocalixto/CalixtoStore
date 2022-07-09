using CalixtoStore.Core.Communication.Mediator;
using CalixtoStore.Core.DomainObjects.DTO;
using CalixtoStore.Core.Extensions;
using CalixtoStore.Core.Messages;
using CalixtoStore.Core.Messages.CommonMessages.IntegrationEvents;
using CalixtoStore.Core.Messages.CommonMessages.Notifications;
using CalixtoStore.Vendas.Application.Events;
using CalixtoStore.Vendas.Domain.Aggregates;
using CalixtoStore.Vendas.Domain.Repository;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CalixtoStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler
        : IRequestHandler<AdicionarItemPedidoCommand, bool>,
        IRequestHandler<AtualizarItemPedidoCommand, bool>,
        IRequestHandler<RemoverItemPedidoCommand, bool>,
        IRequestHandler<AplicarVoucherPedidoCommand, bool>,
        IRequestHandler<IniciarPedidoCommand, bool>
    {
        private readonly IPedidoRepository _repository;
        private readonly IMediatorHandler _mediator;

        public PedidoCommandHandler(IPedidoRepository repository, IMediatorHandler mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;

            var pedido = await _repository.ObterPedidoRascunhoPorClienteId(message.ClienteId);
            var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);

            if (pedido == null)
            {
                pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
                pedido.AdicionarItem(pedidoItem);

                _repository.Adicionar(pedido);
                pedido.AdicionarEvento(new PedidoRascunhoIniciadoEvent(message.ClienteId, message.ProdutoId));
            }
            else
            {
                var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
                pedido.AdicionarItem(pedidoItem);

                if (pedidoItemExistente)
                {
                    _repository.AtualizarItem(pedido.PedidoItems.FirstOrDefault(x => x.ProdutoId == pedidoItem.ProdutoId));
                }
                else
                {
                    _repository.AdicionarItem(pedidoItem);
                }

                pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            }

            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, message.ProdutoId, pedido.Id, message.ValorUnitario, message.Quantidade, message.Nome));

            return await _repository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(AtualizarItemPedidoCommand request, CancellationToken cancellationToken)
        {
            if (ValidarComando(request)) return false;

            var pedido = await _repository.ObterPedidoRascunhoPorClienteId(request.ClienteId);

            if (pedido == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            var pedidoItem = await _repository.ObterItemPorPedido(pedido.Id, request.ProdutoId);

            if (!pedido.PedidoItemExistente(pedidoItem))
            {
                await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Item do pedido não encontrado!"));
                return false;
            }

            pedido.AtualizarUnidades(pedidoItem, request.Quantidade);

            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AdicionarEvento(new PedidoProdutoAtualizadoEvent(pedido.ClienteId, pedido.Id, request.ProdutoId, request.Quantidade));

            _repository.AtualizarItem(pedidoItem);
            _repository.Atualizar(pedido);

            return await _repository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(RemoverItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;

            var pedido = await _repository.ObterPedidoRascunhoPorClienteId(message.ClienteId);

            if (pedido == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            var pedidoItem = await _repository.ObterItemPorPedido(pedido.Id, message.ProdutoId);

            if (pedidoItem != null && !pedido.PedidoItemExistente(pedidoItem))
            {
                await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Item do pedido não encontrado!"));
                return false;
            }

            pedido.RemoverItem(pedidoItem);
            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AdicionarEvento(new PedidoProdutoRemovidoEvent(message.ClienteId, pedido.Id, message.ProdutoId));

            _repository.RemoverItem(pedidoItem);
            _repository.Atualizar(pedido);

            return await _repository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(AplicarVoucherPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;

            var pedido = await _repository.ObterPedidoRascunhoPorClienteId(message.ClienteId);

            if (pedido == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            var voucher = await _repository.ObterVoucherPorCodigo(message.CodigoVoucher);

            if (voucher == null)
            {
                await _mediator.PublicarNotificacao(new DomainNotification("pedido", "Voucher não encontrado!"));
                return false;
            }

            var voucherAplicacaoValidation = pedido.AplicarVoucher(voucher);
            if (!voucherAplicacaoValidation.IsValid)
            {
                foreach (var error in voucherAplicacaoValidation.Errors)
                {
                    await _mediator.PublicarNotificacao(new DomainNotification(error.ErrorCode, error.ErrorMessage));
                }

                return false;
            }

            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AdicionarEvento(new VoucherAplicadoPedidoEvent(message.ClienteId, pedido.Id, voucher.Id));

            _repository.Atualizar(pedido);

            return await _repository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(IniciarPedidoCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarComando(request)) return false;

            var pedido = await _repository.ObterPedidoRascunhoPorClienteId(request.ClienteId);
            pedido.IniciarPedido();

            var itensList = new List<Item>();
            pedido.PedidoItems.ForEach(i => itensList.Add(new Item { Id = i.ProdutoId, Quantidade = i.Quantidade }));
            var listaProdutosPedido = new ListaProdutosPedido { PedidoId = pedido.Id, Itens = itensList };

            pedido.AdicionarEvento(new PedidoIniciadoEvent(pedido.Id, pedido.ClienteId, listaProdutosPedido, pedido.ValorTotal, request.NomeCartao, request.NumeroCartao, request.ExpiracaoCartao, request.CvvCartao));

            _repository.Atualizar(pedido);
            return await _repository.UnitOfWork.Commit();
        }

        private bool ValidarComando(Command message)
        {
            if (message.EValido()) return true;

            foreach (var item in message.ValidationResult.Errors)
            {
                _mediator.PublicarNotificacao(new DomainNotification(message.MessageType, item.ErrorMessage));
            }

            return false;
        }
    }
}