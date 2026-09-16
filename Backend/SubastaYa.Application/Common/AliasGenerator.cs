using System.Security.Cryptography;
using System.Text;

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
