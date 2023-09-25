using System;
using System.Collections.Generic;
using Lab2.Models;
using Lab2.Interfaces;
using Lab2.Utils;

namespace Lab2.Services
{
    internal class CurrencyServices : ICurrencyServices
    {
        private CurrencyType _databaseCurrency { get; set; }
        public CurrencyType GlobalCurrency { get; set; } 
        private static readonly Random _random = new Random();
        public event Action OnCurrencyChanged;

        public CurrencyServices(IStoreConfig storeConfig)
        {
            _databaseCurrency = storeConfig.DataBaseCurrency;
            GlobalCurrency = _databaseCurrency;

        }

        private static readonly Dictionary<CurrencyType, decimal> _baseConversionRates = new Dictionary<CurrencyType, decimal>
        {
            { CurrencyType.SEK, 1.0m },
            { CurrencyType.USD, 0.11m },
            { CurrencyType.EUR, 0.10m },
            { CurrencyType.GBP, 0.09m },
            { CurrencyType.JPY, 12.34m }
        };

        public decimal ConvertToGlobalCurrency(decimal amount)
        {
            if (_databaseCurrency != CurrencyType.SEK)
            {
                decimal randomAdjustment = (decimal)_random.NextDouble() * 0.02m - 0.01m;
                _baseConversionRates[_databaseCurrency] += _baseConversionRates[_databaseCurrency] * randomAdjustment;
            }

            if (_databaseCurrency == GlobalCurrency) return amount;

            decimal amountInSEK = amount / _baseConversionRates[_databaseCurrency];
            return amountInSEK * _baseConversionRates[GlobalCurrency];
        }

        public string GetCurrencySymbol(CurrencyType? currency = null)
        {
            currency ??= GlobalCurrency;

            if (_baseConversionRates.ContainsKey(currency.Value))
            {
                return currency.ToString();
            }
            
            throw new ArgumentOutOfRangeException(nameof(currency), "Unknown currency type");
        }

        public decimal ConvertToDatabaseCurrency(decimal amount)
        {
            if (_databaseCurrency == GlobalCurrency) return amount;

            decimal amountInSEK = amount / _baseConversionRates[GlobalCurrency];
            return amountInSEK * _baseConversionRates[_databaseCurrency];
        }

        public void SetGlobalCurrency(CurrencyType currency)
        {
            GlobalCurrency = currency;
            OnCurrencyChanged?.Invoke();
        }
    }
}
