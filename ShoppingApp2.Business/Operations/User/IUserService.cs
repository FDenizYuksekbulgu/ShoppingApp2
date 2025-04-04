using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingApp2.Business.Operations.User.Dtos;
using ShoppingApp2.Business.Types;

namespace ShoppingApp2.Business.Operations.User
{
    public interface IUserService
    {
        
        Task<ServiceMessage> AddUser(AddUserDto user); // async çünkü UnitOfWork kullanılacak.

        
    }
}
