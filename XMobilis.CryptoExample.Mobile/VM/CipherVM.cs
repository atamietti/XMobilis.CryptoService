using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using XMobilis.CryptoExample.Model;

namespace XMobilis.CryptoExample.Mobile.VM
{
    public class CipherVM : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isEncrypted;

        private User _user;

        public User U
        {
            set
            {
                _user = value;
                OnPropertyChanged(nameof(U));
            }
            get => _user;

        }

        private string _cipherKey;

        public string CipherKey
        {
            set
            {
                _cipherKey = value;
                OnPropertyChanged(nameof(CipherKey));
            }

            get => _cipherKey;
        }

        private CryptoService.CryptoService _cipherService;

        public CryptoService.CryptoService CipherService => _cipherService ??= new CryptoService.CryptoService(_cipherKey, 128);


        private ICommand _keyCommand;
        public ICommand KeyCommand => _keyCommand ??= new Command(
              execute: () =>
              {
                  this.CipherKey = Guid.NewGuid().ToString();
                  this.CipherService.CipherKey = CipherKey;

              }, canExecute: () =>
              {
                  return !_isEncrypted;
              });

        private ICommand _cipherCommand;
        public ICommand CipherCommand => _cipherCommand ??= new Command(
               execute: () =>
               {

                   _isEncrypted = !_isEncrypted;
                   RefreshCanExecutes();

                   if (_isEncrypted)
                       this.CipherService.Encrypt(this.U);
                   else
                       this.CipherService.Decrypt(this.U);

                   OnPropertyChanged(nameof(U));
               });

        public CipherVM()
        {
            U = new User() { Login = "xMobilis", Name = "Crypto Example", Password = "password@123!" };
            CipherKey = Guid.NewGuid().ToString(); ;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void RefreshCanExecutes()
        {
            (KeyCommand as Command).ChangeCanExecute();

        }

    }

}
