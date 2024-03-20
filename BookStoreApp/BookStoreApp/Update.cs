using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookStoreApp
{
    public partial class Update : Form
    {
        public Update()
        {
            InitializeComponent();
        }
        //kapatma işlemi yapar.
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Windows Forms uygulamasında bir form yüklenirken gerçekleştirilecek olan işlemleri tanımlar.
        private void Update_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.GetBooksToEdit().Tables[0];
            dataGridView1.Columns[0].Visible = false;
        }

        // DataGridView hücresinin düzenleme işlemi tamamlandığında, bu kod çalışarak verileri alacak
        // ve bir veritabanı işlevi aracılığıyla güncelleyecektir.
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int Satir = e.RowIndex;
            int ID =Convert.ToInt32 (dataGridView1.Rows[Satir].Cells[0].Value);
            string BarkodNo = dataGridView1.Rows[Satir].Cells["BARKOD NO."].Value.ToString();
            string KitapAdi = dataGridView1.Rows[Satir].Cells["KITAP ADI"].Value.ToString();
            string Yazar = dataGridView1.Rows[Satir].Cells["YAZARI"].Value.ToString();
            int  SayfaNo =Convert.ToInt32( dataGridView1.Rows[Satir].Cells["SAYFA SAYISI"].Value);
            string Tur= dataGridView1.Rows[Satir].Cells["KİTAP TÜRÜ"].Value.ToString();
            string Dili = dataGridView1.Rows[Satir].Cells["DİLİ"].Value.ToString();
            string Yayinci = dataGridView1.Rows[Satir].Cells["YAYINCISI"].Value.ToString();
            string YayinYili = dataGridView1.Rows[Satir].Cells["YILI"].Value.ToString();
            db.Update(ID, BarkodNo, KitapAdi, Yazar, Tur, SayfaNo,  Dili, Yayinci, YayinYili);


        }
        // bu kod parçası, kullanıcının bir metin kutusuna kitap adı veya diğer bilgileri girmesiyle
        // , bu bilgilere göre veritabanında kitap araması yapar ve sonuçları DataGridView bileşeninde görüntüler.

        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource=db.SearchBookUpdate(txtAra.Text .ToUpper()).Tables[0];
        }
    }
}
