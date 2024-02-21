using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using ZT_StockMNG.Model;

namespace ZT_StockMNG.ViewModel
{
    public partial class MainPageViewModel : BaseViewModel
    {
        public string ArticlesSearchBar { get; set; } = string.Empty;

        private ObservableCollection<Article> _stockArticles = new();
        public ObservableCollection<Article> StockArticles { get => _stockArticles; set => _stockArticles = value; }

        public MainPageViewModel()
        {
            Title = "Magazzino";
        }

        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]
        async Task GetAllArticlesList()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                StockArticles.Clear();

                var articlesList = await _svc.GetArticlesAsync();

                foreach (var item in articlesList)
                    StockArticles.Add(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                await Shell.Current.DisplayAlert("Error", $"Impossibile caricare gli articoli: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task SearchList(string text)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                IsRefreshing = true;

                StockArticles.Clear();

                var filteredList = await _svc.GetArticlesByAsync(text);

                foreach (var article in filteredList)
                    StockArticles.Add(article);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", $"Errore ricerca articolo: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        async Task RemoveArticle(Article article)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var canExecute = await Shell.Current.DisplayAlert("Elimina", "Sei sicuro di voler eliminare l'articolo selezionato?", "Si", "Annulla");

                if (!canExecute)
                    return;

                await _svc.RemoveArticleAsync(article.ArticleCode);

                await Shell.Current.DisplayAlert("Successo", "L'articolo selezionato è stato eliminato con successo!", "Ok");

                IsBusy = false;

                await SearchList(ArticlesSearchBar);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", $"Errore rimozione articolo: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task ModifyArticleQuantity(Article article)
        {
            if (IsBusy)
                return;

            try
            {
                string lastQuantity = Convert.ToString(article.Qty);
                string newQuantityStr = await Shell.Current.DisplayPromptAsync($"Articolo: {article.ArticleCode}", "Modifica quantità", "Salva", "Annulla", lastQuantity, keyboard: Keyboard.Numeric, initialValue: lastQuantity);

                if (string.IsNullOrEmpty(newQuantityStr))
                    return;

                IsBusy = true;

                article.Qty = Convert.ToDecimal(newQuantityStr);

                await _svc.ModifyArticleAsync(article);

                await Shell.Current.DisplayAlert("Successo", "L'articolo è stato modficato con successo", "Ok");

                IsBusy = false;

                await SearchList(ArticlesSearchBar);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", $"Errore modifica quantità articolo: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
