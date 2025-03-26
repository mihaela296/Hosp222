using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HospitalD
{
    public partial class DepartmentsPage : Page
    {
        public DepartmentsPage()
        {
            InitializeComponent();
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            using (var db = new HospitalDRmEntities())
            {
                DepartmentsDataGrid.ItemsSource = db.Departments.ToList();
            }
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedDepartment = DepartmentsDataGrid.SelectedItem as Departments;

            if (selectedDepartment != null)
            {
                NavigationService.Navigate(new AddEditDepartmentPage(selectedDepartment));
            }
            else
            {
                MessageBox.Show("Выберите отделение для редактирования!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditDepartmentPage(null));
        }

        private void ButtonDel_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedDepartment = DepartmentsDataGrid.SelectedItem as Departments;

            if (selectedDepartment == null)
            {
                MessageBox.Show("Выберите отделение для удаления!",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить это отделение?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            using (var context = new HospitalDRmEntities())
            {
                try
                {
                    // 1. Получаем полную сущность из базы
                    var departmentToDelete = context.Departments
                        .FirstOrDefault(d => d.ID_Department == selectedDepartment.ID_Department);

                    if (departmentToDelete == null)
                    {
                        MessageBox.Show("Отделение не найдено в базе данных!",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // 2. Проверяем связанные данные (врачи и процедуры)
                    bool hasDoctors = context.Staff.Any(d => d.ID_Department == departmentToDelete.ID_Department);
                    bool hasProcedures = context.MedicalProcedures.Any(p => p.ID_Staff == departmentToDelete.ID_Department);

                    if (hasDoctors || hasProcedures)
                    {
                        string message = "Невозможно удалить отделение, так как существуют связанные данные:\n";
                        if (hasDoctors) message += "- Врачи в этом отделении\n";
                        if (hasProcedures) message += "- Медицинские процедуры\n";

                        MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // 3. Удаляем отделение
                    context.Departments.Remove(departmentToDelete);
                    context.SaveChanges();

                    // 4. Обновляем список
                    DepartmentsDataGrid.ItemsSource = new HospitalDRmEntities().Departments.ToList();

                    MessageBox.Show("Отделение успешно удалено!",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException dbEx)
                {
                    // Обработка ошибок базы данных
                    MessageBox.Show($"Ошибка базы данных при удалении: {dbEx.InnerException?.Message ?? dbEx.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}