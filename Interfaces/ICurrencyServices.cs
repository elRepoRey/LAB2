using Lab2.Utils;

namespace Lab2.Interfaces
{
    internal interface ICurrencyServices
    {
        CurrencyType GlobalCurrency { get; set; }

        decimal ConvertToGlobalCurrency(decimal amount);

        string GetCurrencySymbol(CurrencyType? currency);

        void SetGlobalCurrency(CurrencyType currency);

        event Action OnCurrencyChanged;
        decimal ConvertToDatabaseCurrency(decimal amount);
    }
}
