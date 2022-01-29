using System;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.Logging;

namespace TrialByFire.Tresearch.Services
{
    public class ArchivingService
    {
        private readonly ISqlDAO _sqlDAO;
        private readonly LogService _logService;

        public ArchivingService()
        {
        }

        public ArchivingService(ISqlDAO sqlDAO, LogService logService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
        }

        public async string ArchiveLogs()
        {
            try
            {
                //wait til the first day of the month 
                int timeout = 1000;
                var task = _sqlDAO.ArchiveLogs();
                if(await Task.WhenAny(task, Task.Delay(timeout)) == task)
                {
                    return "success";
                }
                else
                {
                    return "ArchiveLogs timeout";
                }
            } 
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> ArchiveTime()
        {
            Task<bool> timeTask 
        }
    }
}
