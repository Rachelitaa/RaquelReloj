
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Rellotge
{
    /// <summary>
    /// Interaction logic for DlgEliminarPais.xaml
    /// </summary>
    public partial class DlgEliminarPais : Window
    {
        public DlgEliminarPais()
        {
            InitializeComponent();
        }

        private void DlgEliminarPais_Loaded(object sender, RoutedEventArgs e)
        {
            TBNomPais.Focus();
        }
        // La propietat DialogResult és la que es retorna al cridar ShowDialog() per mostrar la finestra
        // No tenim equivalent pel cas de Cancel perquè hem marcat el botó Cancel amb la propietat IsCancel que ho fa auromàticament
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            //Recogemos el país introducido en la textBox TBNomPais
            this.DialogResult = true;
        }
        // Propietat per accedir al text del TextBox des de fora de la classe, al retornar del diàleg
        public string Resposta
        {
            get { return TBNomPais.Text; }
        }

        // Habilitem o deshabilitem el botó Ok segons si hi ha text o no al TextBox
        // Trim elimina els espais en blanc al principi i final de la cadena
        private void TBNomPais_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (TBNomPais.Text.Trim().Length > 0)
                BtnOk.IsEnabled = true;
            else
                BtnOk.IsEnabled = false;
        }

        // Interceptem les tecles al TextBox del nom del país
        // Només permetem escriure els caràcters alfabètics i l'espai
        // La resta de tecles les interceptem posant la propietat Handled de l'event a true
        private void TBNomPais_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (((e.Key < Key.A) || (e.Key > Key.Z))
                && (e.Key != Key.Enter)
                && (e.Key != Key.Delete) && (e.Key != Key.Back)
                && (e.Key != Key.Space))
                e.Handled = true;
        }
    }
}
