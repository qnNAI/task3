using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using task3.Abstract;

namespace task3.Services {

    public class KeyGenerator : IKeyGenerator {

        public byte[] GenerateKey() {
            byte[] key = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(key);
            return key;
        }

        public byte[] GenerateHmac(byte[] key, string value) {
            using var hmac = new HMACSHA256(key);
            return hmac.ComputeHash(Encoding.ASCII.GetBytes(value));
        }
    }
}
