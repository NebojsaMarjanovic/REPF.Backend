using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace REPF.Backend.Enumerations
{
    public enum Location
    {
        Barajevo,
        Cukarica,
        Grocka,
        Lazarevac,
        Mladenovac,
        NoviBeograd,
        Obrenovac,
        Palilula,
        Rakovica,
        SavskiVenac,
        Sopot,
        StariGrad,
        Surcin,
        Vozdovac,
        Vracar,
        Zemun,
        Zvezdara
    }

    public static class LocationMap
    {
        public static readonly ImmutableDictionary<Location, Tuple<string, string[]>> locations;
        static LocationMap()
        {
            locations = new Dictionary<Location, Tuple<string, string[]>>()
            {
                {Location.Barajevo, 
                    new Tuple<string,string[]>("Barajevo", 
                    new string[]{"1","1.5","2","2.5","3","3.5","4"})},
                {Location.Cukarica,
                    new Tuple<string,string[]>("Čukarica",
                    new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5"})},
                {Location.Grocka,
                    new Tuple<string,string[]>("Grocka",
                    new string[]{"0.5","1","1.5","2","2.5","3","3.5","4"})},
                {Location.Lazarevac,
                    new Tuple<string,string[]>("Lazarevac",
                    new string[]{"2.5","3","4"})},
                {Location.Mladenovac,
                    new Tuple<string,string[]>("Mladenovac",
                    new string[]{"1","1.5","2","2.5","3","4"})},
                {Location.NoviBeograd,
                    new Tuple<string,string[]>("Novi Beograd",
                    new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5"})},
                {Location.Obrenovac,
                    new Tuple<string,string[]>("Obrenovac",
                    new string[]{"2","2.5","3","3.5"})},
                {Location.Palilula,
                    new Tuple<string,string[]>("Palilula",
                    new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5"})},
                {Location.Rakovica,
                    new Tuple<string,string[]>("Rakovica",
                    new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5"})},
                {Location.SavskiVenac,
                    new Tuple<string,string[]>("Savski venac",
                    new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5"})},
                {Location.Sopot,
                    new Tuple<string,string[]>("Sopot",
                    new string[]{""})},
                {Location.StariGrad,
                    new Tuple<string,string[]>("Stari grad",
                    new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5"})},
                {Location.Surcin,
                    new Tuple<string,string[]>("Surčin",
                    new string[]{"1.5","2","2.5","3"})},
                {Location.Vozdovac,
                    new Tuple<string,string[]>("Voždovac",
                    new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5"})},
                {Location.Vracar,
                    new Tuple<string,string[]>("Vračar",
                    new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5"})},
                {Location.Zemun,
                    new Tuple<string,string[]>("Zemun",
                    new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5"})},
                {Location.Zvezdara,
                    new Tuple<string,string[]>("Zvezdara",
                    new string[]{"0.5","1","1.5","2","2.5","3","3.5","4","4.5","5"})}
            }.ToImmutableDictionary();
        }
    }
}
