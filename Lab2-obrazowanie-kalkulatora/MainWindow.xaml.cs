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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using System.Globalization;

namespace Lab2_obrazowanie_kalkulatora
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static SpeechSynthesizer ss;
        static SpeechRecognitionEngine sre;
        static bool done = false;
        public MainWindow()
        {
            ss = new SpeechSynthesizer();
            ss.SetOutputToDefaultAudioDevice();
            ss.Speak("Witam w kalkulatorze");
            CultureInfo ci = new CultureInfo("pl-PL");
            sre = new SpeechRecognitionEngine(ci);
            sre.SetInputToDefaultAudioDevice();
            sre.SpeechRecognized += Sre_SpeechRecognized;
            Grammar grammar = new Grammar(".\\Grammars\\Grammar1.xml", "rootRule");
            grammar.Enabled = true;
            sre.LoadGrammar(grammar);
            sre.RecognizeAsync(RecognizeMode.Multiple);
            InitializeComponent();
        }

        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string txt = e.Result.Text;
            float confidence = e.Result.Confidence;
            double sum;
            List<Button> buttonsToChange = new List<Button> { but11, but12, but13, but14, but15, but16, 
                but17, but18, but19, but10, but21, but22, but23, but24, but25, but26, but27, but28, but29,
                but20, butPlus, butMinus, butTimes, butSplit};
            if (confidence>0.7)
            {
                foreach (var button in buttonsToChange)
                {
                    button.Background = Brushes.White;
                }
                int first = Convert.ToInt32(e.Result.Semantics["first"].Value);
                int second = Convert.ToInt32(e.Result.Semantics["second"].Value);
                string operation = e.Result.Semantics["operation"].Value.ToString();
                switch(first)
                {
                    case 1:
                        but11.Background = Brushes.Green;
                        break;
                    case 2:
                        but12.Background = Brushes.Green;
                        break;
                    case 3:
                        but13.Background = Brushes.Green;
                        break;
                    case 4:
                        but14.Background = Brushes.Green;
                        break;
                    case 5:
                        but15.Background = Brushes.Green;
                        break;
                    case 6:
                        but16.Background = Brushes.Green;
                        break;
                    case 7:
                        but17.Background = Brushes.Green;
                        break;
                    case 8:
                        but18.Background = Brushes.Green;
                        break;
                    case 9:
                        but19.Background = Brushes.Green;
                        break;
                    case 0:
                        but10.Background = Brushes.Green;
                        break;
                }
                switch (second)
                {
                    case 1:
                        but21.Background = Brushes.Green;
                        break;
                    case 2:
                        but22.Background = Brushes.Green;
                        break;
                    case 3:
                        but23.Background = Brushes.Green;
                        break;
                    case 4:
                        but24.Background = Brushes.Green;
                        break;
                    case 5:
                        but25.Background = Brushes.Green;
                        break;
                    case 6:
                        but26.Background = Brushes.Green;
                        break;
                    case 7:
                        but27.Background = Brushes.Green;
                        break;
                    case 8:
                        but28.Background = Brushes.Green;
                        break;
                    case 9:
                        but29.Background = Brushes.Green;
                        break;
                    case 0:
                        but20.Background = Brushes.Green;
                        break;
                }
                if (operation=="suma")
                {
                    butPlus.Background = Brushes.Green;
                    sum = first + second;
                    Response(sum, box1);
                }
                if (operation == "roznica")
                {
                    butMinus.Background = Brushes.Green;
                    sum = first - second;
                    Response(sum, box1);
                }
                if (operation == "iloczyn")
                {
                    butTimes.Background = Brushes.Green;
                    sum = first * second;
                    Response(sum, box1);
                }
                if (operation == "iloraz")
                {
                    if(second==0)
                    {
                        ss.SpeakAsync("Nie można dzielić przez zero");
                        ss.SpeakAsync("Proszę podać liczby jeszcze raz");
                        box1.Text = "Proszę podać liczby jeszcze raz";
                    }
                    else
                    {
                        butSplit.Background = Brushes.Green;
                        sum = first / (double) second;
                        sum = Math.Round(sum, 2);
                        Response(sum, box1);
                    }
                }
            }
            else
            {
                foreach (var button in buttonsToChange)
                {
                    button.Background = Brushes.White;
                }
                ss.Speak("Proszę powtórzyć");
                box1.Text = "Proszę powtórzyć";
            }
        }

        private static void Response(double sum, TextBox box1)
        {
            if (sum == 0)
                ss.Speak("Wynik twojego działania to zero");
            else if (sum > 0 && sum < 1)
            {
                double sumka = sum * 100;
                ss.Speak("Wynik twojego działania to zero i" + sumka);
            }
            else if (sum < 0)
                ss.Speak("Wynik twojego działania to minus" + sum);
            else
                ss.Speak("Wynik twojego działania to" + sum);
            box1.Text = "Wynik twojego działania to " + sum;
        }
    }
}