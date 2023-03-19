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
using System.IO;
using Microsoft.Win32;
using System.Drawing.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace NotePad
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source); // Создание списка шрифтов
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 }; // Заранее заданные размеры шрифта
            cmbFontFamily.SelectedIndex = 0; // шрифт Arial
            cmbFontSize.SelectedIndex = 5; // размер 14 пт
            cmbColor.SelectedIndex = 0;


            // Создаем список имеющихся цветов
            List<string> colorList = new List<string>();
            var color = typeof(System.Drawing.KnownColor);
            var colors = Enum.GetValues(color);
            var from = 27;
            var to = colors.Length - 7;
            for (int i = from; i < to; i++)
            {
                colorList.Add(colors.GetValue(i).ToString());
            }
            cmbColor.ItemsSource = colorList.ToArray();
            cmbColor.SelectedItem = "AliceBlue";

            btnBold.IsChecked = false; 
            btnItalic.IsChecked = false;
            btnUnderline.IsChecked = false;
            PreviousTextRead();

            this.Background = Brushes.Bisque;
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string fileName = "myFile.txt";
            string text = textBox.FontFamily.ToString() + "\n" + textBox.FontSize.ToString() + "\n" + cmbColor.Text + "\n" + textBox.Text;
            string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            lines = lines.Where(line => !string.IsNullOrEmpty(line)).ToArray();

            File.WriteAllLines(fileName, lines);
        }

        private void PreviousTextRead()
        {
            string fileName = "myFile.txt";
            if (System.IO.File.Exists(fileName))
            {
                string[] fileLines = System.IO.File.ReadAllLines(fileName);

                cmbFontFamily.Text = fileLines[0];
                cmbFontSize.Text = fileLines[1];
                cmbColor.Text = fileLines[2];
               
                for (int i = 3; i < fileLines.Length; i++)
                {
                    textBox.Text += fileLines[i] + "\n";

                }
            }
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                textBox.FontFamily = new FontFamily(cmbFontFamily.SelectedItem.ToString()); 
        }


        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cmbFontSize.SelectedItem != null)
                textBox.FontSize = double.Parse(cmbFontSize.Text);
            else if (cmbFontSize.Text != "" && cmbFontSize.Text != "0")
            {
                textBox.FontSize = double.Parse(cmbFontSize.Text);
            }
        }

        private void cmbColor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (cmbColor.SelectedItem != null)
            {
                toolBar.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(cmbColor.SelectedItem.ToString());
                dockPanel.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(cmbColor.SelectedItem.ToString());
            }    
               
        }

        private void btnBold_Checked(object sender, RoutedEventArgs e)
        {
            textBox.FontWeight = FontWeights.Bold;
        }

        private void btnBold_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox.FontWeight = FontWeights.Thin;
        }

        private void btnItalic_Checked(object sender, RoutedEventArgs e)
        {
            textBox.FontStyle = FontStyles.Italic;
        }

        private void btnItalic_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox.FontStyle = FontStyles.Normal;
        }

        private void btnUnderline_Checked(object sender, RoutedEventArgs e)
        {
            textBox.TextDecorations = TextDecorations.Underline;
        }

        private void btnUnderline_Unchecked(object sender, RoutedEventArgs e)
        {
            textBox.TextDecorations = null;
        }
    }

}

