using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MyDocumentView.xaml 的互動邏輯
    /// </summary>
    public partial class MyDocumentView : Window
    {
        public MyDocumentView()
        {
            InitializeComponent();

            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                CMBFontStyle.Items.Add(fontFamily);
            }
            CMBFontStyle.SelectedIndex = 0;

            CMBFontSize.ItemsSource = new List<double>() {8, 9, 10, 12, 14, 16, 18, 20, 24, 28, 32, 64, 128, 256};
            CMBFontSize.SelectedIndex = 3;
        }

        private void RichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Color FontColor = Colors.Black;

            Object propety = RTB.Selection.GetPropertyValue(TextElement.FontWeightProperty);

            Bold_Button.IsChecked = (propety != DependencyProperty.UnsetValue) && (propety.Equals(FontWeights.Bold));

            propety = RTB.Selection.GetPropertyValue(TextElement.FontStyleProperty);
            Italic_Button.IsChecked = (propety != DependencyProperty.UnsetValue) && (propety.Equals(FontStyles.Italic));

            propety = RTB.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            Underline_Button.IsChecked = (propety != DependencyProperty.UnsetValue) && (propety.Equals(TextDecorations.Underline));

            propety = RTB.Selection.GetPropertyValue(TextElement.FontFamilyProperty);
            CMBFontStyle.SelectedItem = propety;

            propety = RTB.Selection.GetPropertyValue(TextElement.FontSizeProperty);
            CMBFontSize.SelectedItem = propety;

            SolidColorBrush foregroundPropety = RTB.Selection.GetPropertyValue(Inline.ForegroundProperty) as SolidColorBrush;
            if (foregroundPropety != null)
            {
                Font_Color_Picker.SelectedColor = foregroundPropety.Color;
            }

        }

        private void CMBFontStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CMBFontStyle.SelectedItem != null)
            {
                RTB.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, CMBFontStyle.SelectedItem);
            }
        }

        private void CMBFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CMBFontSize.SelectedItem != null)
            {
                RTB.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, CMBFontSize.SelectedValue);
            }
        }

        private void Font_Color_Picker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color fontcolor = (Color)e.NewValue;
            SolidColorBrush fontBrush = new SolidColorBrush(fontcolor);
            RTB.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, fontBrush);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            RTB.Document.Blocks.Clear();
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Rich text format|*.rt|HTML|*.html|All file|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
                Title = openFileDialog.FileName;
                TextRange range = new TextRange(RTB.Document.ContentStart, RTB.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MyDocumentView myDocumentView = new MyDocumentView();
            myDocumentView.Show();
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Rich text format|*.rt|HTML|*.html|All file|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create);
                TextRange range = new TextRange(RTB.Document.ContentStart, RTB.Document.ContentEnd);
                range.Save(fs, DataFormats.Rtf);
            }
        }

        private void BackGroundColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color backcolor = (Color)e.NewValue;
            SolidColorBrush backBrush = new SolidColorBrush(backcolor);
            //RTB.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, fontBrush);
            RTB.Background = backBrush;
        }
    }
}
