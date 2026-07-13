using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        public HttpResponseMessage Found(object obj)
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, obj);
        }

        public HttpResponseMessage Found()
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage DoesNotExist()
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.NotFound);
        }

        public HttpResponseMessage AlreadyExists(string msg)
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.Conflict, msg ?? "The resource already exists.");
        }

        public HttpResponseMessage ValidationFails(string[] msg)
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.BadRequest, msg ?? new[] { "Validation failed" });
        }

        protected (bool, string[]) ValidateModel<T>(T model) where T : class
        {
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(model, context, results, true);
            if (!isValid)
            {
                return (false, results.Select(s => s.ErrorMessage).ToArray());
            }

            return (true, null);
        }

    }
}