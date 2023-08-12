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
                {Location.Rakovica,new List<string>{"Kanarevo Brdo", "Kijevo", "Labudovo Brdo", "Miljakovac", "Rakovica - Centar", "Kneževac", "Petlovo Brdo", "Resnik", "Skojevsko Naselje", "Stari Košutnjak", "Vidikovac"}},
                {Location.Obrenovac, new List<string>{ "Obrenovac - Centar" }},
                {Location.Mladenovac, new List<string>{ "Mladenovac - Centar" }},
                {Location.Lazarevac, new List<string>{"Lazarevac - Centar" }},
                {Location.Vozdovac, new List<string>{"Stepa Stepanović", "Autokomanda", "Banjica", "Braće Jerković", "Voždovac - Centar", "Dušanovac", "Jajinci", "Kumodraž", "Lekino Brdo", "Medaković", "Mitrovo Brdo", "Konjarnik" }},
                {Location.Grocka, new List<string>{ "Gorcka - Centar", "Kaluđerica"}},
                {Location.Barajevo, new List<string>{"Barajevo - Centar" }},
                {Location.Cukarica, new List<string>{"Banovo Brdo", "Cerak", "Cerak Vinogradi", "Čukarica - Centar", "Čukarička Padina", "Filmski Grad", "Golf Naselje", "Julino Brdo", "Železnik", "Žarkovo", "Orlovača" }},
                {Location.NoviBeograd, new List<string>{"Bežanijska kosa", "Novi Beograd - Centar", "Blok 1", "Blok 2", "Blok 3", "Blok 4", "Blok 6", "Blok 7", "Blok 8", "Blok 9", "Blok 11", "Blok19", "Blok 21", "Blok 22", "Blok 23", "Blok 28",
                "Blok 29", "Blok 30", "Blok 31", "Blok 32", "Blok 33", "Blok 34", "Blok 37", "Blok 38", "Blok 44", "Blok 45", "Blok 61", "Blok 62", "Blok 63", "Blok 64", "Blok 65", "Blok 67", "Blok 70", "Blok 71", "Blok 72", "Studentski Grad", "Tošin bunar", "Ledine" }},
                {Location.Palilula, new List<string>{"Borča", "Ovča", "Padinska Sekla", "Krnjača", "Bogoslovija", "Višnjička banja", "Tašmajdan", "Kotež", "Palilula - Centar", "Profesorska Kolonija", "Hadžipopovac", "Botanička bašta", "Poštanska Štedionica", "Palilulska pijaca"}},
                {Location.SavskiVenac, new List<string>{"Beograd na vodi", "Dedinje", "Autokomanda", "Senjak", "Beograđanka", "Savski Venac - Centar", "Klinički centar", "Savamala", "Zeleni Venac", "Savski trg", "Lisičji potok", "Mostarska petlja", "Topčider" }},
                {Location.Sopot, new List<string>{"Sopot - Centar" }},
                {Location.Surcin, new List<string>{"Surčin - Centar", "Ledine", "Bečmen", "Jakovo" }},
                {Location.StariGrad, new List<string>{"Dorćol", "Stari grad - Centar","Terazije", "Gundulićev Venac", " Skadarlija", "Skupština", "Bajlonijeva Pijaca", "Kalemegdan", "Obilićev Venac", "Kopitareva gradina", "Kosančićev Venac", "Cvetni trg", "Politika", "Trg Republike", "Zeleni Venac", "Studentski Trg","Topličin Venac", "Andrićev venac"  }},
                {Location.Vracar, new List<string>{ "Neimar (Hram Svetog Save)", "Vračar - Centar", "Kalenić pijaca", "Crveni Krst", "Vukov spomenik", "Južni Bulevar", "Čubura", "Slavija", "Cvetni Trg", "Krunska", "Karađorđev park"}},
                {Location.Zemun, new List<string>{"Gornji grad", "Batajnica", "Altina", "Pregrevica", "Zemun - Centar","Meandri", "Novi Grad", "Galenika", "Save Kovačevića", "Zemun Polje", "Retenzija", "Cara Dušana", "Kalvarija", "Poštanska štedionica", "Kej", "Karađorđev trg", "Donji Grad" }},
                {Location.Zvezdara, new List<string>{ "Mirijevo", "Zvezdara - Centar", "Lion", "Cvetkova pijaca", "Učiteljsko naselje", "Kluz", "Vukov Spomenik", "Olimp", "Konjarnik", "Denkova Bašta", "Cvetanova Ćuprija", "Đeram pijaca", "Lipov lad", "Severni Bulevar", "Veljko Vlahović","Veliki mokri lug", "Crveni Krst", "Bulbuder" }},

            }.ToImmutableDictionary();
        }
    }
}
