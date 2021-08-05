using DataAccessLayer.Entities;
using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common
{
    public static class Security
    {
        public static string HashSHA1WithSalt(string plainTextString, string salt)
        {
            HashAlgorithm Algorithm = new SHA1Managed();
            var SaltBytes = Encoding.ASCII.GetBytes(salt);
            var PlainTextBytes = Encoding.ASCII.GetBytes(plainTextString);

            var PlainTextWithSaltBytes = AppendByteArray(PlainTextBytes, SaltBytes);
            var SaltedSHA1Bytes = Algorithm.ComputeHash(PlainTextWithSaltBytes);
            var SaltedSHA1WithAppendedSaltBytes = AppendByteArray(SaltedSHA1Bytes, SaltBytes);

            return Convert.ToBase64String(SaltedSHA1WithAppendedSaltBytes);
        }
        private static byte[] AppendByteArray(byte[] byteArray1, byte[] byteArray2)
        {
            var byteArrayResult =
                    new byte[byteArray1.Length + byteArray2.Length];

            for (var i = 0; i < byteArray1.Length; i++)
                byteArrayResult[i] = byteArray1[i];
            for (var i = 0; i < byteArray2.Length; i++)
                byteArrayResult[byteArray1.Length + i] = byteArray2[i];

            return byteArrayResult;
        }

        public static User AuthenticateUser(BaatCheetDbContext dbContext, int userId, string token)
        {
           return dbContext.Users.FirstOrDefault(x => x.Id == userId && x.Token == getToken(token));
        }

        public static string getToken(string token)
        {
            if (AuthenticationHeaderValue.TryParse(token, out var headerValue))
                return headerValue.Parameter;
            return null;
        }
    }
}
