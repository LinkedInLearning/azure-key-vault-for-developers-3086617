using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace FilePortal.Web.Auth
{
   
    public class MultiSchemaAuthorizationRequirement : AuthorizationHandler<MultiSchemaAuthorizationRequirement>, IAuthorizationRequirement
    {
        string[] _authSchemasRequired;
        public MultiSchemaAuthorizationRequirement(params string[] authSchemasRequired)
        {
            _authSchemasRequired=authSchemasRequired;
        }
        /// <summary>
        /// Makes a decision if authorization is allowed based on a specific requirement.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MultiSchemaAuthorizationRequirement requirement)
        {
            var user = context.User;
            var userIsAnonymous =
                user?.Identity == null ||
                !user.Identities.Any(i => i.IsAuthenticated);
            
            if (!userIsAnonymous)
            {
                foreach(var schema in _authSchemasRequired)
                {
                    var identity = user.Identities.FirstOrDefault(z => z.AuthenticationType == schema);
                    if (identity == null || !identity.IsAuthenticated) return;
                   
                }
                
                
                context.Succeed(requirement);
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{nameof(MultiSchemaAuthorizationRequirement)}: Requires an all schemas have an identity and authenticated.";
        }
    }
}
