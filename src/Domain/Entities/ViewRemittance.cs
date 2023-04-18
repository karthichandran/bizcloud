using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities
{
    /// <summary>
    /// ID's & No's are handled as string as the requirement is to preserve the leading zeros
    /// </summary>
    public class ViewRemittance
    {
        public int RemittanceID { get; set; }
        public int ClientPaymentTransactionID { get; set; }
        public string ChallanID { get; set; }
        public DateTime ChallanDate { get; set; }
        public string ChallanAckNo { get; set; }
        public decimal ChallanAmount { get; set; }
        [ObsoleteAttribute]
        public Guid ChallanFileID { get; set; }
        public DateTime? F16BDateOfReq { get; set; }
        public string F16BRequestNo { get; set; }
        public string F16BCertificateNo { get; set; }
        [ObsoleteAttribute]
        public Guid F16BFileID { get; set; }
        public int? Form16BlobID { get; set; }
        public int? ChallanBlobID { get; set; }
        public string F16CustName { get; set; }
        public DateTime? F16UpdateDate { get; set; }
        public decimal? F16CreditedAmount { get; set; }
        public bool EmailSent { get; set; }
        public DateTime? EmailSentDate { get; set; }
        public int CustomerID { get; set; }
        public DateTime? Form16BFileDownloadDate { get; set; }
        public Guid OwnershipID { get; set; }
        public int ClientPaymentID { get; set; }
    }
}
