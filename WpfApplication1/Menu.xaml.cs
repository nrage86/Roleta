using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace com.indes.jogo_roleta
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    /// 

    /***********************************************************************************************
     * Menu (Classe)
     * ********************************************************************************************/
    public partial class Menu : Window
    {
        FlowDocument myFlowDoc = new FlowDocument();
        private System.Windows.Threading.DispatcherTimer dispatcherTimer =
            new System.Windows.Threading.DispatcherTimer();

        /*******************************************************************************************
         * Menu (Inicio)
         * ****************************************************************************************/
        public Menu()
        {
            InitializeComponent();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //Abre jogo (MainWindow)
            Menu menu = new Menu();
            App.Current.MainWindow = menu;
            this.Close();
            menu.Show();
            dispatcherTimer.Stop();
        }
        /*******************************************************************************************
         * Jogar_Click
         * ****************************************************************************************/
        private void Jogar_Click(object sender, RoutedEventArgs e)
        {
            //Abre jogo (MainWindow)
            JogoRoleta main = new JogoRoleta();
            App.Current.MainWindow = main;
            this.Close();
            main.Show();
        }

        /*******************************************************************************************
         * Tutorial_Click
         * ****************************************************************************************/
        private void Tutorial_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Abre tutorial (Captivate)
            System.Diagnostics.Process.Start(@".\Captivate\indes.htm");
        }

        /*******************************************************************************************
         * Regras_Click
         * ****************************************************************************************/
        private void Regras_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Abre regras (Axmag)
            System.Diagnostics.Process.Start(
                "http://data.axmag.com/data/201301/U85511_F183964/FLASH/index.html");
        }

        /*******************************************************************************************
         * Creditos_Click
         * ****************************************************************************************/
        private void Creditos_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Abre Creditos (Cred)
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 12);
            Cred cred = new Cred();
            App.Current.MainWindow = cred;
            this.Close();
            cred.Show();
            dispatcherTimer.Start();

        }

        /*******************************************************************************************
         * Exit_Click
         * ****************************************************************************************/
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            Menu menu = new Menu();
            App.Current.MainWindow = menu;
            this.Close();
            menu.Show();
        }
    }
}
