namespace LegacyApp;

public class UserDataAccessRepositoryAdapter : IUserRepository
{
    public void AddUser(User user)
    {
        UserDataAccess.AddUser(user);
    }
}