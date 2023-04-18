using ReProServices.Domain.Common;

namespace ReProServices.Domain.Extensions
{
    public static class CurrencyExtension
    {
        public static PlaceValues GeneratePlaceValues(this int n)
        {
            var valueObj = new PlaceValues
            {
                Crores =  n / 10000000,
                Lakhs =  ((n / 100000) % 100),
                Thousands = ((n / 1000) % 100),
                Hundreds =  ((n / 100) % 10),
                Tens = ((n / 10) % 10),
                Ones =  (n % 100) % 10,
            };
            return valueObj;


        }
    }
}
