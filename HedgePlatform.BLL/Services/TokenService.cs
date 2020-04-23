using AutoMapper;
using HedgePlatform.BLL.DTO;
using HedgePlatform.BLL.Interfaces;
using System;
using System.Security.Cryptography;

namespace HedgePlatform.BLL.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateToken()
        {
            return GenerateSeqString(32);
        }

        public string GenerateUid()
        {
            return GenerateSeqString(16);
        }

        private string GenerateSeqString(int len)
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[len];
                rng.GetBytes(tokenData);
                string code_str = Convert.ToBase64String(tokenData);
                return code_str;
            }
        }
    }
}
