using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DataAccessLayer;
using DataAccessLayer.Repositories;
using ViewModelLayer.Models.User;

namespace BusinessLogicLayer.Repositories
{
    public class AccountBl
    {
        private readonly UserRepository _userRepository = new UserRepository();

        private readonly RoleRepository _roleRepository = new RoleRepository();

        public List<DisplayUserModel> DisplayUsers()
        {
            var users = _userRepository.GetAll().ToList();
            var models = new List<DisplayUserModel>();
            foreach (var user in users)
            {
                models.Add(InitDisplayUserModel(user));
            }

            return models;
        }

        public DisplayUserModel DisplayUserDetails(int id)
        {
            var user = _userRepository.GetById(id);

            return InitDisplayUserModel(user);
        }

        public UserSessionModel CreateSessionModel(LoginUserModel model)
        {
            var user = _userRepository.SearchByUsername(model.Username);
            return new UserSessionModel
            {
                Id = user.Id,
                Firstname = user.Name,
                Lastname = user.Surname,
                Gender = user.Gender,
                Email = user.Email,
                Username = user.Username,
                Role = _roleRepository.GetById(user.RoleId).Type
            };
            
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
                RoleId = _roleRepository.GetByType("User").Id,
                TokenCount = 0
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

        public void ChangeUserDetails(DisplayUserModel model)
        {
            var user = _userRepository.GetById(model.Id);
            bool changed = false;
            if (!user.Name.Equals(model.Firstname))
            {
                user.Name = model.Firstname;
                changed = true;
            }

            if (!user.Surname.Equals(model.Lastname))
            {
                user.Surname = model.Lastname;
                changed = true;
            }

            if (!user.Email.Equals(model.Email))
            {
                user.Email = model.Email;
                changed = true;
            }

            if (changed)
            {
                _userRepository.Save(user);
            }
        }

        public bool ChangePassword(PasswordChangeUserModel model)
        {
            var user = _userRepository.GetById(model.Id);
            if (user == null) return false;
            if (!CheckCredentials(user.Username, model.OldPassword)) return false;
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var hashedPassword = HashThePassword(model.NewPassword, salt);
            user.Password = hashedPassword;
            _userRepository.Save(user);
            return true;

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
