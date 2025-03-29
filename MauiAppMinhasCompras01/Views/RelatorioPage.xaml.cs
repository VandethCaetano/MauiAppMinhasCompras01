using MauiAppMinhasCompras01.Models;

namespace MauiAppMinhasCompras01.Views
{
    public partial class RelatorioPage : ContentPage
    {
        public RelatorioPage()
        {
            InitializeComponent();
            CarregarRelatorio();
        }

        private async void CarregarRelatorio()
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

            collectionRelatorio.ItemsSource = relatorio;
        }
    }
}
