namespace TrialByFire.Tresearch.Exceptions
{
    public class AccountCreationFailedException : Exception
    {
        public AccountCreationFailedException()
        {
        }

        public AccountCreationFailedException(string message) : base(message)
        {
        }

        public AccountCreationFailedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}