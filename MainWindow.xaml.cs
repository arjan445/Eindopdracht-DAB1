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
        public bool KeuzeKW;




        public MainWindow()
        {
            InitializeComponent();
        }

        //Merken gedeelte
        private void Merk_Loaded(object sender, RoutedEventArgs e)
        {
            Connectie.Open();
            string query = "SELECT strMerknaam FROM tblMerk;";
            SqlCommand cmd = new SqlCommand(query, Connectie);
            SqlDataAdapter adapter = new SqlDataAdapter(query, Connectie);
            DataTable data = new DataTable();
            adapter.Fill(data);

            for (int i = 0; i < data.Rows.Count; i++)
            {
                Merk.Items.Add(data.Rows[i]["strMerknaam"].ToString());
            }
            Connectie.Close();
        }
        private void Merk_Changed(object sender, SelectionChangedEventArgs e)
        {
            //listview updaten naar types gebaseerd op onderstaande text
            int selectedmerk = Merk.SelectedIndex + 1;

            string query = "select * from tblLand inner join tblHoofdlocatie on tblHoofdlocatie.landID = tblLand.ID inner join tblMerk on tblMerk.hoofdlocatieID = tblHoofdlocatie.ID WHERE tblMerk.ID =" + selectedmerk;
            SqlCommand cmd = new SqlCommand(query, Connectie);
            Connectie.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Land.Content = reader["strlandnaam"].ToString();
                    Hoofdkantoor.Content = reader["strstadsnaam"].ToString();
                    Logo.Source = new ImageSourceConverter().ConvertFromString(reader["strMerklogo"].ToString()) as ImageSource;

                }
            }
            Connectie.Close();

            //updaten van de gegevens in de listbox (ff query schrijven)
            //Doet milan
        }

        private void KW_Checked(object sender, RoutedEventArgs e)
        {
            KeuzeKW = true;
            // hercalcureren waardes

        }

        private void PK_Checked(object sender, RoutedEventArgs e)
        {
            KeuzeKW = false;
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
