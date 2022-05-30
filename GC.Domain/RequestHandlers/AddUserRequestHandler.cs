using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GC.Data.Objects;
using GC.Domain.Services.Repositories;
using FluentValidation;
using FluentValidation.Results;
using GC.Domain.Services;

namespace GC.Domain.RequestHandlers
{

    public class AddUserRequestHandler
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordService passwordService;

        public AddUserRequestHandler(IUserRepository userRepository, IPasswordService passwordService)
        {
            this.userRepository = userRepository;
            this.passwordService = passwordService;
        }

        public async Task Process(AddUserRequest request)
        {
            if (await this.userRepository.IsLoginInUse(request.Login))
            {
                throw new ValidationException(new List<ValidationFailure>() {
                    new ValidationFailure(nameof(request.Login), "A user with same login already exists")
                });
            }

            var hashAndSalt = await this.passwordService.GeneratePassword(request.Password);
            
            var newUser = new User
            {
                Login = request.Login,
                Name = request.Name,
                Role = request.Role,
                CreationDate = DateTime.UtcNow,
                PasswordHash = hashAndSalt.Hash,
                PasswordSalt = hashAndSalt.Salt
            };

            await this.userRepository.AddUser(newUser);
            return;
        }
    }

    public class AddUserRequest
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public UserRole Role { get; set; }
    }
}
