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
        public static IEnumerable<object[]> UčitajPodatkeCSV(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var rows = csv.GetRecords<dynamic>();
                foreach (var row in rows)
                {
                    var values = ((IDictionary<String, Object>)row).Values;
                    var elements = values.Select(elem => elem.ToString()).ToList();
                       if(fileName.Equals("NeispravniPodaciLokacija.csv"))
                        yield return new object[] { elements[0], elements[1],
                        elements[2], elements[3], elements[4], elements[5],double.Parse(elements[6]) };

                       else if(fileName.Equals("NeispravniPodaciZivotinja.csv"))
                        yield return new object[] { DateTime.Parse(elements[0]), double.Parse(elements[1]), 
                            double.Parse(elements[2])};

                       else if(fileName.Equals("NeispravniPodaciProizvod.csv"))
                        yield return new object[] { elements[0],DateTime.Parse(elements[1]),
                            DateTime.Parse(elements[2]),Int32.Parse(elements[3]) };
                }
            }
        }


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
        //Implementirala Selma Hadžijusufović (izuzeci obrađeni testovima)
        [TestMethod]
        //Životinja nije registrovana u bazi
        [ExpectedException(typeof(ArgumentException))]
        public void TestIzuzetakRadSaZivotinjama1()
        {
            Farma farma = new Farma();
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 500);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(-2), 1000, 150, l);
            farma.RadSaZivotinjama("Brisanje", z);
        }
        [TestMethod]
        //Životinja je već registrovana u bazi
        [ExpectedException(typeof(ArgumentException))]
        public void TestIzuzetakRadSaZivotinjama2()
        {
            Farma farma = new Farma();
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 500);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(-2), 1000, 150, l);
             
            farma.RadSaZivotinjama("Dodavanje", z);
            farma.RadSaZivotinjama("Dodavanje", z);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestIzuzetakDodavanjeLokacije()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 100);
            Farma farma = new Farma();
            farma.DodavanjeNoveLokacije(l);
            farma.DodavanjeNoveLokacije(l);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestIzuzetakSpecijalizacijaFarme()
        {
            Farma farma = new Farma();
            farma.SpecijalizacijaFarme(ZivotinjskaVrsta.Koza, 100);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIzuzetakSpecijalizacijaFarme2()
        {
            Farma farma = new Farma();
            farma.SpecijalizacijaFarme(ZivotinjskaVrsta.Krava, 1500);
        }
        #endregion

        #region Lokacija

        static IEnumerable<object[]> NeispravnaLokacijaCSV
        {
            get
            {
                return UčitajPodatkeCSV("NeispravniPodaciLokacija.csv");
            }
        }
        [TestMethod]
        [DynamicData("NeispravnaLokacijaCSV")]
        [ExpectedException(typeof(ArgumentException))]
        public void TestKonstruktoraLokacijeCSV (string naziv, string adresa, string brojUlice, string grad,
            string postanskiBroj, string drzava, double povrsina)
        {
            List<string> parametri=new List<String>();
            parametri.Add(naziv);
            parametri.Add(adresa);
            parametri.Add(brojUlice);
            parametri.Add(grad);
            parametri.Add(postanskiBroj);
            parametri.Add(drzava);

            Lokacija l = new Lokacija(parametri,povrsina);
        }


        //[TestMethod]        
        //public void TestLokacija()
        //{
        //    List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
        //    List<string> parametri2 = new List<string> { "Naziv", "Adresa", "Sarajevo", "10001", "Bosna i Hercegovina" };

        //    Lokacija l = new Lokacija(parametri2,100);
        //    l = new Lokacija(parametri, 100);

        //    Assert.AreEqual(l.Adresa, "Adresa");
        //    Assert.AreEqual(l.Naziv, "Naziv");
        //    Assert.AreEqual(l.BrojUlice, 2);
        //    Assert.AreEqual(l.Grad, "Sarajevo");
        //    Assert.AreEqual(l.PoštanskiBroj, 10001);
        //    Assert.AreEqual(l.Država, "Bosna i Hercegovina");
        //    Assert.AreEqual(l.Površina, 100);
        //}

        //Implementirala Selma Hadžijusufović (izuzeci obrađeni testovima)
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIzuzetakSetterNaziva()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija lokacija = new Lokacija(parametri, 1000);
            lokacija.Naziv = "";


        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIzuzetakSetterAdrese()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija lokacija = new Lokacija(parametri, 1000);
            lokacija.Adresa = "";
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIzuzetakNepodrzanGrad()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija lokacija = new Lokacija(parametri, 1000);
            lokacija.Grad = "Paris";
        }
    
       [TestMethod]
       [ExpectedException(typeof(ArgumentException))]
        public void TestIzuzetakNepodrzanaDrzava()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija lokacija = new Lokacija(parametri, 1000);
            lokacija.Država = "Francuska";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIzuzetakNevalidanBrojUlice()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija lokacija = new Lokacija(parametri, 1000);
            lokacija.BrojUlice = -2;
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIzuzetakNevalidanPoštanskiBroj()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija lokacija = new Lokacija(parametri, 1000);
            lokacija.PoštanskiBroj = 500;
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIzuzetakNevalidnaPovršina()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija lokacija = new Lokacija(parametri, 1000);
            lokacija.Površina = 0;
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIzuzetakNevalidnaPovršina2()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija lokacija = new Lokacija(parametri, 0);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIzuzetakPrazniParametri()
        {
            List<string> parametri = new List<string> { "", "", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija lokacija = new Lokacija(parametri, 1000);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIzuzetakNeispravanBrojParametara()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001" };
            Lokacija lokacija = new Lokacija(parametri, 0);
        }

        #endregion

        #region Zivotinja

        static IEnumerable<object[]> NeispravnaZivotinjaCSV
        {
            get
            {
                return UčitajPodatkeCSV("NeispravniPodaciZivotinja.csv");
            }
        }
        [TestMethod]
        [DynamicData("NeispravnaZivotinjaCSV")]
        [ExpectedException(typeof(FormatException))]
        public void TestKonstruktoraZivotinjeCSV(DateTime starost,double masa, double visina)
        { 
            List<string> parametri = new List<string>();
            parametri.Add("Naziv");
            parametri.Add("Adresa");
            parametri.Add("17");
            parametri.Add("Sarajevo");
            parametri.Add("71000");
            parametri.Add("Bosna i Hercegovina");
            Lokacija l = new Lokacija(parametri,10000);

            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, starost, masa, visina, l);
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
        public void TestProvjeriStanjeZivotinje()
        {
            List<string> parametri = new List<string>();
            parametri.Add("Naziv");
            parametri.Add("Adresa");
            parametri.Add("1814");
            parametri.Add("Sarajevo");
            parametri.Add("71000");
            parametri.Add("Bosna i Hercegovina");

            //slucaj kada je zivotinja starija od 10 godina
            Zivotinja zivotinja1 = new Zivotinja(ZivotinjskaVrsta.Krava, new System.DateTime(2011, 4, 1), 800, 150, new Lokacija(parametri, 12000));

            zivotinja1.ProvjeriStanjeZivotinje();

            Assert.IsFalse(zivotinja1.Proizvođač);



            //slucaj kada je zivotinja starija od 7 godina i najnoviji pregled ima ocjenu 3.5 ili manje
            Zivotinja zivotinja2 = new Zivotinja(ZivotinjskaVrsta.Ovca, new System.DateTime(2014, 11, 24), 180, 117, new Lokacija(parametri, 12000));

            zivotinja2.PregledajZivotinju("", "", "4.0");
            zivotinja2.PregledajZivotinju("", "", "3.2");

            zivotinja2.ProvjeriStanjeZivotinje();

            Assert.IsFalse(zivotinja2.Proizvođač);



            //slucaj kada je zivotinja starija od 7 godina, ali najnoviji pregled ima ocjenu vecu od 3.5
            Zivotinja zivotinja3 = new Zivotinja(ZivotinjskaVrsta.Ovca, new System.DateTime(2014, 11, 24), 180, 117, new Lokacija(parametri, 12000));

            zivotinja3.PregledajZivotinju("", "", "3.8");
            zivotinja3.PregledajZivotinju("", "", "3.4");
            zivotinja3.PregledajZivotinju("", "", "3.6");

            zivotinja3.ProvjeriStanjeZivotinje();

            Assert.IsFalse(zivotinja3.Proizvođač);



            //posljednji slucaj
            Zivotinja zivotinja4 = new Zivotinja(ZivotinjskaVrsta.Patka, new System.DateTime(2020, 9, 1), 22, 60, new Lokacija(parametri, 12000));

            zivotinja4.PregledajZivotinju("", "", "4.0");
            zivotinja4.PregledajZivotinju("", "", "3.2");
            zivotinja4.PregledajZivotinju("", "", "3.5");

            zivotinja4.ProvjeriStanjeZivotinje();

            Assert.IsFalse(zivotinja4.Proizvođač);



            Zivotinja zivotinja5 = new Zivotinja(ZivotinjskaVrsta.Patka, new System.DateTime(2020, 9, 1), 22, 60, new Lokacija(parametri, 12000));

            zivotinja5.PregledajZivotinju("", "", "4.0");
            zivotinja5.PregledajZivotinju("", "", "4.8");
            zivotinja5.PregledajZivotinju("", "", "4.7");

            zivotinja5.ProvjeriStanjeZivotinje();

            Assert.IsTrue(zivotinja5.Proizvođač);


        }
        //Implementirala Selma Hadžijusufović (izuzeci obrađeni testovima)
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestIzuzetakNevalidnaStarost()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Zivotinja zivotinja = new Zivotinja(ZivotinjskaVrsta.Krava, new System.DateTime(2011, 4, 1), 800, 150, new Lokacija(parametri, 12000));
            zivotinja.Starost = new DateTime(2022, 4, 11);
            
        }
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestIzuzetakNevalidnaTjelesnaMasa()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Zivotinja zivotinja = new Zivotinja(ZivotinjskaVrsta.Krava, new System.DateTime(2011, 4, 1), 800, 150, new Lokacija(parametri, 12000));
            zivotinja.TjelesnaMasa = 0;

        }
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestIzuzetakNevalidnaVisina()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Zivotinja zivotinja = new Zivotinja(ZivotinjskaVrsta.Krava, new System.DateTime(2011, 4, 1), 800, 150, new Lokacija(parametri, 12000));
            zivotinja.Visina = 0;

        }
        [TestMethod]
        public void TestGetHashCode()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Zivotinja zivotinja1 = new Zivotinja(ZivotinjskaVrsta.Krava, new System.DateTime(2011, 4, 1), 800, 150, new Lokacija(parametri, 12000));
            Zivotinja zivotinja2 = new Zivotinja(ZivotinjskaVrsta.Krava, new System.DateTime(2011, 4, 1), 800, 150, new Lokacija(parametri, 11000));
            Zivotinja zivotinja3 = new Zivotinja(ZivotinjskaVrsta.Kokoška, new System.DateTime(2011, 1, 1), 300, 50, new Lokacija(parametri, 5000));

            Assert.AreEqual(zivotinja1.GetHashCode(), zivotinja2.GetHashCode());
            Assert.AreNotEqual(zivotinja1.GetHashCode(), zivotinja3.GetHashCode());

        }
        #endregion

        #region Proizvod
        static IEnumerable<object[]> NeispravanProizvodCSV
        {
            get
            {
                return UčitajPodatkeCSV("NeispravniPodaciProizvod.csv");
            }
        }
        [TestMethod]
        [DynamicData("NeispravanProizvodCSV")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestKonstruktoraProizvodaCSV(string vrsta, DateTime proizvodnja, DateTime rok, int kol)
        {
            List<string> parametri = new List<string>();
            parametri.Add("Naziv");
            parametri.Add("Adresa");
            parametri.Add("17");
            parametri.Add("Sarajevo");
            parametri.Add("71000");
            parametri.Add("Bosna i Hercegovina");
            Lokacija l = new Lokacija(parametri, 10000);

            Zivotinja zivotinja = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Now, 20, 50, l);
            Proizvod p = new Proizvod("", "",vrsta, zivotinja, proizvodnja, rok, kol);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestKonstruktoraProizvoda()
        {
            ///preostale kombinacije proizvoda i zivotinje
            List<string> parametri = new List<string>();
            parametri.Add("Naziv");
            parametri.Add("Adresa");
            parametri.Add("17");
            parametri.Add("Sarajevo");
            parametri.Add("71000");
            parametri.Add("Bosna i Hercegovina");
            Lokacija l = new Lokacija(parametri, 10000);

            Zivotinja zivotinja = new Zivotinja(ZivotinjskaVrsta.Kokoška, DateTime.Now, 20, 50, l);
            Proizvod p = new Proizvod("", "", "Sir", zivotinja, DateTime.Now, new DateTime(2022,5,5), 10);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestKonstruktoraProizvoda2()
        {
            ///preostale kombinacije proizvoda i zivotinje
            List<string> parametri = new List<string>();
            parametri.Add("Naziv");
            parametri.Add("Adresa");
            parametri.Add("17");
            parametri.Add("Sarajevo");
            parametri.Add("71000");
            parametri.Add("Bosna i Hercegovina");
            Lokacija l = new Lokacija(parametri, 10000);

            Zivotinja zivotinja = new Zivotinja(ZivotinjskaVrsta.Kokoška, DateTime.Now, 20, 50, l);
            Proizvod p = new Proizvod("", "", "Mlijeko", zivotinja, DateTime.Now, new DateTime(2022, 5, 5), 12);
        }
        //Implementirala Selma Hadžijusufović (izuzeci obrađeni testovima)
        [TestMethod]
        public void TestGetteraProizvoda()
        {
            List<string> parametri = new List<string>();
            parametri.Add("Naziv");
            parametri.Add("Adresa");
            parametri.Add("17");
            parametri.Add("Sarajevo");
            parametri.Add("71000");
            parametri.Add("Bosna i Hercegovina");
            Lokacija l = new Lokacija(parametri, 10000);

            Zivotinja zivotinja = new Zivotinja(ZivotinjskaVrsta.Kokoška, DateTime.Now, 20, 50, l);
            DateTime rok = new DateTime(2022, 5, 5);
            DateTime datumProizvodnje = DateTime.Now;
            Proizvod p = new Proizvod("", "", "Jaja", zivotinja, datumProizvodnje, rok, 12);
            Assert.AreEqual("Jaja", p.Vrsta);
            Assert.AreEqual(zivotinja, p.Proizvođač);
            Assert.AreEqual(datumProizvodnje, p.DatumProizvodnje);
            Assert.AreEqual(rok, p.RokTrajanja);

        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        //Vrsta proizvoda koja ne postoji
        public void TestIzuzetkaVrsta()
        {
            ///preostale kombinacije proizvoda i zivotinje
            List<string> parametri = new List<string>();
            parametri.Add("Naziv");
            parametri.Add("Adresa");
            parametri.Add("17");
            parametri.Add("Sarajevo");
            parametri.Add("71000");
            parametri.Add("Bosna i Hercegovina");
            Lokacija l = new Lokacija(parametri, 10000);

            Zivotinja zivotinja = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Now, 20, 50, l);
            Proizvod p = new Proizvod("", "", "Mlijeko", zivotinja, DateTime.Now, new DateTime(2022, 5, 5), 12);
            p.Vrsta = "Maslo";
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestIzuzetkaProizvođača()
        {
            ///preostale kombinacije proizvoda i zivotinje
            List<string> parametri = new List<string>();
            parametri.Add("Naziv");
            parametri.Add("Adresa");
            parametri.Add("17");
            parametri.Add("Sarajevo");
            parametri.Add("71000");
            parametri.Add("Bosna i Hercegovina");
            Lokacija l = new Lokacija(parametri, 10000);

            Zivotinja zivotinja = new Zivotinja(ZivotinjskaVrsta.Kokoška, DateTime.Now, 20, 50, l);
            Proizvod p = new Proizvod("", "", "Jaja", zivotinja, DateTime.Now, new DateTime(2022, 5, 5), 12);
            Zivotinja zivotinja2 = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Now, 20, 50, l);
            p.Proizvođač = zivotinja2;
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestIzuzetkaDatumaProizvodnje()
        {
            ///preostale kombinacije proizvoda i zivotinje
            List<string> parametri = new List<string>();
            parametri.Add("Naziv");
            parametri.Add("Adresa");
            parametri.Add("17");
            parametri.Add("Sarajevo");
            parametri.Add("71000");
            parametri.Add("Bosna i Hercegovina");
            Lokacija l = new Lokacija(parametri, 10000);

            Zivotinja zivotinja = new Zivotinja(ZivotinjskaVrsta.Kokoška, DateTime.Now, 20, 50, l);
            Proizvod p = new Proizvod("", "", "Jaja", zivotinja, DateTime.Now, new DateTime(2022, 5, 5), 12);
            p.DatumProizvodnje = new DateTime(2022, 11, 12);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestIzuzetkaRokaTrajanja()
        {
            ///preostale kombinacije proizvoda i zivotinje
            List<string> parametri = new List<string>();
            parametri.Add("Naziv");
            parametri.Add("Adresa");
            parametri.Add("17");
            parametri.Add("Sarajevo");
            parametri.Add("71000");
            parametri.Add("Bosna i Hercegovina");
            Lokacija l = new Lokacija(parametri, 10000);

            Zivotinja zivotinja = new Zivotinja(ZivotinjskaVrsta.Kokoška, DateTime.Now, 20, 50, l);
            Proizvod p = new Proizvod("", "", "Jaja", zivotinja, DateTime.Now, new DateTime(2022, 5, 5), 12);
            p.RokTrajanja = new DateTime(2021, 11, 23);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestIzuzetkaKoličine()
        {
            ///preostale kombinacije proizvoda i zivotinje
            List<string> parametri = new List<string>();
            parametri.Add("Naziv");
            parametri.Add("Adresa");
            parametri.Add("17");
            parametri.Add("Sarajevo");
            parametri.Add("71000");
            parametri.Add("Bosna i Hercegovina");
            Lokacija l = new Lokacija(parametri, 10000);

            Zivotinja zivotinja = new Zivotinja(ZivotinjskaVrsta.Kokoška, DateTime.Now, 20, 50, l);
            Proizvod p = new Proizvod("", "", "Jaja", zivotinja, DateTime.Now, new DateTime(2022, 5, 5), 12);
            p.KoličinaNaStanju = 0;
        }
        #endregion

        #region Kupovina
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

        #region Veterinar
        //Implementirala Selma Hadžijusufović (izuzeci obrađeni testovima)
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void TestIzuzetkaOcjeneZivotinje()
        {
            List<string> parametri = new List<string> { "Naziv", "Adresa", "2", "Sarajevo", "10001", "Bosna i Hercegovina" };
            Lokacija l = new Lokacija(parametri, 100);
            Zivotinja z = new Zivotinja(ZivotinjskaVrsta.Krava, DateTime.Today.AddYears(-2), 1000, 150, l);
            Veterinar veterinar = new Veterinar();
            veterinar.ocjenaZdravstvenogStanjaZivotinje(z);
        }


        #endregion
    }
}
