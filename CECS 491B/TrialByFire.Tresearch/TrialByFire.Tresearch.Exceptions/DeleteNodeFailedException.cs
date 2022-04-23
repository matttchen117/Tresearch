namespace TrialByFire.Tresearch.Exceptions
{
    public class DeleteNodeFailedException : Exception
    {
        public DeleteNodeFailedException()
        {
        }

        public DeleteNodeFailedException(string message) : base(message)
        {
        }

        public DeleteNodeFailedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
