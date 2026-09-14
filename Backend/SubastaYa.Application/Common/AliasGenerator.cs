using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SubastaYa.Application.Common
{
    public static class AliasGenerator
    {
        public static string Generate(int auctionId, int buyerId)
        {
            var input = $"{auctionId}-{buyerId}";
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = SHA256.HashData(inputBytes);
            var hashString = Convert.ToHexString(hashBytes);

            return $"Postor-{hashString[..6]}";
        }
    }
}
