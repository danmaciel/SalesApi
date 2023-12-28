

using AutoMapper;
using SalesApi.src.Data.Dtos;
using SalesApi.src.Models;

namespace SalesApi.src.Profiles;

public class CustomerProfile: Profile{
    public CustomerProfile(){
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();
        CreateMap<Customer, ReadCustomerDto>()
        .ForMember(
            customerDto => customerDto.Debts,
            opt => opt.MapFrom(
                customer => customer.Debts
            )
        );
    }
}