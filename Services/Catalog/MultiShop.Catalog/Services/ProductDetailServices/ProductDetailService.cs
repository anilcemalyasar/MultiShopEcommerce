using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ProductDetailDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ProductDetailServices
{
    public class ProductDetailService : IProductDetailService
    {

        private readonly IMongoCollection<ProductDetail> _productDetailCollection;
        private readonly IMapper _mapper;

        public ProductDetailService(IMapper mapper, IDatabaseSettings _dbSettings)
        {
            var client = new MongoClient(_dbSettings.ConnectionString);
            var database = client.GetDatabase(_dbSettings.DatabaseName);
            _productDetailCollection = database.GetCollection<ProductDetail>(_dbSettings.ProductDetailCollectionName);
            _mapper = mapper;
        }
        public async Task CreateProductDetailAsync(CreateProductDetailDto productDetailDto)
        {
            var value = _mapper.Map<ProductDetail>(productDetailDto);
            await _productDetailCollection.InsertOneAsync(value);
        }

        public async Task DeleteProductDetailAsync(string id)
        {
            await _productDetailCollection.DeleteOneAsync(id);
        }

        public async Task<List<ResultProductDetailDto>> GetAllProductDetailsAsync()
        {
            var values = await _productDetailCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultProductDetailDto>>(values);
        }

        public async Task<GetByIdProductDetailDto> GetByIdProductDetailAsync(string id)
        {
            var value = await _productDetailCollection.Find<ProductDetail>(x => x.ProductId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetByIdProductDetailDto>(value);
        }

        public async Task UpdateProductDetailAsync(UpdateProductDetailDto productDetailDto)
        {
            var value = _mapper.Map<ProductDetail>(productDetailDto);
            await _productDetailCollection.FindOneAndReplaceAsync(x => x.ProductDetailId == productDetailDto.ProductDetailId, value);
        }
    }
}
