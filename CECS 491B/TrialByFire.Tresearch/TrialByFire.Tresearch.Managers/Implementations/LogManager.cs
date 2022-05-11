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
    /// <summary>
    ///     LogManager: Class that is part of the Manager abstraction layer that handles business rules related to Logging
    /// </summary>
    public class LogManager : ILogManager
    {
        private ILogService _logService;
        private IMessageBank _messageBank;
        private BuildSettingsOptions _buildSettings;

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(15));
        /// <summary>
        ///     public LogManager():
        ///         Constructor for LogManager class
        /// </summary>
        /// <param name="logService">Service object for Service abstraction layer to perform services related to Logging</param>
        /// <param name="messageBank">Object that contains error and success messages</param>
        /// <param name="options">Snapshot object that represents the setings/configurations of the application</param>
        public LogManager(ILogService logService, IMessageBank messageBank, IOptionsSnapshot<BuildSettingsOptions> options)
        {
            _logService = logService;
            _messageBank = messageBank;
            _buildSettings = options.Value;
        }

        /// <summary>
        ///     StoreAnalyticLogAsync:
        ///         Async method that creates a Log and sends it to the Service layer to be stored in the Analytic table in the database
        /// </summary>
        /// <param name="timestamp">The time of the Log</param>
        /// <param name="level">The level of the Log, valid values can be found in the enum of the interface</param>
        /// <param name="category">The category of the Log, valid values can be found in the enum of the interface</param>
        /// <param name="description">The description of the Log</param>
        /// <returns>Result for now</returns>
        public async Task<string> StoreAnalyticLogAsync(DateTime timestamp, Levels level, Categories category,
            string description)
        {
            ILog log = await _logService.CreateLogAsync(timestamp, level.ToString(), category.ToString(),
                description, _cancellationTokenSource.Token).ConfigureAwait(false);
            string result = await _logService.StoreLogAsync(log, _buildSettings.AnalyticTable,
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

        /// <summary>
        ///     StoreArchvieLogAsync:
        ///         Async method that creates a Log and sends it to the Service layer to be stored in the Archive table in the database
        /// </summary>
        /// <param name="timestamp">The time of the Log</param>
        /// <param name="level">The level of the Log, valid values can be found in the enum of the interface</param>
        /// <param name="category">The category of the Log, valid values can be found in the enum of the interface</param>
        /// <param name="description">The description of the Log</param>
        /// <returns>Result for now</returns>
        public async Task<string> StoreArchiveLogAsync(DateTime timestamp, Levels level, Categories category,
            string description)
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
