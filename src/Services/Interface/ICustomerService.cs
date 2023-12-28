
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace SalesApi.src.Services.Interface;

public interface ICustomerService
{
    public ICollection<ReadCustomerDto> GetAll();

    public ICollection<ReadCustomerDto> GetByNameFilter(string filter);

    public ReadCustomerDto? GetById(int id);

    public ICollection<ReadCustomerDto> GetByRange(int skip,int Take);

    public Customer AddCustomer(CreateCustomerDto dto);

    public Customer? Update(int id, UpdateCustomerDto dto);

    public Customer? PartialUpdate(int id, JsonPatchDocument<UpdateCustomerDto> patch, ControllerBase controllerBase);

    public Customer? Delete(int id);
}