using System;
using CSharpFunctionalExtensions;
using LegacyApp.Enums;

namespace LegacyApp
{
    public class UserService(
        IClientRepository _clientRepository,
        IUserCreditService _userCreditService,
        IUserRepository _userRepository)
    {
        // This constructor is for keeping the legacy code working
        public UserService() : this(new ClientRepository(), new UserCreditService(), new UserDataAccessRepositoryAdapter()) { }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            var (_, isFailure, user) = User.Create(firstName, lastName, email, dateOfBirth);
            if (isFailure)
                return false;

            var client = _clientRepository.GetById(clientId);
            user.SetClient(client);

            SetCreditLimit(user, client);

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            _userRepository.AddUser(user);
            return true;
        }
        
        private void SetCreditLimit(User user, Client client)
        {
            if (client is null)
                return;
            
            switch (client.Type)
            {
                case ClientType.VeryImportantClient:
                    user.HasCreditLimit = false;
                    break;
                case ClientType.ImportantClient:
                    user.CreditLimit = CalculateCreditLimit(user) * 2;
                    break;
                case ClientType.NormalClient:
                default:
                    user.HasCreditLimit = true;
                    user.CreditLimit = CalculateCreditLimit(user);
                    break;
            }
        }

        private int CalculateCreditLimit(User user)
        {
            return _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
        }
    }
}
