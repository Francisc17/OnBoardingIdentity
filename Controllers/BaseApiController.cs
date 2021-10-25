using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OnBoardingIdentity.Infrastructure;
using OnBoardingIdentity.Infrastructure.Managers;
using OnBoardingIdentity.Models;

namespace OnBoardingIdentity.Controllers
{
    //SOLUTIONS TO CORS PROBLEM:
    //https://stackoverflow.com/questions/18619656/enable-cors-in-web-api-2
    //Mosharaf Hossain answer was first used but currently
    //we are not using that.
    //can be good comment the next line in the startup.cs
    //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BaseApiController : ApiController
    {
        private ModelFactory _modelFactory;
        private ApplicationUserManager _AppUserManager = null;
        private ApplicationRoleManager _AppRoleManager = null;
        private ApplicationProjectManager _AppProjectManager = null;
        private ApplicationTaskManager _AppTaskManager = null;

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        protected ApplicationProjectManager AppProjectManager 
        {
            get
            {
                if (_AppProjectManager == null)
                {
                    _AppProjectManager = new ApplicationProjectManager(new ApplicationDbContext());
                }

                return _AppProjectManager;    
            }
        }

        protected ApplicationTaskManager AppTaskManager
        {
            get
            {
                if (_AppTaskManager == null)
                {
                    _AppTaskManager = new ApplicationTaskManager(new ApplicationDbContext());
                }

                return _AppTaskManager;
            }
        }

        public BaseApiController()
        {
        }

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                }
                return _modelFactory;
            }
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
