
using ZT_StockMNG.ViewModel;

namespace ZT_StockMNG.View.StockArticle;

public partial class NewStockArticle : ContentPage
{
    public NewStockArticle(NewStockArticleViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}