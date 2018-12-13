﻿using AutoMapper;
using BedeSlots.DataModels;
using BedeSlots.ViewModels.GlobalViewModels;

namespace BedeSlots.Infrastructure.MappingProvider.Mappings
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
