using System;
using System.Collections;
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
using DashBoard.DataAccess;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace DashBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadData();

        }

        public void LoadData()
        {
            //
            int account = 0;
            int orderTotal = 0;
            decimal totalSale = 0;
            int totalProduct = 0;
            List<BestSeller> bestSeller = new List<BestSeller>();
            List<Revenue> revenue = new List<Revenue>();
            string connectionString = "Data Source=.;Initial Catalog=ShoeStoreManagement;User ID=sa;Password=123456";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Select total user
                string sqlQuery = "SELECT COUNT(*) from account where role = '0'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        // Access data from the reader
                        account = reader.GetInt32(0);
                    }
                    reader.Close();
                }

                // Select total product
                sqlQuery = "SELECT COUNT(*) from product";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        // Access data from the reader
                        totalProduct = reader.GetInt32(0);
                    }
                    reader.Close();
                }

                //Select total order
                sqlQuery = "Select COUNT(order_id) as totalOrder, SUM(total_price) as total from [order]";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        // Access data from the reader
                        orderTotal = reader.GetInt32(0);
                        totalSale = reader.GetDecimal(1);
                    }
                    reader.Close();
                }

                //Select best seller
                sqlQuery = "select top 4 SUM(order_detail.quantity) as TotalOrder, product.name from order_detail inner join \r\nproduct on order_detail.product_id=product.product_id\r\ngroup by product.name\r\norder by TotalOrder desc";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        // Access data from the reader
                        BestSeller best = new BestSeller();
                        best.TotalOrder = reader.GetInt32(0);
                        best.Name = reader.GetString(1);
                        bestSeller.Add(best);
                    }
                    reader.Close();
                }

                //Select revenue
                sqlQuery = "Select YEAR(order_date) as Year, MONTH(order_date) as month,\r\nsum(total_price) as Total_price\r\nfrom [order] where YEAR(order_date)='2023'\r\ngroup by Year(order_date), Month(order_date)\r\norder by Month(order_date) asc";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        // Access data from the reader
                        Revenue re = new Revenue();
                        re.Year = reader.GetInt32(0);
                        re.Month = reader.GetInt32(1);
                        re.TotalPrice = Convert.ToDouble(reader.GetDecimal(2));
                        revenue.Add(re);
                    }
                    reader.Close();
                }
                connection.Close();
            }
            //Bind data
            txtUser.Text = Convert.ToString(account);
            txtOrder.Text = Convert.ToString(orderTotal);
            txtTotalSale.Text = Convert.ToString(totalSale);
            txtProduct.Text = Convert.ToString(totalProduct);

            //Add revenue
            ArrayList listRevenue = new ArrayList();

            for (double i = 1; i <= 12; i++)
            {
                foreach (var item in revenue)
                {
                    if (item.Month == i)
                    {
                        listRevenue.Add(item.TotalPrice);
                    }
                }
                listRevenue.Add(0d);
            }

            // Add elements to the ArrayList
            //arrayList.AddRange(new double[] { 8, 6, 5, 2, 4, 6, 7, 2, 4, 2, 5, 8 });

            // Convert ArrayList to ChartValues<double>
            ChartValues<double> chartValues = new ChartValues<double>(listRevenue.Cast<double>());

            // Update the code with the converted ChartValues
            SeriesCollection = new SeriesCollection
{
                new LineSeries
                {
                    Title = "Series 1",
                    Values = chartValues
                }
            };
            /*SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 8, 6, 5, 2, 4, 6, 7, 2, 4, 2, 5, 8 }
                }   
            };*/

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "August", "September", "October", "November", "December" };
            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart


            //modifying any series values will also animate and update the chart


            //DataContext = this;


            PSeriesCollection = new SeriesCollection
            {
                new StackedRowSeries
                {
                    Values = new ChartValues<double> {4, 5, 6, 8},
                    StackMode = StackMode.Percentage,
                    DataLabels = true,
                    LabelPoint = p => p.X.ToString()
                },
                new StackedRowSeries
                {
                    Values = new ChartValues<double> {2, 5, 6, 7},
                    StackMode = StackMode.Percentage,
                    DataLabels = true,
                    LabelPoint = p => p.X.ToString()
                }  
            };

            //adding series updates and animates the chart
            PSeriesCollection.Add(new StackedRowSeries
            {
                Values = new ChartValues<double> { 6, 2, 7 },
                StackMode = StackMode.Percentage,
                DataLabels = true,
                LabelPoint = p => p.X.ToString()
            });

            //adding values also updates and animates
            PSeriesCollection[2].Values.Add(4d);

            PLabels = new[] { "Chrome", "Mozilla", "Opera", "IE" };
            PFormatter = val => val.ToString("P");

            //DataContext = this;

            //Dung de add best seller
            DSeriesCollection = new SeriesCollection();
            foreach (var item in bestSeller)
            {
                DSeriesCollection.Add(new PieSeries
                {
                    Title = item.Name.Substring(0, 10) + "...",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(item.TotalOrder) },
                    DataLabels = true
                });
            }

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public SeriesCollection PSeriesCollection { get; set; }
        public string[] PLabels { get; set; }
        public Func<double, string> PFormatter { get; set; }

        public SeriesCollection DSeriesCollection { get; set; }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CartesianChart_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SideMenuItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LoadData(); 
        }
    }



}
