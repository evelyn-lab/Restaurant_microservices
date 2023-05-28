namespace KPO_hw.Models;

/*
 * Класс полей регистрации пользователя для метода Post в RegistrationController
 * UserName - имя пользователя
 * Email - email пользователя
 * Password - пароль пользователя
 * Role - роль пользователя
 */
public class Registration
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }

}