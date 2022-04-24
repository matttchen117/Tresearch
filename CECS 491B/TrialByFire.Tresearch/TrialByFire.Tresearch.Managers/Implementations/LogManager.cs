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
using static TrialByFire.Tresearch.Managers.Contracts.ILogManager;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class LogManager : ILogManager
    {
        private ILogService _logService;
        private IMessageBank _messageBank;
        private BuildSettingsOptions _buildSettings;

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(15));
        public LogManager(ILogService logService, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _logService = logService;
            _messageBank = messageBank;
            _buildSettings = options.Value;
        }

        public async Task<string> StoreAnalyticLogAsync(DateTime timestamp, Levels level, Categories category, 
            string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            ILog log = await _logService.CreateLogAsync(timestamp, level.ToString(), category.ToString(), 
                description, _cancellationTokenSource.Token).ConfigureAwait(false);
            string result = await _logService.StoreLogAsync(log, _buildSettings.AnalyticTable, 
                _cancellationTokenSource.Token).ConfigureAwait(false);
            if(result.Equals(_messageBank.GetMessage(IMessageBank.Responses.logSuccess)))
            {
                return result;
            }
            else
            {
                log = await _logService.CreateLogAsync(DateTime.UtcNow, Levels.Error.ToString(), 
                    Categories.Business.ToString(), await _messageBank.GetMessage(IMessageBank.Responses.logTimeExceeded)
                    .ConfigureAwait(false) + log.ToString(), _cancellationTokenSource.Token).ConfigureAwait(false);
                return await _logService.StoreLogAsync(log, _buildSettings.ArchiveTable,
                _cancellationTokenSource.Token).ConfigureAwait(false);
            }
        }

        public async Task<string> StoreArchiveLogAsync(DateTime timestamp, Levels level, Categories category,
            string description, CancellationToken cancellationToken = default)
        {
            ILog log = await _logService.CreateLogAsync(timestamp, level.ToString(), category.ToString(),
                description, _cancellationTokenSource.Token).ConfigureAwait(false);
            string result = await _logService.StoreLogAsync(log, _buildSettings.ArchiveTable,
                _cancellationTokenSource.Token).ConfigureAwait(false);
            if (result.Equals(_messageBank.GetMessage(IMessageBank.Responses.logSuccess)))
            {
                return result;
            }
            else
            {
                log = await _logService.CreateLogAsync(DateTime.UtcNow, Levels.Error.ToString(),
                    Categories.Business.ToString(), await _messageBank.GetMessage(IMessageBank.Responses.logTimeExceeded)
                    .ConfigureAwait(false) + log.ToString(), _cancellationTokenSource.Token).ConfigureAwait(false);
                return await _logService.StoreLogAsync(log, _buildSettings.ArchiveTable,
                _cancellationTokenSource.Token).ConfigureAwait(false);
            }
        }
    }
}
