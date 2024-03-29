﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.DAL.Contracts
{
    public interface ILogService
    {
        public Task<ILog> CreateLogAsync(DateTime timestamp, string level, string category, 
            string description, CancellationToken cancellationToken = default);
        public Task<string> StoreLogAsync(ILog log, string destination, 
            CancellationToken cancellationToken = default);
    }
}
