using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ProductDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Product> _productCollection;
        public ProductService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _productCollection = database.GetCollection<Product>(_databaseSettings.ProductCollectionName);
            _mapper = mapper;
        }
        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            // Map DTO to real product database entity
            var product = _mapper.Map<Product>(createProductDto);
            await _productCollection.InsertOneAsync(product);
        }

        public async Task DeleteProductAsync(string id)
        {
            await _productCollection.DeleteOneAsync(p => p.ProductId == id);
        }

        public async Task<List<ResultProductDto>> GetAllProductsAsync()
        {
            // get all products from database collection
            var productList = await _productCollection.Find(x => true).ToListAsync();
            
            // map product list to ResultProductDto list
            return _mapper.Map<List<ResultProductDto>>(productList);
        }

        public async Task<GetByIdProductDto> GetByIdProductAsync(string id)
        {
            // first find corresponding entity with given id
            var product = await _productCollection.Find(p => p.ProductId == id).FirstOrDefaultAsync();

            // map corresponding database entity to dto
            return _mapper.Map<GetByIdProductDto>(product);
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            // map dto to product entity
            var product = _mapper.Map<Product>(updateProductDto);

            // find corresponding entity with given id, update it and replace
            await _productCollection.FindOneAndReplaceAsync(p => p.ProductId == updateProductDto.ProductId, product);
        }
    }
}
