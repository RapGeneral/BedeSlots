﻿using AutoMapper;
using BedeSlots.DataModels;
using BedeSlots.GlobalData.GlobalViewModels;

namespace BedeSlots.GlobalData.MappingProvider.Mappings
{
    public class BalanceToMoneyViewModel : Profile
    {
        public BalanceToMoneyViewModel()
        {
            CreateMap<Balance, MoneyViewModel>()
                .ForMember(dest => dest.Amount, opts => opts.MapFrom(src => src.Money))
                .ForMember(dest => dest.Currency, opts => opts.MapFrom(src => src.Currency.CurrencyName))
                .ReverseMap();
        }
    }
}
