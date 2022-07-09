using CalixtoStore.Core.Communication.Mediator;
using CalixtoStore.Core.Messages.CommonMessages.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalixtoStore.WebApp.MVC.Controllers
{
    public class ControllerBase : Controller
    {
        private readonly DomainNotificationHandler _notification;
        private readonly IMediatorHandler _mediator;

        protected Guid ClienteId = Guid.Parse("4885e451-b0e4-4490-b959-04fabc806d32");

        public ControllerBase(INotificationHandler<DomainNotification> notification, IMediatorHandler mediator)
        {
            _mediator = mediator;
            _notification = (DomainNotificationHandler)notification;
        }

        protected bool OperacaoValida()
        {
            return !_notification.TemNotificacao();
        }

        protected void NotificarErro(string codigo, string mensagem)
        {
            _mediator.PublicarNotificacao(new DomainNotification(codigo, mensagem));
        }

        protected IEnumerable<string> ObterMensagensErro()
        {
            return _notification.ObterNotificacoes().Select(x => x.Value).ToList();
        }
    }
}