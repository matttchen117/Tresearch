namespace TrialByFire.Tresearch.Exceptions
{
    public class SQLConnectionFailedException : Exception
    {
        public SQLConnectionFailedException() { }

        public SQLConnectionFailedException(string message) : base(message) { }

        public SQLConnectionFailedException(string message, Exception inner) : base(message, inner) { }
    }
}