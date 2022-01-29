using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DomainModels;

namespace TrialByFire.Tresearch.DAL
{
    public interface ISqlDAO
    {
        Account GetAccount(Account account);
        string CreateAccount(List<Account> accounts);
        string UpdateAccount(List<Account> accounts);
        string DeleteAccount(List<Account> accounts);
        string DisableAccount(List<Account> accounts);
        string EnableAccount(List<Account> accounts);
        string StoreLog(Log log);
        string ArchiveLogs(DateTime now);
        string CreateUpdateQuery(List<Account> accounts);
        string StoreOTP(Account user, string otp);
        List<Log> GetLogsOlderThan30Days(DateTime now);
        string CompressLogs(List<Log> _logs);
        string DeleteLogs(DateTime now);
        
    }
}
