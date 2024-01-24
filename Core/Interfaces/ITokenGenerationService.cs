﻿using System.Security.Claims;

namespace Core.Interfaces
{
    public interface ITokenGenerationService
    {
        string GenerateToken(IEnumerable<Claim> claims, int expiryInMinutes = 60);
    }
}

