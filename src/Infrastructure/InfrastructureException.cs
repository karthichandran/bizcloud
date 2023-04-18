using System;

namespace ReProServices.Infrastructure
{

    public class InfrastructureException : Exception
    {
        internal InfrastructureException(string businessMessage) : base(businessMessage) { }
    }
}