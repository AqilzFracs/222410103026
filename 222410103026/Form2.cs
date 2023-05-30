using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _222410103026
{
    public partial class FormUpdate : Form
    {
        DBHelpers db;
        public FormUpdate()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new DBHelpers().Execute($"UPDATE laptop SET nama_laptop = '{textBox2.Text}', harga_laptop = '{textBox3.Text}', stok = '{textBox4.Text}' where id_laptop = '{textBox1.Text}'");
        }
    }
}
