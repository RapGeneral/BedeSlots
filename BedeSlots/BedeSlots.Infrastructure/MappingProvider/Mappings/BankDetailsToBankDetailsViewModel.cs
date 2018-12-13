using AutoMapper;
using BedeSlots.DataModels;
using BedeSlots.ViewModels.GlobalViewModels;
using System;

namespace BedeSlots.Infrastructure.MappingProvider.Mappings
{
    public class BankDetailsToBankDetailsViewModel : Profile
    {
        public BankDetailsToBankDetailsViewModel()
        {
            CreateMap<BankDetails, BankDetailsViewModel>()
                .ForMember(dest => dest.Number, opts => opts.MapFrom(src => src.Number.Substring(src.Number.Length - 4).PadLeft(src.Number.Length, '*')))
                .ReverseMap();
        }
    }
}
