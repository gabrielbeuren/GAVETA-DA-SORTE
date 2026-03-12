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
    /// Lógica interna para EsqueceuSenha.xaml
    /// </summary>
    public partial class EsqueceuSenha : Window
    {
        public EsqueceuSenha()
        {
            InitializeComponent();
        }

        private void btnAlterarSenha_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsuarioRecuperar.Text == "" || txtNovaSenha.Password == "" || txtConfirmarSenha.Password == "")
            {
                MessageBox.Show("Preencha todos os campos.");
                return;
            }

            if (txtNovaSenha.Password != txtConfirmarSenha.Password)
            {
                MessageBox.Show("As senhas não coincidem.");
                return;
            }

            try
            {
                Database db = new Database();

                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = "UPDATE dbo.usuarios SET senha = @senha WHERE usuario = @usuario";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@usuario", txtUsuarioRecuperar.Text);
                    cmd.Parameters.AddWithValue("@senha", txtNovaSenha.Password);

                    int linhas = cmd.ExecuteNonQuery();

                    if (linhas > 0)
                    {
                        MessageBox.Show("Senha alterada com sucesso!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Usuário não encontrado.");
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

        
    }
}
