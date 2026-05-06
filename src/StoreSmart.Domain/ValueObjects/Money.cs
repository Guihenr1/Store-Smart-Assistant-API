namespace StoreSmart.Domain.ValueObjects;

public class Money
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "USD";

    private Money() { }

    public static Money Create(decimal amount, string currency = "USD")
    {
        if (amount < 0) throw new ArgumentException("Amount cannot be negative");
        return new Money { Amount = amount, Currency = currency.ToUpper() };
    }
}