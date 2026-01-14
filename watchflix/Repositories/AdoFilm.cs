using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using watchflix.Models;
using System.Transactions;


namespace watchflix.Repositories
{
    internal class AdoFilm : Ado
    {
        public static List<Film> getAll()
        {
            List<Models.Film> films = new List<Models.Film>();
            open();
            string query = $"SELECT * FROM Film";
            SqlCommand cmd = new SqlCommand(query, connexion);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                films.Add(new Film(reader.GetInt32(0), reader.GetString(1)));
            }

            close();
            return films;

        }

        public static List<Film> getOneById(int id)
        {
            List<Models.Film> film = new List<Models.Film>();
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM film WHERE id_film = @Id_film";
            cmd.Parameters.AddWithValue("@Id_film", id);
            cmd.ExecuteNonQuery();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                film.Add(new Film(reader.GetInt32(0), reader.GetString(1)));
            }
            close();
            return film;
        }

        public static List<Film> getOneByName(string titre)
        {
            List<Models.Film> films = new List<Models.Film>();
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM Film  WHERE titre_film LIKE '%' + @Titre_film + '%'";
            cmd.Parameters.AddWithValue("@Titre_film", titre);
            cmd.ExecuteNonQuery();
            
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                films.Add(new Film(reader.GetInt32(0), reader.GetString(1)));
            }
            close();
            return films;
        }

        public static void delete(int Id_film)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;

            //casser lelien entre le film et ses musiques
            cmd.CommandText = "DELETE FROM film_musique WHERE id_film = @Id_film";
            cmd.Parameters.Clear(); //évite les bug silencieux
            cmd.Parameters.AddWithValue("@Id_film", Id_film);
            cmd.ExecuteNonQuery();

            //casser lelilen entre les musiques et les artistes
            cmd.CommandText = "DELETE am FROM artiste_musique am INNER JOIN Musique m ON m.id_musique = am.id_musique LEFT JOIN film_musique fm ON fm.id_musique = m.id_musique WHERE fm.id_musique IS NULL";
            cmd.Parameters.Clear();
            cmd.ExecuteNonQuery();

            //supp les musiques orphelines
            cmd.CommandText = "DELETE FROM Musique WHERE NOT EXISTS (SELECT 1 FROM film_musique film WHERE film.id_musique = Musique.id_musique)";
            cmd.Parameters.Clear();
            cmd.ExecuteNonQuery();

            //casser le lien entre le film et ses catégories
            cmd.CommandText = "DELETE FROM categorie_film WHERE id_film = @Id_film";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Id_film", Id_film);
            cmd.ExecuteNonQuery();


            // supp le film
            cmd.CommandText = "DELETE FROM Film WHERE id_film = @Id_film";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Id_film", Id_film);
            cmd.ExecuteNonQuery();
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

        public static void addCategorieToFilm(int Id_film, int Id_categorie)
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
