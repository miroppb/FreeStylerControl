using System.Security.Principal;

namespace FSControl
{
    public class AuthenticatedUser : IIdentity
    {
        public AuthenticatedUser(string authenticationType, bool isAuthenticated, string name)
        {
            AuthenticationType = authenticationType;
            IsAuthenticated = isAuthenticated;
            Name = name;
        }

        public string AuthenticationType { get; }

        public bool IsAuthenticated { get; }

        public string Name { get; }
    }

    public class FSControlUser
    {
        public string? username { get; set; }
        public string? password { get; set; }
    }
}
