using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BedeSlots.Utilities.Middlewares
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate next;

        public NotFoundMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await next.Invoke(context);

            if (context.Response.StatusCode == 404)
            {
                context.Response.Redirect("Error/PageNotFound");
            }
        }
    }
}
