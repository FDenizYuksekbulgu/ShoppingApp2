using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingApp2.Business.Operations.User.Dtos;
using ShoppingApp2.Business.Types;
using ShoppingApp2.Data.Entities;
using ShoppingApp2.Data.Enums;
using ShoppingApp2.Data.Repositories;
using ShoppingApp2.Data.UnitOfWork;

namespace ShoppingApp2.Business.Operations.User
{
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserEntity> _userRepository;


        public UserManager(IUnitOfWork unitOfWork, IRepository<UserEntity> userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;

        }

        public async Task<ServiceMessage> AddUser(AddUserDto user)
        {
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == user.Email.ToLower());

            if (hasMail.Any())
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Email adresi zaten mevcut."
                };
            }

            var userEntity = new UserEntity
            {
                Email = user.Email,
                Password = user.Password, //Şifreleme
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = UserType.Customer,
            };

            _userRepository.Add(userEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Kullanıcı kaydı sırasında bir hata oluştu.");
            }

            return new ServiceMessage
            {
                IsSucceed = true
            };
        }
    }

       
    }
