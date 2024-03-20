using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookStoreApp
{
    public partial class Book : Form
    {
        public Book()
        {
            InitializeComponent();
        }
        //Bu kod, bir Windows Forms uygulamasında bir butona tıklanınca çalışacak bir olay işleyicisini tanımlar.
        //Kod, belirli koşullar altında uygulamanın davranışını değiştirir.
        private void button2_Click(object sender, EventArgs e)
        {
            bool Dolu = true;

            foreach (Control c in this.Controls)
            {
                if (c is TextBox && c.TabStop == true)
                {
                        c.BackColor = Color.White;

                }
            }
            foreach (Control c in this.Controls  )
            {
                if (c is TextBox && c.TabStop == true && c.Tag=="B")
                {
                    if (c.Text == string.Empty)
                    {
                        Dolu = false;
                        c.BackColor = Color.Red;
                    }
                }
            }
            if (!Dolu)
            {
                if (MessageBox.Show("Barkod Numarası ve Kitap Adı sekmesi boş bırakılamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.Yes) ;
                return;
            }
            //veritabanına giriş 11-03- 2024
            db.SaveBook(db.KitapID,txtBarkodNo.Text,txtKitapAdi.Text,txtYazar.Text, txtSayfaNo.Text== string.Empty
                ? 0:Convert.ToInt32(txtSayfaNo.Text),txtTur.Text,txtDil.Text,txtYayinci.Text,txtYayinYili.Text,PicData);
            db.KitapID = 0;
            this.Close();
        }
      //uyarı mesajı 
        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bu pencere kapatılırsa verileriniz kaybolacaktır.\n Devam edilsin mi?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                db.KitapID = 0;
                this.Close();
            }
          
            
        }
        //picture box cift tıklama ve foto ekleme
        byte[] PicData = null;
       
        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog selectPic = new OpenFileDialog();
            selectPic.Filter = "( *.jpg)|*.jpg";
            if (selectPic.ShowDialog() == DialogResult.No) return;
            PicData = File.ReadAllBytes(selectPic.FileName);
            pboxKitap.Image = Image.FromFile(selectPic.FileName);

        }

        private void Book_Load(object sender, EventArgs e)
        {
           Options();
            if (db.KitapID > 0)
            {//barkod no degistirilemez yaptma
                txtBarkodNo.ReadOnly = true;
                SqlConnection con = new SqlConnection(db.Constr);
                SqlCommand com = new SqlCommand("select*from Kitap where KitapID=@ID", con);
                com.Parameters.AddWithValue("@ID", db.KitapID);
                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                String filename = "";
                while (dr.Read())
                {
                    txtKitapAdi.Text = dr["KitapAdi"].ToString();
                    txtBarkodNo.Text = dr["BarkodNo"].ToString();
                    txtYazar.Text = dr["Yazar"].ToString();
                    txtSayfaNo.Text = dr["SayfaNo"].ToString();
                    txtTur.Text = dr["Tur"].ToString();
                    txtDil.Text = dr["Dil"].ToString();
                    txtYayinci.Text = dr["Yayinci"].ToString();
                    txtYayinYili.Text = dr["Yil"].ToString();
                    //burada fotografi veritabanindan alip formda gosterilir
                    PicData = (byte[])dr["Resim"];
                    if (PicData.Length > 4)
                    {
                        Guid guid = Guid.NewGuid();
                        filename = "D:\\BookStore\\Resim\\image" + guid.ToString().Substring(0, 5) + ".jpg";
                        FileStream fs = new FileStream(filename,FileMode.Create);
                        fs.Write(PicData, 0, PicData.Length);
                        fs.Flush();
                        fs.Close();
                        pboxKitap.Image = Image.FromFile(filename);
                    }


                }
                dr.Close();
                con.Close();
            }
        }

        private void Options()
        {
            txtYazar.AutoCompleteCustomSource = db.Yazarlar();
        }

        //temizleme işlemi yapar
        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Girmiş olduğunuz veriler silinecektir!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) 

            foreach (Control c in this.Controls)
            {
                if (c is TextBox && c.TabStop == true)
                {
                    c.ResetText();
                }
                if (c is PictureBox)
                    (c as PictureBox).Image = null;
            }
        }
    }
}
