using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfa_1_aars_eksamens_projekt.Classes
{
    //Done by Natasha
    //This class maps class members to CSV fields.
    public class WasteElementMap : ClassMap<WasteElement>
    {
        public WasteElementMap()
        {
            //We map the columns in the CSV files to variables specified in the waste element class.
            Map(x => x.id).Index(0);
            Map(x => x.amount).Index(1);
            Map(x => x.measurementUnit).Index(2);
            Map(x => x.categoryID).Index(3);
            Map(x => x.description).Index(4);
            Map(x => x.responsibleEmployee).Index(5);
            Map(x => x.companyID).Index(6);
            Map(x => x.dateTimePlainText).Index(7);
        }
    }
}
