using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace HospitalD
{
    /// <summary>
    /// Логика взаимодействия для DepartmentsPage.xaml
    /// </summary>
    public partial class MedicationsPage : Page
    {
        public MedicationsPage()
        {
            InitializeComponent();
            MedicationsDataGrid.ItemsSource = new HospitalDRmEntities().Medications.ToList();
        }
        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {

        }
        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
