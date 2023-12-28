
using AutoMapper;
using SalesApi.src.Data.Dtos;
using SalesApi.src.Models;

namespace SalesApi.src.Profiles;

public class DebtProfile: Profile{
    public DebtProfile(){
        CreateMap<CreateDebtDto, Debt>();
        CreateMap<UpdateDebtDto, Debt>();
        CreateMap<Debt, ReadDebtDto>();
    }
}