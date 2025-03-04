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

namespace _41_размер_Shennikova
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private int failedAttempts = 0;
        private string generatedCaptcha = "";

        public AuthPage()
        {
            InitializeComponent();
        }


        private void ButtonLоgin_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text.Trim();
            string password = Password.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Есть пустые поля!");
                return;
            }

            // Проверка логина и пароля в БД
            User user = Shennikova41Entities.GetContext().User.FirstOrDefault(p => p.UserLogin == login && p.UserPassword == password);

            if (user != null)
            {
                MessageBox.Show("Успешный вход!");
                Manager.MainFrame.Navigate(new ProductPage(user));
                ResetAuthForm();
            }
            else
            {
                failedAttempts++;

                if (failedAttempts == 1)
                {
                    MessageBox.Show("Введены неверные данные! Введите капчу.");
                    GenerateCaptcha();
                    CaptchaPanel.Visibility = Visibility.Visible;
                }
                else if (failedAttempts >= 2)
                {
                    if (CaptchaInput.Text.Trim() != generatedCaptcha)
                    {
                        MessageBox.Show("Капча введена неверно! Блокировка входа на 10 секунд.");
                        BlockLoginButton();
                    }
                    else
                    {
                        MessageBox.Show("Успешная проверка капчи, но логин или пароль неверны!");
                    }
                }
            }
        }

        private void GenerateCaptcha()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random rand = new Random();
            generatedCaptcha = new string(Enumerable.Range(0, 4).Select(_ => chars[rand.Next(chars.Length)]).ToArray());

            // Устанавливаем символы в TextBlock и меняем их позиции
            captchaOne.Text = generatedCaptcha[0].ToString();
            captchaTwo.Text = generatedCaptcha[1].ToString();
            captchaThree.Text = generatedCaptcha[2].ToString();
            captchaFour.Text = generatedCaptcha[3].ToString();

         
        }

        private async void BlockLoginButton()
        {
            ButtonLоgin.IsEnabled = false;
            await Task.Delay(10000); // Блокировка на 10 секунд
            ButtonLоgin.IsEnabled = true;
        }
        private void ResetAuthForm()
        {
            Login.Clear();
            Password.Clear();
            CaptchaInput.Clear();
            CaptchaPanel.Visibility = Visibility.Collapsed;
            failedAttempts = 0;
        }
        private void Gost_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ProductPage(null));
            ResetAuthForm();
        }
    }
}
