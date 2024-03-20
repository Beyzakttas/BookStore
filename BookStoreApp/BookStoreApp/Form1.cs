using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookStoreApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           Options();
            GetAllBook(GetDataGridView1());

        }

        private void Options()
        {//dosya olusturma
            if (!Directory.Exists("D:\\BookStore"))
            {
                Directory.CreateDirectory("D:\\BookStore");
                Directory.CreateDirectory("D:\\BookStore\\Resim");
            }
            //klasor içini boşaltma
           string []files= Directory.GetFiles("D:\\BookStore\\Resim");
            foreach (string file in files)
            {
                File.Delete(file);
            }
            this.Text = "Kütüphane Uygulaması      Versiyon" + Assembly.GetExecutingAssembly().GetName().Version;
        }

        private DataGridView GetDataGridView1()
        {
            return dataGridView1;
        }

        private void GetAllBook(DataGridView dataGridView1)
        {
            dataGridView1.DataSource = db.GetBooks().Tables[0];
            dataGridView1.Columns["ID"].Visible = false;
         // dataGridView1.Columns["RESİM"].Visible = false;
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            db.Kapa = true;
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel =! db.Kapa;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Book Kitap = new Book();
            Kitap.ShowDialog();
            GetAllBook(GetDataGridView1());
        }
        int Satir = 0;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Satir = e.RowIndex;
            db.KitapID = Convert.ToInt32(dataGridView1.Rows[Satir].Cells["ID"].Value);
            Book Kitap = new Book();
            Kitap.ShowDialog();
            GetAllBook();
        }
        // 2.si 1. gormuyor
        private void GetAllBook()
        {

            dataGridView1.DataSource = db.GetBooks().Tables[0];
            dataGridView1.Columns["ID"].Visible = false;
           // dataGridView1.Columns["RESİM"].Visible = true;
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Satir = e.RowIndex;
        }
        //veri silma
        private void button3_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Seçilen Kitap Silinecektir\n Devam edilsin mi?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int KitapID = Convert.ToInt32(dataGridView1.Rows[Satir].Cells["ID"].Value);
                db.DeleteBook(KitapID);
                GetAllBook();

            }
        }
        //arama islemi
        private void button4_Click(object sender, EventArgs e)
        {
            db.Search = !db.Search;
            if (db.Search)
            {
                txtarama.ResetText();
                txtarama.Focus();
                
            }
            gbSearch.Visible = db.Search;
        }

        private void txtarama_TextChanged(object sender, EventArgs e)
        {
            string rbName = "";
            foreach(Control c in gbSearch.Controls)
            {
                if(c is RadioButton && (c as RadioButton).Checked)
                {
                    rbName = c.Text;

                }
            }
            dataGridView1.DataSource = db.SearchBook((sender as TextBox).Text,rbName).Tables[0];
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Update book = new Update();
            book.ShowDialog();
            GetAllBook();
        }
    }
}
