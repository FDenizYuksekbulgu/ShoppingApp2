using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingApp2.Business.Operations.Product.Dtos;
using ShoppingApp2.Business.Types;

namespace ShoppingApp2.Business.Operations.Feature
{
    public interface IProductService
    {
        Task<ProductDto> GetProduct(int id);
        Task<ServiceMessage> AddProduct(AddProductDto product);

        Task<ServiceMessage> DeleteHotel(int id);
        Task<ServiceMessage> UpdateHotel(UpdateProductDto product);
    }
}
