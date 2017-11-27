using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace DatabaseProject
{
    class Program
    {
        //first_last
        static string DB_NAME = "morgan_loring";
        //first_last
        static string DB_USER_NAME = "morgan_loring";
        static string DB_USER_PWD = "Chichen2015$";

        static void Main(string[] args)
        {

            string rawData = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\MovieData9.txt");
            string[] MovieData = rawData.Split('*');

            foreach (string data in MovieData)
            {
                Movie obj = JsonConvert.DeserializeObject<Movie>(data);
                //null release date
                WriteMovie(obj);
                WriteLanguage(obj);
            }
        }

        public static SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString =
                "Data Source=aura.students.cset.oit.edu" +
                ";Initial Catalog=" + DB_NAME +
                ";Integrated Security=False" +
                ";User ID=" + DB_USER_NAME + ";Password=" + DB_USER_PWD;

            return connection;
        }

        public static void WriteMovie(Movie obj)
        {
            SqlConnection connection = null;
            try
            {
                connection = GetConnection();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        "INSERT INTO " + "Project" + ".Movies (Title, ContentRating, ReleaseDate, Runtime) " +
                        "VALUES (@Title, @ContentRating, @ReleaseDate, @Runtime)" +
                        "Select Scope_Identity()";

                    command.Parameters.AddWithValue("@Title", obj.Title);
                    if (string.IsNullOrEmpty(obj.Content_Rating))
                    {
                        command.Parameters.AddWithValue("@ContentRating", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ContentRating", obj.Content_Rating);
                    }
                    if (obj.Release_Date == default(DateTime))
                    {
                        command.Parameters.AddWithValue("@ReleaseDate", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ReleaseDate", obj.Release_Date);
                    }
                    if (string.IsNullOrEmpty(obj.Length))
                    {
                        command.Parameters.AddWithValue("@Runtime", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Runtime", Convert.ToInt32(obj.Length));
                    }
                    connection.Open();

                    obj.MovieID = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        public static void WriteLanguage(Movie obj)
        {
            SqlConnection connection = null;

            try
            {
                using (connection = GetConnection())
                {
                    connection.Open();

                    foreach (string lang in obj.Metadata.Languages)
                    {
                        int LanguageID = -1;

                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandText =
                                "Select LanguageID " +
                                "From Project.Languages " +
                                "Where Language = '" + lang + "'";

                            SqlDataReader LangQueryReader = command.ExecuteReader();

                            if (LangQueryReader.HasRows)
                            {
                                LangQueryReader.Read();
                                LanguageID = LangQueryReader.GetInt32(0);
                                LangQueryReader.Close();
                            }
                            else
                            {
                                command.CommandText =
                                    "Insert Project.Languages (Language) " +
                                    "Values ('" + lang + "')" +
                                    "Select Scope_Identity()";

                                LangQueryReader.Close();

                                LanguageID = Convert.ToInt32(command.ExecuteScalar());
                            }
                        }
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandText =
                                "INSERT INTO " + "Project" + ".MovieLanguages (LanguageID, MovieID) " +
                                "VALUES (@LanguageID, @MovieID)";

                            command.Parameters.AddWithValue("@LanguageID", LanguageID);
                            command.Parameters.AddWithValue("@MovieID", obj.MovieID);

                            command.ExecuteNonQuery();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
    }
}


public class Movie
{
    public int MovieID { get; set; }
    public string Title { get; set; }
    public string Content_Rating { get; set; }
    public DateTime Release_Date { get; set; }
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


