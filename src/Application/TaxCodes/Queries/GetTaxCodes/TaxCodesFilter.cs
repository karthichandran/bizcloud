using ReProServices.Domain.Enums;

namespace ReProServices.Application.TaxCodes.Queries.GetTaxCodes
{
    public class TaxCodesFilter
    {
        public string  TaxName { get; set; }

        public ETaxType? TaxType { get; set; }

    }
}

