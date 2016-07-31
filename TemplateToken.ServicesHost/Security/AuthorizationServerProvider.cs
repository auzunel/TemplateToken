using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TemplateToken.Business.UserManagement;

namespace TemplateToken.ServicesHost.Security
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        // OAuthAuthorizationServerProvider sınıfının client erişimine izin verebilmek için ilgili ValidateClientAuthentication metotunu override ediyoruz.
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        // OAuthAuthorizationServerProvider sınıfının kaynak erişimine izin verebilmek için ilgili GrantResourceOwnerCredentials metotunu override ediyoruz.
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // CORS ayarlarını set ediyoruz.
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            // Kullanıcının access_token alabilmesi için gerekli validation işlemlerini yapıyoruz.
            var identity = UserManagement.Login(context.UserName, context.Password);
            if (identity != null && identity.IsAuthenticated)
            {
                //var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                //identity.AddClaim(new Claim("sub", context.UserName));
                //identity.AddClaim(new Claim("role", "user"));

                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "Wrong username or password");
            }
        }
    }
}