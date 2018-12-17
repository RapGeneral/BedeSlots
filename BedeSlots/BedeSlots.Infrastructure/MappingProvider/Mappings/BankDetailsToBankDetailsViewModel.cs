using AutoMapper;
using BedeSlots.DataModels;
using BedeSlots.GlobalData.GlobalViewModels;
using System;

namespace BedeSlots.GlobalData.MappingProvider.Mappings
{
    public class BankDetailsToBankDetailsViewModel : Profile
    {
        public BankDetailsToBankDetailsViewModel()
        {
            CreateMap<BankDetails, BankDetailsViewModel>()
                .ForMember(dest => dest.Number, opts => opts.MapFrom(src => src.Number.Substring(src.Number.Length - 4).PadLeft(src.Number.Length, '*')))
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
