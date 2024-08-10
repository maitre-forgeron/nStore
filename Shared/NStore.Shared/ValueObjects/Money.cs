using NStore.Shared.Exceptions;

namespace NStore.Shared.ValueObjects
{
    public record Money
    {
        public decimal Amount { get; init; }

        public Currency Currency { get; init; }

        private Money()
        {
        }

        public Money(decimal amount, Currency currency) : this()
        {
            Amount = amount;
            Currency = currency;

            Validate();
        }

        public static Money CreateUSD(decimal amount)
        {
            return new Money(amount, Currency.USD);
        }

        private void Validate()
        {
            if (Amount < 0)
            {
                throw new DomainException(DomainErrorMessagesProvider.LessThanErrorMessage(nameof(Money), nameof(Amount), "0"), ExceptionLevel.Warning);
            }

            if(!Enum.IsDefined(typeof(Currency), Currency))
            {
                throw new DomainException(DomainErrorMessagesProvider.UnsupportedEnumerationErrorMessage(nameof(Currency)), ExceptionLevel.Warning);
            }
        }
    }
}
