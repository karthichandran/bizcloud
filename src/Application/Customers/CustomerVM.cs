using System.Collections.Generic;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.Customers
{
    public class CustomerVM
    {
        public ICollection<CustomerDto> customers { get; set; }
        //public ICollection<CustomerPropertyDto> customerProperty { get; set; } = new Collection<CustomerPropertyDto>();
        public ICollection<ViewCustomerPropertyBasic> customersView { get; set; }
    }
}
