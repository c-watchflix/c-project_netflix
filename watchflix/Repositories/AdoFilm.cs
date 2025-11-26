using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace watchflix.Repositories
{
    internal class AdoEpreuve : Ado
    {
        public static void getAll(Models.Film film)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM Film";
            cmd.ExecuteNonQuery();                  
            close();
        } 

        public static List<Models.Film> getAll()
        {
            List<Models.Film> films = new List<Models.Film>();
            open();
            string query = $"SELECT * FROM Film";
            SqlCommand cmd = new SqlCommand(query, connexion);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                films.Add(new Models.Film(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3)));
            }

            close();
            return films;

        }

        public static void getOneById(Models.Film film)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM film WHERE id_film = @Id_film";
            cmd.Parameters.AddWithValue("@Id_film", film.id_film);
            cmd.ExecuteNonQuery();
            close();
        }

        public static void getOneByName(Models.Film film)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM Film  WHERE titre_film = @Titre_film";
            cmd.Parameters.AddWithValue("@Titre_film", film.titre_film);
            cmd.ExecuteNonQuery();
            close();
        }

        public static void delete(int Id_film)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "DELETE FROM Film WHERE id_film = @Id_film";
            cmd.Parameters.AddWithValue("@Id_film", Id_film);
            cmd.ExecuteNonQuery();
            close();
        }

        public static void addMusicToFilm(int Id_film, int Id_musique)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "INSERT INTO film_musique VALUES ('@Id_film','@Id_musique')";
            cmd.Parameters.AddWithValue("@Id_film", Id_film);
            cmd.Parameters.AddWithValue("@Id_musique", Id_musique);
            cmd.ExecuteNonQuery();
            close();
        }

        public static void addCategorieToFilm(int Id_film, int Id_categorie
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "INSERT INTO film_musique VALUES ('@Id_film','@Id_categorie')";
            cmd.Parameters.AddWithValue("@Id_film", Id_film);
            cmd.Parameters.AddWithValue("@Id_musique", Id_categorie);
            cmd.ExecuteNonQuery();
            close();
        }

    }
}
