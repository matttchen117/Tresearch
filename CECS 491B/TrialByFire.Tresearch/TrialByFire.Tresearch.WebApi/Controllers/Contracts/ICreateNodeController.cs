﻿using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface ICreateNodeController
    {
        public Task<IActionResult> CreateNodeAsync(IAccount account, INode node);
    }
}
