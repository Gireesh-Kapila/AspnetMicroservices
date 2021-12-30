using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Basket.API.Controllers
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class BasketController : ControllerBase
  {
    private readonly IBasketRepository _BasketRepository;

    public BasketController(IBasketRepository basketRepository)
    {
      _BasketRepository = basketRepository;
    }
    // GET: api/<BasketController>
    [HttpGet("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
    {
      var basket =  await _BasketRepository.GetBasket(userName);
      return Ok(basket ?? new ShoppingCart(userName));
    }


    // POST api/<BasketController>
    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart newbasket)
    {
      var updatedBasket = await _BasketRepository.UpdateBasket(newbasket);
      return Ok(updatedBasket);
    }

    // DELETE api/<BasketController>/5
    [HttpDelete("{userName}")]
    public async Task<ActionResult> Delete(string userName)
    {
      await _BasketRepository.DeleteBasket(userName);
      return Ok();
    }
  }
}
