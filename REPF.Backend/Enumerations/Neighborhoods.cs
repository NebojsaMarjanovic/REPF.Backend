using System.Collections.Immutable;

namespace REPF.Backend.Enumerations
{
    public static class Neighborhoods
    {
        public static readonly ImmutableDictionary<Location, List<string>> NeighborhoodsMap;


        static Neighborhoods()
        {
            NeighborhoodsMap = new Dictionary<Location, List<string>>()
            {
                {Location.Rakovica,new List<string>{"Kanarevo Brdo", "Kijevo", "Labudovo Brdo", "Miljakovac I", "Miljakovac II", "Miljakovac III", "Rakovica - Centar", "Kneževac", "Petlovo Brdo", "Resnik", "Skojevsko Naselje", "Stari Košutnjak", "Vidikovac"}},
                {Location.Obrenovac, new List<string>{ }},
                {Location.Mladenovac, new List<string>{ }},
                {Location.Lazarevac, new List<string>{ }},
                {Location.Vozdovac, new List<string>{ }},
                {Location.Grocka, new List<string>{ }},
                {Location.Barajevo, new List<string>{ }},
                {Location.Cukarica, new List<string>{"Cukaricka padina" }},
                {Location.NoviBeograd, new List<string>{ }},
                {Location.Palilula, new List<string>{ }},
                {Location.SavskiVenac, new List<string>{ }},
                {Location.Sopot, new List<string>{ }},
                {Location.StariGrad, new List<string>{ }},
                {Location.Surcin, new List<string>{ }},
                {Location.Vracar, new List<string>{ }},
                {Location.Zemun, new List<string>{ }},
                {Location.Zvezdara, new List<string>{ }},

            }.ToImmutableDictionary();
        }
    }
}
