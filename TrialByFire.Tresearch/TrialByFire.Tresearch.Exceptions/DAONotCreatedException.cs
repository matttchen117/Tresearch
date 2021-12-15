namespace TrialByFire.Tresearch.Exceptions
{
    public class DAONotCreatedException : Exception
    {
        public DAONotCreatedException() { }

        public DAONotCreatedException(string message) : base(message) { }

        public DAONotCreatedException(string message, Exception inner) : base(message, inner) { }

    }
}
