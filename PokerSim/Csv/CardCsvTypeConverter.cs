using System;
using CsvHelper;
using CsvHelper.Configuration;

namespace PokerSim.Cards
{
    class CardCsvTypeConverter : CsvHelper.TypeConversion.ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            throw new NotImplementedException();
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return value.ToString();
        }
    }
}
