using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnBoardingIdentity.Infrastructure.Data;
using OnBoardingIdentity.Models;
using OnBoardingIdentity.Models.AccountBindings;


namespace OnBoardingIdentity.Controllers
{
	[RoutePrefix("api/users")]
	public class AccountsController : BaseApiController
    {

		//TODO: Add try catch (InternalServerError) for all methods

		//list all users endpoint
		[Authorize(Roles = "Gestor Projeto")]
		[Route()]
		[HttpGet]
		public IHttpActionResult GetUsers()
		{
			return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
		}

		//search by id endpoint
		[Authorize(Roles = "Gestor Projeto")]
		[Route("{id:guid}", Name = "GetUserById")]
		[HttpGet]
		public async Task<IHttpActionResult> GetUser(string Id)
		{
			var user = await this.AppUserManager.FindByIdAsync(Id);

			if (user != null)
			{
				return Ok(this.TheModelFactory.Create(user));
			}

			return NotFound();

		}

        //get all programmers
        [Authorize(Roles = "Gestor Projeto")]
        [Route("role/{role}", Name = "GetUserByRole")]
        [HttpGet]
        public IHttpActionResult GetUsersByRole(string role)
        {
            var users = this.AppUserManager.Users.ToList();
            if (users == null || users.Count == 0)
                return NotFound();


            var roleUserIdsQuery = from roleDb in AppRoleManager.Roles
                                   where roleDb.Name == role
                                   from user in roleDb.Users
                                   select user.UserId;

            var usersReturn = this.AppUserManager.Users.Where(u => roleUserIdsQuery.Contains(u.Id)).ToList().Select(u => this.TheModelFactory.Create(u));

            return Ok(usersReturn);
        }

        //Search by username endpoint
        [Authorize(Roles = "Gestor Projeto")]
		[Route("{username}")]
		[HttpGet]
		public async Task<IHttpActionResult> GetUserByName(string username)
		{
			var user = await this.AppUserManager.FindByNameAsync(username);

			if (user != null)
			{
				return Ok(this.TheModelFactory.Create(user));
			}

			return NotFound();
		}

		[AllowAnonymous] //Anyone can access this resource
		[Route("create")]
		[HttpPost]
		public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = new ApplicationUser()
			{
				UserName = createUserModel.Username,
				Email = createUserModel.Email,
				FirstName = createUserModel.FirstName,
				LastName = createUserModel.LastName,
				Level = 3,
				JoinDate = DateTime.Now.Date,
				//we could setup an email verification, for now i will leave it here
				EmailConfirmed = true
			};

			IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

			if (addUserResult.Succeeded)
			{
				var result = AppUserManager.AddToRole(user.Id, createUserModel.RoleName);

                if (result.Succeeded)
                {
					Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

					return Created(locationHeader, TheModelFactory.Create(user));
				}

			}
			return GetErrorResult(addUserResult);
		}


		[Authorize]
		[Route("ChangePassword")]
		public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			IdentityResult result = await this.AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
			
			if (!result.Succeeded)
			{
				return GetErrorResult(result);
			}

			return Ok();
		}

		
		[Authorize]
		[Route("{id:guid}")]
		public async Task<IHttpActionResult> DeleteUser(string id)
		{

			//Only SuperAdmin or Admin can delete users (Later when implement roles)

			var appUser = await this.AppUserManager.FindByIdAsync(id);

			if (appUser != null)
			{
				IdentityResult result = await this.AppUserManager.DeleteAsync(appUser);

				if (!result.Succeeded)
				{
					return GetErrorResult(result);
				}

				return Ok();

			}

			return NotFound();
		}
	}
}
