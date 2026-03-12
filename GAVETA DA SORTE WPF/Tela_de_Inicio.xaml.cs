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
using Microsoft.Win32;


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
    }
}
