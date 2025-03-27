﻿using System.Security.Claims;
using ApiOwn.Models;

namespace ApiOwn.Extensions;

public static class RoleClaimsExtension
{
    public static IEnumerable<Claim> GetClaims(this User user)
    {
        var result = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Email)
        };
        result.AddRange(user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Slug)));
        return result;
        //
    }
}