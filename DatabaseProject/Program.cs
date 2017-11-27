using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using GetMovieData;

namespace DatabaseProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] ids = { "tt0800369", "tt0115610", "tt3501632", "tt1981115", "tt5649108", "tt1613750", "tt2239822", "tt4555426", "tt1792794", "tt2720826",
            "tt1568911", "tt1596363", "tt0119081", "tt4686844", "tt7126746", "tt0200211", "tt2278388", "tt1860357", "tt6611130", "tt0077413",
            "tt5151440", "tt1260572", "tt0073629", "tt1499658", "tt3165612", "tt0384806", "tt2170439", "tt0098067", "tt0190590", "tt0844708",
            "tt0091949", "tt1528071", "tt0256415", "tt6772950", "tt0053291", "tt6474202", "tt2025690", "tt5698320", "tt0243655", "tt2450258",
            "tt1646987", "tt0367027", "tt5710514", "tt0071853", "tt0963743", "tt4799064", "tt2370248", "tt0029162", "tt0083598", "tt0385002",
            "tt4196450", "tt0357413", "tt0027300", "tt1838722", "tt1241721", "tt0451079", "tt0103786", "tt0091419", "tt0990407", "tt0113161",
            "tt1767354", "tt5152640", "tt1571222", "tt1843309", "tt0078767", "tt0096969", "tt4058368", "tt4365566", "tt0068833", "tt0074860" };

            string baseUrl = "http://www.theimdbapi.org/api/movie?movie_id=";

            System.IO.StreamWriter file = new System.IO.StreamWriter(
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Inserts.txt");

            using (WebClient wc = new WebClient())
            {
                foreach (string id in ids)
                {
                    string json = wc.DownloadString(baseUrl + id);
                    Movie movie = JsonConvert.DeserializeObject<Movie>(json);

                    string insertMovie = "Insert Into Project.Movies (Title, ContentRating, ReleaseDate, Runtime)";
                    string insertLanguage = "Insert Into Project.MovieLanguages (LanguageID, MovieID)";

                    file.WriteLine(insertMovie);
                    file.WriteLine(GenerateValues.CreateMovieInsert(movie));
                    file.WriteLine();
                }
                file.Close();
            }
        }


        

        //    public static SqlConnection GetConnection()
        //    {
        //        SqlConnection connection = new SqlConnection();
        //        connection.ConnectionString =
        //            "Data Source=aura.students.cset.oit.edu" +
        //            ";Initial Catalog=" + DB_NAME +
        //            ";Integrated Security=False" +
        //            ";User ID=" + DB_USER_NAME + ";Password=" + DB_USER_PWD;

        //        return connection;
        //    }

        //    public static void WriteMovie(Movie obj)
        //    {
        //        SqlConnection connection = null;
        //        try
        //        {
        //            connection = GetConnection();

        //            using (SqlCommand command = new SqlCommand())
        //            {
        //                command.Connection = connection;
        //                command.CommandText =
        //                    "INSERT INTO " + "Project" + ".Movies (Title, ContentRating, ReleaseDate, Runtime) " +
        //                    "VALUES (@Title, @ContentRating, @ReleaseDate, @Runtime)" +
        //                    "Select Scope_Identity()";

        //                command.Parameters.AddWithValue("@Title", obj.Title);
        //                if (string.IsNullOrEmpty(obj.Content_Rating))
        //                {
        //                    command.Parameters.AddWithValue("@ContentRating", DBNull.Value);
        //                }
        //                else
        //                {
        //                    command.Parameters.AddWithValue("@ContentRating", obj.Content_Rating);
        //                }
        //                if (obj.Release_Date == default(DateTime))
        //                {
        //                    command.Parameters.AddWithValue("@ReleaseDate", DBNull.Value);
        //                }
        //                else
        //                {
        //                    command.Parameters.AddWithValue("@ReleaseDate", obj.Release_Date);
        //                }
        //                if (string.IsNullOrEmpty(obj.Length))
        //                {
        //                    command.Parameters.AddWithValue("@Runtime", DBNull.Value);
        //                }
        //                else
        //                {
        //                    command.Parameters.AddWithValue("@Runtime", Convert.ToInt32(obj.Length));
        //                }
        //                connection.Open();

        //                obj.MovieID = Convert.ToInt32(command.ExecuteScalar());
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            throw e;
        //        }
        //        finally
        //        {
        //            if (connection != null)
        //            {
        //                connection.Close();
        //            }
        //        }
        //    }

        //    public static void WriteLanguage(Movie obj)
        //    {
        //        SqlConnection connection = null;

        //        try
        //        {
        //            using (connection = GetConnection())
        //            {
        //                connection.Open();

        //                foreach (string lang in obj.Metadata.Languages)
        //                {
        //                    int LanguageID = -1;

        //                    using (SqlCommand command = new SqlCommand())
        //                    {
        //                        command.Connection = connection;
        //                        command.CommandText =
        //                            "Select LanguageID " +
        //                            "From Project.Languages " +
        //                            "Where Language = '" + lang + "'";

        //                        SqlDataReader LangQueryReader = command.ExecuteReader();

        //                        if (LangQueryReader.HasRows)
        //                        {
        //                            LangQueryReader.Read();
        //                            LanguageID = LangQueryReader.GetInt32(0);
        //                            LangQueryReader.Close();
        //                        }
        //                        else
        //                        {
        //                            command.CommandText =
        //                                "Insert Project.Languages (Language) " +
        //                                "Values ('" + lang + "')" +
        //                                "Select Scope_Identity()";

        //                            LangQueryReader.Close();

        //                            LanguageID = Convert.ToInt32(command.ExecuteScalar());
        //                        }
        //                    }
        //                    using (SqlCommand command = new SqlCommand())
        //                    {
        //                        command.Connection = connection;
        //                        command.CommandText =
        //                            "INSERT INTO " + "Project" + ".MovieLanguages (LanguageID, MovieID) " +
        //                            "VALUES (@LanguageID, @MovieID)";

        //                        command.Parameters.AddWithValue("@LanguageID", LanguageID);
        //                        command.Parameters.AddWithValue("@MovieID", obj.MovieID);

        //                        command.ExecuteNonQuery();
        //                    }
        //                }

        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            throw e;
        //        }
        //        finally
        //        {
        //            if (connection != null)
        //            {
        //                connection.Close();
        //            }
        //        }
        //    }
    }
}


public class Movie
{
    public int MovieID { get; set; }
    public string Title { get; set; }
    public string Content_Rating { get; set; }
    public string Release_Date { get; set; }
    public string Length { get; set; }

    //public string Address { get; set; }
    //public string City { get; set; }
    //public string State { get; set; }
    //public string Country { get; set; }

    //public List<string> Writers { get; set; }
    public string Director { get; set; }
    public List<string> Genre { get; set; }

    public Metadata Metadata { get; set; }

    public string Rating_Count { get; set; }
    public string IMDB_ID { get; set; }
    public string Rating { get; set; }

    public List<Actor> Cast { get; set; }
    public List<Trailer> Trailer { get; set; }

}

public class Metadata
{
    public List<string> Languages { get; set; }
    public string Budget { get; set; }
    public string Gross { get; set; }
}

public class Actor
{
    public string Character { get; set; }
    public string Name { get; set; }
}

public class Trailer
{
    public string Definition { get; set; }
    public string VideoUrl { get; set; }
}


