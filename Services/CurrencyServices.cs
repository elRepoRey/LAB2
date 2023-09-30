using System;
using System.Collections.Generic;
using Lab2.Models;

using Lab2.Utils;

namespace Lab2.Services
{
    internal static class CurrencyServices
    {
        public static CurrencyType DatabaseCurrency { get; private set; } = CurrencyType.SEK;
        public static CurrencyType GlobalCurrency { get; private set; } = CurrencyType.SEK;
        private static readonly Random random = new Random();
        public static event Action OnCurrencyChanged = delegate { };

        static CurrencyServices()
        {
            DatabaseCurrency = StoreConfig.DataBaseCurrency;
        }

        private static readonly Dictionary<CurrencyType, decimal> _baseConversionRates = new Dictionary<CurrencyType, decimal>
        {
            { CurrencyType.SEK, 1.0m },
            { CurrencyType.USD, 0.11m },
            { CurrencyType.EUR, 0.10m },
            { CurrencyType.GBP, 0.09m },
            { CurrencyType.JPY, 12.34m }
        };

        public static decimal ConvertToGlobalCurrency(decimal amount)
        {
            if (DatabaseCurrency != CurrencyType.SEK)
            {
                decimal randomAdjustment = (decimal)random.NextDouble() * 0.02m - 0.01m;
                _baseConversionRates[DatabaseCurrency] += _baseConversionRates[DatabaseCurrency] * randomAdjustment;
            }

            if (DatabaseCurrency == GlobalCurrency) return amount;

            decimal amountInSEK = amount / _baseConversionRates[DatabaseCurrency];
            return amountInSEK * _baseConversionRates[GlobalCurrency];
        }

        public static string GetCurrencySymbol(CurrencyType? currency = null)
        {
            currency ??= GlobalCurrency;

            if (_baseConversionRates.ContainsKey(currency.Value))
            {
                return currency.ToString();
            }
            
            throw new ArgumentOutOfRangeException(nameof(currency), "Unknown currency type");
        }

        public static decimal ConvertToDatabaseCurrency(decimal amount)
        {
            if (DatabaseCurrency == GlobalCurrency) return amount;

            decimal amountInSEK = amount / _baseConversionRates[GlobalCurrency];
            return amountInSEK * _baseConversionRates[DatabaseCurrency];
        }

        public static void SetGlobalCurrency(CurrencyType currency)
        {
            GlobalCurrency = currency;
            OnCurrencyChanged?.Invoke();
        }
    }
}
