using System.Collections.Immutable;

namespace REPF.Backend.Enumerations
{
    public static class HeatingType
    {
        public static readonly ImmutableDictionary<string, string> HeatingTypeMap;

        static HeatingType()
        {
            HeatingTypeMap = new Dictionary<string, string>()
            {
                {"Etažno", "central"},
                {"Centralno", "district"},
                {"Struja", "electricity"},
                {"Gas", "gas"},
            }.ToImmutableDictionary();
            //FALE KLIMA I RADIJATOR - STORAGEHEATER I AIRCONDITIONER
        }

    }
}
