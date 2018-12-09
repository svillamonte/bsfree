﻿using BSFree.Services.Interfaces;
using BSFree.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSFree.Server.Controllers
{
    [Route("api/[controller]")]
    public class ShoutsController : Controller
    {
        private readonly IShoutsClient _shoutsClient;

        public ShoutsController(IShoutsClient shoutsClient)
        {
            _shoutsClient = shoutsClient;
        }

        [HttpGet("[action]")]
        public async Task<ShoutsResponse> LatestShouts()
        {
            return await _shoutsClient.GetLatestShouts();
        }

        // Temp until I know why Blazor rejects receiving a ShoutsResponse
        [HttpGet("[action]")]
        public async Task<IEnumerable<Shout>> LatestShoutsCollection()
        {
            var shoutsResponse = await _shoutsClient.GetLatestShouts();
            return shoutsResponse.Shouts;
        }
    }
}
