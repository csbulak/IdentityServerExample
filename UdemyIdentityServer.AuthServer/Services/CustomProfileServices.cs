﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using UdemyIdentityServer.AuthServer.Repository;

namespace UdemyIdentityServer.AuthServer.Services
{
    public class CustomProfileServices : IProfileService
    {
        private readonly ICustomUserRepository _customUserRepository;

        public CustomProfileServices(ICustomUserRepository customUserRepository)
        {
            _customUserRepository = customUserRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subId = context.Subject.GetSubjectId();

            var user = await _customUserRepository.FindById(int.Parse(subId));

            var claim = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("name",user.UserName),
                new Claim("city",user.City)
            };

            if (user.Id == 1)
            {
                claim.Add(new Claim("role", "admin"));
            }
            else
            {
                claim.Add(new Claim("role", "customer"));
            }

            context.AddRequestedClaims(claim);
            //context.IssuedClaims = claim; // Uygun değildir. Token içinde görmek için kullanılabilir.
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userId = context.Subject.GetSubjectId();

            var user = await _customUserRepository.FindById(Convert.ToInt32(userId));

            context.IsActive = user != null;
        }
    }
}