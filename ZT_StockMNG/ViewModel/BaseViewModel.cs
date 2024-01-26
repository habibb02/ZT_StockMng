using CommunityToolkit.Mvvm.ComponentModel;
using ZT_StockMNG.Service;

namespace ZT_StockMNG.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        protected RestService _svc;

        public BaseViewModel()
        {
            _svc = new RestService();
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(isNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        public bool isNotBusy => !IsBusy;
    }
}
