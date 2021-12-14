using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.Logging;
using TrialByFire.Tresearch.Services;

namespace TrialByFire.Tresearch.Managers
{
    public class ArchivingManager
    {
        private readonly MSSQLDAO _mssqlDAO;
        private readonly LogService _logService;

        public ArchivingManager()
        {
        }

        public ArchivingManager(MSSQLDAO mssqlDAO, LogService logService)
        {
            _mssqlDAO = mssqlDAO;
            _logService = logService;
        }

        public bool ArchiveLogs()
        {
            try
            {
                ArchivingService archivingService = new ArchivingService(_mssqlDAO, _logService);
                return archivingService.Archive();
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
