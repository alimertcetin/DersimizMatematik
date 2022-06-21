//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class instance_btnManager : MonoBehaviour
//{
//        //OpenTheDoor scripti hangi kapıya bağlı olduğunu bu methodla buraya bildiriyor
//        //ve bu diğer methodlar scriptin bağlı olduğu kapı üzerinde çalışıyor.
//    //instance_OpenTheDoor AcilacakKapinin_Scripti;
//    //public void GetTheScriptFromDoor(GameObject testObject)
//    //{
//    //    AcilacakKapinin_Scripti = testObject.GetComponent<instance_OpenTheDoor>();
//    //}
//    instance_LittlePeopleController LP_Controller;
//    instance_Player_Inventory inventory;

//    [SerializeField]
//    TMP_Text txt_InputField = null;
//    [SerializeField]
//    GameObject Txt_Warning_GO = null;

//    //Sayı pozitif girilmeli. Genelde 1 kullanılacak ancak belki bazı durumlarda farklı kullanılanılabilir.
//    //DegeriAl fonksiyonunda değiştiriliyor.
//    //Envanterden eksiltilecek değer
//    int GonderilecekDeger = 0;
    
//    string GonderilecekTag = null;

//    private void Awake()
//    {
//        LP_Controller = FindObjectOfType<instance_LittlePeopleController>();
//        inventory = FindObjectOfType<instance_Player_Inventory>();
//    }

//    #region Kilitli kapı için buttonların kullanacağı methodlar

//    //button tarafından gönderilen tagi GonderilecekTag değişkenine ata
//    public void TagiAl(string alinanTag)
//    {
//        GonderilecekTag = alinanTag;
//    }

//    //button tarafından gönderilen değeri GonderilecekDeger değişkenine ata
//    public void DegeriAl(int alinanDeger)
//    {
//        GonderilecekDeger = alinanDeger;
//    }

//    bool Gonder;
//    void EnvanterKontrol(string _GonderilecekTag)
//    {
//        //Sayi_0
//        if (_GonderilecekTag == "Sayı_0" && inventory.Sayi_0 > 0)
//        {
//            Gonder = true;
//        }
//        //Sayi_1
//        else if (_GonderilecekTag == "Sayı_1" && inventory.Sayi_1 > 0)
//        {
//            Gonder = true;
//        }
//        //Sayi_2
//        else if (_GonderilecekTag == "Sayı_2" && inventory.Sayi_2 > 0)
//        {
//            Gonder = true;
//        }
//        //Sayi_3
//        else if (_GonderilecekTag == "Sayı_3" && inventory.Sayi_3 > 0)
//        {
//            Gonder = true;
//        }
//        //Sayi_4
//        else if (_GonderilecekTag == "Sayı_4" && inventory.Sayi_4 > 0)
//        {
//            Gonder = true;
//        }
//        //Sayi_5
//        else if (_GonderilecekTag == "Sayı_5" && inventory.Sayi_5 > 0)
//        {
//            Gonder = true;
//        }
//        //Sayi_6
//        else if (_GonderilecekTag == "Sayı_6" && inventory.Sayi_6 > 0)
//        {
//            Gonder = true;
//        }
//        //Sayi_7
//        else if (_GonderilecekTag == "Sayı_7" && inventory.Sayi_7 > 0)
//        {
//            Gonder = true;
//        }
//        //Sayi_8
//        else if (_GonderilecekTag == "Sayı_8" && inventory.Sayi_8 > 0)
//        {
//            Gonder = true;
//        }
//        //Sayi_9
//        else if (_GonderilecekTag == "Sayı_9" && inventory.Sayi_9 > 0)
//        {
//            Gonder = true;
//        }
//        else
//            Gonder = false;
//    }
//    public void DegerleriGonderVeYazdir(string Txt_YazilacakDeger)
//    {
//        EnvanterKontrol(GonderilecekTag);

//        if (Gonder)
//        {
//            inventory.CapacityHasChanged(GonderilecekTag, -GonderilecekDeger);
//            //Girdi alanına yazılan değer.
//            txt_InputField.text += Txt_YazilacakDeger;

//            Gonder = false;
//        }
//    }

//    public void SilMethodu()
//    {
//        if(txt_InputField.text.Length == 0)
//        {
//            return;
//        }
//        else
//        {
//            //Son girilen yazıyı bul ve last input'a ata.
//            string last_input = txt_InputField.text.Substring(txt_InputField.text.Length - 1);
//            //Son girilen yazıyı sil.
//            txt_InputField.text = txt_InputField.text.Remove(txt_InputField.text.Length - 1);
//            //Gönderilecek Değeri ata
//            GonderilecekDeger = 1;
//            //Silinen yazının hangi sayı olduğunu bul
//            FindTheTag(int.Parse(last_input));
//            //Envanter kapasitesini düzenle
//            inventory.CapacityHasChanged(GonderilecekTag, GonderilecekDeger);
//        }
//    }

//    void FindTheTag(int silinenSayi)
//    {
//        if(silinenSayi == 0)
//        {
//            GonderilecekTag = "Sayı_0";
//        }
//        if (silinenSayi == 1)
//        {
//            GonderilecekTag = "Sayı_1";
//        }
//        if (silinenSayi == 2)
//        {
//            GonderilecekTag = "Sayı_2";
//        }
//        if (silinenSayi == 3)
//        {
//            GonderilecekTag = "Sayı_3";
//        }
//        if (silinenSayi == 4)
//        {
//            GonderilecekTag = "Sayı_4";
//        }
//        if (silinenSayi == 5)
//        {
//            GonderilecekTag = "Sayı_5";
//        }
//        if (silinenSayi == 6)
//        {
//            GonderilecekTag = "Sayı_6";
//        }
//        if (silinenSayi == 7)
//        {
//            GonderilecekTag = "Sayı_7";
//        }
//        if (silinenSayi == 8)
//        {
//            GonderilecekTag = "Sayı_8";
//        }
//        if (silinenSayi == 9)
//        {
//            GonderilecekTag = "Sayı_9";
//        }
//    }

//    public void CevaplaMethodu()
//    {
//        //if(txt_InputField.text == AcilacakKapinin_Scripti.DoorLockedPassword)
//        //{
//        //    AcilacakKapinin_Scripti.DoorIsLocked = false;
//        //    txt_InputField.text = "";
//        //    LP_Controller.Allow_Input = true;
//        //}
//        //else
//        //{
//        //    StartCoroutine(WaitGivenSecond(Txt_Warning_GO));
//        //}
//    }

//    IEnumerator WaitGivenSecond(GameObject target)
//    {
//        //--Setting transparency of text
//        //-->
//        //byte r, g, b, a;
//        //var text = Txt_Warning_GO.GetComponentInChildren<TMP_Text>();
//        //r = (byte)text.color.r;
//        //g = (byte)text.color.g;
//        //b = (byte)text.color.b;
//        //a = (byte)text.color.a;
//        //text.faceColor = new Color32(r, g, b, 255 - 200);
//        target.SetActive(true);
//        yield return new WaitForSeconds(2);
//        target.SetActive(false);
//    }

//    #endregion


//    //instance_OpenTheDoor tarafından çağırılan sil methodu -> Canvas kapandığında sayıları envantere ekle.
//    public void TextiTamamenTemizle()
//    {
//        if (txt_InputField.text.Length == 0) return;
//        else
//        {
//            while (txt_InputField.text.Length != 0)
//            {

//                //Son girilen yazıyı bul ve last input'a ata.
//                string last_input = txt_InputField.text.Substring(txt_InputField.text.Length - 1);
//                //Son girilen yazıyı sil.
//                txt_InputField.text = txt_InputField.text.Remove(txt_InputField.text.Length - 1);
//                //Gönderilecek Değeri ata
//                GonderilecekDeger = 1;
//                //Silinen yazının hangi sayı olduğunu bul
//                FindTheTag(int.Parse(last_input));
//                //Envanter kapasitesini düzenle
//                inventory.CapacityHasChanged(GonderilecekTag, GonderilecekDeger);

//                if (txt_InputField.text.Length == 0) break;
//            }
//        }
//    }

//    #region BlackBoard için buttonların kullanacağı RAW methodlar

//    /*
//     Text alanına birinci sayı girildikten sonra operatöre tıklanacak ve sonrasında ikinci sayı girilecek.
//     -Yani operatörlerin(butonların) fonksiyonları çalışıp girilen değeri alacak gerekli fonksiyona gönderecek.
//     Bunun için tek bir SayiyiAl methodu yapılabilir. -Bütün operatörler girilen sayıyı alacak sonuçta.
//     Eğer bir operatör tıklandıysa diğer butonlar deaktif olacak ve işlemi iptal et butonu aktif hale gelecek.
     
//         */


//    [SerializeField]
//    GameObject GO_SayiEklenecekUI = null;
//    [SerializeField]
//    TMP_Text BlackBoard_txt_InputField = null;

//    string BlackBoard_GonderilecekTag = null;
//    //Envanterden eksiltilecek değer
//    int BlackBoard_GonderilecekDeger = 0, 
//        sayi1, sayi2;
//    bool ilkSayiAlindi, islem_Secildi, islem_Toplama, islem_Cikartma, ikinciSayiAlindi;

//    //Bu methodları işlemi tamamla butonu çağıracak.
//    public int ToplaMethodu(int _sayi_1, int _sayi_2)
//    {
//        int _sonuc = _sayi_1 + _sayi_2;
//        return _sonuc;
//    }
//    public int CikartMethodu(int _sayi_1, int sayi_2)
//    {
//        int _sonuc = _sayi_1 - sayi_2;
//        return _sonuc;
//    }


//    //btn_SayıAl
//    public void Ac_SayiEklencekUI()
//    {
//        GO_SayiEklenecekUI.SetActive(!GO_SayiEklenecekUI.activeSelf);
//        gameObject.SetActive(false);
//    }

//    //button tarafından gönderilen tagi BlackBoard_GonderilecekTag değişkenine ata
//    public void BlackBoard_TagiAl(string alinanTag)
//    {
//        BlackBoard_GonderilecekTag = alinanTag;
//    }

//    //button tarafından gönderilen değeri BlackBoard_GonderilecekDeger değişkenine ata
//    public void BlackBoard_DegeriAl(int alinanDeger)
//    {
//        BlackBoard_GonderilecekDeger = alinanDeger;
//    }

//    //Button tıklandığında BlackBoard için input alanı uygun mu değil mi kontrol edilecek.
//    //Envanter kontrol edildi.
//    //Eğer InputField üzerinde bulunan karakter sayısı birden küçükse 
//    //ve Gonder true ise;
//    //Değer envanterden eksiltildi.
//    //Değer ekrana yazdırıldı.
//    //Gonder tekrar false yapıldı.
//    public void BlackBoard_InputFieldControl_Send_Print(string InputFieldYazilacakDeger)
//    {
//        Debug.Log("ilkSayiAlindi : " + ilkSayiAlindi + " İşlem Seçildi : " + islem_Secildi + " ikinciSayiAlindi : " + ikinciSayiAlindi);
//        EnvanterKontrol(BlackBoard_GonderilecekTag);
//        if (BlackBoard_txt_InputField.text.Length == 0)
//        {
//            inventory.CapacityHasChanged(BlackBoard_GonderilecekTag, -BlackBoard_GonderilecekDeger);
//            BlackBoard_txt_InputField.text = InputFieldYazilacakDeger;
//            sayi1 = int.Parse(BlackBoard_txt_InputField.text.Substring(BlackBoard_txt_InputField.text.Length - 1));

//            ilkSayiAlindi = true;
//            Gonder = false;
//        }
//        //Ekrandaki sayı eğer tamsayı olabiliyorsa if statement çalışmayacak.
//        //Yani sonucu göstere tıklandığında 2 basamaklı bir cevap aldığımızda başka bir sayı daha yazılamayacak.
//        bool success = int.TryParse(BlackBoard_txt_InputField.text, out int testsayisi);
//        if (BlackBoard_txt_InputField.text.Length == 2 && !success)
//        {
//            inventory.CapacityHasChanged(BlackBoard_GonderilecekTag, -BlackBoard_GonderilecekDeger);
//            BlackBoard_txt_InputField.text += InputFieldYazilacakDeger;
//            sayi2 = int.Parse(BlackBoard_txt_InputField.text.Substring(BlackBoard_txt_InputField.text.Length - 1));

//            ikinciSayiAlindi = true;
//        }
//    }

//    public void BlackBoard_SonucuGoster()
//    {
//        if (islem_Cikartma)
//        {
//            if (CikartMethodu(sayi1, sayi2) <= 0)
//            {
//                Debug.LogWarning("İşlem Sonucu 0'dan küçük veya eşit olamaz!!");
//                return;
//            }
//            else
//            {
//                var sonuc = CikartMethodu(sayi1, sayi2);
//                BlackBoard_txt_InputField.text = sonuc.ToString();
//                Debug.LogError($"ÇIKARTMA {sayi1} - {sayi2} = Envantere Eklenecek Sayı : {sayi1 - sayi2}");
//                sayi1 = 0;
//                sayi2 = 0;
//            }
//        }
//        if (islem_Toplama)
//        {
//            if (ToplaMethodu(sayi1, sayi2) <= 0)
//            {
//                Debug.LogWarning("İşlem Sonucu 0'dan küçük veya eşit olamaz!!");
//                return;
//            }
//            else
//            {
//                var sonuc = ToplaMethodu(sayi1, sayi2);
//                BlackBoard_txt_InputField.text = sonuc.ToString();
//                Debug.LogError($" TOPLAMA {sayi1} + {sayi2} = sonuç : {sayi1 + sayi2}");
//                sayi1 = 0;
//                sayi2 = 0;
//            }
//        }

//        ilkSayiAlindi = true;
//        islem_Secildi = false;
//        ikinciSayiAlindi = false;
//        islem_Toplama = false;
//        islem_Cikartma = false;
//    }

//    //Buttonların gönderdiği string bilgisine göre işlem seçilecek.
//    public void BlackBoard_OperatorSecimi(string _Operator)
//    {
//        if (islem_Secildi)
//        {
//            Debug.LogWarning("Daha önce işlem seçilmiş. İşlemi silin ve tekrar deneyin.");
//            return;
//        }
//        else if (BlackBoard_txt_InputField.text.Length == 1)
//        {
//            if (_Operator == "+")
//            {
//                islem_Cikartma = false;
//                islem_Toplama = true;
//                Debug.LogWarning("Toplama : " + islem_Toplama);
//            }

//            if (_Operator == "-")
//            {
//                islem_Toplama = false;
//                islem_Cikartma = true;
//                Debug.LogWarning("Çıkartma : " + islem_Cikartma);
//            }

//            BlackBoard_txt_InputField.text += _Operator;
//            islem_Secildi = true;
//        }

//    }

//    public string BlackBoard_FindTheTag(int silinenSayi)
//    {
//        string GonderilecekTag = null;
//        if (silinenSayi == 0)
//        {
//            GonderilecekTag = "Sayı_0";
//        }
//        if (silinenSayi == 1)
//        {
//            GonderilecekTag = "Sayı_1";
//        }
//        if (silinenSayi == 2)
//        {
//            GonderilecekTag = "Sayı_2";
//        }
//        if (silinenSayi == 3)
//        {
//            GonderilecekTag = "Sayı_3";
//        }
//        if (silinenSayi == 4)
//        {
//            GonderilecekTag = "Sayı_4";
//        }
//        if (silinenSayi == 5)
//        {
//            GonderilecekTag = "Sayı_5";
//        }
//        if (silinenSayi == 6)
//        {
//            GonderilecekTag = "Sayı_6";
//        }
//        if (silinenSayi == 7)
//        {
//            GonderilecekTag = "Sayı_7";
//        }
//        if (silinenSayi == 8)
//        {
//            GonderilecekTag = "Sayı_8";
//        }
//        if (silinenSayi == 9)
//        {
//            GonderilecekTag = "Sayı_9";
//        }

//        return GonderilecekTag;
//    }

//    public void BlackBoard_SilMethodu()
//    {
//        if (BlackBoard_txt_InputField != null && inventory.Inventory_Capacity > 0)
//        {
//            string str_last_input;
//            //Son karakteri al.
//            str_last_input = BlackBoard_txt_InputField.text.Substring(BlackBoard_txt_InputField.text.Length - 1);
//            //Son karakteri sil
//            BlackBoard_txt_InputField.text = BlackBoard_txt_InputField.text.Remove(BlackBoard_txt_InputField.text.Length - 1);
//            bool success = int.TryParse(str_last_input, out int int_last_input);
//            if (success)
//            {
//                BlackBoard_GonderilecekTag = BlackBoard_FindTheTag(int_last_input);
//                BlackBoard_GonderilecekDeger = 1;
//                inventory.CapacityHasChanged(BlackBoard_GonderilecekTag, BlackBoard_GonderilecekDeger);
//            }
//            else
//            {
//                islem_Secildi = false;
//            }
//        }

//    }
   

//    #endregion
//}
