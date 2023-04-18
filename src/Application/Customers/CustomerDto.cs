using AutoMapper;
using ReProServices.Application.Common.Mappings;
using System;
using System.Collections.Generic;

namespace ReProServices.Application.Customers
{
    public class CustomerDto : IMapFrom<Domain.Entities.Customer>
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string PAN { get; set; }
        public string AddressPremises { get; set; }
        public string AdressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? StateId { get; set; }
        public bool IsTracesRegistered { get; set; }
        public string TracesPassword { get; set; }
        public bool? AllowForm16B { get; set; }
        public string ISD { get; set; }
        public string AlternateNumber { get; set; }
        public string Sellers { get; set; }
        public bool? IsPanVerified { get; set; }
        public bool? OnlyTDS { get; set; }
        public string IncomeTaxPassword { get; set; }
        public virtual ICollection<Domain.Entities.CustomerProperty> CustomerProperty { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Customer, CustomerDto>();
        }

    }
}
