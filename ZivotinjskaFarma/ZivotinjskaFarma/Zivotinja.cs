using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ZivotinjskaFarma
{
    public class Zivotinja
    {
        #region Atributi

        int ID;
        ZivotinjskaVrsta vrsta;
        DateTime starost;
        double tjelesnaMasa, visina;
        List<string> pregledi;
        bool proizvođač;
        Lokacija prebivalište;
        static int brojac = 1;

        #endregion

        #region Properties

        public ZivotinjskaVrsta Vrsta { get => vrsta; set => vrsta = value; }
        public DateTime Starost
        {
            get => starost; 
            set
            {
                if (value > DateTime.Now)
                    throw new FormatException("Životinja ne može biti rođena u budućnosti!");
                starost = value;
            }
        }
        public double TjelesnaMasa
        {
            get => tjelesnaMasa;
            set
            {
                if (value < 0.1)
                    throw new FormatException("Tjelesna masa ne može biti manja od 0.1 kg!");
                tjelesnaMasa = value;
            }
        }
        public double Visina 
        { 
            get => visina; 
            set
            {
                if (value < 1)
                    throw new FormatException("Visina ne može biti manja od 1 cm!");
                visina = value;
            }
        }
        public List<string> Pregledi { get => pregledi; }
        public bool Proizvođač { get => proizvođač; set => proizvođač = value; }
        internal Lokacija Prebivalište { get => prebivalište; set => prebivalište = value; }
        public int ID1 { get => ID; }

        #endregion

        #region Konstruktor

        public Zivotinja(ZivotinjskaVrsta vrsta, DateTime starost, double masa, double visina, Lokacija prebivaliste)
        {
            ID = brojac;
            brojac++;
            Vrsta = vrsta;
            Starost = starost;
            TjelesnaMasa = masa;
            Visina = visina;
            pregledi = new List<string>();
            Proizvođač = true;
            Prebivalište = prebivaliste;
        }

        #endregion

        #region Metode

        /// <summary>
        /// Metoda koja vrši provjeru stanja životinje.
        /// Ukoliko je životinja starija od 10 godina, automatski prestaje biti proizvođač.
        /// Ukoliko je životinja starija od 7 godina i najnoviji pregled ima sadržaj "OCJENA : 3.5" ili manje,
        /// životinja prestaje biti proizvođač.
        /// U suprotnom, potrebno je izvršiti provjeru 3 najnovija pregleda i ukoliko je prosječna ocjena manja od 4,
        /// životinja prestaje biti proizvođač.
        /// </summary>
        /// Implementirao Hamza Dautović
        public void ProvjeriStanjeZivotinje()
        {
            if ((DateTime.Now-starost).Days >= (10 * 365)) { 
                proizvođač = false; 
            }
            else if ((DateTime.Now - starost).Days >= (7 * 365) && Double.Parse(pregledi[pregledi.Count - 1].Substring(pregledi[pregledi.Count -1].Length -3)) <= 3.5) {
                proizvođač = false; 
            }
            else
            {
                double suma = 0;
                int brojac = 0;
                for (int i = pregledi.Count - 1 ; ; i--)
                {
                    suma += Double.Parse(pregledi[i].Substring(pregledi[i].Length - 3));
                    brojac++;
                    if (brojac == 3) break;

                }

                if ((suma / 3) < 4) {
                    Console.WriteLine(vrsta + " " + proizvođač);
                    proizvođač = false; 
                }
            }
        }

        public void PregledajZivotinju(string osnovneInfo, string napomena, string ocjena)
        {
            string pregled = "OSNOVNE INFORMACIJE: " + osnovneInfo + "\n"
                            + "NAPOMENA: " + napomena + "\n"
                            + "OCJENA: " + ocjena;

            pregledi.Add(pregled);
        }

        public override bool Equals(object obj)
        {
            return obj is Zivotinja zivotinja &&
                   Vrsta == zivotinja.Vrsta &&
                   Starost == zivotinja.Starost &&
                   TjelesnaMasa == zivotinja.TjelesnaMasa &&
                   Visina == zivotinja.Visina;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Vrsta, Starost, TjelesnaMasa, Visina);
        }


        #endregion
    }
}
