using AutoMapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Transcore.TagInventory.BusinessLogic;
using Transcore.TagInventory.Web.Common;
using Transcore.TagInventory.Web.Models;
using DTO = Transcore.TagInventory.Entity;

namespace InventoryManagement.Controllers
{
    [RoutePrefix("api/tag")]
    public class TagController : ApiController
    {
        private readonly ITagProvider _provider;
        private readonly IHttpRequestHandler _requestHandler;
        private readonly IMapper _mapper;
        private readonly ILog _logger;
        public TagController(ITagProvider tagProvider,
            IMapper mapper,
            ILog logger,
            IHttpRequestHandler reqHandler)
        {
            _provider = tagProvider;
            _requestHandler = reqHandler;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get shipment by shipment ID
        /// </summary>
        /// <param name="shipmentID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]

        public IHttpActionResult Get([FromUri] long id)
        {
            DTO.Core.Tag tag = _provider.GetTag(id);

            if (tag == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Tag>(tag));

        }


        /// <summary>
        /// Get shipment by shipment ID
        /// </summary>
        /// <param name="shipmentID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public IHttpActionResult Get([FromUri] TagSearch searchOptions, [FromUri] int pageSize, [FromUri] int pageNumber)
        {

            var page = _provider.GetTags(_mapper.Map<DTO.Model.TagSearch>(searchOptions), pageSize, pageNumber);

            if (page.Data == null || page.Data.Count == 0)
            {
                return NotFound();
            }

            List<Tag> webResponse = _mapper.Map<List<Tag>>(page.Data);

            return Ok(new Page<Tag>() { Data = webResponse, SearchCount = page.SearchCount, TotalCount = page.TotalCount });
        }



        /// <summary>
        /// Update status,visualcheckstatus, rfidcheckstats of tags 
        /// </summary>
        /// <param name="shipmentID"></param>
        /// <returns>httpresponse</returns>
        [HttpPut]
        [Route("list")]
        public IHttpActionResult Put([FromBody] List<ScannedTagUpdate> tagsStatusModels)
        {
            IHttpActionResult result = null;

            if (!ModelState.IsValid)
            {
                string errors = _requestHandler.ValidateModel(ModelState);

                return BadRequest(errors);
            }

            try
            {
                List<DTO.Core.Tag> tags = _mapper.Map<List<ScannedTagUpdate>, List<DTO.Core.Tag>>(tagsStatusModels);

                _provider.UpdateTagsStatus(tags);

                result = Ok();
            }
            catch (SqlException ex)
            {
                _logger.Error(ex.Message);

                result = BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);

                result = InternalServerError();
            }

            return result;
        }


        /// <summary>
        /// Get Tag History by id
        /// </summary>
        /// <param name="shipmentID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("history/{id}")]

        public IHttpActionResult GetTagHist([FromUri] long id)
        {
            var tagHist = _provider.GetTagHistory(id);

            if (tagHist == null || tagHist.Count == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<TagActivityHistory>>(tagHist));

        }

        /// <summary>
        /// Update tag
        /// </summary>
        /// <param name="shipmentID"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update-tag")]

        public IHttpActionResult Put([FromBody]Tag tag)
        {
            try
            {
                _provider.UpdateTag(_mapper.Map<DTO.Core.Tag>(tag));

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return InternalServerError();
            }         

        }

    }
}
