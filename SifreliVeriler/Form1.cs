using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SifreliVeriler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-9DQS8R7\\MSSQLSERVER01;Initial Catalog=Rehber;Integrated Security=True");

        void listele()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from TBLVERILER", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string admetin= textBox1.Text;
            byte[] addizi = ASCIIEncoding.UTF8.GetBytes(admetin);
            string adsifre = Convert.ToBase64String(addizi);

            string soymetin = textBox2.Text;
            byte[] soyaddizi = ASCIIEncoding.UTF8.GetBytes(soymetin);
            string soysifre = Convert.ToBase64String(soyaddizi);
            
            string emailmetin = textBox3.Text;
            byte[] emailaddizi = ASCIIEncoding.UTF8.GetBytes(emailmetin);
            string emailsifre = Convert.ToBase64String(emailaddizi);

            string sifremetin = textBox4.Text;
            byte[] sifredizi = ASCIIEncoding.UTF8.GetBytes(sifremetin);
            string sifre = Convert.ToBase64String(sifredizi);

            
            string hesapnometin = textBox5.Text;
            byte[] hesapnoaddizi = ASCIIEncoding.UTF8.GetBytes(hesapnometin);
            string hesapnosifre = Convert.ToBase64String(hesapnoaddizi);

            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into TBLVERILER (Ad,Soyad,MAIL,SIFRE,HesapNo) values (@p1,@p2,@p3,@p4,@p5)", baglanti);
            komut.Parameters.AddWithValue("@p1", adsifre);
            komut.Parameters.AddWithValue("@p2", soysifre);
            komut.Parameters.AddWithValue("@p3", emailsifre);
            komut.Parameters.AddWithValue("@p4", sifre);
            komut.Parameters.AddWithValue("@p5", hesapnosifre);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Veriler Şifreli Bir Şekilde Kaydedildi");
            listele();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
        }
        private string DecodeBase64Safe(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;
            try
            {
                byte[] bytes = Convert.FromBase64String(value.ToString());
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return value.ToString();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Ad", typeof(string));
            dt.Columns.Add("Soyad", typeof(string));
            dt.Columns.Add("Mail", typeof(string));
            dt.Columns.Add("Sifre", typeof(string));
            dt.Columns.Add("HesapNo", typeof(string));

            using (var conn = new SqlConnection(baglanti.ConnectionString))
            using (var cmd = new SqlCommand("SELECT * FROM TBLVERILER", conn))
            {
                conn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string id = dr.IsDBNull(0) ? string.Empty : dr[0].ToString();
                        string ad = DecodeBase64Safe(dr.IsDBNull(1) ? null : dr[1]);
                        string soyad = DecodeBase64Safe(dr.IsDBNull(2) ? null : dr[2]);
                        string mail = DecodeBase64Safe(dr.IsDBNull(3) ? null : dr[3]);
                        string sifre = DecodeBase64Safe(dr.IsDBNull(4) ? null : dr[4]);
                        string hesap = DecodeBase64Safe(dr.IsDBNull(5) ? null : dr[5]);

                        dt.Rows.Add(id, ad, soyad, mail, sifre, hesap);
                    }

                }
            }

            dataGridView1.DataSource = dt;
        }
    }
}
