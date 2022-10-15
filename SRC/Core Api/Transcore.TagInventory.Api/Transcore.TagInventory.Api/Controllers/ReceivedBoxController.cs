using AutoMapper;
using Inventory.BusinessLogic;
using Transcore.TagInventory.Web.Common;
using Transcore.TagInventory.Web.Models;
using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Transcore.TagInventory.Common.Exceptions;
using System.Data.SqlClient;
using DTO = Transcore.TagInventory.Entity;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using Serilog;

namespace Transcore.TagInventory.Web.Controller
{
    [Route("api/received-box")]
    [ApiController]
    public class ReceivedBoxController : ControllerBase
    {
        protected readonly IReceivedBoxProvider _provider;
        protected readonly IHttpRequestHandler _requestHandler;
        protected readonly ILog _logger;
        protected readonly IMapper _mapper;
        protected readonly IConfiguration _appSettings;

        public ReceivedBoxController(IReceivedBoxProvider provider,
            IHttpRequestHandler requestHandler,
            ILog logger,
            IMapper mapper,
            IConfiguration appSettings)

        {
            this._provider = provider;
            this._requestHandler = requestHandler;
            this._mapper = mapper;
            this._logger = logger;
            this._appSettings = appSettings;
        }

        [HttpPost]
        [Route("import")]
        public IActionResult Import()
        {


            if (Request.Form.Files.Count == 0)
                return BadRequest("No files are posted");

            try
            {

                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();

                long shipmentID = long.Parse(Request.Form["shipmentID"]);

                byte boxTye = byte.Parse(Request.Form["shipmentType"]);

                string rootFilesUploadFolder = _appSettings.GetValue<string>("ImportFolder");

                string shipmentFolder = Path.Combine(rootFilesUploadFolder, shipmentID.ToString(), $"{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}");

                _requestHandler.SavePostedShipmentFiles(Request, rootFilesUploadFolder, shipmentFolder);

                var response = _provider.ImportShipmentFileToDB(shipmentFolder, shipmentID, boxTye);

                var webResponse = _mapper.Map<List<ReceivedBox>>(response.Data);

                stopwatch.Stop();

                long ms = stopwatch.ElapsedMilliseconds;

                return Ok(webResponse);
            }


            catch (SqlException ex)
            {

                Log.Error(ex, ex.Message);

                return BadRequest(ex.Message);
            }

            catch (FileSpaceMissingException ex)
            {
                Log.Error(ex, ex.Message);

                return BadRequest(ex.ErrorMessage);
            }

            catch (FileAlreadyExist ex)
            {
                Log.Error(ex, ex.Message);

                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                // log error
                Log.Error(ex, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get received box by its id.
        /// </summary>
        /// <param name="receivedBoxID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]

        public IActionResult Get(long id)
        {
            var page = _provider.GetReceivedBox(new DTO.Model.ReceivedBoxSearch() { ReceivedBoxID = id }, 1, 1);

            if (page.Data == null && page.Data.Count == 0) return NotFound();

            var webResponse = _mapper.Map<List<ReceivedBox>>(page.Data);

            if (webResponse.Count == 0)
                return NotFound();

            return Ok(webResponse[0]);

        }

        /// <summary>
        /// Get list of shipment
        /// </summary>
        /// <param name="shipmentID">IF value is -1 then all boxes will be retrieved , if not -1 then received boxes with associated shipment will be retrieved</param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public IActionResult GetList([FromQuery] ReceivedBoxSearch searchOptions, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            var page = _provider.GetReceivedBox(_mapper.Map<ReceivedBoxSearch, DTO.Model.ReceivedBoxSearch>(searchOptions), pageSize, pageNumber);

            if (page.Data == null || page.Data.Count == 0)
            {
                return NotFound();
            }

            var webResponse = _mapper.Map<List<ReceivedBox>>(page.Data);

            return Ok(new Page<ReceivedBox>() { Data = webResponse, SearchCount = page.SearchCount, TotalCount = page.TotalCount });
        }

        [HttpPut]
        [Route("")]
        public IActionResult Put([FromBody] ReceivedBoxUpdate boxUpdate)
        {

            try
            {
                _provider.UpdateStatus(_mapper.Map<DTO.Model.ReceivedBoxUpdate>(boxUpdate));

                return Ok();
            }

            catch (SqlException ex)
            {
                _logger.Error(ex);

                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                _logger.Error(ex);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("update-scan-tags")]
        public IActionResult UpdateScanBoxTags(ScannedReceivedBoxUpdate boxTags)
        {
            var dataTransfer = _mapper.Map<DTO.Model.ScannedReceivedBoxUpdate>(boxTags);

            try
            {
                var result = _provider.UpdateScannedBox(dataTransfer);

                var response = _mapper.Map<ReceivedBox>(result);

                return Ok(response);
            }

            catch (SqlException ex)
            {
                _logger.Error(ex);

                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                _logger.Error(ex);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}

