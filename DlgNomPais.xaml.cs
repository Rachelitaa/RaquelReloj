using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Rellotge
{
    /// <summary>
    /// Lógica de interacción para DlgNomPais.xaml
    /// </summary>
    public partial class DlgNomPais : Window
    {
        DiferenciaHoraria diferenciaHoraria = null;
        public static  List<DiferenciaHoraria> diferenciasList= new List<DiferenciaHoraria>(); // declaro la lista donde guardaré posteriormente objetos del tipo DiferenciaHoraria.
        
        public DlgNomPais()
        {
            diferenciaHoraria = new DiferenciaHoraria(); //instanciamos un objeto de la clase DiferenciaString llamando a uno de sus constructores sin parametrizar

            InitializeComponent();
            
            //Rellenamos las combobox cBSigno Y CBDiferenciaH  que indican la diferencia horaria
            CBSigno.Items.Add("+");
            CBSigno.Items.Add("-");
            for(int i = 0; i <= 12; i++)
            {
                CBDiferenciaH.Items.Add(i);
            }
            


        }

        // Donem el focus al TextBox per escriure directament
        private void DlgNomPais1_Loaded(object sender, RoutedEventArgs e)
        {
            TBNomPais.Focus();
        }

        // La propietat DialogResult és la que es retorna al cridar ShowDialog() per mostrar la finestra
        // No tenim equivalent pel cas de Cancel perquè hem marcat el botó Cancel amb la propietat IsCancel que ho fa auromàticament
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            String pais, signo, diferenciaString = "";
            float diferencia = 0;

            //Recogemos el país introducido en la textBox TBNomPais
            pais = TBNomPais.Text;
            //Recogemos el valor de las 2 Combobox CBSigno y CBDiferenciaH.
            signo = CBSigno.SelectedItem.ToString();
            diferenciaString = CBDiferenciaH.SelectedItem.ToString();
            

            diferencia = float.Parse(signo+diferenciaString); //parseamos de String a float para guardar un numero (positivo o negativo) en el objeto.
            //actualizo los valores del objeto
            diferenciaHoraria.NomPais = pais;
            diferenciaHoraria.DifHora = diferencia;
            diferenciasList.Add(diferenciaHoraria); //Guardamos el objecto en una lista del tipo List

            this.DialogResult = true;
        }

       

        // Propietat per accedir al text del TextBox des de fora de la classe, al retornar del diàleg
        public string Resposta
        {
            get { return TBNomPais.Text; }
        }

        public List<DiferenciaHoraria> RetornaLista
        {
            get {return diferenciasList; }
        }

        // Habilitem o deshabilitem el botó Ok segons si hi ha text o no al TextBox
        // Trim elimina els espais en blanc al principi i final de la cadena
        private void TBNomPais_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            
            if ((TBNomPais.Text.Trim().Length > 0))
                BtnOk.IsEnabled = true;
            else
                BtnOk.IsEnabled = false;
        }

        // Interceptem les tecles al TextBox del nom del país
        // Només permetem escriure els caràcters alfabètics i l'espai
        // La resta de tecles les interceptem posant la propietat Handled de l'event a true
        private void TBNomPais_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ( ((e.Key < Key.A) || (e.Key > Key.Z)) 
                && (e.Key != Key.Enter) 
                && (e.Key != Key.Delete) && (e.Key != Key.Back) 
                && (e.Key != Key.Space) )
                e.Handled = true;
        }

        private void CBSigno_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
             
        }

     
    }
}
