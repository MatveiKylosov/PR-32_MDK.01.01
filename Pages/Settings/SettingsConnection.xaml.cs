using System;
using System.Collections.Generic;
using System.Data.Common;
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

namespace VinylRecordsApplication.Pages.Settings
{
    /// <summary>
    /// Логика взаимодействия для SettingsConnection.xaml
    /// </summary>
    public partial class SettingsConnection : Page
    {
        public SettingsConnection()
        {
            InitializeComponent();
            tbServer.Text = Classes.DBConnection.ConnectionSettings.Split(';')[0].Substring(7);
            tbName.Text = Classes.DBConnection.ConnectionSettings.Split(';')[2].Substring(9);
            tbUser.Text = Classes.DBConnection.ConnectionSettings.Split(';')[3].Substring(5);
            tbPWD.Text = Classes.DBConnection.ConnectionSettings.Split(';')[4].Substring(4);
        }

        private void ApplySettings(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(tbServer.Text) || string.IsNullOrEmpty(tbUser.Text) || string.IsNullOrEmpty(tbPWD.Text) || string.IsNullOrEmpty(tbName.Text))
            {
                MessageBox.Show($"Заполните полностью все поля!");
                return;
            }

            Classes.DBConnection.ConnectionSettings = $"server={tbServer.Text};Trusted_Connection=No;DataBase={tbName.Text};User={tbUser.Text};PWD={tbPWD.Text}";
            MessageBox.Show("Настройки были применены!");
        }
    }
}
