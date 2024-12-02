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
    /// Логика взаимодействия для VYIstoriaStatysaChange.xaml
    /// </summary>
    public partial class VYIstoriaStatysaChange : Window
    {
        private VY_Istoria_Statysa selectedVYIS;
        public VYIstoriaStatysaChange(VY_Istoria_Statysa selectedVYIS)
        {
            InitializeComponent();
            this.selectedVYIS = selectedVYIS;
            LoadComboBox();
            LoadTextBox();
        }
        public class VYDisplayItem
        {
            public int ID { get; set; }
            public string DisplayValue { get; set; }
        }
        private void LoadComboBox()
        {
            using (var db = new GIBDDEntities())
            {
                var statys_VY = db.Statys_VY.ToList();
                Statys_VY_IDCB.ItemsSource = statys_VY;
                Statys_VY_IDCB.DisplayMemberPath = "Nazvanie";
                Statys_VY_IDCB.SelectedValuePath = "ID";

                var VY = db.Voditelskoe_ydostoverenie
                            .Select(s => new VYDisplayItem
                            {
                                ID = s.ID,
                                DisplayValue = s.Seria + " " + s.Nomer
                            })
                            .ToList();
                VY_IDCB.ItemsSource = VY;
            }
        }

        private void VY_IDCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedVY_ID = VY_IDCB.SelectedItem as VYDisplayItem;
            if (selectedVY_ID != null)
            {
                MessageBox.Show("Selected VY_ID: " + selectedVY_ID.ID);
            }
            else
            {
                MessageBox.Show("No selection made.");
            }
        }
        private void LoadTextBox()
        {
            Statys_VY_IDCB.SelectedValue = selectedVYIS.Statys_VY_ID;
            VY_IDCB.SelectedValue = selectedVYIS.VY_ID;
            Data_smeni_statysaTB.Text = selectedVYIS.Data_smeni_statysa;
            KommentariyTB.Text = selectedVYIS.Kommentariy;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы перешли на окно 'Изменение истории статуса водительского удостоверения'");
        }
        private void ButtonNazad(object sender, RoutedEventArgs e)
        {
            VYIstoriaStatysaWindow vyIstoriaStatysaWindow = new VYIstoriaStatysaWindow();
            vyIstoriaStatysaWindow.Show();
            this.Close();
        }

        private void ButtonChange(object sender, RoutedEventArgs e)
        {
            var selectedStatys_VY_ID = Statys_VY_IDCB.SelectedItem as Statys_VY;
            var selectedVY_ID = VY_IDCB.SelectedItem as VYDisplayItem;
            string data_smeni_statysa = Data_smeni_statysaTB.Text;
            string Kommentariy = KommentariyTB.Text;

            if (data_smeni_statysa == null || selectedStatys_VY_ID == null || selectedVY_ID == null)
            {
                MessageBox.Show("Заполните поля! Все поля, кроме 'Комментарий' обязательны");
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

            VY_Istoria_Statysa existingVYIS = null;
            try
            {
                using (GIBDDEntities db = new GIBDDEntities())
                {
                    existingVYIS = db.VY_Istoria_Statysa.FirstOrDefault(u => u.ID == selectedVYIS.ID);

                    if (existingVYIS != null)
                    {
                        if (existingVYIS.Statys_VY_ID != selectedStatys_VY_ID.ID ||
                            existingVYIS.VY_ID != selectedVY_ID.ID ||
                            existingVYIS.Data_smeni_statysa != data_smeni_statysa ||
                            existingVYIS.Kommentariy != Kommentariy)
                        {
                            existingVYIS.Statys_VY_ID = selectedStatys_VY_ID.ID;
                            existingVYIS.VY_ID = selectedVY_ID.ID;
                            existingVYIS.Data_smeni_statysa = data_smeni_statysa;
                            existingVYIS.Kommentariy = Kommentariy;
                            db.SaveChanges();
                            MessageBox.Show("Данные успешно обновлены.");
                        }
                        else
                        {
                            MessageBox.Show("Нет изменений.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Запись не найдена.");
                    }
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
                MessageBox.Show("Произошла ошибка при сохранении изменений: " + sb.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            VYIstoriaStatysaWindow vyIstoriaStatysaWindow = new VYIstoriaStatysaWindow();
            vyIstoriaStatysaWindow.Show();
            this.Close();
        }



    }
}

