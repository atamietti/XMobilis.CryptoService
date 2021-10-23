using System;
using XMobilis.CryptoService;

namespace XMobilis.CryptoExample.Model
{
    public class User : CryptoModel
    {

        public virtual string Login { get; set; } //this property wont be able to cipher

        [Encrypted] //this proerty will be cipher by the current CryptoRequestKey Value
        public string Password { get; set; }

        [Encrypted("keyToCipher")] //this property will be Cipher by this value: "keyToCipher"
        public string Name { get; set; }

        public DateTime Date { get; set; } // for now it is only for string and classess

    }
}
