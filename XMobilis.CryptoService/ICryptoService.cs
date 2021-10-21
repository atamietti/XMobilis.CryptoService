namespace XMobilis.CryptoService
{
    public interface ICryptoService
    {

        string CipherKey { get; set; }
        void Encrypt<T>(T element, string key = "") where T : CryptoModel;
        void Decrypt<T>(T element, string key = "") where T : CryptoModel;
    }
}
