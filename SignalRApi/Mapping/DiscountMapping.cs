﻿using AutoMapper;
using SignalR.DtoLayer.AboutDto;
using SignalR.DtoLayer.DiscountDto;
using SignalR.EntityLayer.DAL.Entities;

namespace SignalRApi.Mapping
{
    public class DiscountMapping:Profile
    {
        public DiscountMapping()
        {
            CreateMap<Discount, ResultDiscountDto>().ReverseMap();
            CreateMap<Discount, CreateDiscountDto>().ReverseMap();
            CreateMap<Discount, GetDiscountDto>().ReverseMap();
            CreateMap<Discount, UpdateDiscountDto>().ReverseMap();
        }
    }
}
