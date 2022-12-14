using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Transcore.TagInventory.BusinessLogic;

namespace Transcore.TagInventory.Web.Controllers
{
    public class SettingsController : ApiController
    {
        private readonly ISettingsProvider _settings;
        public SettingsController(ISettingsProvider settings)
        {
            this._settings = settings;
        }

        [HttpPost]
        public IHttpActionResult SetAttenuation(int value)
        {
            _settings.SetAttenuation(value);

            return Ok();
        }

        public IHttpActionResult RestartReader()
        {
            return null;
        }
    }
}
