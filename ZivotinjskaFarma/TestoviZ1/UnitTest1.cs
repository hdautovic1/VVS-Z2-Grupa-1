using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ZivotinjskaFarma;

namespace TestoviZ1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestVerificiraj()
        {
            List<string> parametri = new List<string>();
            parametri.Add("naziv");
            parametri.Add("Adresa");
            parametri.Add("Mostar");
            parametri.Add("10001");
            parametri.Add("Bosna i Hercegovina");

            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, System.DateTime.Now.AddDays(-500), 1200, 120, l);
            Proizvod p = new Proizvod("Mlijeko", "opis", "Mlijeko", z, System.DateTime.Now.AddDays(-10), System.DateTime.Now.AddDays(10),20);


            //U slučaju kada su sve varijable odgovarajuće
            Kupovina k = new Kupovina("kupacID", System.DateTime.Now, System.DateTime.Now.AddDays(5), p, 10, false);
            bool verificirana = k.VerificirajKupovinu();
            Assert.IsTrue(verificirana);

            //U slučaju kada je rok postavljen na više od 7 dana (u slučaju Mlijeko, Jaja ili Sir)
            k.RokIsporuke = System.DateTime.Now.AddDays(10);
            verificirana = k.VerificirajKupovinu();
            Assert.IsFalse(verificirana);


            //U slučaju kada je rok postavljen na manje od 2 dana (u slučaju Mlijeko, Jaja ili Sir)
            k.RokIsporuke = System.DateTime.Now.AddDays(1);
            verificirana = k.VerificirajKupovinu();
            Assert.IsFalse(verificirana);


            //U slučaju kada je količina veća od količine proizvoda (u slučaju Mlijeko, Jaja ili Sir)
            k.RokIsporuke = System.DateTime.Now.AddDays(5);
            k.Kolicina = 30;
            verificirana = k.VerificirajKupovinu();
            Assert.IsFalse(verificirana);


            //U slučaju kada je rok za vunu manji od 30 dana
            k.Kolicina = 10;
            k.KupljeniProizvod.Ime = "Vuna";
            verificirana = k.VerificirajKupovinu();
            Assert.IsFalse(verificirana);



            //U slučaju kada je rok za vunu veći od 30 dana
            k.RokIsporuke = System.DateTime.Now.AddDays(31);
            verificirana = k.VerificirajKupovinu();
            Assert.IsTrue(verificirana);
        }
        [TestMethod]
        public void TestObracunaPoreza()
        {
            List<Lokacija> lokacije = new List<Lokacija>();
            List<string> parametri = new List<string>();
            Farma farma = new Farma();

            //Lokacija 1, površina > 10000
            parametri.Add("naziv");
            parametri.Add("Adresa");
            parametri.Add("Mostar");
            parametri.Add("10001");
            parametri.Add("Bosna i Hercegovina");

            Lokacija lokacija1 = new Lokacija(parametri, 15000);

            //Lokacija 2, površina >= 1000 & povrsina <= 10000, država Bosna i Hercegovina
            parametri.Clear();
            parametri.Add("naziv");
            parametri.Add("Adresa");
            parametri.Add("Sarajevo");
            parametri.Add("10001");
            parametri.Add("Bosna i Hercegovina");

            Lokacija lokacija2 = new Lokacija(parametri, 5400);

            //Lokacija 3, površina >= 1000 & povrsina <= 10000
            parametri.Clear();
            parametri.Add("naziv");
            parametri.Add("Adresa");
            parametri.Add("Rijeka");
            parametri.Add("34789");
            parametri.Add("Hrvatska");

            Lokacija lokacija3 = new Lokacija(parametri, 9785);


            //Lokacija 4, površina < 1000, grad Banja Luka
            parametri.Clear();
            parametri.Add("naziv");
            parametri.Add("Adresa");
            parametri.Add("Banja Luka");
            parametri.Add("10111");
            parametri.Add("Bosna i Hercegovina");

            Lokacija lokacija4 = new Lokacija(parametri, 756);



            //Lokacija 5, površina < 1000
            parametri.Clear();
            parametri.Add("naziv");
            parametri.Add("Adresa");
            parametri.Add("Trebinje");
            parametri.Add("47890");
            parametri.Add("Bosna i Hercegovina");

            Lokacija lokacija5 = new Lokacija(parametri, 756);

            farma.DodavanjeNoveLokacije(lokacija1);
            farma.DodavanjeNoveLokacije(lokacija2);
            farma.DodavanjeNoveLokacije(lokacija3);
            farma.DodavanjeNoveLokacije(lokacija4);
            farma.DodavanjeNoveLokacije(lokacija5);

            double porez = farma.ObračunajPorez();
            double ocekivano = 1.25;
          
            Assert.AreEqual(ocekivano, porez);

        }
    }
}
