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

namespace LinqHomework
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection connection;
        private ExampleDataContext example;
        public MainWindow()
        {
            InitializeComponent();
            connection = new SqlConnection("Server=localhost;Database=pubs;Integrated Security=true;");
            example = new ExampleDataContext(connection);
            if (example.DatabaseExists())
                ExampleDataGrid.ItemsSource = example.authors;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                example.SubmitChanges();
            }
            catch (Exception zxc)
            {
                MessageBox.Show("фейл по причине пидорас");
            }
        }
    }
}
