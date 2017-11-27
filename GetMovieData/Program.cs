using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GetMovieData
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseUrl = "http://www.theimdbapi.org/api/movie?movie_id=";
            String[] IMDBTitles = { "Thor", "Captain" };
            string[] ids = { "tt0800369", "tt0115610", "tt3501632", "tt1981115", "tt5649108", "tt1613750", "tt2239822", "tt4555426", "tt1792794", "tt2720826",
            "tt1568911", "tt1596363", "tt0119081", "tt4686844", "tt7126746", "tt0200211", "tt2278388", "tt1860357", "tt6611130", "tt0077413",
            "tt5151440", "tt1260572", "tt0073629", "tt1499658", "tt3165612", "tt0384806", "tt2170439", "tt0098067", "tt0190590", "tt0844708",
            "tt0091949", "tt1528071", "tt0256415", "tt6772950", "tt0053291", "tt6474202", "tt2025690", "tt5698320", "tt0243655", "tt2450258",
            "tt1646987", "tt0367027", "tt5710514", "tt0071853", "tt0963743", "tt4799064", "tt2370248", "tt0029162", "tt0083598", "tt0385002",
            "tt4196450", "tt0357413", "tt0027300", "tt1838722", "tt1241721", "tt0451079", "tt0103786", "tt0091419", "tt0990407", "tt0113161",
            "tt1767354", "tt5152640", "tt1571222", "tt1843309", "tt0078767", "tt0096969", "tt4058368", "tt4365566", "tt0068833", "tt0074860" };
            System.IO.StreamWriter file = new System.IO.StreamWriter(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\MovieData9.txt");
            Console.WriteLine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName);
            using (WebClient wc = new WebClient())
            {
                foreach (string ID in ids)
                {
                    string json = wc.DownloadString(baseUrl + ID);


                    file.WriteLine(json + "\r\n\r\n*\r\n");
                }
            }
            file.Close();
        }
    }
}
