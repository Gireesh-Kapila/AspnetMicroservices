using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class CatalogController : ControllerBase
  {
    private readonly IProductRepository _Repository;
    private readonly ILogger<CatalogController> _Logger;

    public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
    {
      _Repository = repository ?? throw new ArgumentNullException(nameof(repository));
      _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
      var products = await _Repository.GetProducts();
      return Ok(products);
    }

    [HttpGet("{id:length(24)}", Name = "GetProduct")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductById(string id)
    {
      var product = await _Repository.GetProduct(id);
      if (product == null)
      {
        _Logger.LogError($"Product with Id {id}, not found.");
        return NotFound();
      }
      return Ok(product);
    }

    [HttpGet("[action]/{category}", Name = "GetProductByCategory")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
    {
      var product = await _Repository.GetProductByCategory(category);
      return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<Product>> CreateProduct([FromBody]Product product)
    {
      await _Repository.CreateProduct(product);
      return CreatedAtRoute("GetProduct", new {id = product.Id}, product);
    }
    [HttpPut]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product) => Ok(await _Repository.UpdateProduct(product));

    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteProduct(string id) => Ok(await _Repository.DeleteProduct(id));
  }
}
