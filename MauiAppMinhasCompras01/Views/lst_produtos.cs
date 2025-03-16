using MauiAppMinhasCompras01.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras01.Views
{
    internal class ListaProdutos
    {
        public static ObservableCollection<Produto> ItemsSource { get; set; } = new ObservableCollection<Produto>();

        public static void AdicionarProduto(Produto produto)
        {
            if (produto != null)
            {
                ItemsSource.Add(produto);
            }
        }

        public static void RemoverProduto(Produto produto)
        {
            if (produto != null && ItemsSource.Contains(produto))
            {
                ItemsSource.Remove(produto);
            }
        }
    }
}
