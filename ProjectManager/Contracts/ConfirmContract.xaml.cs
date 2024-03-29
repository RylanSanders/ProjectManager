﻿using ProjectManager.Utils;
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
using System.Windows.Shapes;

namespace ProjectManager.Contracts
{
    /// <summary>
    /// Interaction logic for ConfirmContract.xaml
    /// </summary>
    public partial class ConfirmContract : Window
    {

        public enum ConfirmCategories
        {
            ConfirmCancel, OK,Cancel
        }
        public ConfirmContract(string ConfirmTexts, ConfirmCategories type=ConfirmCategories.ConfirmCancel)
        {
            InitializeComponent();
            ConfirmTextBlock.Text = ConfirmTexts;
            if (type == ConfirmCategories.OK)
            {
                CancelButton.Visibility = Visibility.Collapsed;
                Grid.SetColumnSpan(OKButton, 2);
            }
            else if (type == ConfirmCategories.Cancel)
            {
                OKButton.Visibility = Visibility.Collapsed;
                Grid.SetColumn(CancelButton, 0);
                Grid.SetColumnSpan(CancelButton, 2);
                CancelButton.Content = "OK";
            }
            WindowUtil.ApplyDarkWindowStyle(this);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
