using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Configuration;
using TrialByFire.Tresearch.Managers;
using TrialByFire.Tresearch.Logging;
using TrialByFire.Tresearch.DAL;

namespace TrialByFire.Tresearch.Controllers
{
    public class ArchivingController
    {
        ArchivingManager archivingManager;

        public ArchivingController()
        {
        }

        public ArchivingController(ISqlDAO sqlDAO, LogService logService)
        {
            archivingManager = new ArchivingManager(sqlDAO, logService);
        }

        public string ArchiveLogs()
        {
            try
            {
                string result = archivingManager.ArchiveLogs();
                return result;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
