namespace HotelChief.API.Attributes
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Net;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class IdentityServerOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!IsRequestFromLocalhost(context.HttpContext))
            {
                context.Result = new StatusCodeResult(403); // Forbidden
            }
        }

        private bool IsRequestFromLocalhost(HttpContext context)
        {
            var remoteIpAddress = context.Connection.RemoteIpAddress;
            return IPAddress.IsLoopback(remoteIpAddress) || IPAddress.Equals(remoteIpAddress, IPAddress.Parse("::1"));
        }
    }
}
