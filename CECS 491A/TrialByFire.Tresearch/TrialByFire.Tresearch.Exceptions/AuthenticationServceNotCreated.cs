namespace TrialByFire.Tresearch.Exceptions
{
    public class AuthenticationServiceNotCreatedException : Exception
    {
        public AuthenticationServiceNotCreatedException() { }

        public AuthenticationServiceNotCreatedException(string message) : base(message) { }

        public AuthenticationServiceNotCreatedException(string message, Exception inner) : base(message, inner) { }
    }
}