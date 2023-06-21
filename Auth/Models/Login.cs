namespace KPO_hw.Models;

/*
 * Класс полей входа пользователя для метода Post в LoginController
 * Email - email пользователя
 * Password - пароль пользователя
 */
public class Login
{
    public string? Email { get; init; }
    public string? Password { get; init; }
}