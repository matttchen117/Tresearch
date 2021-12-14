namespace TrialByFire.Tresearch.Exceptions
{
    public class AccountManagerCreationFailedException : Exception
    {
        public AccountManagerCreationFailedException() { }

        public AccountManagerCreationFailedException(string message) : base(message) { }

        public AccountManagerCreationFailedException(string message, Exception inner) : base(message, inner) { }
    }
}
