using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KalkulatorWalut
{
    /// <summary>
    /// Interaction logic for KantorWalutWindow.xaml
    /// </summary>
    public partial class KantorWalutWindow : Window
    {
        KantorWalutModelView modelView=new KantorWalutModelView();
        public KantorWalutWindow()
        {
            InitializeComponent();
            DataContext = modelView;
        }

        private void changeOption_Click(object sender, RoutedEventArgs e)
        {
            if (modelView.currencySelectedIGetForeignCurrency==true)
            {
                modelView._inputDecimal = 0;
                posiadamLabel.Visibility = Visibility.Collapsed;
                IGetForeignCurrency.Visibility = Visibility.Collapsed;
                otrzymamLabel.Visibility = Visibility.Visible;
                IHaveForeignCurrency.Visibility = Visibility.Visible;
                IHaveForeignCurrency.SelectedItem = IGetForeignCurrency.SelectedItem;
                modelView.input = string.Empty;
                modelView.output = string.Empty;

                modelView.currencySelectedIHaveForeignCurrency = true;
                modelView.currencySelectedIGetForeignCurrency = false;


                return;
            }
            if (modelView.currencySelectedIHaveForeignCurrency==true)
            {
                modelView._inputDecimal = 0;
                posiadamLabel.Visibility = Visibility.Visible;
                IGetForeignCurrency.Visibility = Visibility.Visible;
                otrzymamLabel.Visibility = Visibility.Collapsed;
                IHaveForeignCurrency.Visibility = Visibility.Collapsed;
                IGetForeignCurrency.SelectedItem = IHaveForeignCurrency.SelectedItem;
                modelView.input = string.Empty;
                modelView.output = string.Empty;
                modelView.currencySelectedIHaveForeignCurrency = false;
                modelView.currencySelectedIGetForeignCurrency = true;
                return;
            }

        }

        private void IGetForeignCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (modelView.inputCorrect==true)
            {
                modelView.CalculateOutput(IGetForeignCurrency.SelectedItem,IGetForeignCurrency.Name);
            }
           
        }
        private void IHaveForeignCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (modelView.inputCorrect == true)
            {
                modelView.CalculateOutput(IHaveForeignCurrency.SelectedItem,IHaveForeignCurrency.Name);
            }
        }


        private void iHave_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            incorrectValues.Visibility = Visibility.Hidden;
            if (textBox.Text==string.Empty)
            {
                textBox.Text = "0";
                return;
            }
            modelView.CheckInputValue(textBox.Text);
            if (modelView.inputCorrect == false)
            {
                incorrectValues.Visibility = Visibility.Visible;
                OtrzymamWrapPanel.Visibility = Visibility.Collapsed;
                changeOption.Visibility = Visibility.Collapsed;
                return;
            }
            if (modelView.inputCorrect == true)
            {
                incorrectValues.Visibility = Visibility.Collapsed;
                OtrzymamWrapPanel.Visibility = Visibility.Visible;
                changeOption.Visibility = Visibility.Visible;
            }
            if (modelView.currencySelectedIGetForeignCurrency==true&& IGetForeignCurrency.SelectedItem!=null)
            {
                modelView.CalculateOutput(IGetForeignCurrency.SelectedItem,IGetForeignCurrency.Name);
            }
            if (modelView.currencySelectedIHaveForeignCurrency == true && IHaveForeignCurrency.SelectedItem != null)
            {
                modelView.CalculateOutput(IHaveForeignCurrency.SelectedItem,IHaveForeignCurrency.Name);
            }
        }

    }
}
