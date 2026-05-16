//using CineReserve.API.Data;
//using CineReserve.API.DTOs;
//using CineReserve.API.Models;

//namespace CineReserve.API.Services
//{
//    public class AuthService
//    {
//        private readonly AppDbContext _context;
//        private readonly PasswordService _passwordService;
//        private readonly JwtService _jwtService;

//        public AuthService(AppDbContext context, PasswordService passwordService, JwtService jwtService)
//        {
//            _context = context;
//            _passwordService = passwordService;
//            _jwtService = jwtService;
//        }

//        public async Task<string> Register(RegisterDto dto)
//        {
//            var user = new User
//            {
//                Name = dto.Name,
//                Email = dto.Email,
//                PasswordHash = _passwordService.HashPassword(dto.Password),
//                Role = "User",
//                CreditBalance = 1000
//            };

//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();

//            return "User Registered";
//        }

//        public async Task<string> Login(LoginDto dto)
//        {
//            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

//            if (user == null)
//                throw new Exception("Invalid Email");

//            if (!_passwordService.VerifyPassword(dto.Password, user.PasswordHash))
//                throw new Exception("Invalid Password");

//            return _jwtService.GenerateToken(user);
//        }
//    }
//}


using CineReserve.API.Data;
using CineReserve.API.DTOs;
using CineReserve.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CineReserve.API.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly JwtService _jwtService;

        public AuthService(AppDbContext context, PasswordService passwordService, JwtService jwtService)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        public async Task<string> Register(RegisterDto dto, string role = "User")
        {
            // Fail early if email already exists
            var existingUser = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (existingUser)
                throw new Exception("Email already registered.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = _passwordService.HashPassword(dto.Password),
                Role = string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase) ? "Admin" : "User",
                CreditBalance = 1000 // Starting mock balance
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "User Registered Successfully";
        }

        public async Task<string> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                throw new Exception("Invalid Email or Password");

            if (!_passwordService.VerifyPassword(dto.Password, user.PasswordHash))
                throw new Exception("Invalid Email or Password");

            return _jwtService.GenerateToken(user);
        }
    }
}
