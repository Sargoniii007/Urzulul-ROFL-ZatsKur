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

namespace Urzulul_ROFL_.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        public ServicesPage()
        {
            InitializeComponent();

            LViewServices.ItemsSource = App.Context.Service.ToList();

            ComboDiscount.SelectedIndex = 0;
            ComboSortBy.SelectedIndex = 0;
            UpdateServices();
        }

        private void btnAddService_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditServicePage());
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void ComboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ComboDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void UpdateServices()
        {
            var services = App.Context.Service.ToList();

            if (ComboSortBy.SelectedIndex == 0)
            {
                services = services.OrderBy(p => p.CostWithDiscount).ToList();
            }
            else
            {
                services = services.OrderByDescending(p => p.CostWithDiscount).ToList();
            }

            if (ComboDiscount.SelectedIndex == 1)
            {
                services = services.Where(p => p.Discount >= 0 && p.Discount < 0.05).ToList();
            }
            if (ComboDiscount.SelectedIndex == 2)
            {
                services = services.Where(p => p.Discount >= 0.05 && p.Discount < 0.15).ToList();

            }
            if (ComboDiscount.SelectedIndex == 3)
            {
                services = services.Where(p => p.Discount >= 0.15 && p.Discount < 0.30).ToList();
            }
            if (ComboDiscount.SelectedIndex == 4)
            {
                services = services.Where(p => p.Discount >= 0.30 && p.Discount < 0.70).ToList();
            }
            if (ComboDiscount.SelectedIndex == 5)
            {
                services = services.Where(p => p.Discount >= 0.70 && p.Discount < 1).ToList();
            }

            services = services.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            LViewServices.ItemsSource = services;
        }

        private void LViewServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentService = (sender as Button).DataContext as Entities.Service;
            if (MessageBox.Show($"Вы уверенны, что хотите удалить услугу: {currentService.Title}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                App.Context.Service.Remove(currentService);
                App.Context.SaveChanges();
                UpdateServices();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var currentService = (sender as Button).DataContext as Entities.Service;
            NavigationService.Navigate(new AddEditServicePage(currentService));
        }
    }
}
