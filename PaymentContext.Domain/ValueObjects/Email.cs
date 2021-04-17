using Flunt.Notifications;
using Flunt.Validations;
using PaymentContext.Shered.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public Email(string address)
        {
            Address = address;

            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsEmail(Address, "Email.Address", "E-mail inválido")
            );
        }

        public string Address { get; private set; }
         
    }
}