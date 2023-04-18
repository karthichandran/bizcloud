using System;
using ReProServices.Domain.Common;

namespace ReProServices.Domain.Extensions
{
    public static class DateExtension
    {
        public static DatePart GenerateDatePart(this DateTime date)
        {
            var dateObj = new DatePart()
            {
                Day = date.Day,
                Month = date.ToString("MMM"),
                Year = date.Year
            };
            return dateObj;
        }
    }
}
