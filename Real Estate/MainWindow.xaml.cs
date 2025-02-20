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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Real_Estate
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Collections.Generic;

    public partial class MainWindow : Window
    {
        private List<Property> _properties = new List<Property>();
        private readonly DataService _dataService = new DataService();
        private Property _editingProperty = null; // Для отслеживания редактируемого объекта

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            _properties = _dataService.LoadProperties();
            RefreshList();
        }

        // Обработчик кнопки "Добавить/Обновить"
        private void AddProperty_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            if (_editingProperty != null)
            {
                // Обновляем существующий объект
                UpdateExistingProperty();
            }
            else
            {
                // Добавляем новый объект
                AddNewProperty();
            }

            SaveAndReset();
        }

        private bool ValidateInput()
        {
            if (!int.TryParse(RoomsTextBox.Text, out _))
            {
                MessageBox.Show("Некорректное количество комнат!");
                return false;
            }
            return true;
        }

        private void UpdateExistingProperty()
        {
            _editingProperty.Address = AddressTextBox.Text;
            _editingProperty.Rooms = int.Parse(RoomsTextBox.Text);
            _editingProperty.Phone = PhoneTextBox.Text;
            _editingProperty.District = DistrictTextBox.Text;
            _editingProperty.Notes = NotesTextBox.Text;
        }

        private void AddNewProperty()
        {
            _properties.Add(new Property
            {
                Address = AddressTextBox.Text,
                Rooms = int.Parse(RoomsTextBox.Text),
                Phone = PhoneTextBox.Text,
                District = DistrictTextBox.Text,
                Notes = NotesTextBox.Text
            });
        }

        private void SaveAndReset()
        {
            _dataService.SaveProperties(_properties);
            RefreshList();
            ClearFields();
            _editingProperty = null;
            AddButton.Content = "Добавить"; // Возвращаем исходный текст кнопки
        }

        // Обработчик кнопки "Редактировать"
        private void EditProperty_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag is Property selectedProperty)
            {
                _editingProperty = selectedProperty;
                FillForm(selectedProperty);
                AddButton.Content = "Обновить"; // Меняем текст кнопки
            }
        }

        private void FillForm(Property property)
        {
            AddressTextBox.Text = property.Address;
            RoomsTextBox.Text = property.Rooms.ToString();
            PhoneTextBox.Text = property.Phone;
            DistrictTextBox.Text = property.District;
            NotesTextBox.Text = property.Notes;
        }

        // Обработчик кнопки "Отмена"
        private void CancelEdit_Click(object sender, RoutedEventArgs e)
        {
            _editingProperty = null;
            ClearFields();
            AddButton.Content = "Добавить";
        }


        private void DeleteProperty_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag is Property selectedProperty)
            {
                var result = MessageBox.Show(
                    "Вы уверены, что хотите удалить эту запись?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    _properties.Remove(selectedProperty);
                    _dataService.SaveProperties(_properties);
                    RefreshList();
                }
            }
        }

        private void SearchProperty_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchTextBox.Text.ToLower();
            var filtered = _properties
                .Where(p => p.Address.ToLower().Contains(searchText) ||
                          p.District.ToLower().Contains(searchText))
                .ToList();

            PropertyListBox.ItemsSource = filtered;
        }

        private void RefreshList()
        {
            PropertyListBox.ItemsSource = null;
            PropertyListBox.ItemsSource = _properties;
        }

        private void ClearFields()
        {
            AddressTextBox.Clear();
            RoomsTextBox.Clear();
            PhoneTextBox.Clear();
            DistrictTextBox.Clear();
            NotesTextBox.Clear();
        }
    }
}
