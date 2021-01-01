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

namespace Reversi
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ReversiEngine engine = new ReversiEngine(1);
        private SolidColorBrush[] colors = { Brushes.Ivory, Brushes.Green, Brushes.Sienna };
        string[] playername = { "", "green", "brown" };
        private Button[,] field;
        private bool IsFieldInitialized
        {
            get
            {
                return field[engine.FieldWidth - 1, engine.FieldHeight - 1] != null;
            }
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
                    field[i, j] = button;
                }
            }
            AgreeFieldContent();
        }
    }
}
