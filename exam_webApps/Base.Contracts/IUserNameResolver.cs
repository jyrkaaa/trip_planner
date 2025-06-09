namespace Base.Contracts;

public interface IUserNameResolver
{
    string CurrentUserName { get; }
    string CurrentUserId { get; }
}