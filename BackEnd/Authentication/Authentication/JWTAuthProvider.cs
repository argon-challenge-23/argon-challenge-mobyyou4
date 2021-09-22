using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Infra.Authentication
{
    public static class JWTAuthProvider
    {
        private static readonly string _appID = "139265";
        private const string pemPath = "<FilePath>";
        
        /// <summary>
        /// Get JWT encrypted token by private key
        /// TODO: replace basic auth to github app JWT token auth
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
        {
            string privateKey = File.ReadAllText(pemPath);

            var token = CreateToken(privateKey);
            Console.WriteLine(token);
            return token;
        }

        public static string CreateToken(string privateRsaKey)
        {
            RSAParameters rsaParams;
            using (var tr = new StringReader(privateRsaKey))
            {
                var pemReader = new PemReader(tr);
                var keyPair = pemReader.ReadObject() as AsymmetricCipherKeyPair;
                if (keyPair == null)
                {
                    throw new Exception("Could not read RSA private key");
                }
                var privateRsaParams = keyPair.Private as RsaPrivateCrtKeyParameters;
                rsaParams = DotNetUtilities.ToRSAParameters(privateRsaParams);
            }
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                return Jose.JWT.Encode(GetJWTPayload(), rsa, Jose.JwsAlgorithm.RS256);
            }
        }
        private static Dictionary<string, object> GetJWTPayload()
        {
            return new Dictionary<string, object>() {
                { "exp", DateTimeOffset.UtcNow.AddMinutes(9).ToUnixTimeSeconds()},
                { "iat", DateTimeOffset.UtcNow.AddMinutes(1).ToUnixTimeSeconds()},
                { "iss", _appID }
            };
        }
    }
}
