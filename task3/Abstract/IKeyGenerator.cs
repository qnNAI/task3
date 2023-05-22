using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task3.Abstract {
    internal interface IKeyGenerator {
        
        public byte[] GenerateKey();
        public byte[] GenerateHmac(byte[] key, string value);
    }
}
