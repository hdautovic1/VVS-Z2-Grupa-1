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
        public void TestZivotinjaKonstruktor()
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
        #endregion

        #region Proizvod

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
            Assert.AreEqual(z.Vrsta, "Jaja");

            z.Vrsta = ZivotinjskaVrsta.Guska;
            p = new Proizvod("", "", "Jaja", z, DateTime.Today, DateTime.Today.AddDays(5), 100);
            Assert.AreEqual(z, p.Proizvođač);
            Assert.AreEqual(z.Vrsta, "Jaja");

        }
        #endregion
    }
}
