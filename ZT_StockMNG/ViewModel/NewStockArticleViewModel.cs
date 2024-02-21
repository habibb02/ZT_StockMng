using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZT_StockMNG.Model;

namespace ZT_StockMNG.ViewModel
{
    public partial class NewStockArticleViewModel : BaseViewModel
    {
        public Article NewArticleModel { get; private set; } = new();

        public NewStockArticleViewModel()
        {
            Title = "Nuovo Articolo";
        }

        [RelayCommand]
        async Task AddStockArticle()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                if(NewArticleModel.ArticleCode == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Codice articolo obbligatorio!", "Ok");
                    return;
                }

                await _svc.AddArticleAsync(NewArticleModel);

                await Shell.Current.DisplayAlert("Success", $"l'articolo {NewArticleModel.ArticleCode} è stato aggiunto con successo!", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", $"{ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
