using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace LinqLabs
{

    internal class Program
    {
        static void Main(string[] args)
        {
            //-----------------------------LinqLab1--------------------------------//

            //LinqLab1.StartLab1_words("../../../../materials/lab1_words.txt");
            //LinqLab1.StartLab1_digits("../../../../materials/lab1_digits.txt");


            //-----------------------------LinqLab2--------------------------------//

            //LinqLab2.StartLab2_Frequency_Text_Analysis("../../../../materials/lab1_digits.txt");
            //LinqLab2.StartLab2_Except_Intersect_Union();


            //-----------------------------LinqLab3--------------------------------//

            //LinqLab3.StartLab3("../../../../materials/xml_file.xml");


            //-----------------------------LinqLab4--------------------------------//

            //LinqLab4.StartLab4("../../../../materials/dbLinqLab4.accdb");


            //-----------------------------LinqLab5--------------------------------//

            //LinqLab5.StartLab5_CreateXMLfromDataSet("../../../../materials/dbLinqLab4.accdb");
            //LinqLab5.StartLab5_CreateXMLfromDataSet("../../../../materials/dbLinqLab4.accdb");
            LinqLab5.StartLab5_CreateXMLfromDataSet_Only_Changes_2("../../../../materials/dbLinqLab4.accdb");
        }
    }
}