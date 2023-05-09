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
        public static readonly ImmutableDictionary<Location, string> locations;
        static LocationMap()
        {
            locations = new Dictionary<Location, string>()
            {
                {Location.Barajevo, "Barajevo"},
                {Location.Cukarica, "Cukarica"},
                {Location.Grocka, "Grocka"},
                {Location.Lazarevac, "Lazarevac"},
                {Location.Mladenovac, "Mladenovac"},
                {Location.NoviBeograd, "Novi Beograd"},
                {Location.Obrenovac, "Obrenovac"},
                {Location.Palilula, "Palilula"},
                {Location.Rakovica, "Rakovica"},
                {Location.SavskiVenac, "Savski Venac"},
                {Location.Sopot, "Sopot"},
                {Location.StariGrad, "Stari Grad"},
                {Location.Surcin, "Surcin"},
                {Location.Vozdovac, "Vozdovac"},
                {Location.Vracar, "Vracar"},
                {Location.Zemun, "Zemun"},
                {Location.Zvezdara, "Zvezdara"},
            }.ToImmutableDictionary();
        }
    }
}
