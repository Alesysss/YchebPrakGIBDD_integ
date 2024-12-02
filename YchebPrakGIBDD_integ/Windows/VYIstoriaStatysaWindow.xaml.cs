using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using YchebPrakGIBDD_integ.Entities;

namespace YchebPrakGIBDD_integ.Windows
{
    /// <summary>
    /// Логика взаимодействия для VYIstoriaStatysaWindow.xaml
    /// </summary>
    public partial class VYIstoriaStatysaWindow : Window
    {
        private int VYIS_Statys_VY_ID;
        private int VYIS_VY_ID;
        private string VYIS_Data_smeni_statysa;
        private string VYIS_Kommentariy;
        public VYIstoriaStatysaWindow()
        {
            InitializeComponent();
            PopulateVYIS();
        }
        public VYIstoriaStatysaWindow(int Statys_VY_ID, int VY_ID, string
                                      Data_smeni_statysa, string Kommentariy)
        {
            InitializeComponent();
            VYIS_Statys_VY_ID = Statys_VY_ID;
            VYIS_VY_ID = VY_ID;
            VYIS_Data_smeni_statysa = Data_smeni_statysa;
            VYIS_Kommentariy = Kommentariy;
            PopulateVYIS();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы перешли на окно 'История статуса водительского удостоверения'");
        }
        public void PopulateVYIS()
        {
            using (GIBDDEntities db = new GIBDDEntities())
            {
                var VYIS = db.VY_Istoria_Statysa.ToList();
                VYISDG.ItemsSource = VYIS;
            }
        }

        private void AddButton(object sender, RoutedEventArgs e)
        {
            VYIstoriaStatysaAdd vyIstoriaStatysaAdd = new VYIstoriaStatysaAdd();
            vyIstoriaStatysaAdd.Show();
            PopulateVYIS();
            this.Close();
        }
        private void ChangeButton(object sender, RoutedEventArgs e)
        {
            var selectedVYIS = VYISDG.SelectedItem as VY_Istoria_Statysa;
            if (selectedVYIS != null)
            {
                VYIstoriaStatysaChange vyIstoriaStatysaChange = new VYIstoriaStatysaChange(selectedVYIS);
                vyIstoriaStatysaChange.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите автомобиль для изменения", "Предупреждение",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void DeleteButton(object sender, RoutedEventArgs e)
        {
            var selectedVYIS = VYISDG.SelectedItem as VY_Istoria_Statysa;

            if (selectedVYIS != null)
            {
                using (GIBDDEntities db = new GIBDDEntities())
                {

                    MessageBoxResult result2 = MessageBox.Show("Вы уверены, что хотите удалить выбранную историю статуса водительского удостоверения?",
                                                           "Подтверждение удаления",
                                                           MessageBoxButton.YesNo,
                                                           MessageBoxImage.Question);
                    if (result2 == MessageBoxResult.Yes)
                    {
                        db.VY_Istoria_Statysa.Attach(selectedVYIS);
                        db.VY_Istoria_Statysa.Remove(selectedVYIS);
                        db.SaveChanges();
                        PopulateVYIS();
                        MessageBox.Show("История статуса водительского удостоверения успешно удалена", "Успех",
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите историю статуса водительского удостоверения для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}