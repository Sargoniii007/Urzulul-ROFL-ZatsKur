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
    /// Логика взаимодействия для AddEditServicePage.xaml
    /// </summary>
    public partial class AddEditServicePage : Page
    {
        private Entities.Service _correntService = null;
        public AddEditServicePage()
        {
            InitializeComponent();

        }
        public AddEditServicePage(Entities.Service service)
        {
            InitializeComponent();

            _correntService = service;
            Title = "Редоктировать услугу";
            TBoxTitle.Text = _correntService.Title;
            TBoxCost.Text = _correntService.Cost.ToString("N2");
            TBoxDuration.Text = (_correntService.DurationInSeconds / 60).ToString();
            TBoxDescription.Text = _correntService.Description;
            if (_correntService.Discount > 0)
                TBoxDiscount.Text = (_correntService.Discount.Value * 100).ToString();
            if (_correntService.MainImage != null)
                ImageService.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(_correntService.MainImage);

             

            
        }
        private string CheckErrors()
        {
            var errorBulder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(TBoxTitle.Text))
                errorBulder.AppendLine("Название услуги обязательно для заполнения");
            var serviceFromBD = App.Context.Service.ToList().FirstOrDefault(p => p.Title.ToLower() == TBoxTitle.Text.ToLower());
            if (serviceFromBD != null && serviceFromBD != _correntService)
                errorBulder.AppendLine("Такая услуга уже есть в базе данных");

            decimal cost = 0;
            if (decimal.TryParse(TBoxCost.Text, out cost) == false || cost <= 0)
                errorBulder.AppendLine("Стоимость услуги должна быть положительным числом");

            int durationInMinets = 0;
            if (int.TryParse(TBoxDuration.Text,out durationInMinets) == false || durationInMinets > 240 || durationInMinets <= 0)
            {
                errorBulder.AppendLine("Длительость оказания услуги должна быть положительным числом (не больше, чем 4 часа)");
            }
            if (!string.IsNullOrEmpty(TBoxDiscount.Text))
            {
                int discount = 0;
                if (int.TryParse(TBoxDiscount.Text,out discount)== false || discount < 0 || discount > 100)
                {
                    errorBulder.AppendLine("Размер скидки - целое число от 0 до 100%");
                }
            }
            if (errorBulder.Length > 0)
                errorBulder.Insert(0, "Устраниете следующие ошибки: \n");
            return errorBulder.ToString();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errorMas = CheckErrors();
            if(errorMas.Length > 0)
            {
                MessageBox.Show(errorMas, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            { if (_correntService == null)
                {

                    var service = new Entities.Service
                    {
                        Title = TBoxTitle.Text,
                        Cost = decimal.Parse(TBoxCost.Text),
                        DurationInSeconds = int.Parse(TBoxDuration.Text) * 60,
                        Description = TBoxDescription.Text,
                        Discount = string.IsNullOrWhiteSpace(TBoxDiscount.Text) ? 0 : double.Parse(TBoxDiscount.Text) / 100
                    };
                    App.Context.Service.Add(service);
                    App.Context.SaveChanges();
                    
                }
            else
                {
                    _correntService.Title = TBoxTitle.Text;
                    _correntService.Cost = decimal.Parse(TBoxCost.Text);
                    _correntService.DurationInSeconds = int.Parse(TBoxDuration.Text);
                    _correntService.Description = TBoxDescription.Text;
                    _correntService.Discount = string.IsNullOrWhiteSpace(TBoxDiscount.Text) ? 0 : double.Parse(TBoxDiscount.Text) / 100;
                    App.Context.SaveChanges();
                }
                NavigationService.GoBack();
            }
        }
    }
}
