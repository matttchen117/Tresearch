namespace TrialByFire.Tresearch.Exceptions
{
    public class RolePrincipalCreationFailedException : Exception
    {
        public RolePrincipalCreationFailedException()
        {
        }

        public RolePrincipalCreationFailedException(string message) : base(message)
        {
        }

        public RolePrincipalCreationFailedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}