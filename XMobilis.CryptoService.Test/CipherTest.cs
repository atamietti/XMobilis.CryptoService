using System;
using XMobilis.CryptoExample.Model;
using Xunit;

namespace XMobilis.CryptoService.Test
{
    public class CipherTest
    {
        string key;
        CryptoService cService;

        [Fact]
        public void Test1()
        {
            //Cipher Key Setup Encrypt/Decrypt 1
            key = Guid.NewGuid().ToString();
            cService = new CryptoService(key, 128);

            //Initial Model
            var modelTest = new ModelTest()
            {
                Login = "atamietti",
                Name = "Andre",
                Password = "0@011#222&3&33ad444",
                Model2 = new ModelTest() { Login = "jseph", Name = "Joseph", Password = "$31$5@188972$2020@@" }
            };

            var pwd = modelTest.Password;
            var name = modelTest.Name;
            var modelTestName = modelTest.Model2.Name;

            //Model Encrypt 1
            cService.Encrypt(modelTest);
            var ePwd1 = modelTest.Password;
            var eName1 = modelTest.Name;
            var eModelTestName1 = modelTest.Model2.Name;

            //Model Decrypt 1
            cService.Decrypt(modelTest);
            var dPwd1 = modelTest.Password;
            var dName1 = modelTest.Name;
            var dModelTestName1 = modelTest.Model2.Name;

            //Cipher Key Setup Encrypt/Decrypt 2
            SetupNewKey(Guid.NewGuid().ToString());

            //Encrypt 2
            cService.Encrypt(modelTest);
            var ePwd2 = modelTest.Password;
            var eName2 = modelTest.Name;
            var eModelTestName2 = modelTest.Model2.Name;

            //Decrypt 2
            cService.Decrypt(modelTest);
            var dPwd2 = modelTest.Password;
            var dName2 = modelTest.Name;
            var dModelTestName2 = modelTest.Model2.Name;

            //The two encrypted values must be different; Once values were used by different keys.
            Assert.NotEqual(ePwd1, ePwd2);
            Assert.NotEqual(eName1, eName2);
            Assert.NotEqual(eModelTestName1, eModelTestName2);

            //Decrypted 1 - values must be equals to the originals 
            Assert.Equal(pwd, dPwd1);
            Assert.Equal(name, dName1);
            Assert.Equal(modelTestName, dModelTestName1);


            //Decrypted 2 - values must be equals to the originals
            Assert.Equal(pwd, dPwd2);
            Assert.Equal(name, dName2);
            Assert.Equal(modelTestName, dModelTestName2);



        }


        private void SetupNewKey(string newkey)
        {
            if (!string.IsNullOrWhiteSpace(newkey))
                cService.CipherKey = newkey;
        }
    }

    public class ModelTest : User
    {

        [Encrypted] //This class properties will be Cipher
        public override string Login { get => base.Login; set => base.Login = value; }

        [Encrypted] //This class properties will be Cipher
        public User Model2 { get; set; }

    }

}
