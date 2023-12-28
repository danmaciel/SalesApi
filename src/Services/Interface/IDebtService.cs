
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesApi.src.Data.Dtos;
using SalesApi.src.Models;

namespace SalesApi.src.Services.Interface;

public interface IDebtervice
{
    public ICollection<ReadDebtDto> GetAll();

    public ICollection<ReadDebtDto> GetByCustomerId(int customerId);

    public ReadDebtDto? GetById(int id);

    public ICollection<ReadDebtDto> GetByRange(int skip,int Take);

    public Debt Add(CreateDebtDto dto);

    public Debt? Update(int id, UpdateDebtDto dto);

    public Debt? PartialUpdate(int id, JsonPatchDocument<UpdateDebtDto> patch, ControllerBase controllerBase);

    public Debt? Delete(int id);

    public bool HasDebitsPendinPayment(int userId);
}