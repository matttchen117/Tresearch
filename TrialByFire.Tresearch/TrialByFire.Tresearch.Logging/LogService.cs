using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.DomainModels;

namespace TrialByFire.Tresearch.Logging
{
    public class LogService
    {
        private readonly SqlDAO _sqlDAO;

        public LogService()
        {
            Log log;
        }

        public LogService(SqlDAO SqlDAO)
        {
            _sqlDAO = SqlDAO;
        }

        public string CreateLog(string timeStamp, string level, string username, string category, string description)
        {
            string result = "CreateLog failed";
            try
            {
                Log log = new Log(timeStamp, level, username, category, description);
                result = _sqlDAO.StoreLog(log);
                return result;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = ex.Message;
                return result;
            }
        }
    }
}