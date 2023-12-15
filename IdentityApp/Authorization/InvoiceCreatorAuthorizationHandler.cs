using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using IdentityApp.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace IdentityApp.Authorization
{
    public class InvoiceCreatorAuthorizationHandler : 
        AuthorizationHandler<OperationAuthorizationRequirement, Invoice>
    {   

        UserManager<IdentityUser> _userManager;
        //Gets current authenticated user by using Usermanager that is accesseble because of dependancy injection
        public InvoiceCreatorAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
                _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, 
            Invoice invoice)
        {
            if(context.User == null || invoice == null)
                return Task.CompletedTask;
            
            if(requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName &&
                requirement.Name != Constants.DeleteOperationName &&
                requirement.Name != Constants.UpdateOperationName)
            {
                return Task.CompletedTask;
            }

            if (invoice.CreatorId == _userManager.GetUserId(context.User))
                context.Succeed(requirement);
            
            return Task.CompletedTask;
        }
    }
}
