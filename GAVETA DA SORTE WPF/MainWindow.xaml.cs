using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace GAVETA_DA_SORTE_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

  

        private void btnEsqueceuSenha_Click(object sender, RoutedEventArgs e)
        {
            EsqueceuSenha tela = new EsqueceuSenha();
        
            tela.Owner = this;
            tela.ShowDialog();
        }

        private void btnLOGAR_Click(object sender, RoutedEventArgs e)
        {
            if (txtUSUARIO.Text == "" || SENHA.Password == "")
            {
                MessageBox.Show("Preencha usuário e senha.");
                return;
            }

            try
            {
                Database db = new Database();

                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT id, usuario FROM usuarios WHERE usuario = @usuario AND senha = @senha";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@usuario", txtUSUARIO.Text);
                    cmd.Parameters.AddWithValue("@senha", SENHA.Password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Sessao.UsuarioId = Convert.ToInt32(reader["id"]);
                        Sessao.UsuarioNome = reader["usuario"].ToString()!;

                        Tela_de_Inicio tela = new Tela_de_Inicio();
                        tela.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Usuário ou senha inválidos.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void BtnFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnIrParaCadastro_Click(object sender, RoutedEventArgs e)
        {
            Tela_de_Cadastro tela = new Tela_de_Cadastro();
            tela.Show();
            this.Close();
        }
    }
}