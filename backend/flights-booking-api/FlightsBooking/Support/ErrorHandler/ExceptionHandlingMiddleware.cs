using FlightsBooking.Support.ErrorHandler.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FlightsBooking.Support.ErrorHandler
{
    public class ExceptionHandlingMiddleware
    {
        public RequestDelegate requestDelegate;
        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }
        private static Task HandleException(HttpContext context, Exception ex)
        {
            var errorMessageObject = new Error { Message = "Unknown error", Title = "Flight application" };
            var statusCode = (int)HttpStatusCode.InternalServerError;
            switch (ex)
            {
                case InvalidArrivalTimeException:
                    errorMessageObject.Message = "Arrival time can not be in the past.";
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case InvalidDepartureTimeException:
                    errorMessageObject.Message = "Departure time can not be in the past.";
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case InvalidPriceException:
                    errorMessageObject.Message = "Price can not be negative.";
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case InvalidPurchasedDateException:
                    errorMessageObject.Message = "Purchased date can not be in the future.";
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case InvalidTotalTicketsCountException:
                    errorMessageObject.Message = "Tickets count can not be negative.";
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case DepartureIsAfterArrivalException:
                    errorMessageObject.Message = "You think you can arrive before you departure?";
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                default:
                    errorMessageObject.Message = ex.Message;
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var errorMessage = JsonConvert.SerializeObject(errorMessageObject);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(errorMessage);
        }
    }
}
