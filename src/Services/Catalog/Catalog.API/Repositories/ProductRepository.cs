using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
  public class ProductRepository : IProductRepository
  {
    private readonly ICatalogContext _Context;

    public ProductRepository(ICatalogContext context)
    {
      _Context = context;
    }
    public async Task CreateProduct(Product product) => await _Context.Products.InsertOneAsync(product);

    public async Task<bool> DeleteProduct(string id)
    {
      FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(p => p.Id, id);
      var result = await _Context.Products.DeleteOneAsync(filterDefinition);
      return result.IsAcknowledged && result.DeletedCount == 1;
    }
    public async Task<bool> UpdateProduct(Product product)
    {
      var result = await _Context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
      return result.IsAcknowledged && result.ModifiedCount > 0;
    }
    public async Task<Product> GetProduct(string id) => await _Context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
    {
      FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
      return await _Context.Products.Find(filterDefinition).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
      FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(p => p.Name, name);
      return await _Context.Products.Find(filterDefinition).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProducts() => await _Context.Products.Find(p => true).ToListAsync();
    
  }
}
