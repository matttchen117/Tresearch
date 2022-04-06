using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class LogManager : ILogManager
    {
        private ILogService _logService;
        private BuildSettingsOptions _buildSettings;
        public LogManager(ILogService logService, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _logService = logService;
            _buildSettings = options.Value;
        }

        public async Task<string> StoreAnalyticLogAsync(DateTime timestamp, string level, string username, 
            string authorizationLevel, string category, string description, 
            CancellationToken cancellationToken = default)
        {
            ILog log = await _logService.CreateLogAsync(timestamp, level, username, authorizationLevel, 
                category, description, cancellationToken).ConfigureAwait(false);
            return await _logService.StoreLogAsync(log, _buildSettings.AnalyticTable, cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> StoreArchiveLogAsync(DateTime timestamp, string level, string username, 
            string authorizationLevel, string category, string description, 
            CancellationToken cancellationToken = default)
        {
            ILog log = await _logService.CreateLogAsync(timestamp, level, username, authorizationLevel,
                category, description, cancellationToken).ConfigureAwait(false);
            return await _logService.StoreLogAsync(log, _buildSettings.ArchiveTable, cancellationToken).ConfigureAwait(false);
        }
    }
}
