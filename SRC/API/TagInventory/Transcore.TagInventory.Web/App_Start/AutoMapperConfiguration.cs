using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Transcore.TagInventory.Entity;
using Transcore.TagInventory.Web.Models;


namespace Transcore.TagInventory.App_Start
{
    public class AutoMapperConfiguration
    {
        public static void Configure(IMapperConfigurationExpression config)
        {
            config.CreateMap<ShipmentCreateUpdate, DTO.Core.Shipment>().ReverseMap();

            config.CreateMap<Shipment, DTO.Core.Shipment>()
                .AfterMap((src, dest) =>
                {
                    if (src.ReceivedBoxes != null)
                    {
                        foreach (var item in dest.ReceivedBoxes)
                        {
                            item.Shipment = dest;
                        }
                    }

                })
                .ReverseMap()
                .AfterMap((src, dest) =>
                {
                    if (src.ReceivedBoxes != null)
                    {
                        foreach (var item in dest.ReceivedBoxes)
                        {
                            item.Shipment = dest;
                        }
                    }



                });


            config.CreateMap<ReceivedBox, DTO.Core.ReceivedBox>()
                .AfterMap((src, dest) =>
                {

                    if (src.Tags != null && src.Tags.Count > 0)
                    {
                        foreach (Tag item in src.Tags)
                        {
                            item.ReceivedBox = src;
                        }
                    }

                }).ReverseMap()
                .AfterMap((src, dest) =>
                {
                    if (src.Tags != null && src.Tags.Count > 0)
                    {
                        foreach (DTO.Core.Tag item in src.Tags)
                        {
                            item.ReceivedBox = src;
                        }
                    }
                });

            config.CreateMap<Tag, DTO.Core.Tag>().ReverseMap();

            config.CreateMap<ScannedTagUpdate, DTO.Core.Tag>().ReverseMap();

            config.CreateMap<IssuedBox, DTO.Core.IssuedBox>().ReverseMap();

            config.CreateMap<IssuedBoxBase, DTO.Core.IssuedBox>().ReverseMap();

            config.CreateMap<TagSearch, DTO.Model.TagSearch>();

            config.CreateMap<ShipmentSearch, DTO.Model.ShipmentSearch>();

            config.CreateMap<ReceivedBoxSearch, DTO.Model.ReceivedBoxSearch>();

            config.CreateMap<IssuedBoxSearch, DTO.Model.IssuedBoxSearch>();

            config.CreateMap<ScannedReceivedBoxUpdate, DTO.Model.ScannedReceivedBoxUpdate>().ReverseMap();

            config.CreateMap<ScannedTagUpdate, DTO.Model.ScannedTagUpdate>().ReverseMap();

            config.CreateMap<Distributor, DTO.Core.Distributor>().ReverseMap();

            config.CreateMap<DistributorType, DTO.Core.DistributorType>().ReverseMap();

            config.CreateMap<DistributorAndTypes, DTO.Core.DistributorAndTypes>().ReverseMap();

            config.CreateMap<TagActivityHistory, DTO.Model.TagActivityHistory>().ReverseMap();

            config.CreateMap<IssuedBoxActivityHistory, DTO.Model.IssuedBoxActivityHistory>().ReverseMap();

            config.CreateMap<IssuedBoxActivityHistory, DTO.Model.IssuedBoxActivityHistory>().ReverseMap();

            config.CreateMap<ReceivedBoxUpdate, DTO.Model.ReceivedBoxUpdate>().ReverseMap();

            

        }
    }
}