

using Microsoft.AspNetCore.Mvc;
using SalesApi.src.Models;
using SalesApi.src.Data.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using SalesApi.src.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("[controller]")]
public class DebtController : ControllerBase{
    
    private IDebtervice _service;

    public DebtController(IDebtervice service){
        _service = service;
    }

     [HttpGet]
     [Authorize]
     public IActionResult GetAll(){
        return Ok(_service.GetAll());
     }

    [HttpGet("by-id/{id}")]
    [Authorize]
     public IActionResult GetById(int id){
        ReadDebtDto? f = _service.GetById(id);

        if(f==null){
            return NotFound();
        }

        return Ok(f);   
     }

     [HttpGet("by-customer/{id}")]
     [Authorize]
     public IActionResult GetByCustomerId(int id){
        return Ok(_service.GetByCustomerId(id));
     }

    [HttpGet("{skip}/{take}")]
    [Authorize]
     public IActionResult GetByRange([FromQuery] int? customerId, int skip=0,int take=50){
         List<ReadDebtDto>? f = _service.GetByRange(skip, take).ToList();

        if(f==null){
            return NotFound();
        }
        return Ok(f);
     }

     [HttpPost]
     [Authorize]
     public IActionResult Add([FromBody] CreateDebtDto dto){
        try{
            var customer = _service.Add(dto); 
            return CreatedAtAction(nameof(GetById), new {id = customer.Id}, customer);
        }catch (InvalidOperationException e) {
            return Problem(statusCode: 406, detail: e.Message);
        } catch (Exception e) {
            throw new ApplicationException("Ocorreum um erro na criação da venda!");
        } 
        
     }

     [HttpPut("{id}")]
     [Authorize]
     public IActionResult Update(int id, [FromBody] UpdateDebtDto dto){ 
        Debt? f = _service.Update(id, dto);

        if(f==null){
            return NotFound();
        }
 
        return NoContent();
     }

     [HttpPatch("{id}")]
     [Authorize]
     public IActionResult PartialUpdate(int id, [FromBody] JsonPatchDocument<UpdateDebtDto> patch){ 
        Debt? f = _service.PartialUpdate(id, patch, this);

        if(f==null){
            return ValidationProblem(ModelState);
        }

         return NoContent();
     }

     [HttpDelete("{id}")]
     [Authorize]
     public IActionResult Delete(int id){
        Debt? f = _service.Delete(id);

        if(f==null){
            return NotFound();
        }

        return NoContent();
     }
}