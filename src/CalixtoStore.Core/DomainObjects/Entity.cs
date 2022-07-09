using CalixtoStore.Core.Messages;
using System;
using System.Collections.Generic;

namespace CalixtoStore.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        private List<Event> _events;
        public IReadOnlyCollection<Event> Eventos => _events?.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public void AdicionarEvento(Event evento)
        {
            _events = _events ?? new List<Event>();
            _events.Add(evento);
        }

        public void RemoverEvento(Event eventItem)
        {
            _events?.Remove(eventItem);
        }

        public void LimparEventos()
        {
            _events?.Clear();
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + $"[Id = {Id}]";
        }

        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}