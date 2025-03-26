using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HospitalD
{
    public partial class StaffPage : Page
    {
        public StaffPage()
        {
            InitializeComponent();
            LoadStaff();
        }

        private void LoadStaff()
        {
            using (var db = new HospitalDRmEntities())
            {
                StaffDataGrid.ItemsSource = db.Staff
                    .Include(s => s.Departments)
                    .Include(s => s.Positions)
                    .Include(s => s.Roles)
                    .ToList();
            }
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditStaffPage());
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (StaffDataGrid.SelectedItem is Staff selectedStaff)
            {
                NavigationService.Navigate(new AddEditStaffPage(selectedStaff));
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для редактирования!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(StaffDataGrid.SelectedItem is Staff selectedStaff))
            {
                MessageBox.Show("Выберите сотрудника для удаления!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Удалить выбранного сотрудника?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            using (var db = new HospitalDRmEntities())
            {
                try
                {
                    db.Entry(selectedStaff).State = EntityState.Deleted;
                    db.SaveChanges();
                    LoadStaff();
                    MessageBox.Show("Сотрудник успешно удален!",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}