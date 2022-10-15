using AutoMapper;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Transcore.TagInventory.BusinessLogic;
using Transcore.TagInventory.Web.Common;
using Transcore.TagInventory.Web.Models;
using DTO = Transcore.TagInventory.Entity;

namespace InventoryManagement.Controllers
{

    [Route("api/issued-box")]
    [ApiController]
    public class IssuedBoxController : ControllerBase
    {
        private readonly IIssuedBoxProvider _provider;
        private readonly IHttpRequestHandler _requestHandler;
        private readonly IMapper _mapper;
        private readonly ILog _logger;

        public IssuedBoxController(IIssuedBoxProvider provider, IHttpRequestHandler requestHandler, IMapper mapper, ILog logger)
        {
            _provider = provider;
            _requestHandler = requestHandler;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [Route("")]
        public IActionResult Post([FromBody] IssuedBox issuedBox)
        {

            if (issuedBox == null)
                return BadRequest("Issued box cannot be null");

            try
            {
                var issuedBoxDTO = _mapper.Map<DTO.Core.IssuedBox>(issuedBox);

                long id = _provider.Add(issuedBoxDTO);

                return Ok(id);

               // return CreatedAtAction(nameof(Get), new { id = id }, null);

            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("update-box")]
        public IActionResult Put([FromBody] IssuedBox issuedBox)
        {
            try
            {
                if (issuedBox == null) return BadRequest("Issued box cannot be null");

                var issuedBoxDTO = _mapper.Map<DTO.Core.IssuedBox>(issuedBox);

                _provider.Update(issuedBoxDTO);

                return Ok();

            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("")]
        public IActionResult Put([FromBody] IssuedBox issuedBox, bool updateIssuedBoxKits)
        {
            if (issuedBox == null)
                return BadRequest("Issued box cannot be null");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var issuedBoxDTO = _mapper.Map<DTO.Core.IssuedBox>(issuedBox);

                _provider.UpdateBoxAndTags(issuedBoxDTO, updateIssuedBoxKits);

                return Ok();

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(long id)
        {

            try
            {
                var page = _provider.GetIssuedBox(_mapper.Map<DTO.Model.IssuedBoxSearch>(new IssuedBoxSearch() { IssuedBoxID = id }), 1, 1);

                if (page == null || page.Data == null || page.Data.Count == 0)
                {
                    return NotFound();
                }

                var webResponse = _mapper.Map<List<IssuedBox>>(page.Data);

                return Ok(webResponse[0]);

            }
            catch (Exception ex)
            {
                _logger.Error(ex);


                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("list")]
        public IActionResult GetIssuedBoxList([FromQuery] IssuedBoxSearch searchOptions, int pageSize, int pageNumber)
        {
            try
            {
                var page = _provider.GetIssuedBox(_mapper.Map<DTO.Model.IssuedBoxSearch>(searchOptions), pageSize, pageNumber);

                if (page.Data == null || page.Data.Count == 0)
                {
                    return NotFound();
                }

                var webResponse = _mapper.Map<List<IssuedBox>>(page.Data);

                return Ok(new Page<IssuedBox>() { Data = webResponse, SearchCount = page.SearchCount, TotalCount = page.TotalCount });

            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("print-label")]
        public IActionResult PrintLabel([FromBody] List<IssuedBox> issuedBoxList)
        {
            try
            {
                _logger.Debug("Print Label Invoke");

                var idList = string.Join(",", issuedBoxList.Select(b => b.IssuedBoxID));

                _logger.Debug($"Issued Box List ID {idList}");

                var pdf = _provider.ExportLabelToPDF(idList);

                _logger.Debug("Done exporting pdf");

                MemoryStream ms = new MemoryStream(pdf);

                string base64 = System.Convert.ToBase64String(ms.ToArray(), 0, ms.ToArray().Length);

                _logger.Debug(base64);

                return Ok(base64);

                //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                //result.Content = new StringContent(base64);

                //return result;

            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return StatusCode(500);

                //HttpResponseMessage response = new HttpResponseMessage()
                //{
                //    Content = new StringContent(ex.Message),

                //    StatusCode = HttpStatusCode.InternalServerError

                //};



                //throw new HttpResponseException(response);
            }


        }

        [HttpGet]
        [Route("serial-list")]
        public HttpResponseMessage DownloadSerialList(long? shipmentID = null, string issuedBoxIDList = "")
        {
            var content = _provider.DownLoadSerialList(issuedBoxIDList, shipmentID);

            MemoryStream ms = new MemoryStream(content);

            string base64 = System.Convert.ToBase64String(ms.ToArray(), 0, ms.ToArray().Length);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            result.Content = new StringContent(base64);

            return result;

        }

        [HttpPut]
        [Route("update-boxes-status")]

        public IActionResult UpdateBoxesStatus([FromBody] List<IssuedBox> boxList)
        {

            if (boxList == null || boxList.Count == 0)
            {
                return BadRequest("Box collection is empty");
            }

            try
            {
                _provider.UpdateBoxesStatus(_mapper.Map<List<DTO.Core.IssuedBox>>(boxList));

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return StatusCode(500);
            }
        }


        /// <summary>
        /// Get Issued Box History by id
        /// </summary>
        /// <param name="shipmentID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("history/{id}")]

        public IActionResult GetIssuedBoxHist(long id)
        {
            var issuedBoxHistory = _provider.GetIssuedBoxHistory(id);

            if (issuedBoxHistory == null || issuedBoxHistory.Count == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<IssuedBoxActivityHistory>>(issuedBoxHistory));

        }
    }
}
