using AutoMapper;
using Inventory.BusinessLogic;
using Transcore.TagInventory.Web.Common;
using Transcore.TagInventory.Web.Models;
using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Transcore.TagInventory.Common.Exceptions;
using System.Data.SqlClient;
using DTO = Transcore.TagInventory.Entity;
using System.Threading;


namespace Transcore.TagInventory.Web.Controller
{
    [RoutePrefix("api/received-box")]
    public class ReceivedBoxController : ApiController
    {
        protected readonly IReceivedBoxProvider _provider;
        protected readonly IHttpRequestHandler _requestHandler;
        protected readonly ILog _logger;
        protected readonly IMapper _mapper;

        public ReceivedBoxController(IReceivedBoxProvider provider,
            IHttpRequestHandler requestHandler,
            ILog logger,
            IMapper mapper)
        {
            this._provider = provider;
            this._requestHandler = requestHandler;
            this._mapper = mapper;
            this._logger = logger;
        }

        [HttpPost]
        [Route("import")]
        public IHttpActionResult Import()
        {
            if (HttpContext.Current.Request.Files.Count == 0)
                return BadRequest("No files are posted");                   

            try
            {
                long shipmentID = -1;

                short boxType = -1;

                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();

                var filesDict = _requestHandler.RetrievePostedData(out shipmentID, out boxType);

                var response = _provider.Import(filesDict, shipmentID, boxType);

                var webResponse = _mapper.Map<List<ReceivedBox>>(response.Data);

                stopwatch.Stop();

                long ms = stopwatch.ElapsedMilliseconds;

                //HttpRequestHandler.SaveFiles(filesDict);

                return Ok(webResponse);
            }


            catch (SqlException ex)
            {
                _logger.Error(ex.Message, ex);

                return BadRequest(ex.Message);
            }

            catch(FileSpaceMissingException ex)
            {
                _logger.Error(ex.ErrorMessage, ex);

                return BadRequest(ex.ErrorMessage);
            }

            catch (Exception ex)
            {
                // log error
                _logger.Error(ex.Message, ex);

                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get received box by its id.
        /// </summary>
        /// <param name="receivedBoxID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]

        public IHttpActionResult Get(long id)
        {
            var page = _provider.GetReceivedBox(new DTO.Model.ReceivedBoxSearch() { ReceivedBoxID = id }, 1, 1);

            if (page.Data == null && page.Data.Count == 0)
            {
                return NotFound();
            }

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
        public IHttpActionResult GetList([FromUri] ReceivedBoxSearch searchOptions, [FromUri] int pageSize, [FromUri] int pageNumber)
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

        public IHttpActionResult Put([FromBody] ReceivedBoxUpdate boxUpdate)
        {

            try
            {
                _provider.UpdateStatus(_mapper.Map<DTO.Model.ReceivedBoxUpdate>(boxUpdate));

                return Ok();
            }

            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                //log
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("update-scan-tags")]
        public IHttpActionResult UpdateScanBoxTags(ScannedReceivedBoxUpdate boxTags)
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
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                //log
                return InternalServerError();
            }
        }


    }
}

