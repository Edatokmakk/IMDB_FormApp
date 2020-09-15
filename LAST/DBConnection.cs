using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAST
{
    public class DBConnection
    {
        SqlConnection sqlConnection;
        SqlCommand command;
        SqlDataReader reader_2;
        SqlCommand mSearchCommand;
        SqlCommand personCommand;
        SqlCommand searchCommand;
        SqlCommand castCommand;
        SqlCommand genMapcommand;
        SqlDataReader reader;
        SqlDataReader reader_3;
        SqlCommand genCommand;
        SqlCommand imageCommand;
        SqlCommand perimageCommand;

        public void saveMovies(Movie movie)
        {
            sqlConnection = new SqlConnection(@"server=.\sqlexpress;database=IMDB;trusted_connection=true");
            sqlConnection.Open();
            command = new SqlCommand("insert into Movie (MovieID,movieName,DatePublished,movieRate,MovieDesc) values (@ImdbTitleID,@Name,@DatePublished,@Rate,@movieDesc)", sqlConnection);

            command.Parameters.AddWithValue("@ImdbTitleID", movie.ImdbTitleID);
            command.Parameters.AddWithValue("@Name", movie.Name);
            command.Parameters.AddWithValue("@DatePublished", movie.DatePublished);
            command.Parameters.AddWithValue("@Rate", movie.Rate.ToString());
            command.Parameters.AddWithValue("@movieDesc", movie.Desc);
            command.ExecuteNonQuery();

            imageCommand = new SqlCommand("insert into MovieImage (MovieID,movieImageUrl) values (@MovieID,@movieImageUrl)", sqlConnection);
            imageCommand.Parameters.AddWithValue("@MovieID", movie.ImdbTitleID);
            imageCommand.Parameters.AddWithValue("@movieImageUrl", movie.ImageUrl);
            imageCommand.ExecuteNonQuery();

            foreach (Genre genre in movie.Genres)
            {
                SqlCommand gencontrolCommand = new SqlCommand("select GenreName from Genre where GenreName=@genreName", sqlConnection);
                gencontrolCommand.Parameters.AddWithValue("@genreName", genre.GenreName);
                object genid = gencontrolCommand.ExecuteScalar();
                if (genid == null)
                {
                    saveGenres(genre.GenreName);
                }
                    //genre.GenreName var mı diye db de arat.yoksa kaydet.varsa getir.
                    genMapcommand = new SqlCommand("insert into GenreMapping (GenreID,MovieID) values (@GenreID,@MovieID)", sqlConnection);
                    genMapcommand.Parameters.AddWithValue("@MovieID", movie.ImdbTitleID);
                    genMapcommand.Parameters.AddWithValue("@GenreID", genre.GenreId);
                

            }
            foreach (Cast person in movie.Casts)
            {
                SqlCommand controlCommand = new SqlCommand("select PersonID from Person where PersonID=@personId", sqlConnection);
                controlCommand.Parameters.AddWithValue("@personId", person.ImdbID);
                object id = controlCommand.ExecuteScalar();
                if (id == null)
                {
                    personCommand = new SqlCommand("insert into Person (PersonID,Name,Bio,JobTitle,BirthDate) values (@PersonID,@Name,@Bio,@JobTitle,@BirthDate)", sqlConnection);
                    personCommand.Parameters.AddWithValue("@PersonID", person.ImdbID);
                    personCommand.Parameters.AddWithValue("@Name", person.Name);
                    personCommand.Parameters.AddWithValue("@Bio", person.Bio);
                    personCommand.Parameters.AddWithValue("@JobTitle", person.JobTitle);
                    personCommand.Parameters.AddWithValue("@BirthDate", person.BirthDate);
                    personCommand.ExecuteNonQuery();

                    perimageCommand = new SqlCommand("insert into PersonImage (PersonID,PersonImage) values (@PersonID,@PersonImage)", sqlConnection);
                    perimageCommand.Parameters.AddWithValue("@PersonID", person.ImdbID);
                    perimageCommand.Parameters.AddWithValue("@PersonImage", person.ImageUrl);
                    perimageCommand.ExecuteNonQuery();

                }
                sqlConnection = new SqlConnection(@"server=.\sqlexpress;database=IMDB;trusted_connection=true");
                sqlConnection.Open();
                foreach (Role role in person.Roles)
                {
                    castCommand = new SqlCommand("insert into Cast (MovieID,PersonID,RoleID) values (@MovieID,@PersonID,@RoleID)", sqlConnection);
                    castCommand.Parameters.AddWithValue("@MovieID", movie.ImdbTitleID);
                    castCommand.Parameters.AddWithValue("@PersonID", person.ImdbID);
                    castCommand.Parameters.AddWithValue("@RoleID", (int)role);
                    castCommand.ExecuteNonQuery();
                }
                //foreach()

            }

            sqlConnection.Close();
        }
        
        public bool searchControl(string keyword)
        {
            sqlConnection = new SqlConnection(@"server=.\sqlexpress;database=IMDB;trusted_connection=true");
            sqlConnection.Open();
            bool exist;
            searchCommand = new SqlCommand("select (movieName) from Movie where movieName like '%' + @keyword + '%'", sqlConnection);
            searchCommand.Parameters.AddWithValue("@keyword", keyword);
            reader = searchCommand.ExecuteReader();

            exist = false;
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    exist = true;
                }
            }
            reader.Close();
            return exist;

        }
        public bool movieControl(Movie movie)
        {
            sqlConnection = new SqlConnection(@"server=.\sqlexpress;database=IMDB;trusted_connection=true");
            sqlConnection.Open();
            bool movieExist;
            mSearchCommand = new SqlCommand("select (movieName) from Movie where MovieID =' @movId'", sqlConnection);
            mSearchCommand.Parameters.AddWithValue("@movId", movie.ImdbTitleID);
            reader_2 = mSearchCommand.ExecuteReader();

            movieExist = false;
            while (reader_2.Read())
            {
                if (reader_2.HasRows)
                {
                    movieExist = true;
                }
            }
            reader.Close();
            return movieExist;
        }
        public bool personControl(Cast cast)
        {
            sqlConnection = new SqlConnection(@"server=.\sqlexpress;database=IMDB;trusted_connection=true");
            sqlConnection.Open();
            bool movieExist;
            mSearchCommand = new SqlCommand("select (Name) from Person where PersonID =' @castId'", sqlConnection);
            mSearchCommand.Parameters.AddWithValue("@castId", cast.ImdbID);
            reader_3 = mSearchCommand.ExecuteReader();

            movieExist = false;
            while (reader_3.Read())
            {
                if (reader_3.HasRows)
                {
                    movieExist = true;
                }
            }
            reader.Close();
            return movieExist;
        }
        public void saveGenres(string genreName)
        {
            sqlConnection = new SqlConnection(@"server=.\sqlexpress;database=IMDB;trusted_connection=true");
            sqlConnection.Open();
            SqlCommand gencontrolCommand = new SqlCommand("select GenreName from Genre where GenreName like '%' + @genreName +'%'", sqlConnection);
            gencontrolCommand.Parameters.AddWithValue("@genreName", genreName);
            object genid = gencontrolCommand.ExecuteScalar();
            if (genid == null)
            {
                genCommand = new SqlCommand("insert into Genre (GenreName) values ('@GenreName')", sqlConnection);
                genCommand.Parameters.AddWithValue("@GenreName", genreName);
                
            }
            sqlConnection.Close();
        }
    }
}
