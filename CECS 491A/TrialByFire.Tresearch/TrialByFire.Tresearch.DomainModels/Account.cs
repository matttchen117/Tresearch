namespace TrialByFire.Tresearch.DomainModels
{
    public class Account
    {
        public string Email { get; set; }

        public string Username { get; set; }

        public string Passphrase { get; set; }

        public string AuthorizationLevel { get; set; }

        public string Status { get; set; }


        public Account()
        {
        }

        public Account(string email, string passphrase, string authorizationLevel)
        {
            Email = email;
            Username = email;
            Passphrase = passphrase;
            AuthorizationLevel = authorizationLevel;
            Status = "Enabled";
        }

        public Account(string email, string username, string passphrase, string authorizationLevel)
        {
            Email = email;
            Username = username;
            Passphrase = passphrase;
            AuthorizationLevel = authorizationLevel;
            Status = "Enabled";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Account account = (Account)obj;
                return (Email.Equals(account.Email)) && (Username.Equals(account.Username)) && (Passphrase.Equals(account.Passphrase))
                    && (AuthorizationLevel.Equals(account.AuthorizationLevel)) && (Status.Equals(account.Status));
            }
        }

        public override string ToString()
        {
            return "Email: " + Email + ", Username: " + Username + ", Passphrase: " + Passphrase + ", AuthorizationLevel: " + AuthorizationLevel + ", Status: " + Status;
        }
    }
}