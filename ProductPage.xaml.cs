using System;
using System.Collections.Generic;
using System.Data;
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

namespace _41_размер_Shennikova
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {

        int CountRecords;//Количество записей в таблице
        int CountPage;//Общее количество страниц
        int CurrentPage = 0;//Текущая страница

        List<Product> CurrentPageList = new List<Product>();
        List<Product> TableList;

        private User _user;
        public ProductPage(User user)
        {
            string logi;
            string rol;
            InitializeComponent();
            _user = user;
            var currentProducts = Shennikova41Entities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currentProducts;
            if (user != null)
            {
                logi = user.UserSurname + user.UserName + user.UserPatronymic;
                Imya.Text = logi;
                switch (user.UserRole)
                {
                    case 1:
                        rol = "Клиент";
                        break;
                    case 2:
                        rol = "Менеджер";
                        break;
                    case 3:
                        rol = "Администратор";
                        break;
                    default:
                        rol = "Гость";
                        break;

                }
            }
            else
            {
                rol = "Гость";
            }

            Role.Text = rol;


            UpdateProduct();
        }

        
        

        private void UpdateProduct()

        {
            
            var currentProducts = Shennikova41Entities.GetContext().Product.ToList();

            

            if (ComboType.SelectedIndex == 0)
            {
                currentProducts = currentProducts.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 0 && Convert.ToInt32(p.ProductMaxDiscount) <= 100)).ToList();
            }

            if (ComboType.SelectedIndex == 1)
            {
                currentProducts = currentProducts.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 0 && Convert.ToInt32(p.ProductMaxDiscount) < 9.99)).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentProducts = currentProducts.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 10 && Convert.ToInt32(p.ProductMaxDiscount) < 14.99)).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentProducts = currentProducts.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 15 && Convert.ToInt32(p.ProductMaxDiscount) < 100)).ToList();
            }


            currentProducts = currentProducts.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            ProductListView.ItemsSource = currentProducts.ToList();

            if (RButtonDown.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderByDescending(p => p.ProductCost).ToList();
            }
            if (RButtonUp.IsChecked.Value)
            {
                currentProducts = currentProducts.OrderBy(p => p.ProductCost).ToList();

            }


            ProductListView.ItemsSource = currentProducts;
            TableList = currentProducts;

          
            Count.Text = "Количество " + currentProducts.Count.ToString() + " из " + Shennikova41Entities.GetContext().Product.ToList().Count.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();
        }
        

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProduct();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProduct();
        }
    }
}
