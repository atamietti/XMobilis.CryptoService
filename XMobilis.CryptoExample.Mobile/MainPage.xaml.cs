using Xamarin.Forms;
using XMobilis.CryptoExample.Mobile.VM;

namespace XMobilis.CryptoExample.Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new CipherVM();
        }
    }
}
