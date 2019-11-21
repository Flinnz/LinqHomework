using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Data.Linq;
using System.Reflection;

namespace LinqHomework
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection connection;
        private ExampleDataContext example;
        private Dictionary<string, List<string>> filters = new Dictionary<string, List<string>>();
        public MainWindow()
        {
            InitializeComponent();
            connection = new SqlConnection("Server=localhost;Database=pubs;Integrated Security=true;");
            example = new ExampleDataContext(connection);

            filters[nameof(authors)] = GetFilters(typeof(authors))
                .ToList();
            filters[nameof(titles)] = GetFilters(typeof(titles))
                .ToList();
            Filters.ItemsSource = filters[nameof(authors)];
            if (example.DatabaseExists())
            {
                Titles.ItemsSource = example.titles;
                Authors.ItemsSource = example.authors;
            }
        }

        private IEnumerable<string> GetFilters(Type type)
        {
            foreach(var f in type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => !x.PropertyType.Name.Contains("EntitySet"))
                .Select(x => x.Name))
            {
                yield return f;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                example.SubmitChanges();
            }
            catch (Exception zxc)
            {
                MessageBox.Show(zxc.Message);
            }
        }

        private string GetTabTableName(TabItem tab)
        {
            if (tab.Header is string str)
            {
                switch (str)
                {
                    case "Авторы":
                        return nameof(authors);
                    case "Тайтлы":
                        return nameof(titles);
                    default:
                        throw new NotImplementedException();
                }
            }
            throw new NotImplementedException();
        }

        private Type GetTabTableType(TabItem tab)
        {
            if (tab.Header is string str)
            {
                switch (str)
                {
                    case "Авторы":
                        return typeof(authors);
                    case "Тайтлы":
                        return typeof(titles);
                    default:
                        throw new NotImplementedException();
                }
            }
            throw new NotImplementedException();
        }

        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Filters.SelectedItem == null) return;
            if (string.IsNullOrWhiteSpace(FilterText.Text)) return;
            if (Filters.SelectedItem is string selectedString)
            {
                if (Tables.Items[Tables.SelectedIndex] is TabItem tab)
                { 
                    var table = example.GetTable(GetTabTableType(tab));
                    switch(table)
                    {
                        case Table<authors> authorTable:
                            Authors.ItemsSource = authorTable
                                .Where(selectedString, FilterText.Text.ToLower());
                            break;
                        case Table<titles> titleTable:
                            Titles.ItemsSource = titleTable
                                .Where(selectedString, FilterText.Text.ToLower());
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                }
            }
        }


        private void Tables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is TabControl tabControl)
            {
                if (tabControl.Items[tabControl.SelectedIndex] is TabItem tab)
                {
                    Filters.ItemsSource = filters[GetTabTableName(tab)];
                }
            }
        }
    }
}
