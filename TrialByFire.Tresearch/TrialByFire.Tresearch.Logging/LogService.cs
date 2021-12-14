using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.DomainModels;

namespace TrialByFire.Tresearch.Logging
{
    public class LogService
    {

        private readonly MSSQLDAO _mssqlDAO;

        public LogService()
        {
        }

        public LogService(MSSQLDAO mssqlDAO)
        {
            _mssqlDAO = mssqlDAO;
        }

        public bool CreateLog(DateTime timeStamp, string level, string username, string category, string description)
        {
            bool isSuccessful = false;
            try
            {
                Log log = new Log(timeStamp, level, username, category, description);
                isSuccessful = _mssqlDAO.StoreLog(log);
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return isSuccessful;
        }
    }
}