using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;

namespace _222410103026
{
    public partial class Form1 : Form
    {
        DBHelpers db;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            showdata();
            showdataTransaksi();
            showdataDetailTransaksi();
        }

        private void showdataTransaksi()
        {
            var reader3 = new DBHelpers().Select("Select * from transaksi order by id_transaksi ASC");
            dataGridView3.Rows.Clear();
            while (reader3.Read())
            {
                dataGridView3.Rows.Add(reader3["id_transaksi"], reader3["tanggal_transaksi"], reader3["status_transaksi"]);
            }
        }

        private void showdataDetailTransaksi()
        {
            var reader2 = new DBHelpers().Select("Select * from detail_transaksi order by id_detail_transaksi ASC");
            dataGridView2.Rows.Clear();
            while (reader2.Read())
            {
                dataGridView2.Rows.Add(reader2["id_detail_transaksi"], reader2["id_transaksi"], reader2["id_laptop"], reader2["stok_dibeli"]);
            }

        }

        private void showdata()
        {
            var reader = new DBHelpers().Select("Select * from laptop order by id_laptop ASC");

            dataGridView1.Rows.Clear();


            while (reader.Read())
            {
                dataGridView1.Rows.Add(reader["id_laptop"], reader["nama_laptop"], reader["harga_laptop"], reader["stok"]);
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sql = $"select * from laptop where nama_laptop ilike '%{textBox1.Text}%'";
            var reader = new DBHelpers().Select(sql);

            dataGridView1.Rows.Clear();

            while (reader.Read())
            {
                dataGridView1.Rows.Add(reader["id_laptop"], reader["nama_laptop"], reader["harga_laptop"], reader["stok"]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new DBHelpers().Execute($"INSERT INTO LAPTOP(nama_laptop, harga_laptop, stok) VALUES ('{textBox2.Text}','{textBox3.Text}','{textBox4.Text}')");
            showdata();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormUpdate form = new FormUpdate();
            form.ShowDialog();
            showdata();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormDelete form = new FormDelete();
            form.ShowDialog();
            showdata();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

            var check = new DBHelpers().Select($"SELECT * FROM transaksi ORDER BY id_transaksi DESC LIMIT 1");
            if (!check.Read())
            {
                string sql = $"INSERT INTO transaksi(id_transaksi, tanggal_transaksi, status_transaksi) VALUES ({textBox5.Text}, '{DateTime.Now.ToString("dd/MM/yyyy")}', 1)";
                new DBHelpers().Execute(sql);
                new DBHelpers().Execute($"INSERT INTO detail_transaksi(id_transaksi, id_laptop, stok_dibeli) VALUES ({textBox5.Text},{textBox6.Text},{textBox7.Text})");
                new DBHelpers().Execute($"UPDATE laptop SET stok = stok - (SELECT stok_dibeli FROM detail_transaksi WHERE id_transaksi = {textBox5.Text}) WHERE id_laptop = {textBox6.Text}");
                showdata();
                showdataDetailTransaksi();
                showdataTransaksi();
            }
            else
            {

                var statusTransaksi = check.GetValue(2).ToString();
                if (statusTransaksi == "1")
                {
                    new DBHelpers().Execute($"INSERT INTO detail_transaksi(id_transaksi, id_laptop, stok_dibeli) VALUES ({textBox5.Text},{textBox6.Text},{textBox7.Text})");
                    new DBHelpers().Execute($"UPDATE laptop SET stok = stok - (SELECT stok_dibeli FROM detail_transaksi WHERE id_transaksi = {textBox5.Text}) WHERE id_laptop = {textBox6.Text}");
                    showdata();
                    showdataDetailTransaksi();
                    showdataTransaksi();
                }
                else
                {
                    string sql = $"INSERT INTO transaksi(id_transaksi, tanggal_transaksi, status_transaksi) VALUES ({textBox5.Text}, '{DateTime.Now.ToString("dd/MM/yyyy")}', 1)";
                    new DBHelpers().Execute(sql);
                    new DBHelpers().Execute($"INSERT INTO detail_transaksi(id_transaksi, id_laptop, stok_dibeli) VALUES ({textBox5.Text},{textBox6.Text},{textBox7.Text})");
                    new DBHelpers().Execute($"UPDATE laptop SET stok = stok - (SELECT stok_dibeli FROM detail_transaksi WHERE id_transaksi = {textBox5.Text}) WHERE id_laptop = {textBox6.Text}");
                    showdata();
                    showdataDetailTransaksi();
                    showdataTransaksi();
                }

            }  

        }

        private void button4_Click(object sender, EventArgs e)
        {
            new DBHelpers().Execute($"UPDATE transaksi SET status_transaksi = 2 where id_transaksi = (SELECT id_transaksi FROM transaksi ORDER BY id_transaksi DESC LIMIT 1)");
            showdata();
            showdataDetailTransaksi();
            showdataTransaksi();
        }
        string APIKey = "56dda655da157bc469b3c31722e55383";
        private void button5_Click(object sender, EventArgs e)
        {
            getWeather();
        }

        private void getWeather()
        {
            using (WebClient web = new WebClient())
            {
                string url = string.Format($"https://api.openweathermap.org/data/2.5/weather?q={textBox8.Text}&units=metric&appid={APIKey}");
                var json = web.DownloadString(url);
                WeatherInfo.root Info = JsonConvert.DeserializeObject<WeatherInfo.root>(json);
                icon_pic.ImageLocation = "https://openweathermap.org/img/w/" + Info.weather[0].icon + ".png";
                labelConditions.Text = Info.weather[0].main;
                labelDetail.Text = Info.weather[0].description;
                labelSunrise.Text = convertDateTime(Info.sys.sunrise).ToString();
                labelSunset.Text = convertDateTime(Info.sys.sunset).ToString();
                labelPressure.Text = Info.main.pressure.ToString();
                labelTemp.Text = Info.main.temp.ToString();
            }

        }
        DateTime convertDateTime(long millisec)
        {
            DateTime day = new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc);
            day = day.AddSeconds(millisec).ToLocalTime();
            return day;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
