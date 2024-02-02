using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kotlin.Properties;
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
        public string ArticlesSearchBar;

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
        async Task SaveAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;


                //if (StockArticles.Count == 0)
                //    await Shell.Current.DisplayAlert("Error", "Non ci sono elementi da modificare.", "Ok");

                //if (Material.Count == 0)
                //    throw new Exception("Non ci sono elementi.");

                //using var stream = await FileSystem.OpenAppPackageFileAsync(path);
                //using var reader = new StreamReader(stream);

                //var json = reader.ReadToEnd();



                await Shell.Current.DisplayAlert("Success", "Le quantità sono state aggiornate con successo!", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", $"Si è verificato un errore durante il salvataggio delle quantità: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
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

                var canExecute = await Shell.Current.DisplayAlert("Elimina", "Se sicuro di voler eliminare l'articolo selezionato?", "Si", "Annulla");

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
                if (article == null)
                    throw new Exception("L'articolo selezionato non risulta a sistema");

                string lastQuantity = Convert.ToString(article.Qty);
                string newQuantityStr = await Shell.Current.DisplayPromptAsync($"Articolo:{article.ArticleCode}", "Quantità", "Salva", "Annulla", lastQuantity, keyboard: Keyboard.Numeric, initialValue: lastQuantity);

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
