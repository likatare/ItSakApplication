using System;
using System.Security.Cryptography;

namespace ITSakApp
{
    class Cryptography
    {
        private RSACryptoServiceProvider csp { get; set; }

        public Cryptography()
        {
            //lets take a new CSP with a new 2048 bit rsa key pair
            csp = new RSACryptoServiceProvider(2048);
        }

        // Returns the public key as a string
        public string GetPublicKey()
        {
            RSAParameters pubKey = csp.ExportParameters(false);

            string publicKeyString = GetStringFromRSAParameters(pubKey);

            return publicKeyString;
        }

        // Returns the private key as a string
        public string GetPrivateKey()
        {
            RSAParameters privKey = csp.ExportParameters(true);

            string privateKeyString = GetStringFromRSAParameters(privKey);

            return privateKeyString;
        }

        // Returns the RSA parameters as an XML string
        private string GetStringFromRSAParameters(RSAParameters key)
        {
            //converting the key into a string representation
            string keyString;
            {
                //we need some buffer
                var sw = new System.IO.StringWriter();
                //we need a serializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //serialize the key into the stream
                xs.Serialize(sw, key);
                //get the string from the stream
                keyString = sw.ToString();
            }

            return keyString;
        }

        // Returns the input message as an encrypted string
        public string Encrypt(string plainTextData)
        {
            //for encryption, always handle bytes...
            byte[] bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(plainTextData);

            //apply pkcs#1.5 padding and encrypt our data 
            byte[] bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

            //we might want a string representation of our cypher text... base64 will do
            string cypherText = Convert.ToBase64String(bytesCypherText);

            return cypherText;
        }

        // Decrypts and returns the cypherText as a string
        public string Decrypt(string cypherText)
        {
            //first, get our bytes back from the base64 string ...
            byte[] bytesCypherText = Convert.FromBase64String(cypherText);

            //decrypt and strip pkcs#1.5 padding
            byte[] bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

            //get our original plainText back...
            string plainTextData = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);

            return plainTextData;
        }

        // Returns the input key as RSA parameters
        private RSAParameters GetRSAParametersFromString(string keyString)
        {
            RSAParameters key;
            //converting it back
            {
                //get a stream from the string
                var sr = new System.IO.StringReader(keyString);
                //we need a deserializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //get the object back from the stream
                key = (RSAParameters)xs.Deserialize(sr);
            }

            return key;
        }

        public void SetKey(string keyString)
        {
            RSAParameters key = GetRSAParametersFromString(keyString);
            csp.ImportParameters(key);
        }
    }
}