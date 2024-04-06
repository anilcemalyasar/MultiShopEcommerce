using AutoMapper;
using MongoDB.Driver;
using MultiShop.Catalog.Dtos.ProductImageDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ProductImageServices
{
    public class ProductImageService : IProductImageService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<ProductImage> _ProductImageCollection;
        public ProductImageService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _ProductImageCollection = database.GetCollection<ProductImage>(_databaseSettings.ProductImageCollectionName);
            _mapper = mapper;
        }
        public async Task CreateProductImageAsync(CreateProductImageDto createProductImageDto)
        {
            // Map DTO to real ProductImage database entity
            var ProductImage = _mapper.Map<ProductImage>(createProductImageDto);
            await _ProductImageCollection.InsertOneAsync(ProductImage);
        }

        public async Task DeleteProductImageAsync(string id)
        {
            await _ProductImageCollection.DeleteOneAsync(p => p.ProductImageID == id);
        }

        public async Task<List<ResultProductImageDto>> GetAllProductImagesAsync()
        {
            // get all ProductImages from database collection
            var ProductImageList = await _ProductImageCollection.Find(x => true).ToListAsync();

            // map ProductImage list to ResultProductImageDto list
            return _mapper.Map<List<ResultProductImageDto>>(ProductImageList);
        }

        public async Task<GetByIdProductImageDto> GetByIdProductImageAsync(string id)
        {
            // first find corresponding entity with given id
            var ProductImage = await _ProductImageCollection.Find(p => p.ProductImageID == id).FirstOrDefaultAsync();

            // map corresponding database entity to dto
            return _mapper.Map<GetByIdProductImageDto>(ProductImage);
        }

        public async Task UpdateProductImageAsync(UpdateProductImageDto updateProductImageDto)
        {
            // map dto to ProductImage entity
            var ProductImage = _mapper.Map<ProductImage>(updateProductImageDto);

            // find corresponding entity with given id, update it and replace
            await _ProductImageCollection.FindOneAndReplaceAsync(p => p.ProductImageID == updateProductImageDto.ProductImageID, ProductImage);
        }
    }
}
