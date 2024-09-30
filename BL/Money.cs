using BE_CodeTest.ErrorHandling.Exceptions;
using System;
using System.Linq;

namespace BE_CodeTest.BL
{
	public struct Money
	{
		public static readonly string[] SupportedCurrencies = { "EUR", "SEK" };

		public string Currency { get; }
		public decimal Amount { get; }

		public Money(string currency, decimal amount = 0m)
		{
			if (!SupportedCurrencies.Contains(currency))
			{
				throw new InvalidCurrencyException(currency);
			}

			Currency = currency;
			Amount = amount;
		}

		public static Money operator +(Money a, Money b)
		{
			if (a.Currency != b.Currency) throw new InvalidOperationException("currency mismatch");

			return new Money(a.Currency, a.Amount + b.Amount);
		}

		public static Money operator -(Money a, Money b)
		{
			if (a.Currency != b.Currency) throw new InvalidOperationException("currency mismatch");

			return new Money(a.Currency, a.Amount - b.Amount);
		}

		public static Money operator *(Money a, Money b)
		{
			if (a.Currency != b.Currency) throw new InvalidOperationException("currency mismatch");

			return new Money(a.Currency, a.Amount * b.Amount);
		}

		public static Money operator /(Money a, Money b)
		{
			if (a.Currency != b.Currency) throw new InvalidOperationException("currency mismatch");

			return new Money(a.Currency, a.Amount / b.Amount);
		}

		public static bool operator <(Money a, Money b)
		{
			if (a.Currency != b.Currency) throw new InvalidOperationException("currency mismatch");

			return a.Amount < b.Amount;
		}

		public static bool operator >(Money a, Money b)
		{
			if (a.Currency != b.Currency) throw new InvalidOperationException("currency mismatch");

			return a.Amount > b.Amount;
		}

		public static bool operator ==(Money a, Money b)
		{
			if (a.Currency != b.Currency) throw new InvalidOperationException("currency mismatch");

			return a.Amount == b.Amount;
		}

		public static bool operator !=(Money a, Money b)
		{
			if (a.Currency != b.Currency) throw new InvalidOperationException("currency mismatch");

			return a.Amount != b.Amount;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Money))
				return false;

			return this == (Money)obj;
		}

		public override int GetHashCode()
			=> base.GetHashCode();

		public override string ToString()
			=> $"{Currency} {Amount}";
	}
}