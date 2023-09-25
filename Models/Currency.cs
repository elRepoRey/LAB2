using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Models
{
    public class Currency
    {
        public string Code { get; }
        public decimal ConversionRate { get; private set; }

        public Currency(string code, decimal conversionRate)
        {
            Code = code;
            ConversionRate = conversionRate;
        }

        public void AdjustRateByPercentage(decimal percentage)
        {
            ConversionRate += ConversionRate * percentage / 100;
        }
    }

}