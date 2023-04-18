using ReProServices.Application.Common.Interfaces;
using System;

namespace ReProServices.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
