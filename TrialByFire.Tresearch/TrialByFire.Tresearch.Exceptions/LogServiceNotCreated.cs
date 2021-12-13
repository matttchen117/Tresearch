namespace TrialByFire.Tresearch.Exceptions
{
    public class LogServiceNotCreated : Exception
    {
        public LogServiceNotCreated() { }

        public LogServiceNotCreated(string message) : base(message) { }

        public LogServiceNotCreated(string message, Exception inner) : base(message, inner) { }
    }
}