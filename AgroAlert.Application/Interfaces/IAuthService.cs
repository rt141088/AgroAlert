using AgroAlert.Application.DTOs;
namespace AgroAlert.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    Task<AgricultorDTO?> RegisterAsync(RegisterRequest request);
}
