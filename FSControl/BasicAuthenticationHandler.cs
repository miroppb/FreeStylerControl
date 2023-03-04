using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Dapper;
using MySqlConnector;
using Microsoft.AspNetCore.Mvc;
using Isopoh.Cryptography.Argon2;
using miroppb;

namespace FSControl
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            )
    : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            bool connectionExists = true;
            try
            {
                using (MySqlConnection conn = secrets.GetConnectionString())
                {
                    conn.Open();
                    conn.Close();
                }
            }
            catch { connectionExists = false; }

            if (connectionExists)
            {
                Response.Headers.Add("WWW-Authenticate", "Basic");

                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    return Task.FromResult(AuthenticateResult.Fail("Authorization header missing."));
                }

                // Get authorization key
                var authorizationHeader = Request.Headers["Authorization"].ToString();
                var authHeaderRegex = new Regex(@"Basic (.*)");

                if (!authHeaderRegex.IsMatch(authorizationHeader))
                {
                    return Task.FromResult(AuthenticateResult.Fail("Authorization code not formatted properly."));
                }

                var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authorizationHeader, "$1")));
                var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
                var authUsername = authSplit[0];
                var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

                using (MySqlConnection conn = secrets.GetConnectionString())
                {
                    User? user = conn.Query<User>("SELECT * FROM users WHERE username = @user", new DynamicParameters(new { user = authUsername })).FirstOrDefault();
                    if (user == null)
                        return Task.FromResult(AuthenticateResult.Fail("The username or password is not correct."));

                    //verify password
                    if (!Argon2.Verify(user.hash!, authPassword))
                        return Task.FromResult(AuthenticateResult.Fail("The username or password is not correct."));

                    Program.frm?.Invoke(new System.Action(() =>
                    {
                        Program.frm.TxtOutput.Text = "User " + authUsername + " logged in" + Environment.NewLine;
                        libmiroppb.Log("User " + authUsername + " logged in");
                    }));
                }

                var authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, authUsername);
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));

                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
            }
            else
                return Task.FromResult<AuthenticateResult>(AuthenticateResult.Fail("Cannot connect to database"));
        }
    }
}
