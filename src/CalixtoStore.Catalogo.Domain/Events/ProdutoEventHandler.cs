using CalixtoStore.Catalogo.Domain.Repository;
using CalixtoStore.Core.Communication.Mediator;
using CalixtoStore.Core.Messages.CommonMessages.IntegrationEvents;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CalixtoStore.Catalogo.Domain.Events
{
    public class ProdutoEventHandler :
        INotificationHandler<ProdutoAbaixoEstoqueEvent>,
        INotificationHandler<PedidoIniciadoEvent>
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IMediatorHandler _mediatorHandler;

        public ProdutoEventHandler(IProdutoRepository produtoRepository,
                                   IEstoqueService estoqueService,
                                   IMediatorHandler mediatorHandler)
        {
            _produtoRepository = produtoRepository;
            _estoqueService = estoqueService;
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(ProdutoAbaixoEstoqueEvent notification, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.ObterPorId(notification.AggregateId);

            // Enviar um email para aquisição de mais produtos.
        }

        public async Task Handle(PedidoIniciadoEvent notification, CancellationToken cancellationToken)
        {
            var result = await _estoqueService.DebitarListaProdutosPedido(notification.ProdutosPedido);

            if (result)
            {
                await _mediatorHandler.PublicarEvento(new PedidoEstoqueConfirmadoEvent(notification.PedidoId, notification.ClienteId, notification.Total, notification.ProdutosPedido, notification.NomeCartao, notification.NumeroCartao, notification.ExpiracaoCartao, notification.CvvCartao));
            }
            else
            {
                await _mediatorHandler.PublicarEvento(new PedidoEstoqueRejeitadoEvent(notification.PedidoId, notification.ClienteId));
            }
        }
    }
}