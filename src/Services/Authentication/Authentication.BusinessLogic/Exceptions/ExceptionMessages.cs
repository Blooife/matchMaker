namespace Authentication.BusinessLogic.Exceptions;

public static class ExceptionMessages
{
    public const string ErrorAssigningRole = "Error while assigning a role";
    public const string ErrorRemovingRole = "Error while removing a role";
    public const string LoginFailed = "Username or password is incorrect";
    public const string RoleNotExists = "Role does not exist";
    public const string CreateRoleFailed = "An exception occured while creating the role";
    public const string DeleteUserFailed = "An exception occured while deleting user";
}