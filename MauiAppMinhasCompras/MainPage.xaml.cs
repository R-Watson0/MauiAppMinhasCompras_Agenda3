using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras
{
    public partial class MainPage : ContentPage
    {
        private SQLiteDatabaseHelper database;

        public MainPage()
        {
            InitializeComponent();

            database = App.Db;

            CarregarProdutos();
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

            await CarregarProdutos();

            await DisplayAlert(
                "Sucesso",
                "Produto cadastrado.",
                "OK"
            );
        }

        private async Task CarregarProdutos()
        {
            listaProdutos.ItemsSource = await database.GetAll();
        }
    }
}