using System.Collections;
using System.Collections.Generic;

public class CSV_Importer
{
    public static List<Dictionary<string, object>> EXP = new (CSVReader.Read("EXP"));
}
