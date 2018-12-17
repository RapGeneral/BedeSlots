using BedeSlots.Utilities.Middlewares;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedeSlots.Utilities.Extentions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseNotFoundExceptionHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<NotFoundMiddleware>();
        }
    }
}
