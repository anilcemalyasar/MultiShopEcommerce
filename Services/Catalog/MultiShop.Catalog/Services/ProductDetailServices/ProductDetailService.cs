using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ProductDetailDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Services.ProductDetailServices;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ProductDetailDetailServices
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<ProductDetail> _productDetailCollection;

        public ProductDetailService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _productDetailCollection = database.GetCollection<ProductDetail>(_databaseSettings.ProductDetailCollectionName);
            _mapper = mapper;
        }
        public async Task CreateProductDetailAsync(CreateProductDetailDto createProductDetailDto)
        {
            // Map DTO to real ProductDetail database entity
            var ProductDetail = _mapper.Map<ProductDetail>(createProductDetailDto);
            await _productDetailCollection.InsertOneAsync(ProductDetail);
        }

        public async Task DeleteProductDetailAsync(string id)
        {
            await _productDetailCollection.DeleteOneAsync(p => p.ProductDetailID == id);
        }

        public async Task<List<ResultProductDetailDto>> GetAllProductDetailsAsync()
        {
            // get all ProductDetails from database collection
            var ProductDetailList = await _productDetailCollection.Find(x => true).ToListAsync();

            // map ProductDetail list to ResultProductDetailDto list
            return _mapper.Map<List<ResultProductDetailDto>>(ProductDetailList);
        }

        public async Task<GetByIdProductDetailDto> GetByIdProductDetailAsync(string id)
        {
            // first find corresponding entity with given id
            var ProductDetail = await _productDetailCollection.Find(p => p.ProductDetailID == id).FirstOrDefaultAsync();

            // map corresponding database entity to dto
            return _mapper.Map<GetByIdProductDetailDto>(ProductDetail);
        }

        public async Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto)
        {
            // map dto to ProductDetail entity
            var ProductDetail = _mapper.Map<ProductDetail>(updateProductDetailDto);

            // find corresponding entity with given id, update it and replace
            await _productDetailCollection.FindOneAndReplaceAsync(p => p.ProductDetailID == updateProductDetailDto.ProductDetailId, ProductDetail);
        }
    }
}
