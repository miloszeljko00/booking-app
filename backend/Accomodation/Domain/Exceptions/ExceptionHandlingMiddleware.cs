using Domain.Exceptions.CustomExceptions;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Domain.Exceptions
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
            var errorMessageObject = new Error { Message = "Unknown error", Title = "Accommodation application" };
            var statusCode = (int)HttpStatusCode.InternalServerError;
            switch (ex)
            {
                case InvalidDateRangeException:
                    errorMessageObject.Message = "Da l' sam dos'o il' sam pos'o";
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case InvalidAddressException:
                    errorMessageObject.Message = "Nevalidna adresa, proverite jos jednom unete podatke";
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case InvalidPriceException:
                    errorMessageObject.Message = "Cena mora biti pozitivan broj";
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case InvalidAccommodationException:
                    errorMessageObject.Message = "Smestaj nije validan, proverite naziv.";
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case InvalidPictureException:
                    errorMessageObject.Message = "Nevalidan naziv slike i/ili opis, proverite ekstenziju naziva slike kao i duzinu opisa";
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case InvalidEmailException:
                    errorMessageObject.Message = "Nevalidan format email adrese.";
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case NumberIsLessOrEqualToZeroException:
                    errorMessageObject.Message = ex.Message;
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case DuplicateAccommodationException:
                    errorMessageObject.Message = "Ovaj smestaj vec postoji u nasem sistemu...";
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
