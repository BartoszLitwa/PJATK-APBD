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

        public static Result<User, UserException> Create()
        {
            return new User()
            {
                
            }
        }
    }
}