using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using watchflix.Models;

namespace watchflix.Repositories;

public class AdoMusique : Ado
{
    public static List<Musique> GetAll()
    {
        List<Musique> musiques = new List<Musique>();
        open();
        string query = "SELECT id_musique, titre_musique FROM Musique";
        SqlCommand cmd = new SqlCommand(query, connexion);

        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            musiques.Add(new Musique(reader.GetInt32(0), reader.GetString(1)));
        }

        close();
        return musiques;
    }

    public static Musique? GetOneById(int id)
    {
        open();
        string query = "SELECT id_musique, titre_musique FROM Musique WHERE id_musique = @Id_musique";
        SqlCommand cmd = new SqlCommand(query, connexion);
        cmd.Parameters.AddWithValue("@Id_musique", id);

        SqlDataReader reader = cmd.ExecuteReader();
        Musique? musique = null;

        if (reader.Read())
        {
            musique = new Musique(reader.GetInt32(0), reader.GetString(1));
        }

        close();
        return musique;
    }

    public static void GetOneByName(Musique musique)
    {
        open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connexion;
        cmd.CommandText = "SELECT * FROM Musique WHERE titre_musique = @Titre_musique";
        cmd.Parameters.AddWithValue("@Titre_musique", musique.Titre);
        cmd.ExecuteNonQuery();
        close();
    }

    public static void Delete(int idMusique)
    {
        open();

        // Supprimer les relations dans artiste_musique
        string deleteArtisteMusiqueQuery = "DELETE FROM artiste_musique WHERE id_musique = @Id_musique";
        SqlCommand cmdArtisteMusique = new SqlCommand(deleteArtisteMusiqueQuery, connexion);
        cmdArtisteMusique.Parameters.AddWithValue("@Id_musique", idMusique);
        cmdArtisteMusique.ExecuteNonQuery();

        // Supprimer les relations dans film_musique
        string deleteFilmMusiqueQuery = "DELETE FROM film_musique WHERE id_musique = @Id_musique";
        SqlCommand cmdFilmMusique = new SqlCommand(deleteFilmMusiqueQuery, connexion);
        cmdFilmMusique.Parameters.AddWithValue("@Id_musique", idMusique);
        cmdFilmMusique.ExecuteNonQuery();

        // Supprimer la musique
        string deleteMusiqueQuery = "DELETE FROM Musique WHERE id_musique = @Id_musique";
        SqlCommand cmdMusique = new SqlCommand(deleteMusiqueQuery, connexion);
        cmdMusique.Parameters.AddWithValue("@Id_musique", idMusique);
        cmdMusique.ExecuteNonQuery();

        close();
    }
}
