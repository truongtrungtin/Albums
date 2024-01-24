using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Responses;

namespace Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            
            catch (DbUpdateException ex)
            {
                ApiResponse res = new ApiResponse();
                res.isSuccess = false;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        res.message = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        res.message = ex.InnerException.Message;
                    }
                }
                else
                {
                    res.message = ex.Message;
                }
                res.code = (int)HttpStatusCode.BadRequest;
            }
        }
        //private Task HandleExceptionAsync(HttpContext context, Exception exception)
        //{

        //    context.Response.ContentType = "application/json";
        //    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //    return context.Response.WriteAsync(new ApiResponse()
        //    {
        //        Code = context.Response.StatusCode,
        //        Message = "Internal Server Error from the custom middleware."
        //    }.ToString());
        //}
    }
}
