

using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesApi.src.Data;
using SalesApi.src.Data.Dtos;
using SalesApi.src.Models;
using SalesApi.src.Services.Interface;

namespace SalesApi.src.Services;

public class DebtService : IDebtervice {

    private readonly ILogger<DebtService> _logger;
    private SalesContext _context;
    private IMapper _mapper;

     public DebtService(ILogger<DebtService> logger, SalesContext context, IMapper mapper){
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public ICollection<ReadDebtDto> GetAll(){
         return _mapper.Map<List<ReadDebtDto>>(_context.Debts).ToList();
    }

    public ReadDebtDto? GetById(int id){
        Debt? f = _context.Debts.FirstOrDefault((f) => f.Id == id);

        if(f==null){
            return null;
        }
        return _mapper.Map<ReadDebtDto>(f);
    }

    public ICollection<ReadDebtDto> GetByCustomerId(int customerId)
    {
       var result = _context.Debts.FromSqlRaw($"SELECT * FROM Debts WHERE CustomerId = {customerId} AND Active = 1");
       return _mapper.Map<List<ReadDebtDto>>(result).ToList();
    }

    public ICollection<ReadDebtDto> GetByRange(int skip, int Take) {
         return _mapper.Map<List<ReadDebtDto>>(_context.Debts.Skip(skip).Take(Take)).Where(dto => dto.Active == 1).ToList();
    }

    public Debt Add(CreateDebtDto dto) {

        if(HasDebitsPendinPayment(dto.CustomerId)){
            throw new InvalidOperationException("O cliente possui débito em aberto, não é possível adicionar um novo!");
        }
        var debt = _mapper.Map<Debt>(dto);
        debt.Active = 1;
        _context.Debts.Add(debt);
        _context.SaveChanges();  

        return debt;
         
    }   

    public Debt? Update(int id, UpdateDebtDto dto) {
        Debt? f = _context.Debts.FirstOrDefault((f) => f.Id == id);

        if(f==null || f.Active == 0){
            return null;
        }

        _mapper.Map(dto, f);
        _context.SaveChanges();  

        return f;
    }

     public Debt? PartialUpdate(int id, JsonPatchDocument<UpdateDebtDto> patch, ControllerBase controllerBase) {
        Debt? f = _context.Debts.FirstOrDefault((f) => f.Id == id);

        if(f==null || f.Active == 0){
            return null;
        }

        var DebtUpdate = _mapper.Map<UpdateDebtDto>(f);
        patch.ApplyTo(DebtUpdate, controllerBase.ModelState);

        if(!controllerBase.TryValidateModel(DebtUpdate)){
            return null;
        }

         _mapper.Map(DebtUpdate, f);
         _context.SaveChanges(); 
         return f; 
    }

    public Debt? Delete(int id) {
        Debt? f = _context.Debts.FirstOrDefault((f) => f.Id == id);

        if(f==null || f.Active == 0){
            return null;
        }
        f.Active = 0;
        
        return Update(id, _mapper.Map<UpdateDebtDto>(f));
    }

    public bool HasDebitsPendinPayment(int userId)
    {
        var result = _context.Debts.FromSqlRaw($"SELECT * FROM Debts WHERE CustomerId = {userId} AND PaidAt IS NULL").Count();
        return result > 0;
    }

}