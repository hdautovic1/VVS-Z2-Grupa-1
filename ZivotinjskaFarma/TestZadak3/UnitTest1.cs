using CsvHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ZivotinjskaFarma;
namespace TestZadak3
{
    [TestClass]
    public class UnitTest1
    {
        #region Farma
        [TestMethod]
        public void TestInicijalizacijaFarme()
        {
            Farma farma = new Farma();
            Assert.AreEqual(farma.Lokacije.Count, 0);
            Assert.AreEqual(farma.Proizvodi.Count, 0);
            Assert.AreEqual(farma.Zivotinje.Count, 0);
            Assert.AreEqual(farma.Kupovine.Count, 0);
        }

        [TestMethod]
        public void TestDodavanjeiBrisanjeLokacijeFarme()
        {
            Farma farma = new Farma();
            List<string> parametri = new List<string> { "Naziv1", "Adresa1", "2", "Mostar", "10001", "Bosna i Hercegovina" };
            List<string> parametri2 = new List<string> { "Naziv2", "Adresa2","69","Sarajevo", "10001", "Bosna i Hercegovina" };

            Lokacija l1 = new Lokacija(parametri, 100);
            Lokacija l2 = new Lokacija(parametri2, 100);
            farma.DodavanjeNoveLokacije(l1);
            farma.DodavanjeNoveLokacije(l2);
            Assert.AreEqual(farma.Lokacije.Count, 2);
            farma.BrisanjeLokacije(l2);
            Assert.AreEqual(farma.Lokacije.Count, 1);
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
        [TestMethod]
        public void TestPraznici()
        {
                 

            Assert.IsTrue(Farma.Praznik(DateTime.Parse("01/01/2000")));
            Assert.IsTrue(Farma.Praznik(DateTime.Parse("03/01/2000")));
            Assert.IsTrue(Farma.Praznik(DateTime.Parse("05/01/2000")));
            Assert.IsTrue(Farma.Praznik(DateTime.Parse("11/25/2000")));
            Assert.IsTrue(Farma.Praznik(DateTime.Parse("12/31/2000")));
        }
        [TestMethod]
        public void TestRadSaZivotinjama()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(-2), 1000, 150, l);
            Farma farma = new Farma();
            farma.RadSaZivotinjama("Dodavanje", z);
            Assert.AreEqual(farma.Zivotinje.Count, 1);
            Assert.AreEqual(farma.Zivotinje[0].Vrsta.ToString(), ZivotinjskaVrsta.Krava.ToString());
            z.Vrsta = ZivotinjskaVrsta.Magarac;
            farma.RadSaZivotinjama("Izmjena", z);
            Assert.AreEqual(farma.Zivotinje[0].Vrsta.ToString(), ZivotinjskaVrsta.Magarac.ToString());
            farma.RadSaZivotinjama("Brisanje", z);
            Assert.AreEqual(farma.Zivotinje.Count, 0);
        }
        [TestMethod]
        public void TestKupovinaProizvoda()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(-2), 1000, 150, l);
            Proizvod p = new Proizvod("", "", "Mlijeko", z, DateTime.Today, DateTime.Today.AddDays(5), 100);

            Farma farma = new Farma();

            bool uspjesnaKupovina = farma.KupovinaProizvoda(p, DateTime.Today.AddDays(3), 30);
            Assert.IsTrue(uspjesnaKupovina);
            Assert.AreEqual(farma.Kupovine.Count,1);
            Kupovina k = farma.Kupovine[0];
            farma.BrisanjeKupovine(k);
            Assert.AreEqual(farma.Kupovine.Count, 0);
            uspjesnaKupovina = farma.KupovinaProizvoda(p, DateTime.Today.AddDays(3), 120);
            Assert.IsFalse(uspjesnaKupovina);
        }
        #endregion

        #region Lokacija
        [TestMethod]        
        public void TestLokacija()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            List<string> parametri2 = new List<string> { "Naziv", "Adresa", "Sarajevo", "10001", "Bosna i Hercegovina" };

            Lokacija l = new Lokacija(parametri2,100);
            l = new Lokacija(parametri, 100);

            Assert.AreEqual(l.Adresa, "Adresa");
            Assert.AreEqual(l.Naziv, "Naziv");
            Assert.AreEqual(l.BrojUlice, 2);
            Assert.AreEqual(l.Grad, "Sarajevo");
            Assert.AreEqual(l.PoštanskiBroj, 10001);
            Assert.AreEqual(l.Država, "Bosna i Hercegovina");
            Assert.AreEqual(l.Površina, 100);
        }

     
        #endregion

        #region Zivotinja
        [TestMethod]
        public void ATestZivotinjaKonstruktor()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
           
            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava,DateTime.Today.AddYears(-2), 1000, 150, l);
            Assert.AreEqual(z.Vrsta.ToString(), ZivotinjskaVrsta.Krava.ToString());
            Assert.AreEqual(z.TjelesnaMasa, 1000);
            Assert.AreEqual(z.Visina, 150);
            Assert.AreEqual(z.Proizvođač, true);

            Assert.AreEqual(z.Starost, DateTime.Today.AddYears(-2));
            Assert.AreEqual(z.Pregledi.Count, 0);
            Assert.AreEqual(z.ID1, 1);
        }

        [TestMethod]
        public void TestPregledajZivotinju()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(-2), 1000, 150, l);

            string osnovneInfo = "xy", napomena = "yx", ocjena = "4";
            string pregled = "OSNOVNE INFORMACIJE: " + osnovneInfo + "\n"
                         + "NAPOMENA: " + napomena + "\n"
                         + "OCJENA: " + ocjena;
            z.PregledajZivotinju(osnovneInfo, napomena, ocjena);

            Assert.AreEqual(z.Pregledi.Count, 1);
            Assert.AreEqual(z.Pregledi[0], pregled);


        }
     

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestZivotinjaPogresanDatum()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(2), 1000, 150, l);}

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestZivotinjaPogresnaVisina()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(-2), 1000, 0, l);}

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestZivotinjaPogresnaMasa()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(-2), 0, 100, l);
        }

        #endregion

        #region Proizvod

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestProizvodPogresnaVrsta()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(-2), 1000, 150, l);
            Proizvod p = new Proizvod("", "", "x", z, DateTime.Today, DateTime.Today.AddDays(5), 100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestProizvodPogresanProizvođač()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(-2), 1000, 150, l);
            Proizvod p = new Proizvod("", "", "Jaja", z, DateTime.Today, DateTime.Today.AddDays(5), 100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestProizvodPogresanDatumProizvodnje()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(-2), 1000, 150, l);
            Proizvod p = new Proizvod("", "", "Mlijeko", z, DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), 100);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestProizvodPogresanRokTrajanja()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(-2), 1000, 150, l);
            Proizvod p = new Proizvod("", "", "Mlijeko", z, DateTime.Today, DateTime.Today.AddDays(-5), 100);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestProizvodPogresnaKolicina()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(-2), 1000, 150, l);
            Proizvod p = new Proizvod("", "", "Mlijeko", z, DateTime.Today, DateTime.Today.AddDays(5), 0);
        }



        [TestMethod]
        public void TestProizvodKonstrukotr()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };

            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(-2), 1000, 150, l);

            Proizvod p = new Proizvod("", "", "Mlijeko", z, DateTime.Today, DateTime.Today.AddDays(5), 100);

            Assert.AreEqual(p.Vrsta, "Mlijeko");
            Assert.AreEqual(z, p.Proizvođač);
            Assert.AreEqual(p.DatumProizvodnje,DateTime.Today);
            Assert.AreEqual(p.RokTrajanja, DateTime.Today.AddDays(5));
            Assert.AreEqual(p.KoličinaNaStanju, 100);

            z.Vrsta = ZivotinjskaVrsta.Ovca;
            p = new Proizvod("", "", "Mlijeko", z, DateTime.Today, DateTime.Today.AddDays(5), 100);
            Assert.AreEqual(z, p.Proizvođač);

            z.Vrsta = ZivotinjskaVrsta.Koza;
            p = new Proizvod("", "", "Mlijeko", z, DateTime.Today, DateTime.Today.AddDays(5), 100);
            Assert.AreEqual(z, p.Proizvođač);

            z.Vrsta = ZivotinjskaVrsta.Magarac;
            p = new Proizvod("", "", "Mlijeko", z, DateTime.Today, DateTime.Today.AddDays(5), 100);
            Assert.AreEqual(z, p.Proizvođač);

            z.Vrsta = ZivotinjskaVrsta.Kokoška;
            p = new Proizvod("", "", "Jaja", z, DateTime.Today, DateTime.Today.AddDays(5), 100);
            Assert.AreEqual(z, p.Proizvođač);
            Assert.AreEqual(p.Vrsta, "Jaja");

            z.Vrsta = ZivotinjskaVrsta.Guska;
            p = new Proizvod("", "", "Jaja", z, DateTime.Today, DateTime.Today.AddDays(5), 100);
            Assert.AreEqual(z, p.Proizvođač);
            Assert.AreEqual(p.Vrsta, "Jaja");

        }
        #endregion

        #region
        [TestMethod]
        public void TestKupovinaKonstruktor() {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(-2), 1000, 150, l);

            Proizvod p = new Proizvod("", "", "Mlijeko", z, DateTime.Today, DateTime.Today.AddDays(5), 100);
            Kupovina k = new Kupovina("2", DateTime.Today, DateTime.Today.AddDays(3), p, 30, false);
            Assert.AreEqual(Kupovina.DajSljedeciBroj(), 1);

            Assert.AreEqual(k.IDKupca1, "2");
            Assert.AreEqual(k.DatumKupovine, DateTime.Today);
            Assert.AreEqual(k.RokIsporuke, DateTime.Today.AddDays(3));
            Assert.AreEqual(k.KupljeniProizvod, p);
            Assert.AreEqual(k.Kolicina, 30);
            Assert.IsFalse(k.Popust);
        }

        [TestMethod]
        public void TestVerificirajKupovinu()
        {
            List<string> parametri = new List<string>();
            parametri.Add("naziv");
            parametri.Add("Adresa");
            parametri.Add("Mostar");
            parametri.Add("10001");
            parametri.Add("Bosna i Hercegovina");

            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, System.DateTime.Now.AddDays(-500), 1200, 120, l);
            Proizvod p = new Proizvod("Mlijeko", "opis", "Mlijeko", z, System.DateTime.Now.AddDays(-10), System.DateTime.Now.AddDays(10), 20);


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
        #endregion
    }
}
