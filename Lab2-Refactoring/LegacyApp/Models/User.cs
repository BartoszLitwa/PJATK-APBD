using System;
using CSharpFunctionalExtensions;
using LegacyApp.Exceptions;

namespace LegacyApp
{
    public class User : Person
    {
        public Client Client { get; internal set; }
        public DateTime DateOfBirth { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public bool HasCreditLimit { get; internal set; }
        public int CreditLimit { get; internal set; }
        
        private User() { }
        
        public static Result<User, UserException> Create(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                DateOfBirth = dateOfBirth
            };
            
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return Result.Failure<User, UserException>(new UserValidationException("First name and last name are required"));
            }

            if (!email.Contains('@') && !email.Contains('.'))
            {
                return Result.Failure<User, UserException>(new UserValidationException("Email is not valid"));
            }
            
            var age = user.GetAge();
            if (age < 21)
            {
                return Result.Failure<User, UserException>(new UserValidationException("User must be at least 21 years old"));
            }

            return Result.Success<User, UserException>(user);
        }
        
        public void SetClient(Client client)
        {
            Client = client;
        }
        
        public int GetAge()
        {
            var now = DateTime.Now;
            var age = now.Year - DateOfBirth.Year;
            if (now.Month < DateOfBirth.Month || (now.Month == DateOfBirth.Month && now.Day < DateOfBirth.Day))
                age--;

            return age;
        }
    }
}