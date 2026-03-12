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
using System.Text.RegularExpressions;

namespace GAVETA_DA_SORTE_WPF
{
    /// <summary>
    /// Lógica interna para Tela_de_Cadastro.xaml
    /// </summary>
    public partial class Tela_de_Cadastro : Window
    {
        public Tela_de_Cadastro()
        {
            InitializeComponent();
        }

        private void btnCADASTRAR_Click(object sender, RoutedEventArgs e)
        {
            if (USUARIOCADASTRO.Text == "" || SENHACADASTRO.Password == "" || confirmarSENHA.Password == "")
            {
                MessageBox.Show("Preencha todos os campos");
                return;
            }

            if (SENHACADASTRO.Password != confirmarSENHA.Password)
            {
                MessageBox.Show("As senhas não coincidem");
                return;
            }

            string senha = SENHACADASTRO.Password;

            if (senha.Length < 8)
            {
                MessageBox.Show("A senha deve ter no mínimo 8 caracteres.");
                return;
            }

            if (!Regex.IsMatch(senha, @"[A-Z]"))
            {
                MessageBox.Show("A senha deve conter pelo menos uma letra maiúscula.");
                return;
            }

            if (!Regex.IsMatch(senha, @"[!@#$%^&*(),.?""{}|<>/+_-]"))
            {
                MessageBox.Show("A senha deve conter pelo menos um caractere especial.");
                return;
            }

            try
            {
                Database db = new Database();

                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = "INSERT INTO usuarios (usuario, senha) VALUES (@usuario, @senha)";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@usuario", USUARIOCADASTRO.Text);
                    cmd.Parameters.AddWithValue("@senha", SENHACADASTRO.Password);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Usuário cadastrado com sucesso!");
                    MainWindow tela = new MainWindow();
                    tela.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar: " + ex.Message);
            }
        }

        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();   
        }
    }
}
