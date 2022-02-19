using System;
using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.Logging;

namespace TrialByFire.Tresearch.Services
{
    public class ArchivingService
    {
        private readonly MSSQLDAO _mssqlDAO;
        private readonly LogService _logService;

        public ArchivingService()
        {
        }

        public ArchivingService(MSSQLDAO mssqlDAO, LogService logService)
        {
            _mssqlDAO = mssqlDAO;
            _logService = logService;
        }

        public bool Archive()
        {
            return _mssqlDAO.Archive();
        }

    }
}
