using AutoMapper;
using BedeSlots.DataModels;
using BedeSlots.ViewModels.Enums;
using BedeSlots.ViewModels.GlobalViewModels;
using System;

namespace BedeSlots.ViewModels.MappingProvider.Mappings
{
    public class TransactionToTransactionViewModel : Profile
    {
        public TransactionToTransactionViewModel()
        {
            CreateMap<Transaction, TransactionViewModel>()
                .ForMember(dest => dest.Username, opts => opts.MapFrom(src => src.Balance.User.UserName))
                .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.Date))
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => Enum.Parse<TypeOfTransaction>(src.Type.Name, true)))
                .ForMember(dest => dest.Amount, opts => opts.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Description))
                .ReverseMap();
        }
    }
}