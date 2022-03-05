namespace TrialByFire.Tresearch.Exceptions
{
    public class OTPClaimCreationFailedException : Exception
    {
        public OTPClaimCreationFailedException()
        {
        }

        public OTPClaimCreationFailedException(string message)
            : base(message)
        {
        }

        public OTPClaimCreationFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}