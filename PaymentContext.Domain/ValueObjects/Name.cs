using Flunt.Notifications;
using Flunt.Localization;
using Flunt.Validations;
using PaymentContext.Shered.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public class Name : ValueObject
    {
        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

            AddNotifications(new Contract<Notification>()
              .Requires()              
              .IsGreaterThan(FirstName, 3, "Name.FirstName", "Nome deve conter pelos menos 3 caracteres")
              .IsGreaterThan(LastName, 3, "Name.LastName","Sobrenome deve conter pelos menos 3 caracteres")
              .IsLowerOrEqualsThan(FirstName, 40, "Name.FirstName", "Nome deve coter até 40 caracteres")
            );
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}