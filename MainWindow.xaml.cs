using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace Rellotge
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DiferenciaHoraria diferenciaHoraria = null;
        List<DiferenciaHoraria> lista = null;
        const string FileName = @"..\..\SavedAlarm.bin";
        const string FileNamepaises = @"..\..\SavedPaises.bin";
        private Alarma AlarmaRellotge;
        DispatcherTimer timer1;
        DispatcherTimer timer2;
        DispatcherTimer timer3;
        DlgNomPais dlgNomPais = null;
        bool mensajeMostrado = false;
        String paisCBSeleccionado = "";
        float difHora = 0;
        public MainWindow()
        {
            AlarmaRellotge = new Alarma();
            diferenciaHoraria = new DiferenciaHoraria(); //instanciamos un objeto de la clase DiferenciaString
            dlgNomPais = new DlgNomPais();//instanciamos un objeto de la clase DlgNomPais para poder usar sus métodos.


            InitializeComponent();

            timer1 = new DispatcherTimer();
            timer1.Interval = TimeSpan.FromSeconds(1);
            timer1.Tick += Timer1_Tick;
            timer1.Start();



        }

        void Timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Content = DateTime.Now.ToLongTimeString();
            if (mensajeMostrado == false)
            {
                comprobacionAlarma();
                
            }
        }

        /*void Timer2_Tick(object sender, EventArgs e)
        {
            lblHoraSecundaria.Content = DateTime.Now.ToLongTimeString();
            
        }*/

        void Timer3_Tick(object sender, EventArgs e)
        {
            lblHoraSecundaria.Content = DateTime.Now.AddHours(difHora).ToLongTimeString();
            //lblHoraSecundaria.Content = DateTime.Now.AddHours(dlgNomPais.RetornaLista[0].DifHora).ToLongTimeString();
            //lblHoraSecundaria.Content = DateTime.Now.ToLongTimeString();

        }

        private void comprobacionAlarma()
        {
            int posicion, posicionInicial = 0;
            String horaAcotada="";
            String horaActual = lblTime.Content.ToString();
            posicion = horaActual.LastIndexOf(":");//evalua la última posicion de ":"
            posicionInicial = horaActual.IndexOf(":");//evalua la primera posición de ":" para saber si es más tarde de las 12pm porque de ser así la hora la muestra sin el 0 de delante ejemplo: 1:00:00 en vez de 01:00:00 y luego hay problemas en la comparación por el REgex establecido.
            horaAcotada = horaActual.Substring(0, posicion);//acota la hora actual a precisión de hora y minuto solamente.

            if (posicionInicial < 2)//estamos en la madrugada y no se muestra el 0 de delante
            {
                horaAcotada = "0"+horaAcotada;
            }
             //MessageBox.Show(horaAcotada + " " + AlarmaRellotge.HoraAlarma);
            if (AlarmaRellotge.HoraAlarma.Equals(horaAcotada) && AlarmaRellotge.AlarmaEsActiva==true){ //lanza un mensaje si la hora coincide
                MessageBox.Show("Wake up!!");
                mensajeMostrado = true;
               


            }


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(FileName))
            {
                Stream TestFileStream = File.OpenRead(FileName);
                BinaryFormatter deserializer = new BinaryFormatter();
                AlarmaRellotge = (Alarma)deserializer.Deserialize(TestFileStream);
                TestFileStream.Close();
            }

            if (File.Exists(FileNamepaises))
            {
                Stream TestFileStreamPaises = File.OpenRead(FileNamepaises);
                BinaryFormatter deserializer = new BinaryFormatter();
                diferenciaHoraria = (DiferenciaHoraria)deserializer.Deserialize(TestFileStreamPaises);
                TestFileStreamPaises.Close();   
            }
            //MessageBox.Show(diferenciaHoraria.NomPais+AlarmaRellotge.HoraAlarma);
            TBAlarma.Text = AlarmaRellotge.HoraAlarma.Trim();
            CBPaisos.Items.Add(diferenciaHoraria.NomPais);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Stream TestFileStream = File.Create(FileName);
            Stream TestFileStreamPaises = File.Create(FileNamepaises);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(TestFileStream, AlarmaRellotge);
            serializer.Serialize(TestFileStreamPaises, diferenciaHoraria);
        }

        private void MISortir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MIAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Autor: Raquel Gómez Montesinos DAM2","Rellotge");
        }

        // Quan canvia el text del TextBox de l'hora de l'alarma
        private void tbAlarma_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Només actualitzem l'hora de l'alarma si és una hora vàlida
            if (Regex.IsMatch(TBAlarma.Text, "^[012][0-9]:[0-5][0-9]$"))
            {
                AlarmaRellotge.HoraAlarma = TBAlarma.Text;
                mensajeMostrado = false;
            }
        }

        private void MINouPais_Click(object sender, RoutedEventArgs e)
        {
            DlgNomPais nouDlg = new DlgNomPais();
            if (nouDlg.ShowDialog() == true)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = nouDlg.Resposta.Trim();
                //comprobarPaisesRepetidos();
                CBPaisos.Items.Add(item);
            }
        }
        
        private void CBActiva_Checked(object sender, RoutedEventArgs e)
        {
            AlarmaRellotge.AlarmaEsActiva = true;
        }

        private void CBActiva_Unchecked(object sender, RoutedEventArgs e)
        {
            AlarmaRellotge.AlarmaEsActiva = false;
        }

        private void MIEliminarPais_Click_1(object sender, RoutedEventArgs e)
        {
            DlgEliminarPais nouDlgEliminar = new DlgEliminarPais();
            List<DiferenciaHoraria> lista = dlgNomPais.RetornaLista;
            ComboBoxItem items = new ComboBoxItem();

            if (nouDlgEliminar.ShowDialog() == true)
            {
                string item = nouDlgEliminar.Resposta.Trim();
                for (int i = 0; i < lista.Count; i++)
                {
                    if (item.Equals(lista[i].NomPais))
                    {
                        lista.RemoveAt(i);
                        recargarCombo();

                    }
                }
               
            }

        }

        private void recargarCombo()
        {
            List<DiferenciaHoraria> lista = dlgNomPais.RetornaLista;
            CBPaisos.Items.Clear();//borro primero toda la combo
           
            
            
            for (int i=0;i< lista.Count; i++)
            {

                CBPaisos.Items.Add(lista[i].NomPais);//relleno la combo  de nuevo con la List lista actualizada.

            }
        }

        private void btn1_Click(object sender, RoutedEventArgs e) {

            lblHoraSecundaria.Content = DateTime.Now.AddHours(dlgNomPais.RetornaLista[0].DifHora).ToLongTimeString();//Recalculo la hora del país seleccionado sumando o restando
            List<DiferenciaHoraria> lista = dlgNomPais.RetornaLista;
            paisCBSeleccionado = CBPaisos.Text;
            
            for (int i = 0; i < lista.Count; i++)
            {
                if (paisCBSeleccionado.Equals(lista[i].NomPais))
                {
                    difHora = lista[i].DifHora;
                    timer3 = new DispatcherTimer();
                    timer3.Interval = TimeSpan.FromSeconds(1);
                    timer3.Tick += Timer3_Tick;
                    timer3.Start();
                    break;
                }
            }

            





        }

        private void CBPaisos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //paisCBSeleccionado =CBPaisos.SelectedValue.ToString();
            //tbprueba.Text = CBPaisos.Text;
            
        }
    }
}
