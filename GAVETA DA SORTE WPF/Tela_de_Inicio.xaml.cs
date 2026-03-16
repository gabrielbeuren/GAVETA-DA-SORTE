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
using System.IO;

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
            CarregarSaldo();
            CarregarFoto();
        }

        private void btnTrocarFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Imagens|*.png;*.jpg;*.jpeg";

            if (dialog.ShowDialog() == true)
            {
                string caminho = dialog.FileName;

                imgPerfil.Source = new BitmapImage(new Uri(caminho));

                Database db = new Database();

                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = "UPDATE usuarios SET foto = @foto WHERE id = @id";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@foto", caminho);
                    cmd.Parameters.AddWithValue("@id", Sessao.UsuarioId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void btnCriarGrupo_Click(object sender, RoutedEventArgs e)
        {
            CriarGrupo tela = new CriarGrupo();
            tela.Owner = this;

            if (tela.ShowDialog() == true)
            {
                CarregarGrupos();
            }
        }

        private void listaGrupos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listaGrupos.SelectedItem != null)
            {
                GrupoItem grupoSelecionado = (GrupoItem)listaGrupos.SelectedItem;
                Grupo tela = new Grupo(grupoSelecionado.Id,grupoSelecionado.Nome);
                tela.ShowDialog();
            }
        }

        private void CarregarGrupos()
        {
            listaGrupos.Items.Clear();

            Database db = new Database();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id, nome FROM grupos WHERE criador_id = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", Sessao.UsuarioId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    GrupoItem grupo = new GrupoItem();

                    grupo.Id = Convert.ToInt32(reader["id"]);
                    grupo.Nome = reader["nome"].ToString();

                    listaGrupos.Items.Add(grupo);
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

        private void CarregarFoto()
        {
            Database db = new Database();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT foto FROM usuarios WHERE id = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", Sessao.UsuarioId);

                object resultado = cmd.ExecuteScalar();

                if (resultado != DBNull.Value)
                {
                    string caminho = resultado.ToString();

                    if (File.Exists(caminho))
                    {
                        imgPerfil.Source = new BitmapImage(new Uri(caminho));
                    }
                }
            }
        }
    }
}
