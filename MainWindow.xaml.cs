﻿using System;
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

namespace Reversi
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ReversiEngine engine = new ReversiEngineAI(1);
        private SolidColorBrush[] colors = { Brushes.Ivory, Brushes.Green, Brushes.Sienna };
        string[] playerName = { "", "green", "brown" };
        private Button[,] field;
        private bool IsFieldInitialized
        {
            get
            {
                return field[engine.FieldWidth - 1, engine.FieldHeight - 1] != null;
            }
        }

        private struct FieldCoordinate
        {
            public int Horiz, Vert;
        }

        private void AgreeFieldContent()
        {
            if (!IsFieldInitialized) return;

            for (var i = 0; i < engine.FieldWidth; i++)
            {
                for (var j = 0; j < engine.FieldHeight; j++)
                {
                    field[i, j].Background = colors[engine.GetFieldState(i, j)];
                    field[i, j].Content = engine.GetFieldState(i, j).ToString();
                }
            }

            colorPlayerButton.Background = colors[engine.NextMovePlayerNumber];
            colorGreenField.Text = engine.CountOfPlayerOneField.ToString();
            colorBrownField.Text = engine.CountOfPlayerTwoField.ToString();
            
        }

        public MainWindow()
        {
            InitializeComponent();

            for (var i = 0; i < engine.FieldWidth; i++)
                fieldGrid.ColumnDefinitions.Add(new ColumnDefinition());
            for (var j = 0; j < engine.FieldHeight; j++)
                fieldGrid.RowDefinitions.Add(new RowDefinition());

            field = new Button[engine.FieldWidth, engine.FieldHeight];
            for (var i = 0; i < engine.FieldWidth; i++)
            {
                for (var j = 0; j < engine.FieldHeight; j++)
                {
                    Button button = new Button();
                    button.Margin = new Thickness(0);
                    fieldGrid.Children.Add(button);
                    Grid.SetColumn(button, i);
                    Grid.SetRow(button, j);
                    button.Tag = new FieldCoordinate { Horiz = i, Vert = j };
                    button.Click += Button_Click;
                    field[i, j] = button;
                }
            }
            AgreeFieldContent();

            //engine.PutStone(2, 4);
            //engine.PutStone(4, 5);
            //AgreeFieldContent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = sender as Button;
            FieldCoordinate coordinate = (FieldCoordinate)clicked.Tag;
            int clickHoriz = coordinate.Horiz;
            int clickVert = coordinate.Vert;

            int playerNumber = engine.NextMovePlayerNumber;
            if (engine.PutStone(clickHoriz, clickVert))
            {
                AgreeFieldContent();
                switch(playerNumber)
                {
                    case 1:
                        moveGreenList.Items.Add(fieldSymbol(clickHoriz, clickVert));
                        break;
                    case 2:
                        moveBrownList.Items.Add(fieldSymbol(clickHoriz, clickVert));
                        break;

                }
            }
            moveGreenList.SelectedIndex = moveGreenList.Items.Count - 1;
            moveBrownList.SelectedIndex = moveBrownList.Items.Count - 1;

            ReversiEngine.FieldSituation fieldSituation = engine.CheckFieldSituation();
            bool gameOver = false;
            switch(fieldSituation)
            {
                case ReversiEngine.FieldSituation.ActualPlayerCantMove:
                    MessageBox.Show("Gracz " + playerName[engine.NextMovePlayerNumber] + " zmuszony jest do oddania ruchu");
                    engine.Pass();
                    AgreeFieldContent();
                    break;
                case ReversiEngine.FieldSituation.BothPlayerCantMove:
                    MessageBox.Show("Obaj gracze nie mogą wykonac ruchu");
                    gameOver = true;
                    break;
                case ReversiEngine.FieldSituation.AllPointOfFieldAreTaken:
                    gameOver = true;
                    break;
            }

            if (gameOver)
            {
                int winner = engine.NextMovePlayerNumber;
                if (winner != 0)
                    MessageBox.Show("Wygrał gracz " + playerName[winner], Title, MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Remis", Title, MessageBoxButton.OK, MessageBoxImage.Information);

                if (MessageBox.Show("Czy rozpocząć gre od nowa?", "Reversi", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                {
                    PrepareNewGame(1, engine.FieldWidth, engine.FieldHeight);
                }
                else
                {
                    fieldGrid.IsEnabled = false;
                    colorPlayerButton.IsEnabled = false;
                }
            }
        }

        private void PrepareNewGame(int PlayerStartNumber, int fieldWidth = 8, int fieldHeight = 8)
        {
            engine = new ReversiEngineAI(PlayerStartNumber, fieldWidth, fieldHeight);
            moveGreenList.Items.Clear();
            moveBrownList.Items.Clear();
            AgreeFieldContent();
            fieldGrid.IsEnabled = true;
            colorPlayerButton.IsEnabled = true;
        }

        private static string fieldSymbol(int horiz, int vert)
        {
            if (horiz > 25 || vert > 8) return "(" + horiz.ToString() + "," + vert.ToString() + ")";
            return "" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[horiz] + "123456789"[vert];
        }

    }
}
