using System;
using Flunt.Notifications;

namespace PaymentContext.Shered.Entities
{
    public abstract class Entity : Notifiable<Notification>
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; private set; }
    }
}