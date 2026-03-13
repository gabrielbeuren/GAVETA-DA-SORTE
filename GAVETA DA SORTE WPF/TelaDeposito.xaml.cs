using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GAVETA_DA_SORTE_WPF
{
    /// <summary>
    /// Lógica interna para TelaDeposito.xaml
    /// </summary>
    public partial class TelaDeposito : Window
    {
        public TelaDeposito()
        {
            InitializeComponent();
        }

        public decimal ValorDepositado { get; private set; }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(txtValorDeposito.Text, out decimal valor))
            {
                MessageBox.Show("Digite um valor válido.");
                return;
            }

            try
            {
                Database db = new Database();

                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = "UPDATE usuarios SET saldo = saldo + @valor WHERE id = @id";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@valor", valor);
                    cmd.Parameters.AddWithValue("@id", Sessao.UsuarioId);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Depósito realizado com sucesso!");

                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
