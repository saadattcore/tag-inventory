using AutoMapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using Transcore.TagInventory.BusinessLogic;
using Transcore.TagInventory.Common.Exceptions;
using Transcore.TagInventory.Web.Common;
using Transcore.TagInventory.Web.Models;
using DTO = Transcore.TagInventory.Entity;
using Models = Transcore.TagInventory.Web.Models;

namespace Transcore.TagInventory.Web.Controller
{
    [RoutePrefix("api/shipment")]
    public class ShipmentController : ApiController
    {
        private readonly IShipmentProvider _shipmentProvider;
        private readonly IHttpRequestHandler _requestHandler;
        private readonly IMapper _mapper;
        private readonly ILog _logger;
        public ShipmentController(IShipmentProvider shipmentProvider,
            IMapper mapper,
            ILog logger,
            IHttpRequestHandler reqHandler)
        {
            _shipmentProvider = shipmentProvider;
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
            var page = _shipmentProvider.GetShipment(new DTO.Model.ShipmentSearch() { ShipmentID = id }, 1, 1);

            if (page.Data == null || page.Data.Count == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Shipment>(page.Data[0]));
        }

        /// <summary>
        /// Get all shipments page wise
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("list")]
        public IHttpActionResult Get([FromUri] ShipmentSearch searchOptions, [FromUri] int pageSize, [FromUri] int pageNumber)
        {
            var page = _shipmentProvider.GetShipment(_mapper.Map<DTO.Model.ShipmentSearch>(searchOptions), pageSize, pageNumber);

            if (page.Data == null && page.Data.Count == 0)
            {
                return NotFound();
            }

            var webResponse = _mapper.Map<List<Shipment>>(page.Data);

            return Ok(new Page<Shipment>() { Data = webResponse, TotalCount = page.TotalCount, SearchCount = page.SearchCount });
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] Models.ShipmentCreateUpdate shipment)

        {
            if (shipment == null)
            {
                return BadRequest("Shipment cannot be null");
            }

            if (!ModelState.IsValid)
            {

                var errors = _requestHandler.ValidateModel(ModelState);
                return BadRequest(errors);
            }

            try
            {
                var shipmentDTO = _mapper.Map<DTO.Core.Shipment>(shipment);

                return Ok(_shipmentProvider.AddShipment(shipmentDTO));
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPut]
        [Route("")]
        public IHttpActionResult Put([FromBody] Models.ShipmentCreateUpdate shipment)
        {
            if (shipment == null)
            {
                return BadRequest("Shipment cannot be null");
            }

            if (!ModelState.IsValid)
            {
                var errors = _requestHandler.ValidateModel(ModelState);
                return BadRequest(errors);
            }

            try
            {
                var shipmentDTO = _mapper.Map<DTO.Core.Shipment>(shipment);

                _shipmentProvider.Update(shipmentDTO);

                return Ok();
            }
            catch (SqlException ex)
            {
                //log
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                //log
                return InternalServerError(ex);
            }



        }
    }
}