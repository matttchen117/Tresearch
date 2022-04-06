namespace TrialByFire.Tresearch.Exceptions
{
    public class CreateNodeFailedException : Exception
    {
        public CreateNodeFailedException()
        {
        }

        public CreateNodeFailedException(string message) : base(message)
        {
        }

        public CreateNodeFailedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
