using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HospitalD
{
    public partial class MedicalProceduresPage : Page
    {
        private HospitalDRmEntities _db = new HospitalDRmEntities();

        public MedicalProceduresPage()
        {
            InitializeComponent();
            LoadMedicalProcedures();
        }

        private void LoadMedicalProcedures()
        {
            MedicalProceduresDataGrid.ItemsSource = _db.MedicalProcedures
                .Include(m => m.Staff)
                .ToList();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedProcedure = MedicalProceduresDataGrid.SelectedItem as MedicalProcedures;
            if (selectedProcedure != null)
            {
                NavigationService.Navigate(new AddEditMedicalProcedurePage(selectedProcedure));
            }
            else
            {
                MessageBox.Show("Выберите процедуру для редактирования!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditMedicalProcedurePage());
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedProcedure = MedicalProceduresDataGrid.SelectedItem as MedicalProcedures;
            if (selectedProcedure == null)
            {
                MessageBox.Show("Выберите процедуру для удаления!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить процедуру '{selectedProcedure.Name}'?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                _db.MedicalProcedures.Remove(selectedProcedure);
                _db.SaveChanges();
                LoadMedicalProcedures();
                MessageBox.Show("Процедура успешно удалена!",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}