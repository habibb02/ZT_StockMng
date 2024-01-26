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
        private const string path = "Data/Material.json";

        private ObservableCollection<Article> _material = new();
        public ObservableCollection<Article> Material
        {
            get => _material;
            set => _material = value;
        }

        public ObservableCollection<Article> StockArticles { get; set; } = new();

        public MainPageViewModel()
        {
            Title = "Magazzino";

            GetMaterialCommand.Execute(this);
        }

        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]
        async Task GetMaterialAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;

                if (Material.Count > 0)
                {
                    if (StockArticles.Count == Material.Count)
                        return;

                    StockArticles.Clear();

                    foreach (var article in Material)
                        StockArticles.Add(article);

                    return;
                }

                //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\Material.json");  //Funziona
                //string path = "Data/Material.json";

                using var stream = await FileSystem.OpenAppPackageFileAsync(path);
                using var reader = new StreamReader(stream);

                var json = reader.ReadToEnd();

                //var json = File.ReadAllText(path);

                List<Article> materialList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Article>>(json);

                foreach (var article in materialList)
                {
                    Material.Add(article);
                    StockArticles.Add(article);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", $"Impossibile caricare il materiale: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        async Task SearchArticleAsync(string text)
        {
            try
            {
                IsBusy = true;

                var list = new ObservableCollection<Article>(Material.Where(m => m.ArticleCode.ToLower().Contains(text.ToLower())));

                if (Material.Count == list.Count)
                    return;

                StockArticles.Clear();

                foreach (var article in list)
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
            }
        }

        [RelayCommand]
        async Task SaveAsync()
        {
            try
            {
                IsBusy = true;

                if (StockArticles.Count == 0)
                    await Shell.Current.DisplayAlert("Error", "Non ci sono elementi da modificare.", "Ok");

                if (Material.Count == 0)
                    throw new Exception("Non ci sono elementi.");

                using var stream = await FileSystem.OpenAppPackageFileAsync(path);
                using var reader = new StreamReader(stream);

                var json = reader.ReadToEnd();

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
    }
}
