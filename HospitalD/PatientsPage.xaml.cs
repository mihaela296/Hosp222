using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public partial class PatientsPage : Page
    {

        public PatientsPage()
        {
            InitializeComponent();
            PatientsDataGrid.ItemsSource = new HospitalDRmEntities().Patients.ToList();
        }
        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedPatient = (Patients)PatientsDataGrid.SelectedItem;

            if(selectedPatient!= null)
            {
                NavigationService.Navigate(new AddEditPatientPage(selectedPatient));
            }
            else
            {
                MessageBox.Show("выберите пациента для редактирования!","Ошибка",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPatientPage(null));
        }
        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedPatient = (Patients)PatientsDataGrid.SelectedItem;

            if (selectedPatient == null)
            {
                MessageBox.Show("Выберите пациента для удаления!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new HospitalDRmEntities())
            {
                try
                {
                    // 1. Получаем пациента из базы, чтобы EF начал его отслеживать
                    var patientToDelete = context.Patients
                        .FirstOrDefault(p => p.ID_Patient == selectedPatient.ID_Patient);

                    if (patientToDelete == null)
                    {
                        MessageBox.Show("Пациент не найден в базе данных!", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // 2. Проверка связанных данных
                    var hasRelatedData = context.Users.Any(u => u.ID_User == patientToDelete.ID_Patient);
                    if (hasRelatedData)
                    {
                        MessageBox.Show("Невозможно удалить пациента, так как у него есть связанные данные!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // 3. Удаление
                    context.Patients.Remove(patientToDelete);
                    context.SaveChanges();

                    // 4. Обновление DataGrid
                    PatientsDataGrid.ItemsSource = new HospitalDRmEntities().Patients.ToList();

                    MessageBox.Show("Пациент успешно удален!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
