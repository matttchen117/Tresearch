using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.Logging;
using TrialByFire.Tresearch.Services;

namespace TrialByFire.Tresearch.Managers
{
    public class ArchivingManager
    {
        private readonly ISqlDAO _sqlDAO;
        private readonly LogService _logService;
        ArchivingService archivingService;

        public ArchivingManager()
        {
        }

        public ArchivingManager(ISqlDAO sqlDAO, LogService logService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            archivingService = new ArchivingService(_sqlDAO, logService);
        }

        public async Task<string> ArchiveLogs()
        {
            Task<bool> timeTask = CheckForAppropriateTime();
            bool isTime = await timeTask;
            try
            {
                return archivingService.ArchiveLogs();
            }
            catch (Exception ex)
            {
                return "Archive Logs failure";
            }
        }

        /*public async Task<string> ArchiveLogs(string sArchiveTime)
        {
            Task<bool> timeTask = CheckForAppropriateTime(sArchiveTime);
            bool isTime = await timeTask;
            try
            {
                ArchivingService archivingService = new ArchivingService(_sqlDAO, _logService);
                return archivingService.ArchiveLogs();
            }
            catch (Exception ex)
            {
                return false;
            }
        }*/

        public async Task<bool> CheckForAppropriateTime()
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime target = now.Date.AddDays(DateTime.DaysInMonth(now.Year, now.Month) + 1 - now.Day);
                if (now == target)
                {
                    return true;
                }
                else
                {
                    while (now <= target)
                    {
                        now = DateTime.Now;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
            
            /*Timer timer;
            DateTime now = DateTime.Now;
            int day = now.Day;
            TimeSpan currentTime = now.TimeOfDay;
            if (day == 1 && TimeSpan.Compare(currentTime, TimeSpan.Zero) == 0)
            {
                return true;
            }
            else
            {
                int currentMonth = now.Month;
                DateTime nextArchivingTime = new DateTime(now.Year, currentMonth + 1, 1);
                TimeSpan timeTillArchive = nextArchivingTime - now;
                timer = new Timer(timeTillArchive.TotalMilliseconds);
                bool isTime = false;
                timer.Elapsed += async (sender, e) => await CheckForAppropriateTime();
                timer.Start();
                return isTime;
            }*/
        }

        public async Task<bool> CheckForAppropriateTime(string sArchiveTime)
        {
            DateTime now = DateTime.Now;
            DateTime target = DateTime.ParseExact(sArchiveTime, "yyyy-MM-dd HH:mm:ss", null);
            if (now == target)
            {
                return true;
            }
            else
            {
                while (now <= target)
                {
                    now = DateTime.Now;
                }
                return true;
            }
            /*Timer timer;
            DateTime now = DateTime.Now;
            int day = now.Day;
            TimeSpan currentTime = now.TimeOfDay;
            if (day == 1 && TimeSpan.Compare(currentTime, TimeSpan.Zero) == 0)
            {
                return true;
            }
            else
            {
                int currentMonth = now.Month;
                DateTime target = DateTime.ParseExact(sArchiveTime, "yyyy-MM-dd HH:mm:ss", null);
                TimeSpan timeTillArchive = target - now;
                timer = new Timer(timeTillArchive.TotalMilliseconds);
                bool isTime = false;
                timer.Elapsed += async (sender, e) => await CheckForAppropriateTime();
                timer.Start();
                return isTime;
            }*/
        }


    }
}
