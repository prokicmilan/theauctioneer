using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DataAccessLayer;
using DataAccessLayer.Repositories;
using ViewModelLayer.Models.User;

namespace BusinessLogicLayer.Repositories
{
    public class UserBl
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public List<DisplayUserModel> DisplayUsers()
        {
            List<User> users = _userRepository.GetAll().ToList();
            List<DisplayUserModel> models = new List<DisplayUserModel>();
            foreach (var user in users)
            {
                models.Add(InitDisplayUserModel(user));
            }

            return models;
        }

        public DisplayUserModel DisplayUserDetails(int id)
        {
            User user = _userRepository.GetById(id);

            return InitDisplayUserModel(user);
        }

        public int CreateUser(CreateUserModel model)
        {
            if (_userRepository.SearchByUsername(model.Username) != null)
            {
                // korisnik sa zadatim username-om vec postoji
                return -1;
            }

            if (_userRepository.SearchByEmail(model.Email) != null)
            {
                // korisnik sa zadatim email-om vec postoji
                return -2;
            }

            var user = new User
            {
                Name = model.Firstname,
                Surname = model.Lastname,
                Gender = model.Gender,
                Email = model.Email,
                Username = model.Username,
                RoleId = 1,
                TokenCount = 9999
            };
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var hashedPassword = HashThePassword(model.Password, salt);
            user.Password = hashedPassword;
            _userRepository.Save(user);

            return 0;
        }

        public bool CheckCredentials(string username, string password)
        {
            var user = _userRepository.SearchByUsername(username);
            if (user != null)
            {
                return CheckPassword(user.Password, password);
            }

            return false;
        }

        private DisplayUserModel InitDisplayUserModel(User user)
        {
            var model = new DisplayUserModel
            {
                Id = user.Id,
                Firstname = user.Name,
                Lastname = user.Surname,
                Gender = user.Gender,
                Email = user.Email,
                Username = user.Username,
                TokenCount = user.TokenCount
            };

            return model;
        }

        private bool CheckPassword(string hashedPassword, string enteredPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            string hashedEnteredPassword = HashThePassword(enteredPassword, salt);
            return hashedPassword.Equals(hashedEnteredPassword);

        }

        private string HashThePassword(string password, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            var hash = pbkdf2.GetBytes(24);
            var hashBytes = new byte[40];
            // kopira sve bajtove salta na pocetak (stvar izbora, hash moze da bude bilo gde)
            Array.Copy(salt, 0, hashBytes, 0, 16);
            // kopira sve bajtove hash-a na kraj
            Array.Copy(hash, 0, hashBytes, 16, 24);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
