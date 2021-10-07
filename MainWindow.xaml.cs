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
using System.Data.SqlClient;
using System.Data;


namespace Eindopdracht
{

    public partial class MainWindow : Window
    {


        //static string connectionstring = "connectionstring van milan";
        static string connectionstring = "Server=LAPTOP-7VVM9TQ3\\SQLEXPRESS;Database=DAB1_Eindopdracht;Trusted_Connection=True;";
        SqlConnection Connectie = new SqlConnection(connectionstring);
        string text;




        public MainWindow()
        {
            InitializeComponent();
        }

        //Merken gedeelte
        private void Merk_Loaded(object sender, RoutedEventArgs e)
        {
            Connectie.Open();
            string query = "SELECT Strland FROM TblLand;";
            SqlCommand cmd = new SqlCommand(query, Connectie);
            SqlDataAdapter adapter = new SqlDataAdapter(query, Connectie);
            DataTable data = new DataTable();
            adapter.Fill(data);

               for (int i = 0; i < data.Rows.Count; i++)
            {
                Merk.Items.Add(data.Rows[i]["StrLand"].ToString());
            }
            Connectie.Close();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(text);
        }

        private void Merk_Changed(object sender, SelectionChangedEventArgs e)
        {
            //listview updaten naar types gebaseerd op onderstaande text
            text = (Merk.Items[Merk.SelectedIndex].ToString());

            //Hiermee kunnen we de tekst van het land/stad updaten (ff query bedenken)
            Hoofdkantoor.Content = "test";
            Land.Content = "test2";


            //updaten van de gegevens in de listbox (ff query schrijven)
            Connectie.Open();
            string query = "SELECT Strland FROM TblLand;";
            SqlCommand cmd = new SqlCommand(query, Connectie);
            SqlDataAdapter adapter = new SqlDataAdapter(query, Connectie);
            DataTable data = new DataTable();
            adapter.Fill(data);

            for (int i = 0; i < data.Rows.Count; i++)
            {
                Modellijst.Items.Add(data.Rows[i]["StrLand"].ToString());
            }
            Connectie.Close();
        }

        private void KW_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("KW berekenen");
            //Maken dat hij hercalculeert met de waarde :)

        }

        private void PK_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("PK Berekenen");
            //Maken dat hij hercalculeert met de waarde :)
        }


        private void Modellijst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //gegevens updaten op aanpassing

            Vermogen.Content = "1000PK";
            Serie.Content = "Golfje";
            Model.Content = "Pizza";
        }
    }
}
