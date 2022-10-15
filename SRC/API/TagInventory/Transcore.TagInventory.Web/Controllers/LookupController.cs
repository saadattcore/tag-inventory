using AutoMapper;
using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Transcore.TagInventory.BusinessLogic.LookUp;
using Transcore.TagInventory.Common.Caching;
using Transcore.TagInventory.Common.Enums;
using Transcore.TagInventory.Web.Models;

namespace InventoryManagement.Controllers
{
    [RoutePrefix("api/lookup")]
    public class LookupController : ApiController
    {
        private readonly ILookupProvider _provider;
        private readonly ICacheManager _cache;
        private readonly ILog _logger;
        private readonly IMapper _mapper;

        public LookupController(ICacheManager cache, ILookupProvider provider, ILog logger,IMapper mapper)
        {
            _provider = provider;
            _cache = cache;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetLookup(string key)
        {
            var allLookups = Enum.GetNames(typeof(Lookup)).ToList();

            var doKeyExist = allLookups.Exists(l => l.ToLower() == key);

            if (!doKeyExist)
                return BadRequest("In valid lookup type");

            if (key.ToLower() == "all")
            {
                Dictionary<string, List<KeyValuePair<short, string>>> response = new Dictionary<string, List<KeyValuePair<short, string>>>();

                foreach (var lookUpKey in allLookups)
                {
                    if (lookUpKey.ToLower() == "all")
                        continue;

                    var result = _cache.GetValue(lookUpKey, _provider.GetLookup);
                    response.Add(lookUpKey.ToLower(), result);
                }
                return Ok(response);
            }
            else
            {
                var result = _cache.GetValue(key, _provider.GetLookup);
                return Ok(result);
            }

        }

        [HttpGet]
        [Route("get-distributors")]
        public IHttpActionResult GetDistributors()
        {
            try
            {
                var result = _provider.GetDistributors();

                if ((result == null || result.Distributors == null || result.DistributorTypes == null)  || (result.Distributors.Count == 0 || result.DistributorTypes.Count == 0)) 
                { 
                    return NotFound();
                }

                return Ok(_mapper.Map<DistributorAndTypes>(result));

            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return InternalServerError();

            }
        }
    }
}
