using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

using Transcore.TagInventory.BusinessLogic;
using Transcore.TagInventory.Common.Exceptions;
using Transcore.TagInventory.Entity.Model;
using Transcore.TagInventory.Web.Common;
using Transcore.TagInventory.Web.Models;
using DTO = Transcore.TagInventory.Entity;
using Models = Transcore.TagInventory.Web.Models;

namespace Transcore.TagInventory.Web.Controller
{
    [Route("api/shipment")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentProvider _shipmentProvider;
        private readonly IHttpRequestHandler _requestHandler;
        private readonly IExportPackageProvider _exportPackageProvider;
        private readonly IMapper _mapper;

        public ShipmentController(IShipmentProvider shipmentProvider,
            IMapper mapper,
            IHttpRequestHandler reqHandler,
            IExportPackageProvider exportPackageProvider)
        {
            _shipmentProvider = shipmentProvider;
            _requestHandler = reqHandler;
            _mapper = mapper;
            _exportPackageProvider = exportPackageProvider;


        }


        /// <summary>
        /// Get shipment by shipment ID
        /// </summary>
        /// <param name="shipmentID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public ActionResult Get(long id)
        {
            Log.Information("Get request for shipment");

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
        public IActionResult Get([FromQuery] Transcore.TagInventory.Web.Models.ShipmentSearch searchOptions, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {

            Log.Information("Get request for shipments");

            var page = _shipmentProvider.GetShipment(_mapper.Map<DTO.Model.ShipmentSearch>(searchOptions), pageSize, pageNumber);

            if (page.Data == null && page.Data.Count == 0) return NotFound();

            var webResponse = _mapper.Map<List<Shipment>>(page.Data);

            return Ok(new Page<Shipment>() { Data = webResponse, TotalCount = page.TotalCount, SearchCount = page.SearchCount });
        }

        [HttpPost]
        [Route("")]
        public IActionResult Post([FromBody] Models.ShipmentCreateUpdate shipment)

        {
            if (shipment == null) return BadRequest("Shipment cannot be null");

            try
            {
                var shipmentDTO = _mapper.Map<DTO.Core.Shipment>(shipment);

                return CreatedAtAction(nameof(Get), new { shipmentID = _shipmentProvider.AddShipment(shipmentDTO), pageSize = 10, pageNumber = 1 }, null);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpGet]
        [Route("create-shipment-package")]
        public IActionResult GetExportPackage([FromQuery] ExportPackage model)
        {
            try
            {
                byte[] exportPackage = _exportPackageProvider.GetShipmentExportPackage(model.ShipmentID, model.ContainsFreeTags);

                MemoryStream ms = new MemoryStream(exportPackage);

                string base64 = System.Convert.ToBase64String(ms.ToArray(), 0, ms.ToArray().Length);

                return File(exportPackage, "application/zip", "shipment_1.zip");

                //return Ok(exportPackage);

            }
            catch (Exception ex)
            {

                //throw new HttpResponseException(HttpStatusCode.NotFound);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            
                
            }

        }

        [HttpPut]
        [Route("")]
        public IActionResult Put([FromBody] Models.ShipmentCreateUpdate shipment)
        {
            if (shipment == null) return BadRequest("Shipment cannot be null");


            try
            {
                var shipmentDTO = _mapper.Map<DTO.Core.Shipment>(shipment);

                _shipmentProvider.Update(shipmentDTO);

                return Ok();
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "Exception");

                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}