using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace AnimalMatchingGame
{
    public partial class MainWindow : Window
    {
        private string[] animais = { "🐶", "🐱", "🐭", "🐹", "🐰", "🦊", "🐻", "🐼" };
        private List<string> cartas;
        private TextBlock primeiraCarta, segundaCarta;
        private int paresEncontrados;
        private DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            SetupGame();
        }

        private void SetupGame()
        {
            // cria lista com 8 pares e embaralha
            cartas = animais.Concat(animais).OrderBy(a => Guid.NewGuid()).ToList();

            // distribui os emojis nos 16 blocos
            int i = 0;
            foreach (var child in GameGrid.Children)
            {
                if (child is TextBlock bloco)
                {
                    bloco.Text = "?";
                    bloco.Tag = cartas[i++];
                    bloco.Background = Brushes.LightBlue;
                    bloco.FontSize = 32;
                    bloco.TextAlignment = TextAlignment.Center;
                    bloco.MouseLeftButtonDown += TextBlock_Click;
                }
            }

            paresEncontrados = 0;
            StatusText.Text = "Pares encontrados: 0/8";
        }

        private void TextBlock_Click(object sender, MouseButtonEventArgs e)
        {
            var clicked = sender as TextBlock;
            if (clicked == null || clicked.Text != "?") return;

            clicked.Text = clicked.Tag.ToString();
            clicked.Background = Brushes.LightYellow;

            if (primeiraCarta == null)
            {
                primeiraCarta = clicked;
            }
            else
            {
                segundaCarta = clicked;
                if (primeiraCarta.Tag.ToString() == segundaCarta.Tag.ToString())
                {
                    primeiraCarta.Background = Brushes.LightGreen;
                    segundaCarta.Background = Brushes.LightGreen;
                    paresEncontrados++;
                    StatusText.Text = $"Pares encontrados: {paresEncontrados}/8";
                    primeiraCarta = segundaCarta = null;

                    if (paresEncontrados == 8)
                        MessageBox.Show("Parabéns! Você venceu 🎉");
                }
                else
                {
                    timer.Start();
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            primeiraCarta.Text = "?";
            segundaCarta.Text = "?";
            primeiraCarta.Background = Brushes.LightBlue;
            segundaCarta.Background = Brushes.LightBlue;
            primeiraCarta = segundaCarta = null;
            timer.Stop();
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            SetupGame();
        }
    }
}
