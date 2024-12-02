using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YchebPrakGIBDD_integ.Entities;

namespace YchebPrakGIBDD_integ.Windows
{
    /// <summary>
    /// Логика взаимодействия для VYIstoriaStatysaAdd.xaml
    /// </summary>
    public partial class VYIstoriaStatysaAdd : Window
    {
        public VYIstoriaStatysaAdd()
        {
            InitializeComponent();
            LoadComboBoxData();
        }
        private void LoadComboBoxData()
        {
            using (GIBDDEntities db = new GIBDDEntities())
            {
                var statys_VY = db.Statys_VY.ToList();
                Statys_VY_IDCB.ItemsSource = statys_VY;
                var vy = db.Voditelskoe_ydostoverenie.ToList();
                VY_IDCB.ItemsSource = vy;
            }
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы перешли на окно 'Добавление статуса изменения водительского удостоверения'");
        }
        private void ButtonNazad(object sender, RoutedEventArgs e)
        {
            VYIstoriaStatysaWindow vyIstoriaStatysaWindow = new VYIstoriaStatysaWindow();
            vyIstoriaStatysaWindow.Show();
            this.Close();
        }
        private void ButtonAdd(object sender, RoutedEventArgs e)
        {
            var selectedStatys_VY_ID = Statys_VY_IDCB.SelectedItem as Statys_VY;
            var selectedVY_ID = VY_IDCB.SelectedItem as Voditelskoe_ydostoverenie;
            string data_smeni_statysa = Data_smeni_statysaTB.Text;
            string Kommentariy = KommentariyTB.Text;

            if (Statys_VY_IDCB.SelectedItem == null || VY_IDCB.SelectedItem == null
                || Data_smeni_statysaTB.Text == null)
            {
                MessageBox.Show("Заполните поля! Все поля, кроме 'Комментарий', обязательны",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Regex.IsMatch(data_smeni_statysa, @"^\d{2}\.\d{2}\.\d{4}$") // Формат ДД.ММ.ГГГГ
                || !int.TryParse(data_smeni_statysa.Split('.')[0], out int day) // День
                || !int.TryParse(data_smeni_statysa.Split('.')[1], out int month) // Месяц
                || !int.TryParse(data_smeni_statysa.Split('.')[2], out int year) // Год
                || year < 1920 || year > 2024 // Проверка года
                || month < 1 || month > 12 // Проверка месяца
                || day < 1 || day > DateTime.DaysInMonth(year, month)) // Проверка дня
            {
                MessageBox.Show("Дата смены статуса введена некорректно. Убедитесь, что она в формате ДД.ММ.ГГГГ и соответствует календарю (год от 1920 до 2024).",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Kommentariy.Length > 30)
            {
                MessageBox.Show("Слишком длинный комментарий!",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (GIBDDEntities db = new GIBDDEntities())
                {
                    int maxId = db.VY_Istoria_Statysa.Any() ? db.VY_Istoria_Statysa.Max(v
                                => v.ID) : 0;

                    // Создаем новый объект Voditel
                    VY_Istoria_Statysa vyis = new VY_Istoria_Statysa
                    {
                        ID = maxId + 1,
                        Statys_VY_ID = selectedStatys_VY_ID.ID,
                        VY_ID = selectedVY_ID.ID,
                        Data_smeni_statysa = data_smeni_statysa,
                        Kommentariy = Kommentariy
                    };

                    db.VY_Istoria_Statysa.Add(vyis);
                    db.SaveChanges();
                    MessageBox.Show("История статуса водительское удостоверение успешно добавлена", "Успех",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        sb.AppendLine($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                    }
                }
                MessageBox.Show("Произошла ошибка при сохранении изменений: " + sb.ToString(),
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            VYIstoriaStatysaWindow vyIstoriaStatysaWindow = new VYIstoriaStatysaWindow();
            vyIstoriaStatysaWindow.Show();
            this.Close();
        }
    }
}