using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ZT_StockMNG.Model;
using ZT_StockMNG.ViewModel;

namespace ZT_StockMNG
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}