﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace com.indes.jogo_roleta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    /***********************************************************************************************
     * JogoRoleta (Classe)
     * ********************************************************************************************/
    public partial class JogoRoleta : Window
    {
        private Button[] allNumBtns_array = new Button[37];
        private Button[] allChipBtns_array;
        private String[] specialBtns;
        private Button[] specialBtns_array = new Button[12];
        private Button luckyRouletteNum = new Button();
        private List<Button> betsBtns_list = new Button[] { }.ToList();
        private List<String> betsNumbers_list = new String[] { }.ToList();
        private List<int> betsValues_list = new int[] { }.ToList();
        private List<Image> betsChips_list = new Image[] { }.ToList();
        private System.Windows.Threading.DispatcherTimer dispatcherTimer =
            new System.Windows.Threading.DispatcherTimer();
        private Image bola = new Image();
        private int bet_value = 0;
        private int balance = 0;
        private int rouletteNumber;


        Canvas myCanvas = new Canvas();
        Image imgChip = new Image();

        /*******************************************************************************************
         * JogoRoleta (Inicio)
         * ****************************************************************************************/
        public JogoRoleta()
        {
            InitializeComponent();
            populateArrays();
            balance = 200;
            refreshBalance();
            lbl_hint.Content = "Selecione a ficha para apostar!";







            /*
            //** Adiciona uma ficha ao ecra
            imgChip.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Ficha.png"));

            Canvas.SetTop(imgChip, 0);
            Canvas.SetLeft(imgChip, 0);
            imgChip.Width = 30;
            imgChip.Height = 30;
            myCanvas.Children.Add(imgChip);
            Canvas.SetZIndex(imgChip, 0);
            */
            tableGrid.Children.Add(myCanvas);
        }

        /*******************************************************************************************
         * dispatcherTimer_Tick
         * ****************************************************************************************/
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            bola = (Image)FindName("bola_" + rouletteNumber);
            bola.Visibility = Visibility.Visible;
            img_animatedBall.Visibility = Visibility.Hidden;
            btn_newGame.Visibility = Visibility.Visible;
            luckyRouletteNum = (Button)FindName("btn_" + rouletteNumber);
            luckyRouletteNum.Style = (Style)FindResource("luckyNumber");
            dispatcherTimer.Stop();
            refreshBalance();
        }

        /*******************************************************************************************
         * populateArrays
         * ****************************************************************************************/
        private void populateArrays()
        {
            int[] chipValues=new int[] {1,5,10,20,50,100};
            allChipBtns_array = new Button[chipValues.Length];
            specialBtns = new String[] {"btn_1_18","btn_even","btn_black","btn_red","btn_odd",
                "btn_19_36","btn_col1","btn_col2","btn_col3","btn_duzia1","btn_duzia2","btn_duzia3"};

            //Preenche um array com todos os botoes especiais
            for (int i = 0; i < 12; i++)
            {
                specialBtns_array[i] = (Button)FindName(specialBtns[i]);
            }

            //Preenche um array com todos os botoes de 0 a 36
            for (int i = 0; i < 37; i++)
            {
                allNumBtns_array[i] = (Button)FindName("btn_" + (i).ToString());
            }

            //Preenche um array com todos os botoes das fichas de aposta
            for (int i = 0; i < allChipBtns_array.Length; i++)
            {
                allChipBtns_array[i] = (Button)FindName("chipBtn_" + (chipValues[i]).ToString());
            }
        }

        /*******************************************************************************************
         * btn_spin_MouseLeftClick
         * - Gira a roleta e gera o numero sorteado de 1 a 36
         * ****************************************************************************************/
        private void btn_spin_MouseLeftClick(object sender, RoutedEventArgs e)
        {
            List<int> testArray = new int[] { }.ToList();
            string[] betNumbers;

            openTable.IsEnabled = false;
            Random random = new Random();

            // Configuração e arranque do temporizador
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
            img_animatedBall.Visibility = Visibility.Visible;
            dispatcherTimer.Start();

            if (bet_value != 0)
            {
                rouletteNumber = random.Next(37);

                // Verifica se o numero gerado pela roleta está entre os numeros apostados
                foreach (String bet in betsNumbers_list)
                {
                    betNumbers = bet.Split(' ');
                    foreach (String betNumber in betNumbers)
                    {
                        if (betNumber != "" && rouletteNumber == int.Parse(betNumber))
                        {
                            calculateBalance(betNumbers.Length);
                            btn_newGame.IsEnabled = true;
                            btn_spin.IsEnabled = false;
                        }
                    }
                }
                btn_spin.IsEnabled = false;
                betsNumbers_list.Clear();   
            }
            else
            {
                MessageBox.Show("Tem de primeiro fazer a aposta.\n\n1º Escolha o valor da"+
                    " aposta\n2º Escolha os números em que deseja apostar", "Informação");
            }
        }

        /****************************************************************************************** 
         * btn_multiple_MouseEnter
         * - Ilumina os botoes/numeros
         ******************************************************************************************/
        private void btn_multiple_MouseEnter(object sender, MouseEventArgs e)
        {
            string[] numbers = ((sender as Button).Content as String).Split(' ');
            
            foreach (string number in numbers)
            {
                if (number != "")
                {
                    int intNumber = int.Parse(number);
                    allNumBtns_array[intNumber].Style = (Style)FindResource("btn_highlight");
                }
            }
        }

        /******************************************************************************************* 
         * btn_multiple_MouseLeave
         * - Retira a iluminacao dos botoes/numeros
         ******************************************************************************************/
        private void btn_multiple_MouseLeave(object sender, MouseEventArgs e)
        {
            string[] parts = sender.ToString().Split(':');
            string[] numbers =parts[1].Split(' ');
            string[] betNumbers;

            // Apaga todos os numeros que estavam acesos 
            foreach (string number in numbers)
            {
                if (number != "")
                {
                    int intNumber = int.Parse(number);

                    if (!allNumBtns_array[intNumber].IsMouseOver)
                        allNumBtns_array[intNumber].Style = (Style)FindResource("btn");
                }
            }

            // Acende os botoes seleccionados nas apostas
            foreach (Button betBtn in betsBtns_list)
            {
                foreach(String specialBtn in specialBtns)
                    if(betBtn.Name == specialBtn)
                        betBtn.Style = (Style)FindResource("btn_highlight");
            }

            // Acende os numeros seleccionados nas apostas
            foreach (String bet in betsNumbers_list)
            {
                betNumbers = bet.Split(' ');

                foreach (string betNumber in betNumbers)
                {
                    if (betNumber != "")
                    {
                        int intBetNumber = int.Parse(betNumber);

                        allNumBtns_array[intBetNumber].Style = (Style)FindResource("btn_highlight");
                    }
                }
            }   
        }

        /******************************************************************************************* 
         * btn_multiple_MouseClick
         * - Função para receber o evento click dos botoes/numeros e guardar os numeros apostados
        *******************************************************************************************/
        private void btn_multiple_MouseClick(object sender, RoutedEventArgs e)
        {
            string[] parts = sender.ToString().Split(':');
            Boolean betExists = false;
            Boolean specialBtnExists = false;

            // Se já foi feita aposta neste botao entao remove o botão do array bet_Btn para 
            // retirar a aposta
            if (betsBtns_list.Count > 0)
            {
                foreach (Button betBtn in betsBtns_list)
                {
                    if (betBtn == sender)
                    {
                        balance += betsValues_list[betsBtns_list.IndexOf(betBtn)];
                        refreshBalance();
                        betsValues_list.RemoveAt(betsBtns_list.IndexOf(betBtn));
                        betsBtns_list.Remove(betBtn);
                        betsNumbers_list.Remove(parts[1]);

                        if(betsBtns_list.Count == 0)
                            btn_spin.IsEnabled = false;
                        
                        foreach (String specialBtn in specialBtns)
                        {
                            if (betBtn.Name == specialBtn)
                            {
                                betBtn.Style = (Style)FindResource("btn");
                                specialBtnExists = true;
                            }
                            else if (!specialBtnExists)
                                betBtn.Style = (Style)FindResource("myStyle4");
                        }
                        
                        betExists = true;
                        break;
                    }
                }
            }

            if (balance < bet_value)
            {
                MessageBox.Show("Não tem saldo suficiente!", "Informação");
            }
            else 
            {
                // Se o botao nao existir no array de apostas bet_Btns então ele é adicionado 
                // ao array
                if(!betExists)
                {
                    lbl_hint.Content = "Gire a roleta ou adicione outra aposta!";
                    btn_spin.IsEnabled = true;
                    balance += -bet_value;
                    refreshBalance();
                    betsBtns_list.Add(sender as Button);
                    betsNumbers_list.Add(parts[1]);
                    betsValues_list.Add(bet_value);










                    imgChip = new Image();
                    imgChip.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Ficha.png"));

                    imgChip.Name = "imgChip_" + (String)((sender as Button).Name);

                    Canvas.SetTop(imgChip, 0);
                    Canvas.SetLeft(imgChip, 0);
                    imgChip.Width = 30;
                    imgChip.Height = 30;

                    betsChips_list.Add(imgChip);

                    myCanvas.Children.Add(imgChip);
                    //tableGrid.Children.Add(myCanvas);
                    Canvas.SetZIndex(imgChip, 0);

                    //** Posiciona a ficha no botao/numero escolhido
                    Point relativePoint = (sender as Button).TransformToAncestor(tableGrid)
                              .Transform(new Point(0, 0));
                    double centroX=(sender as Button).ActualWidth / 2;
                    double centroY=(sender as Button).ActualHeight / 2;
                    Canvas.SetTop(imgChip, relativePoint.Y + centroY - imgChip.Height / 2);
                    Canvas.SetLeft(imgChip, relativePoint.X + centroX - imgChip.Width / 2);











                }
            }
        }

        /******************************************************************************************* 
         * chip_Click
         * - Função para receber o evento click das fichas
        *******************************************************************************************/
        private void chip_Click(object sender, RoutedEventArgs e)
        {
            string[] parts = sender.ToString().Split(':');

            if (balance < 0)
            {
                MessageBox.Show("Não tem saldo suficiente!", "Informação");
            }
            else
            {
                lbl_hint.Content = "Selecione os números em que pretende fazer uma aposta!";

                openTable.IsEnabled = true;
                bet_value = int.Parse(parts[1]);

                foreach (Button chipBtn in allChipBtns_array)
                {
                    if (chipBtn == sender)
                        chipBtn.Style = (Style)FindResource("chip_SelectedStyle");
                    else
                        chipBtn.Style = (Style)FindResource("chip");
                }
            }
        }

        /******************************************************************************************* 
         * btn_newGame_Click
         * - Função iniciar novo jogo
        *******************************************************************************************/
        private void btn_newGame_Click(object sender, RoutedEventArgs e)
        {
            clearBets();
            btn_newGame.Visibility = Visibility.Hidden;
            bola.Visibility = Visibility.Hidden;
            openTable.IsEnabled = false;
            lbl_hint.Content = "Selecione a ficha para apostar!";
        }

        /******************************************************************************************* 
         * refreshBalance
         * - Actualiza o saldo
        *******************************************************************************************/
        private void refreshBalance()
        {
            lbl_saldo.Content = "Saldo: " + balance.ToString() + " c";
        }

        /******************************************************************************************* 
         * calculateBalance
         * - Calcula o saldo
        *******************************************************************************************/
        private void calculateBalance(int betNumbersCount)
        {
            // Este calculo inclui o lucro + o valor da aposta
            balance += bet_value * 36 / (betNumbersCount-1);
        }

        /******************************************************************************************* 
         * clearBets
         * - Limpa todas as apostas da mesa
        *******************************************************************************************/
        private void clearBets()
        {
            betsNumbers_list.Clear();
            betsValues_list.Clear();
            betsBtns_list.Clear();

            foreach (Button numBtn in allNumBtns_array)
            {
                numBtn.Style = (Style)FindResource("btn");
            }

            foreach (Button specialBtn in specialBtns_array)
            {
                specialBtn.Style = (Style)FindResource("btn");
            }

            foreach (Button chipBtn in allChipBtns_array)
            {
                chipBtn.Style = (Style)FindResource("chip");
            }
        }

        /******************************************************************************************* 
         * Voltar_Click
         * - Volta ao menu do jogo
        *******************************************************************************************/
        private void Voltar_Click(object sender, RoutedEventArgs e)
        {
            Menu main = new Menu();
            App.Current.MainWindow = main;
            this.Close();
            main.Show();
        }
    }
}