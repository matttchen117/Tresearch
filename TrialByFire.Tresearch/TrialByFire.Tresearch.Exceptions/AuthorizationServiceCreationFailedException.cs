namespace TrialByFire.Tresearch.Exceptions
{
    public class AuthorizationServiceCreationFailedException : Exception
    {
        public AuthorizationServiceCreationFailedException() { }

        public AuthorizationServiceCreationFailedException(string message) : base(message) { }

        public AuthorizationServiceCreationFailedException(string message, Exception inner) : base(message, inner) { }
    }
}