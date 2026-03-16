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
    /// Lógica interna para Grupo.xaml
    /// </summary>
    public partial class Grupo : Window
    {
        private int grupoId;
        public Grupo(int id, string nome)
        {
            InitializeComponent();

            grupoId = id;

            txtNomeGrupo.Text = nome;

            CarregarParticipantes();
        }

        private void CarregarParticipantes()
        {
            listaParticipantes.Items.Clear();

            Database db = new Database();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"SELECT u.usuario
                         FROM grupo_participantes gp
                         JOIN usuarios u ON gp.usuario_id = u.id
                         WHERE gp.grupo_id = @grupo";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@grupo", grupoId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    listaParticipantes.Items.Add(reader["usuario"].ToString());
                }
            }
        }

        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsuario.Text == "")
            {
                MessageBox.Show("Digite o nome do usuário.");
                return;
            }

            Database db = new Database();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                // verificar se o usuário existe
                string queryUsuario = "SELECT id FROM usuarios WHERE usuario = @usuario";

                SqlCommand cmdUsuario = new SqlCommand(queryUsuario, conn);
                cmdUsuario.Parameters.AddWithValue("@usuario", txtUsuario.Text);

                object resultado = cmdUsuario.ExecuteScalar();

                if (resultado == null)
                {
                    MessageBox.Show("Usuário não encontrado.");
                    return;
                }

                int usuarioId = Convert.ToInt32(resultado);

                // adicionar no grupo
                string queryGrupo = "INSERT INTO grupo_participantes (grupo_id, usuario_id) VALUES (@grupo, @usuario)";

                SqlCommand cmdGrupo = new SqlCommand(queryGrupo, conn);

                cmdGrupo.Parameters.AddWithValue("@grupo", grupoId);
                cmdGrupo.Parameters.AddWithValue("@usuario", usuarioId);

                cmdGrupo.ExecuteNonQuery();
            }

            MessageBox.Show("Participante adicionado!");

            CarregarParticipantes();
        }

        private void btnRemoverUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (listaParticipantes.SelectedItem == null)
            {
                MessageBox.Show("Selecione um participante.");
                return;
            }

            string usuarioSelecionado = listaParticipantes.SelectedItem.ToString();

            try
            {
                Database db = new Database();

                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    // pegar o id do usuário
                    string queryUsuario = "SELECT id FROM usuarios WHERE usuario = @usuario";

                    SqlCommand cmdUsuario = new SqlCommand(queryUsuario, conn);
                    cmdUsuario.Parameters.AddWithValue("@usuario", usuarioSelecionado);

                    int usuarioId = Convert.ToInt32(cmdUsuario.ExecuteScalar());

                    // remover do grupo
                    string queryRemover = "DELETE FROM grupo_participantes WHERE grupo_id = @grupo AND usuario_id = @usuario";

                    SqlCommand cmdRemover = new SqlCommand(queryRemover, conn);

                    cmdRemover.Parameters.AddWithValue("@grupo", grupoId);
                    cmdRemover.Parameters.AddWithValue("@usuario", usuarioId);

                    cmdRemover.ExecuteNonQuery();
                }

                MessageBox.Show("Participante removido!");

                CarregarParticipantes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
