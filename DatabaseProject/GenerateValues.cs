using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetMovieData
{
    static class GenerateValues
    {
        static public string CreateMovieInsert(Movie movie)
        {
            string valuesMovie = "Values ('" + movie.Title + "'";

            if (string.IsNullOrEmpty(movie.Content_Rating))
            {
                valuesMovie = valuesMovie + ", null";
            }
            else
            {
                valuesMovie = valuesMovie + ", '" + movie.Content_Rating + "'";
            }

            if (string.IsNullOrEmpty(movie.Release_Date))
            {
                valuesMovie = valuesMovie + ", null";
            }
            else
            {
                valuesMovie = valuesMovie + ", '" + movie.Release_Date + "'";
            }

            if (string.IsNullOrEmpty(movie.Length))
            {
                valuesMovie = valuesMovie + ", null";
            }
            else
            {
                valuesMovie = valuesMovie + ", " + movie.Length;
            }
            valuesMovie = valuesMovie + ")";

            return valuesMovie;
        }

        //static public string CreateLanguageInsert(Movie movie)
        //{
        //    string

        //    string valuesLanguage = "Values "
        //}
    }
}
