using MauiAppMinhasCompras01.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras01.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
    string textoBuscaAtual = string.Empty;

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        lista.Clear();

        List<Produto> tmp = await App.Db.GetAll();
        lista.Clear();
        tmp.ForEach(i => lista.Add(i));

        // Preenche o Picker com categorias únicas
        var categorias = tmp
            .Select(p => p.Categoria)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct()
            .ToList();

        categorias.Insert(0, "Todas");
        picker_categoria.ItemsSource = categorias;
        picker_categoria.SelectedIndex = 0;
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
        textoBuscaAtual = e.NewTextValue;
        await AplicarFiltros();
    }

    private async void picker_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        await AplicarFiltros();
    }

    private async Task AplicarFiltros()
    {
        string termo = textoBuscaAtual?.ToLower() ?? "";
        string categoriaSelecionada = picker_categoria.SelectedItem?.ToString();

        List<Produto> resultados = await App.Db.GetAll();

        if (!string.IsNullOrWhiteSpace(categoriaSelecionada) && categoriaSelecionada != "Todas")
        {
            resultados = resultados
                .Where(p => p.Categoria?.ToLower() == categoriaSelecionada.ToLower())
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(termo))
        {
            resultados = resultados
                .Where(p => p.Descricao?.ToLower().Contains(termo) == true)
                .ToList();
        }

        lista.Clear();
        resultados.ForEach(p => lista.Add(p));
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;
            Produto p = selecionado.BindingContext as Produto;
            bool confirm = await DisplayAlert("Tem certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;
            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }


    private async void ToolbarItem_Clicked_3(object sender, EventArgs e)
    {   
            List<Produto> todos = await App.Db.GetAll();

            var relatorio = todos
                .GroupBy(p => p.Categoria)
                .Select(g => new
                {
                    Categoria = string.IsNullOrWhiteSpace(g.Key) ? "Sem Categoria" : g.Key,
                    Total = g.Sum(p => p.Total)
                })
                .OrderBy(r => r.Categoria)
                .ToList();

            string textoRelatorio = string.Join(Environment.NewLine,
                relatorio.Select(r => $"?? {r.Categoria}: {r.Total:C}"));

            await DisplayAlert("Total por Categoria", textoRelatorio, "OK");
    }  
        private async void AbrirRelatorio_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RelatorioPage());
        }

    
}
