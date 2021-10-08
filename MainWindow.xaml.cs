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
        public class SerieModel
        {
            public int ID { get; set; }
            public string Seriemodel { get; set; }

        }

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

            string queryMerkgegevens = "select * from tblLand inner join tblHoofdlocatie on tblHoofdlocatie.landID = tblLand.ID inner join tblMerk on tblMerk.hoofdlocatieID = tblHoofdlocatie.ID WHERE tblMerk.ID =" + selectedmerk;
            SqlCommand cmdMerkGegevens = new SqlCommand(queryMerkgegevens, Connectie);
            Connectie.Open();
            using (SqlDataReader reader = cmdMerkGegevens.ExecuteReader())
            {
                while (reader.Read())
                {
                    Land.Content = reader["strlandnaam"].ToString();
                    Hoofdkantoor.Content = reader["strstadsnaam"].ToString();
                    Logo.Source = new ImageSourceConverter().ConvertFromString(reader["strMerklogo"].ToString()) as ImageSource;

                }
            }
            Connectie.Close();


            Modellijst.Items.Clear();
            string querySerieModel = "select CONCAT(strSerienaam,' ',strModelnaam) as data, tblSerieModel.ID FROM tblSerieModel RIGHT JOIN tblSerie ON tblSerieModel.serieID = tblserie.ID LEFT JOIN tblModel ON tblseriemodel.modelID = tblModel.ID WHERE tblserie.merkID =" + selectedmerk;
            SqlCommand cmdSerieModel = new SqlCommand(querySerieModel, Connectie);
            SqlDataAdapter adapter = new SqlDataAdapter(querySerieModel, Connectie);
            DataTable data = new DataTable();
            adapter.Fill(data);


            for(int i = 0; i < data.Rows.Count; i++)
            {
                Modellijst.Items.Add(new SerieModel { Seriemodel = data.Rows[i]["data"].ToString(), ID = Int32.Parse(data.Rows[i]["ID"].ToString()) });
                //Modellijst.Items.Add(data.Rows[i]["data"].ToString());
            }
            Connectie.Close();


        
        }

        private void KW_Checked(object sender, RoutedEventArgs e)
        {
            KeuzeKW = true;
            recalculate();
        }

        private void PK_Checked(object sender, RoutedEventArgs e)
        {
            KeuzeKW = false;
            recalculate();
        }

        private void recalculate()
        {
            int selectedmodel = Convert.ToInt32(Modellijst.SelectedValue);
            string queryModelgegevens = "select * FROM tblSerieModel LEFT JOIN tblSerie ON tblSerie.ID = tblSerieModel.id LEFT JOIN tblModel ON tblSerieModel.modelID = tblModel.ID WHERE tblSerieModel.ID = " + selectedmodel;
            SqlCommand cmdModelgegevens = new SqlCommand(queryModelgegevens, Connectie);
            Connectie.Open();
            using (SqlDataReader reader = cmdModelgegevens.ExecuteReader())
            {
                while (reader.Read())
                {
                    int vermogen = Int32.Parse(reader["intVermogen"].ToString());
                    if (KeuzeKW == false)
                    {
                        double vermogencalculated = Math.Round(vermogen * 1.362, 2);
                        Vermogen.Content = vermogencalculated + " PK";

                    }
                    else
                    {
                        //calculeer vermogen in KW
                        Vermogen.Content = vermogen + " KW";

                    }
                }
            }
            Connectie.Close();

        }


        private void Modellijst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedmodel = Convert.ToInt32(Modellijst.SelectedValue);
            MessageBox.Show(selectedmodel);
            
            string queryModelgegevens = "select * FROM tblSerieModel LEFT JOIN tblSerie ON tblSerie.ID = tblSerieModel.id LEFT JOIN tblModel ON tblSerieModel.modelID = tblModel.ID WHERE tblSerieModel.ID = " + selectedmodel;
            SqlCommand cmdModelgegevens = new SqlCommand(queryModelgegevens, Connectie);
            Connectie.Open();
            using (SqlDataReader reader = cmdModelgegevens.ExecuteReader())
            {
                while (reader.Read())
                {
                    
                    Serie.Content = reader["strSerienaam"].ToString();
                    Model.Content = reader["strModelnaam"].ToString();
                    int vermogen = Int32.Parse(reader["intVermogen"].ToString());
                    if (KeuzeKW == false)
                    {
                        double vermogencalculated = Math.Round(vermogen * 1.362, 2) ;
                        Vermogen.Content = vermogencalculated + " PK";

                    } else
                    {
                        //calculeer vermogen in KW
                        Vermogen.Content = vermogen + " KW";
                        
                    }
                }
            }
            Connectie.Close();
        }
    }
}
