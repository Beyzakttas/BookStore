using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace BookStoreApp
{
    internal class db
    {
        public static int KitapID = 0;
        public static bool Kapa = false;
        public static bool Search= false;
        public static string Constr = "server=.;database=BookStore;user=sa;password=123;";
        //kodu, bir veritabanından kitap bilgilerini almak için kullanılan bir metodu tanımlar
      public static DataSet GetBooks()
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(Constr);
            SqlDataAdapter da = new SqlDataAdapter("select KitapID as [ID],KitapAdi as [KITAP ADI],BarkodNo as [BARKOD NO.],Yazar as [YAZARI],SayfaNo [SAYFA SAYISI]," +
                "Tur as [KİTAP TÜRÜ],Dil as [DİLİ],Yayinci as [YAYINCISI],Yil as[YILI],KayitTarihi as[ARŞİV GİRİŞ TARİHİ],Resim as [RESIM] from Kitap", con);
            da.Fill(ds);
            return ds;
        }
      //  kod, veritabanındaki "Kitap" adlı tablodan verileri alarak bir DataSet döndüren bir metodu tanımlar
        public static DataSet GetBooksToEdit ()
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(Constr);
            SqlDataAdapter da = new SqlDataAdapter("select KitapID as [ID],KitapAdi as [KITAP ADI],BarkodNo as [BARKOD NO.],Yazar as [YAZARI],SayfaNo [SAYFA SAYISI]," +
                "Tur as [KİTAP TÜRÜ],Dil as [DİLİ],Yayinci as [YAYINCISI],Yil as[YILI] from Kitap order by KitapAdi asc", con);
            da.Fill(ds);
            return ds;
        }
        //Bu kod, bir veritabanındaki "Kitap" tablosunu güncellemek için kullanılır.
        public static void Update(int KitapID,string BarkodNo,string KitapAdi,string Yazar,string Tur,int SayfaNo,string Dili,string Yayinci,string YayinYili)
        {
            SqlConnection con = new SqlConnection(Constr);
            SqlCommand com = new SqlCommand("update Kitap set KitapAdi=@KitapAdi,BarkodNo=@BarkodNo,Yazar=@Yazar,SayfaNo=@SayfaNo,Tur=@Tur,Dil=@Dil,Yayinci=@Yayinci,Yil=@Yil where KitapID=@ID", con);
            com.Parameters.AddWithValue("@ID", KitapID);
            com.Parameters.AddWithValue("@BarkodNo", BarkodNo);
            com.Parameters.AddWithValue("@KitapAdi", KitapAdi.ToUpper());
            com.Parameters.AddWithValue("@Yazar", Yazar.ToUpper());
            com.Parameters.AddWithValue("@SayfaNo", SayfaNo);
            com.Parameters.AddWithValue("@Tur", Tur.ToUpper());
            com.Parameters.AddWithValue("@Dil", Dili.ToUpper());
            com.Parameters.AddWithValue("@Yayinci", Yayinci.ToUpper());
            com.Parameters.AddWithValue("@Yil", YayinYili.ToUpper());
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            BackDatabase();
        }
        //Veritabanında kitap bilgilerini kaydeder veya varsa günceller.
        public static void SaveBook(int KitapID,string BarkodNo,string KitapAdi,string Yazar,
            int SayfaNo,string Tur,string Dili,string Yayinci,string YayinYili,byte[] Fotograf)
        {
            
           SqlConnection con = new SqlConnection(Constr);
            SqlCommand com = new SqlCommand("if not exists(select *from Kitap Where KitapID=@ID) insert into Kitap (KitapAdi,BarkodNo,Yazar,SayfaNo,Tur,Dil" +
                ",Yayinci,Yil,KayitTarihi,Resim) values (@KitapAdi,@BarkodNo,@Yazar,@SayfaNo,@Tur,@Dil,@Yayinci,@Yil,@KayitTarihi,@Resim) else update Kitap set" +
                " KitapAdi=@KitapAdi,BarkodNo=@BarkodNo,Yazar=@Yazar,SayfaNo=@SayfaNo,Tur=@Tur,Dil=@Dil,Yayinci=@Yayinci,Yil=@Yil,Resim=@Resim where KitapID=@ID", con);
            com.Parameters.AddWithValue("@ID",KitapID);
            com.Parameters.AddWithValue("@BarkodNo", BarkodNo);
            com.Parameters.AddWithValue("@KitapAdi",KitapAdi.ToUpper());
            com.Parameters.AddWithValue("@Yazar",Yazar.ToUpper());
            com.Parameters.AddWithValue("@SayfaNo",SayfaNo);
            com.Parameters.AddWithValue("@Tur",Tur.ToUpper());
            com.Parameters.AddWithValue("@Dil",Dili.ToUpper());
            com.Parameters.AddWithValue("@Yayinci",Yayinci.ToUpper());
            com.Parameters.AddWithValue("@Yil",YayinYili.ToUpper());
            com.Parameters.AddWithValue("@KayitTarihi",DateTime.Now.ToString("dd-MM-yyyy"));
            if (Fotograf == null)
            {
                com.Parameters.AddWithValue("@Resim", 0);
                 
            }
            else
            {
                com.Parameters.AddWithValue("@Resim", Fotograf);




            }
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            BackDatabase();
        }
        //bir kitabın veritabanından silinmesini sağlar
        public static void DeleteBook(int KitapID)
        {
            SqlConnection con = new SqlConnection(Constr);
            SqlCommand com = new SqlCommand("Delete from Kitap Where KitapID=@ID", con);
            com.Parameters.AddWithValue("@ID", KitapID);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            BackDatabase();
        }
        //SQL veritabanından yazar adlarını alarak otomatik tamamlama özelliği için bir dize koleksiyonu oluşturur.
        public static AutoCompleteStringCollection Yazarlar()
        {
            AutoCompleteStringCollection yazar = new AutoCompleteStringCollection();
            SqlConnection con = new SqlConnection(Constr);
            SqlCommand com = new SqlCommand("select distinct Yazar from Kitap", con);
            con.Open();
            SqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                yazar.Add(dr["Yazar"].ToString());
            }
            dr.Close();
            con.Close();
            return yazar;
        }
        //yaptığımız islemleri backup alacak klasor olusturma 
        public static void BackDatabase()
        {
            SqlConnection con = new SqlConnection(Constr);
            SqlCommand com = new SqlCommand("backup database BookStore to disk='D:\\BookStore\\BookStore.bak' with format", con);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        }
        // veritabanında kitap aramak için kullanılan bir metodu tanımlar. 
        public static DataSet SearchBook(string text,string radiobutton)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(Constr);
            switch (radiobutton)
            {
                case "Barkod No":
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select KitapID as [ID],KitapAdi as [KITAP ADI],BarkodNo as [BARKOD NO.],Yazar as [YAZARI]" +
                            ",SayfaNo [SAYFA SAYISI],Tur as [KİTAP TÜRÜ],Dil as [DİLİ],Yayinci as [YAYINCISI],Yil as[YILI],KayitTarihi as[ARŞİV GİRİŞ TARİHİ],Resim as [RESIM] from Kitap Where BarkodNo like '%'+@BarkodNo+'%' ", con);
                        da.SelectCommand.Parameters.AddWithValue("@BarkodNo", text);
                        da.Fill(ds);
                        break;
                    }
                case "Kitap Adı":
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select KitapID as [ID],KitapAdi as [KITAP ADI],BarkodNo as [BARKOD NO.],Yazar as [YAZARI]" +
                            ",SayfaNo [SAYFA SAYISI],Tur as [KİTAP TÜRÜ],Dil as [DİLİ],Yayinci as [YAYINCISI],Yil as[YILI],KayitTarihi as[ARŞİV GİRİŞ TARİHİ],Resim as [RESIM] from Kitap Where KitapAdi like '%'+@KitapAdi+'%' ", con);
                        da.SelectCommand.Parameters.AddWithValue("@KitapAdi", text.ToUpper());
                        da.Fill(ds);
                        break;
                    }
                case "Yazarı":
                    {
                        SqlDataAdapter da = new SqlDataAdapter("select KitapID as [ID],KitapAdi as [KITAP ADI],BarkodNo as [BARKOD NO.],Yazar as [YAZARI],SayfaNo [SAYFA SAYISI]," +
                            "Tur as [KİTAP TÜRÜ],Dil as [DİLİ],Yayinci as [YAYINCISI],Yil as[YILI],KayitTarihi as[ARŞİV GİRİŞ TARİHİ],Resim as [RESIM] from Kitap Where Yazar like '%'+@Yazar+'%' ", con);
                        da.SelectCommand.Parameters.AddWithValue("@Yazar", text.ToUpper());
                        da.Fill(ds);
                        break;
                    }
            }
            return ds;
        }
        //kod bir metod tanımlar. Bu metod bir kitap veritabanında belirli bir kitap adına göre arama yapar ve sonuçları bir DataSet nesnesi içinde döndürür.
        internal static DataSet SearchBookUpdate(string KitapAdi)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(Constr);
           SqlDataAdapter da = new SqlDataAdapter("select KitapID as [ID],KitapAdi as [KITAP ADI],BarkodNo as [BARKOD NO.],Yazar as [YAZARI],SayfaNo [SAYFA SAYISI]," +
               "Tur as [KİTAP TÜRÜ],Dil as [DİLİ],Yayinci as [YAYINCISI],Yil as[YILI] from Kitap Where KitapAdi like '%'+@KitapAdi+'%' ", con);
            da.SelectCommand.Parameters.AddWithValue("KitapAdi", KitapAdi);
            da.Fill(ds);
            return ds;

        }
    }
}
