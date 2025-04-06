using Microsoft.EntityFrameworkCore;
using ShoppingApp2.Business.Operations.Product.Dtos;
using ShoppingApp2.Business.Types;
using ShoppingApp2.Data.Entities;
using ShoppingApp2.Data.Repositories;
using ShoppingApp2.Data.UnitOfWork;

namespace ShoppingApp2.Business.Operations.Feature
{
    public class ProductManager : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ProductEntity> _repository;

        public ProductManager(IUnitOfWork unitOfWork, IRepository<ProductEntity> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            var product = await _repository.GetAll(x => x.Id == id)
                                        .Select(x => new ProductDto
                                        {
                                            Id = x.Id,
                                            ProductName = x.ProductName,
                                            Price = x.Price,
                                            StockQuantity = x.StockQuantity
                                        }).FirstOrDefaultAsync();

            return product;
        }
        public async Task<ServiceMessage> AddProduct(AddProductDto product)
        {
            var hasProduct = _repository.GetAll(x => x.ProductName.ToLower() == product.ProductName.ToLower()).Any();

            if (hasProduct)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Ürün zaten bulunuyor."
                };
            }

            var productEntity = new ProductEntity
            {
                ProductName = product.ProductName,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
            };

            _repository.Add(productEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Ürün kaydı sırasında bir hata oluştu.");
            }

            return new ServiceMessage
            {
                IsSucceed = true
            };
        }

        public async Task<ServiceMessage> DeleteHotel(int id)
        {
            var hotel = _repository.GetById(id);

            if (hotel is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu id ile eşleşen bir ürün bulunamadı."
                };
            }

            _repository.Delete(id);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Silme işlemi sırasında bir hata oldu.");
            }

            return new ServiceMessage
            {
                IsSucceed = true
            };
        }

        public async Task<ServiceMessage> UpdateHotel(UpdateProductDto product)
        {
            var productEntity = _repository.GetById(product.Id);

            if (productEntity is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Ürün bulunamadı."
                };
            }

            await _unitOfWork.BeginTransaction();

            productEntity.ProductName = product.ProductName;
            productEntity.Price = product.Price;
            productEntity.StockQuantity = product.StockQuantity;

            _repository.Update(productEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                throw new Exception("Ürün bilgilerini güncellenirken bir hata ile karşılaşıldı.");
            }



            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                throw new Exception("Ürün bilgileri güncellenirken bir hata oluştu, süreç başa sarıldı.");
            }

            return new ServiceMessage
            {
                IsSucceed = true
            };
        }
    }
}
