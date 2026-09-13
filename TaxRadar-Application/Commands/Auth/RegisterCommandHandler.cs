using AutoMapper;
using MediatR;
using Tax_Radar_Domain.Entities;
using TaxRadar_Application.DTOs.Users;
using TaxRadar_Application.Exceptions;
using TaxRadar_Application.Interfaces;
using TaxRadar_Application.Queries.Auth;

namespace TaxRadar_Application.Commands.Auth;

public class RegisterCommandHandler(IUserRepository repository, IPasswordHasher passwordHasher, IMapper mapper):IRequestHandler<RegisterQuery, UserDto>
{
    public async Task<UserDto> Handle(RegisterQuery request, CancellationToken cancellationToken)
    {
        var isExist = await repository.CheckIfUserExistByEmail(request.Email,cancellationToken);
        if (isExist) throw new BadRequestException("The user with this email is already existig");
        var hashedPassword = passwordHasher.Hash(request.Password);
        var newUser = new User(request.Email,request.FullName,hashedPassword);
        await repository.AddAsync(newUser, cancellationToken);
        return mapper.Map<UserDto>(newUser);
        
    }
}