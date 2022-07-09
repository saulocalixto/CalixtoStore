using FluentValidation.Results;
using MediatR;
using System;

namespace CalixtoStore.Core.Messages
{
    public class Command : Message, IRequest<bool>
    {
        public DateTime Timestamp { get; set; }
        public ValidationResult ValidationResult { get; set; }

        public Command()
        {
            Timestamp = DateTime.Now;
        }

        public virtual bool EValido()
        {
            throw new NotImplementedException();
        }
    }
}