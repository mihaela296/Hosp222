using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace HospitalD
{
    public partial class PositionsPage : Page
    {
        private HospitalDRmEntities _db = new HospitalDRmEntities();

        public PositionsPage()
        {
            InitializeComponent();
            LoadPositions();
        }


        private void LoadPositions()
        {
            // Отключаем отслеживание изменений для избежания конфликтов
            PositionsDataGrid.ItemsSource = _db.Positions.AsNoTracking().ToList();
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedPosition = PositionsDataGrid.SelectedItem as Positions;
            if (selectedPosition != null)
            {
                // Передаем только ID для избежания конфликтов контекста
                NavigationService.Navigate(new AddEditPositionPage(new Positions { ID_Position = selectedPosition.ID_Position }));
            }
            else
            {
                MessageBox.Show("Выберите должность для редактирования!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Остальные методы остаются без изменений
        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPositionPage());
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedPosition = PositionsDataGrid.SelectedItem as Positions;
            if (selectedPosition == null)
            {
                MessageBox.Show("Выберите должность для удаления!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить должность '{selectedPosition.Name}'?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                var positionToDelete = _db.Positions.Find(selectedPosition.ID_Position);
                if (positionToDelete != null)
                {
                    _db.Positions.Remove(positionToDelete);
                    _db.SaveChanges();
                    LoadPositions();
                    MessageBox.Show("Должность успешно удалена!",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.InnerException?.Message ?? ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}