﻿/***********************************************************
* Author:				Levi Pomeroy & Morgan Loring
* Date Created:			11/21/17
* Last Mod Date:		11/30/17
* Project:              Database Project - Movie DB
* Filename:				Program.cs
*
* Overview:
*	This program retrieves JSON objects from IMDB, serielizes it into our own class. 
*	It also creates SQL insert statements and writes them to a file. These insert statements can 
*	be copied into SQL Server Manager and executed. 
*
*
* Input:
*	N/A
*
* Output:
*	Outputs the generated SQL inserts to the console and to a file called Inserts.sql
*
************************************************************/

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
        static void Main(string[] args)
        {
            //File to export sql statements too
            System.IO.StreamWriter file = new System.IO.StreamWriter(
                Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Inserts.txt");

            int MovieIdIndex = 103500;  //Start index for movieIDs from IMDB
            while (MovieIdIndex < 106000)   //Loop through the next 2500 movies
            {
                //Url for IMDB API, concated with movie id 
                string MovieUrl = "http://www.theimdbapi.org/api/movie?movie_id=tt0" + MovieIdIndex;
                MovieIdIndex++; //move to next movie

                using (WebClient wc = new WebClient())
                {
                    try
                    {
                        string json = wc.DownloadString(MovieUrl);  //Get movie info
                        Movie movie = JsonConvert.DeserializeObject<Movie>(json);   //Parse movie info into class

                        //Fix apostrophes in movie title to work with SQL
                        if (movie.Title.Contains("'"))
                        {
                            movie.Title = movie.Title.Replace("'", "''");
                        }

                        // *************************** Check nulls *************************************************
                        if (string.IsNullOrEmpty(movie.Content_Rating)) movie.Content_Rating = "null";
                        else movie.Content_Rating = "'" + movie.Content_Rating + "'";

                        //We set any invlaid date to 9999-09-09
                        if (string.IsNullOrEmpty(movie.Release_Date)) movie.Release_Date = "9999-09-09"; 
                        if (string.IsNullOrEmpty(movie.Length)) movie.Length = "null";
                        else movie.Length = "'" + movie.Length + "'";


                        // ************************** Inserts for movies table **************************************
                        //We set any invlaid date to 9999-09-09
                        if (movie.Release_Date.Length == 7)
                        {
                            movie.Release_Date = "9999-09-09";
                        }

                        string insertMovie = "Insert Into Project.Movies (Title, ContentRating, ReleaseDate, Runtime) "
                            + "values ('" + movie.Title + "'," + movie.Content_Rating + ",'" + movie.Release_Date + "'," + movie.Length + ")";
                        file.WriteLine(insertMovie);
                        Console.WriteLine(insertMovie);

                        // ************************** Inserts for directors table *************************************
                        if (movie.Director.Contains("'"))
                        {
                            movie.Director = movie.Director.Replace("'", "''");
                        }
                        string insertDirectors = "if not exists (select * from project.Directors where DirectorName='" + movie.Director + "')"
                             + "Insert into Project.Directors (DirectorName) values ('" + movie.Director + "')";
                        file.WriteLine(insertDirectors);
                        Console.WriteLine(insertDirectors);

                        // ************************** Inserts for Moviedirectors table *************************************
                        string insertMovieDirectors = "insert into project.MovieDirectors (DirectorId, MovieId) values (" 
                            +"(select directorId from project.directors where DirectorName ='" + movie.Director + "'), "
                            + "(select movieId from project.movies where title ='" + movie.Title + "'))";
                        file.WriteLine(insertMovieDirectors);
                        Console.WriteLine(insertMovieDirectors);

                        // ************************** Inserts for ratings table **************************************
                        if (string.IsNullOrEmpty(movie.Rating_Count)) movie.Rating_Count = "null";
                        if (string.IsNullOrEmpty(movie.Rating)) movie.Rating = "null";
                        if(movie.Rating_Count.Contains(","))
                        {
                            movie.Rating_Count = movie.Rating_Count.Replace(",", "");
                        }
                        string insertRatings = "Insert into Project.Ratings (NumberOfRatings, IMDBRating, MovieID)" 
                            + "values (" + movie.Rating_Count + "," + movie.Rating + "," 
                            + "(select MovieID from Project.Movies where title ='" + movie.Title + "' and releaseDate ='" + movie.Release_Date + "'))";
                        file.WriteLine(insertRatings);
                        Console.WriteLine(insertRatings);

                        // ************************** Inserts for trailer table **************************************
                        if (movie.Trailer.Count == 0)
                        {
                            string insertTrailer = "Insert into Project.Trailer (movieID, TrailerSequence, url)" + "values ("
                                    + "(select MovieID from Project.Movies where title ='" + movie.Title + "' and releaseDate ='" + movie.Release_Date + "'),"
                                    + 0 + ", null)";
                            file.WriteLine(insertTrailer);
                            Console.WriteLine(insertTrailer);
                        }
                        else
                        {
                            for (int seq = 0; seq < movie.Trailer.Count; seq++)
                            {
                                string insertTrailer = "Insert into Project.Trailer (movieID, TrailerSequence, url)" + "values ("
                                    + "(select MovieID from Project.Movies where title ='" + movie.Title + "' and releaseDate ='" + movie.Release_Date + "'),"
                                    + seq + ",'" + movie.Trailer[seq].VideoUrl + "')";
                                file.WriteLine(insertTrailer);
                                Console.WriteLine(insertTrailer);
                            }
                        }

                        // ************************** Inserts for genre table **************************************
                        for (int genrenum = 0; genrenum < movie.Genre.Count; genrenum++)
                        {
                            string insertGenre = "if not exists (select * from Project.Genres where GenreName = '" + movie.Genre[genrenum]
                                + "') insert into project.genres (GenreName) values ('" + movie.Genre[genrenum] + "')";
                            file.WriteLine(insertGenre);
                            Console.WriteLine(insertGenre);
                        }

                        // ************************** Inserts for MoviGenre table **************************************
                        for (int genrenum = 0; genrenum < movie.Genre.Count; genrenum++)
                        {
                            string insertmovieGenre = "insert into project.MovieGenre (GenreId, MovieId) values ("
                            + "(select genreId from project.genres where genreName ='" + movie.Genre[genrenum] + "'), "
                            + "(select movieId from project.movies where title ='" + movie.Title + "'))";
                            file.WriteLine(insertmovieGenre);
                            Console.WriteLine(insertmovieGenre);
                        }

                        // ************************** Inserts for languages table **************************************
                        for (int langnum = 0; langnum < movie.Metadata.Languages.Count; langnum++)
                        {
                            string insertLanguage = "if not exists (select * from project.Languages where language = '" + movie.Metadata.Languages[langnum]
                                + "') insert into project.Languages (Language) values ('" + movie.Metadata.Languages[langnum] + "')";
                            file.WriteLine(insertLanguage);
                            Console.WriteLine(insertLanguage);
                        }

                        // ************************** Inserts for Movielanguages table **************************************
                        for (int langnum = 0; langnum < movie.Metadata.Languages.Count; langnum++)
                        {
                            string insertMovieLanguages = "insert into project.MovieLanguages (LanguageID, MovieId) values ("
                            + "(select LanguageId from project.languages where language ='" + movie.Metadata.Languages[langnum] + "'), "
                            + "(select movieId from project.movies where title ='" + movie.Title + "'))";
                            file.WriteLine(insertMovieLanguages);
                            Console.WriteLine(insertMovieLanguages);
                        }

                        // ************************** Inserts for writers table **************************************
                        for (int writnum = 0; writnum < movie.Writers.Count; writnum++)
                        {
                            if(movie.Writers[writnum].Contains("'"))
                            {
                                movie.Writers[writnum] = movie.Writers[writnum].Replace("'", "''");
                            }
                            string insertwriter = "if not exists (select * from project.Writers where WriterName = '" + movie.Writers[writnum]
                                + "') insert into project.Writers (WriterName) values ('" + movie.Writers[writnum] + "')";
                            file.WriteLine(insertwriter);
                            Console.WriteLine(insertwriter);
                        }

                        // ************************** Inserts for MovieWriters table **************************************
                        for (int writnum = 0; writnum < movie.Writers.Count; writnum++)
                        {
                            string insertMovieWriters = "insert into project.MovieWriters (WriterId, MovieId) values ("
                            + "(select WriterId from project.Writers where WriterName ='" + movie.Writers[writnum] + "'), "
                            + "(select movieId from project.movies where title ='" + movie.Title + "'))";
                            file.WriteLine(insertMovieWriters);
                            Console.WriteLine(insertMovieWriters);
                        }

                        // ************************** Inserts for Actors table **************************************
                        for (int actnum = 0; actnum < movie.Cast.Count; actnum++)
                        {
                            if (movie.Cast[actnum].Name.Contains("'"))
                            {
                                movie.Cast[actnum].Name = movie.Cast[actnum].Name.Replace("'", "''");
                            }
                            string insertactor = "if not exists (select * from project.Actors where Name = '" + movie.Cast[actnum].Name
                                + "') insert into project.Actors (Name) values ('" + movie.Cast[actnum].Name + "')";
                            file.WriteLine(insertactor);
                            Console.WriteLine(insertactor);
                        }

                        // ************************** Inserts for Cast table **************************************
                        for (int actnum = 0; actnum < movie.Cast.Count; actnum++)
                        {

                            if (movie.Cast[actnum].Character.Contains("'"))
                            {
                                movie.Cast[actnum].Character = movie.Cast[actnum].Character.Replace("'", "''");
                            }
                            if (string.IsNullOrEmpty(movie.Cast[actnum].Character)) movie.Cast[actnum].Character = "null";
                            else movie.Cast[actnum].Character = "'" + movie.Cast[actnum].Character + "'";

                            string insertCast = "insert into project.Cast (ActorId, MovieId, CharacterName) values ("
                            + "(select ActorId from project.Actors where Name ='" + movie.Cast[actnum].Name + "'), "
                            + "(select movieId from project.movies where title ='" + movie.Title + "'),"
                            + movie.Cast[actnum].Character + ")";
                            file.WriteLine(insertCast);
                            Console.WriteLine(insertCast);
                        }

                        file.WriteLine(); //Used as new line to seperate movies
                    }
                    catch (Exception e)
                    {
                        MovieIdIndex++; //If fails for some reason, it will go to next movie
                    }
                }
            }
            file.Close();
        }
    }
}

/***************************************
* This class is used to store the 
* parsed data for each movie. 
***************************************/
public class Movie
{
    public int MovieID { get; set; }
    public string Title { get; set; }
    public string Content_Rating { get; set; }
    public string Release_Date { get; set; }
    public string Length { get; set; }
    public List<string> Writers { get; set; }
    public string Director { get; set; }
    public List<string> Genre { get; set; }
    public Metadata Metadata { get; set; }
    public string Rating_Count { get; set; }
    public string IMDB_ID { get; set; }
    public string Rating { get; set; }
    public List<Actor> Cast { get; set; }
    public List<Trailer> Trailer { get; set; }

}

/***************************************
 This class is used to store 
 the languages of the movie  
***************************************/
public class Metadata
{
    public List<string> Languages { get; set; }
}

/***************************************
 This class is used to store 
 the Actor elements of the movie info 
***************************************/
public class Actor
{
    public string Character { get; set; }
    public string Name { get; set; }
}

/***************************************
 This class is used to store 
 the Trailer elements of the movie info 
***************************************/
public class Trailer
{
    public string VideoUrl { get; set; }
}


