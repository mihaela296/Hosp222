using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HospitalD
{
    public partial class DiagnosesPage : Page
    {
        private HospitalDRmEntities _db = new HospitalDRmEntities();

        public DiagnosesPage()
        {
            InitializeComponent();
            LoadDiagnoses();
        }

        private void LoadDiagnoses()
        {
            // Явно загружаем связанные данные отделений
            DiagnosesDataGrid.ItemsSource = _db.Diagnoses
                .Include(d => d.Departments)
                .AsNoTracking()
                .ToList();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedDiagnosis = DiagnosesDataGrid.SelectedItem as Diagnoses;
            if (selectedDiagnosis != null)
            {
                NavigationService.Navigate(new AddEditDiagnosesPage(selectedDiagnosis));
            }
            else
            {
                MessageBox.Show("Выберите диагноз для редактирования!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditDiagnosesPage());
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedDiagnosis = DiagnosesDataGrid.SelectedItem as Diagnoses;
            if (selectedDiagnosis == null)
            {
                MessageBox.Show("Выберите диагноз для удаления!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить диагноз '{selectedDiagnosis.Name}'?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                _db.Diagnoses.Remove(selectedDiagnosis);
                _db.SaveChanges();
                LoadDiagnoses();
                MessageBox.Show("Диагноз успешно удален!",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.InnerException?.Message ?? ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}