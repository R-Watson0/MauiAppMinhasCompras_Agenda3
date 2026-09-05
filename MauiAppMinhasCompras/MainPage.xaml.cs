using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras
{
    public partial class MainPage : ContentPage
    {
        private SQLiteDatabaseHelper database;

        private ObservableCollection<Produto> produtos = new ObservableCollection<Produto>();

        public MainPage()
        {
            InitializeComponent();

            database = App.Db;

            listaProdutos.ItemsSource = produtos;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await CarregarProdutos();
        }

        private async void OnCadastrarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                await DisplayAlert(
                    "Atenção",
                    "Digite a descrição do produto.",
                    "OK"
                );

                return;
            }

            if (!double.TryParse(txtQuantidade.Text, out double quantidade))
            {
                await DisplayAlert(
                    "Atenção",
                    "Digite uma quantidade válida.",
                    "OK"
                );

                return;
            }

            if (!double.TryParse(txtPreco.Text, out double preco))
            {
                await DisplayAlert(
                    "Atenção",
                    "Digite um preço válido.",
                    "OK"
                );

                return;
            }

            Produto produto = new Produto
            {
                Descricao = txtDescricao.Text,
                Quantidade = quantidade,
                Preco = preco
            };

            await database.Insert(produto);

            txtDescricao.Text = "";
            txtQuantidade.Text = "";
            txtPreco.Text = "";

            searchBar.Text = "";

            await CarregarProdutos();

            await DisplayAlert(
                "Sucesso",
                "Produto cadastrado.",
                "OK"
            );
        }

        private async Task CarregarProdutos()
        {
            List<Produto> lista = await database.GetAll();

            produtos.Clear();

            foreach (Produto produto in lista)
            {
                produtos.Add(produto);
            }
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string textoBusca = e.NewTextValue;

            List<Produto> resultado;

            if (string.IsNullOrWhiteSpace(textoBusca))
            {
                resultado = await database.GetAll();
            }
            else
            {
                resultado = await database.Search(textoBusca);
            }

            produtos.Clear();

            foreach (Produto produto in resultado)
            {
                produtos.Add(produto);
            }
        }
    }
}