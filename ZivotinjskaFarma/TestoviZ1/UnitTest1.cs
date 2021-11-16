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
    }
}
