using System;

namespace XMobilis.CryptoService
{
    public class EncryptedAttribute : Attribute
    {
        private string _cipherKey;
        public string CipherKey
        {
            get { return this._cipherKey; }
            set { this._cipherKey = value; }
        }

        public EncryptedAttribute(string cipherKey = "")
        {
            CipherKey = cipherKey;
        }   
    }
}
