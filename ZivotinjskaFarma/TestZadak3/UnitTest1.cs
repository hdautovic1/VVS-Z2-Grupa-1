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
        
        public void TestLokacijaKonstruktora()
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
    }
}
