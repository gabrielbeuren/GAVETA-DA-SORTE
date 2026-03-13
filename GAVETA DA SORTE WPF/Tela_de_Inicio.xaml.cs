using Microsoft.Data.SqlClient;
using Microsoft.Win32;
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
    /// Lógica interna para Tela_de_Inicio.xaml
    /// </summary>
    public partial class Tela_de_Inicio : Window
    {
        public Tela_de_Inicio()
        {
            InitializeComponent();
            txtOlaUsuario.Text = "Olá, " + Sessao.UsuarioNome;
            CarregarGrupos();
        }

        private void btnTrocarFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Imagens|*.png;*.jpg;*.jpeg";

            if (dialog.ShowDialog() == true)
            {
                imgPerfil.Source = new BitmapImage(new Uri(dialog.FileName));
            }
        }

        private void btnCriarGrupo_Click(object sender, RoutedEventArgs e)
        {
            CriarGrupo tela = new CriarGrupo();
            tela.Owner = this;
            tela.ShowDialog();
        }

        private void listaGrupos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listaGrupos.SelectedItem != null)
            {
                Grupo tela = new Grupo();
                tela.ShowDialog();
            }
        }

        private void CarregarGrupos()
        {
            Database db = new Database();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id, nome FROM grupos WHERE criador_id = @usuario";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@usuario", Sessao.UsuarioId);

                SqlDataReader reader = cmd.ExecuteReader();

                listaGrupos.Items.Clear();

                while (reader.Read())
                {
                    listaGrupos.Items.Add(reader["nome"].ToString());
                }
            }
        }

       

        private void BtnFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSair_Click(object sender, RoutedEventArgs e)
        {
            MainWindow tela = new MainWindow();
            tela .ShowDialog();
            this.Close();
        }

        private void btnDepositar_Click(object sender, RoutedEventArgs e)
        {
            TelaDeposito tela = new TelaDeposito();
            tela.Owner = this;
            
            if (tela.ShowDialog() == true)
            {
                CarregarSaldo();
            }
        }

        private void CarregarSaldo()
        {
            Database db = new Database();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT saldo FROM usuarios WHERE id = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", Sessao.UsuarioId);

                object resultado = cmd.ExecuteScalar();

                if (resultado != DBNull.Value)
                {
                    decimal saldo = Convert.ToDecimal(resultado);

                    txtSaldo.Text = saldo.ToString("C");
                }
            }
        }
    }
}
