using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid
{
    class EncryptedPacket
    {
        public byte[] RsaEncryptedSessionKey;
        public byte[] AesEncryptedData;
        public byte[] IvForAes; //MAG DEZE MEEVERSTUURD WORDEN IN DE PACKET ???
        public byte[] RsaSignedHash;
    }
}
