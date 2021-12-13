namespace Tresearch
{
    public class GetViewFailException : Exception
    {
        public GetViewFailException() { }

        public GetViewFailException(string message) : base(message) { }

        public GetViewFailException(string message, Exception inner) : base(message, inner) { }
    }
}