
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using SalesApi.src.Services.Interface;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase{

    private ICustomerService _service;

    public CustomerController(ICustomerService service){
        _service = service;
    }

     [HttpGet]
     [Authorize]
     public IActionResult GetAll(){
        return Ok(_service.GetAll());
     }

     [HttpGet("by-name-filter/{filter}")]
     [Authorize]
     public IActionResult GetByNameFilter(string filter){
        return Ok(_service.GetByNameFilter(filter));
     }

     [HttpGet("by-id/{id}")]
     [Authorize]
     public IActionResult GetById(int id){

        ReadCustomerDto? f = _service.GetById(id);

        if(f==null){
            return NotFound();
        }

        return Ok(f);
     }

     [HttpGet("{skip}/{take}")]
     [Authorize]
     public IActionResult GetByRange(int skip,int Take){

        List<ReadCustomerDto>? f = _service.GetByRange(skip, Take).ToList();

        if(f==null){
            return NotFound();
        }
        return Ok(f);
     }

     [HttpPost]
     [Authorize]
     public IActionResult Add([FromBody] CreateCustomerDto dto){

        var customer = _service.AddCustomer(dto); 
        return CreatedAtAction(nameof(GetById), new {id = customer.Id}, customer);
     }

     [HttpPut("{id}")]
     [Authorize]
     public IActionResult Upadte(int id, [FromBody] UpdateCustomerDto dto){ 
      Customer? f = _service.Update(id, dto);

        if(f==null){
            return NotFound();
        }
 
        return NoContent();
     }

     [HttpPatch("{id}")]
     [Authorize]
     public IActionResult PartialPut(int id, [FromBody] JsonPatchDocument<UpdateCustomerDto> patch){ 
        Customer? f = _service.PartialUpdate(id, patch, this);

        if(f==null){
            return ValidationProblem(ModelState);
        }

         return NoContent();
     }

     [HttpDelete("{id}")]
     [Authorize]
     public IActionResult Delete(int id){
       Customer? f = _service.Delete(id);

        if(f==null){
            return NotFound();
        }

        return NoContent();
     }
}