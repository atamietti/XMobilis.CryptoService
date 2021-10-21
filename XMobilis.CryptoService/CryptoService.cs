using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace XMobilis.CryptoService
{
    public class CryptoService : ICryptoService
    {

        public string CipherKey { get; set; }

        public CryptoService(string cryptokey, int blockSize = 256)
        {
            this.CipherKey = cryptokey;
            StringCipher.Keysize = blockSize;
        }

        public void Encrypt<T>(T element, string key = "") where T : CryptoModel
        {
            var properties = element.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(EncryptedAttribute), true));

            Parallel.ForEach(properties, (propInfo) =>
               {
                   var attr = (EncryptedAttribute)propInfo.GetCustomAttributes(true).FirstOrDefault(f => f is EncryptedAttribute);
                   var propvalue = propInfo.GetValue(element, null);

                   if (propvalue != null && propvalue is CryptoModel[] basearray)
                       Parallel.ForEach(basearray, (itemArray) => Encrypt(itemArray, key));

                   else if (propvalue != null && propvalue is List<CryptoModel> listRequest)
                       Parallel.ForEach(listRequest, (itemArray) => Encrypt(itemArray, key));

                   else if (propvalue != null && propvalue is List<string> listObjectRequest)
                       Parallel.ForEach(listObjectRequest, (itemArray) => EncryptElement(element, propInfo, attr, itemArray, key, listObjectRequest.ToArray()));

                   else if (propvalue != null && propvalue is CryptoModel baseaRequest)
                       Encrypt(baseaRequest, key);
                   else if (!string.IsNullOrWhiteSpace(propvalue?.ToString()) && propvalue is object)
                       EncryptElement(element, propInfo, attr, propvalue?.ToString(), key);
               });
        }

        public void EncryptElement<T>(T element, PropertyInfo propInfo, EncryptedAttribute encAttribute, object valor, string key = "", object[] index = null)
        {

            try
            {
                string cryptoKey;
                if (string.IsNullOrWhiteSpace(key)) //search for the cryptokey to be used 
                {
                    if (!string.IsNullOrWhiteSpace(encAttribute.CipherKey)) //1º search in the attribute the informed value
                        cryptoKey = encAttribute.CipherKey;
                    else                                              //2º set the value returned in the request as the next key
                        cryptoKey = CipherKey;
                }
                else //takes the value entered with encryption key
                    cryptoKey = key;

                var encryptedValue = valor.ToString().Encrypt(cryptoKey);

                if (index == null)
                    propInfo.SetValue(element, encryptedValue);
                else
                    propInfo.SetValue(element, encryptedValue, index);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Decrypt<T>(T element, string key = "") where T : CryptoModel
        {
            var properties = element.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(EncryptedAttribute), true));

            Parallel.ForEach(properties, (propInfo) =>
             {
                 var attr = (EncryptedAttribute)propInfo.GetCustomAttributes(true).FirstOrDefault(f => f is EncryptedAttribute);
                 var propvalue = propInfo.GetValue(element, null);

                 if (propvalue != null && propvalue is CryptoModel[] basearray)
                     Parallel.ForEach(basearray, (itemArray) => Decrypt(itemArray, key));

                 else if (propvalue != null && propvalue is List<CryptoModel> listRequest)
                     Parallel.ForEach(listRequest, (itemArray) => DencryptElement(element, propInfo, attr, itemArray, key));

                 else if (propvalue != null && propvalue is List<string> listObjectRequest)
                     Parallel.ForEach(listObjectRequest, (itemArray) => DencryptElement(element, propInfo, attr, itemArray, key, listObjectRequest));

                 else if (propvalue != null && propvalue is CryptoModel baseaRequest)
                     Decrypt(baseaRequest, key);

                 else if (!string.IsNullOrWhiteSpace(propvalue?.ToString()) && propvalue is object)
                     DencryptElement(element, propInfo, attr, propvalue?.ToString(), key);


             });
        }

        public void DencryptElement<T>(T element, PropertyInfo propInfo, EncryptedAttribute encAttribute, object valor, string key = "", List<string> index = null)
        {
            string cryptoKey;
            if (string.IsNullOrWhiteSpace(key)) //search for the cryptokey to be used 
            {
                if (!string.IsNullOrWhiteSpace(encAttribute.CipherKey)) // 1º procura no valor informado no atributo
                    cryptoKey = encAttribute.CipherKey;
                else  //2º set the value returned in the request as the next key
                    cryptoKey = CipherKey;
            }
            else //takes the value entered with encryption key
                cryptoKey = key;

            var decryptedValue = valor.ToString().Decrypt(cryptoKey);

            if (index == null)
                propInfo.SetValue(element, decryptedValue);
            else
            {
                var nlist = index.IndexOf(valor.ToString());
                index[nlist] = decryptedValue;
                propInfo.SetValue(element, index);
            }
        }

    }
}
