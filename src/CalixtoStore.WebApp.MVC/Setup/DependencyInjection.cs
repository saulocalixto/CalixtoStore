using CalixtoStore.Catalogo.Application.Services;
using CalixtoStore.Catalogo.Database;
using CalixtoStore.Catalogo.Database.Repository;
using CalixtoStore.Catalogo.Domain;
using CalixtoStore.Catalogo.Domain.Events;
using CalixtoStore.Catalogo.Domain.Repository;
using CalixtoStore.Core.Communication.Mediator;
using CalixtoStore.Core.Messages.CommonMessages.Notifications;
using CalixtoStore.Vendas.Application.Commands;
using CalixtoStore.Vendas.Application.Events;
using CalixtoStore.Vendas.Application.Queries;
using CalixtoStore.Vendas.Database.Repository;
using CalixtoStore.Vendas.Domain.Repository;
using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace CalixtoStore.WebApp.MVC.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Mediatr
            services.AddScoped<IMediatorHandler, MediatrHandler>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Catalogo
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoAppService, ProdutoAppService>();
            services.AddScoped<IEstoqueService, EstoqueService>();
            services.AddScoped<CatalogoContext>();

            services.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();

            services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<AtualizarItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<RemoverItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<AplicarVoucherPedidoCommand, bool>, PedidoCommandHandler>();

            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IPedidoQueries, PedidosQueries>();

            services.AddScoped<INotificationHandler<PedidoRascunhoIniciadoEvent>, PedidoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoAtualizadoEvent>, PedidoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoItemAdicionadoEvent>, PedidoEventHandler>();
        }
    }
}