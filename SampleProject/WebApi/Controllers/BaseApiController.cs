using System;
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

        public HttpResponseMessage ValidationFails(params string[] msg)
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

        protected HttpResponseMessage NotAuthorized(string msg = null)
        {
            return ControllerContext.Request.CreateResponse(HttpStatusCode.Unauthorized, msg ?? "Not authorized.");
        }

        protected void AssertParams(int? take, int maxTake)
        {
            if (take.HasValue && (take <= 0 || take > maxTake))
                throw new Exception($"Incorrect value of \"take\" parameter: {take}");
        }
        protected Guid GetCurrentUserId()
        {
            return new Guid("1422740e-6426-4c46-8445-3f5274a62424"); // test comments: for test purposes we just use a hard coded user id. In a real application we need to get the value based on the current authentication or admin logic.
        }
    }
}