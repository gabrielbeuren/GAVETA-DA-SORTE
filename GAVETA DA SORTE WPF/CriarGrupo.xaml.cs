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
using Microsoft.Data.SqlClient;

namespace GAVETA_DA_SORTE_WPF
{
    /// <summary>
    /// Lógica interna para CriarGrupo.xaml
    /// </summary>
    public partial class CriarGrupo : Window
    {
        public CriarGrupo()
        {
            InitializeComponent();
        }

        private void btnCriarGrupo_Click(object sender, RoutedEventArgs e)
        {
            if (txtNomeGrupo.Text == "")
            {
                MessageBox.Show("Digite o nome do grupo.");
                return;
            }

            try
            {
                Database db = new Database();

                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = "INSERT INTO grupos (nome, criador_id) VALUES (@nome, @criador)";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@nome", txtNomeGrupo.Text);
                    cmd.Parameters.AddWithValue("@criador", Sessao.UsuarioId);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Grupo criado com sucesso!");

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
