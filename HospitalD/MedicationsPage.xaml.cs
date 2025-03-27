using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HospitalD
{
    public partial class MedicationsPage : Page
    {
        private HospitalDRmEntities _db = new HospitalDRmEntities();

        public MedicationsPage()
        {
            InitializeComponent();
            LoadMedications();
        }

        private void LoadMedications()
        {
            MedicationsDataGrid.ItemsSource = _db.Medications.ToList();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedMedication = (Medications)MedicationsDataGrid.SelectedItem;
            if (selectedMedication != null)
            {
                NavigationService.Navigate(new AddEditMedicationPage(selectedMedication));
            }
            else
            {
                MessageBox.Show("Выберите лекарство для редактирования!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditMedicationPage());
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedMedication = (Medications)MedicationsDataGrid.SelectedItem;
            if (selectedMedication == null)
            {
                MessageBox.Show("Выберите лекарство для удаления!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить лекарство '{selectedMedication.Name}'?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                _db.Medications.Remove(selectedMedication);
                _db.SaveChanges();
                LoadMedications();
                MessageBox.Show("Лекарство успешно удалено!",
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