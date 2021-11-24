using ZivotinjskaFarma;

//Implementirala Selma Hadžijusufović
namespace Zadatak_2
{

    class Spy : IVeterinar
    {
        public int Opcija { get; set; }

        public double ocjenaZdravstvenogStanjaZivotinje(Zivotinja zivotinja)
        {
            //Ocjena zdravstvenog stanja > 4
            if (Opcija == 0)
            {
                Opcija = 1;
                return 5;
            }
            //Ocjena zdravstvenog stanja > 3
            else
            {
                return 4;
            }

        }
    }
}
