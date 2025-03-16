using MauiAppMinhasCompras01.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras01.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        List<Produto> tmp = await App.Db.GetAll();
        lista.Clear();
        tmp.ForEach(i => lista.Add(i));
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NovoProduto());
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);
        string msg = $"O total é: {soma:C}";
        DisplayAlert("Total dos produtos:", msg, "OK");
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        string q = e.NewTextValue;
        lista.Clear();
        List<Produto> tmp = await App.Db.Search(q);
        lista.Clear();
        tmp.ForEach(i => lista.Add(i));

    }
}
