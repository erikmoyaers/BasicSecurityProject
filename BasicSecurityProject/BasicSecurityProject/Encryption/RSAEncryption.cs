using BasicSecurityProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid
{
    public class RSAEncryption : IRSAEncryption
    {
        //generate keypair in specific folder
        public void GenerateKeysInFile(string folder, string publicKeyName, string privateKeyName)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false; //--> we willen de key container niet gebruiken

                var privateKeyFile = File.Create(folder + "/" + privateKeyName);
                privateKeyFile.Close();
                RSAParameters privateKey = rsa.ExportParameters(true);
                
                var publicKeyFile = File.Create(folder + "/" + publicKeyName);
                publicKeyFile.Close();
                RSAParameters publicKey = rsa.ExportParameters(false);
                File.WriteAllText(folder + "/" + privateKeyName, convertKeyToString(rsa.ExportParameters(true)));
                File.WriteAllText(folder + "/" + publicKeyName, convertKeyToString(rsa.ExportParameters(false)));
            }
        }

        //RSA encryption
        public byte[] EncryptData(byte[] dataToEncrypt, RSAParameters publicKey)
        {
            byte[] cipherbytes;

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(publicKey);

                cipherbytes = rsa.Encrypt(dataToEncrypt, false); //2de parameter = fOAEP --> zorgt voor "extra randomness": staap op false, anders error
            }

            return cipherbytes;
        }

        //RSA decryption
        public byte[] DecryptData(byte[] dataToDecrypt, RSAParameters privateKey)
        {
            byte[] plain;

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;

                rsa.ImportParameters(privateKey);
                plain = 
                    rsa.Decrypt(dataToDecrypt, false); // alweer dat "extra randomness" element
            }

            return plain;
        }

        //signing of data
        public byte[] SignData(byte[] hashOfDataToSign, RSAParameters privateKey)
        {
            using(var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(privateKey);

                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");

                return rsaFormatter.CreateSignature(hashOfDataToSign);
            }
        }

        //verification of signature
        public bool VerifySignature(byte[] hashOfSignedData, byte[] signature, RSAParameters publicKey)
        {
            using(var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.ImportParameters(publicKey);

                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA256");

                return rsaDeformatter.VerifySignature(hashOfSignedData, signature);
            }
        }

        //conversion of key to XML string
        public static string convertKeyToString(RSAParameters key)
        {
            //we need some buffer
            var stringWriter = new StringWriter();
            //we need a serializer
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //serialize the key into the stream
            serializer.Serialize(stringWriter, key);
            //get the string from the stream
            return stringWriter.ToString();
        }

        
        //conversion of XML string to key
        public static RSAParameters convertStringToKey(string keyAsString)
        {
            //get a stream from the string
            var stringReader = new StringReader(keyAsString);
            //we need a deserializer
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //get the object back from the stream
            return (RSAParameters)serializer.Deserialize(stringReader);
        }
        
    }
}
