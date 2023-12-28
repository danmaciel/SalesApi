

using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using SalesApi.src.Data;
using SalesApi.src.Services.Interface;
using Microsoft.AspNetCore.Mvc;
namespace SalesApi.src.Services;


public class CustomerService: ICustomerService{

    private readonly ILogger<CustomerService> _logger;
    private SalesContext _context;
    private IMapper _mapper;


    public CustomerService(ILogger<CustomerService> logger, SalesContext context, IMapper mapper){
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public ICollection<ReadCustomerDto> GetAll() {
        return _mapper.Map<List<ReadCustomerDto>>(_context.Customers).Where(dto => dto.Active == 1).ToList();
    }

    public ICollection<ReadCustomerDto> GetByNameFilter(string filter)
    {
         return _mapper.Map<List<ReadCustomerDto>>(_context.Customers).Where(dto => dto.Active == 1 && dto.FullName.Contains(filter)).ToList();
    }

    public ReadCustomerDto? GetById(int id) {
        Customer? f = _context.Customers.FirstOrDefault((f) => f.Id == id);

        if(f==null || f.Active == 0){
            return null;
        }
        return _mapper.Map<ReadCustomerDto>(f);
    }

    public ICollection<ReadCustomerDto> GetByRange(int skip, int Take){
        return _mapper.Map<List<ReadCustomerDto>>(_context.Customers.Skip(skip).Take(Take)).Where(dto => dto.Active == 1).ToList();
    }

    public Customer AddCustomer(CreateCustomerDto dto){
        try{
            var customer = _mapper.Map<Customer>(dto);
            customer.Active = 1;

            Console.WriteLine($"CLiente = {customer.ToString()}");
            _context.Customers.Add(customer);
            _context.SaveChanges();  
            return customer;
        }catch (Exception e) {
            _logger.LogError(e, "Erro ao salvar o Customer");
            throw new ApplicationException("Ocorreum um erro na criação do cliente!");
        }   
    }

    public Customer? Update(int id, UpdateCustomerDto dto){
       Customer? f = _context.Customers.FirstOrDefault((f) => f.Id == id);

        if(f==null || f.Active == 0){
            return null;
        }

        _mapper.Map(dto, f);
        _context.SaveChanges();  

        return f;
    }

    public Customer? PartialUpdate(int id, JsonPatchDocument<UpdateCustomerDto> patch, ControllerBase controllerBase){
        Customer? f = _context.Customers.FirstOrDefault((f) => f.Id == id);

        if(f==null || f.Active == 0){
            return null;
        }

        var customerUpdate = _mapper.Map<UpdateCustomerDto>(f);
        patch.ApplyTo(customerUpdate, controllerBase.ModelState);

        if(!controllerBase.TryValidateModel(customerUpdate)){
            return null;
        }

         _mapper.Map(customerUpdate, f);
         _context.SaveChanges(); 
         return f; 
    }

    public Customer? Delete(int id){
        Customer? f = _context.Customers.FirstOrDefault((f) => f.Id == id);

        if(f==null || f.Active == 0){
            return null;
        }
        f.Active = 0;
        
        return Update(id, _mapper.Map<UpdateCustomerDto>(f));
    }

    
}