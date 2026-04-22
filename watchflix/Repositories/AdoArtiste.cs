using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace watchflix.Repositories;
using watchflix.Models;

public class AdoArtiste : Ado
{
    public AdoArtiste(IConfiguration _configuration) : base(_configuration)
    {
        Configuration = _configuration;
        cs = Configuration.GetConnectionString("DefaultConnection");
    }
    public static List<Artiste> GetAllArtistes()
    {
        List<Artiste> artistes = new List<Artiste>();
        open();
        string query = "SELECT id_artiste, nom_artiste FROM Artiste";
        SqlCommand cmd = new SqlCommand(query, connexion);

        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            artistes.Add(new Artiste(reader.GetInt32(0), reader.GetString(1)));
        }

        close();
        return artistes;
    }

    public static Artiste? GetOneById(int id_artiste)
    {
        open();
        string query = "SELECT id_artiste, nom_artiste FROM Artiste WHERE id_artiste = @id_artiste";
        SqlCommand cmd = new SqlCommand(query, connexion);
        cmd.Parameters.AddWithValue("@id_artiste", id_artiste);

        SqlDataReader reader = cmd.ExecuteReader();
        Artiste? artiste = null;

        if (reader.Read())
        {
            artiste = new Artiste(reader.GetInt32(0), reader.GetString(1));
        }

        close();
        return artiste;
    }

    public static Artiste? GetOneByName(string nom_artiste)
    {
        open();
        string query = "SELECT id_artiste, nom_artiste FROM Artiste WHERE nom_artiste = @nom_artiste";
        SqlCommand cmd = new SqlCommand(query, connexion);
        cmd.Parameters.AddWithValue("@nom_artiste", nom_artiste);

        SqlDataReader reader = cmd.ExecuteReader();
        Artiste? artiste = null;

        if (reader.Read())
        {
            artiste = new Artiste(reader.GetInt32(0), reader.GetString(1));
        }

        close();
        return artiste;
    }

    public static void DeleteArtiste(int id_artiste)
    {
        open();

        // Supprimer les relations dans artiste_musique
        string deleteArtisteMusiqueQuery = "DELETE FROM artiste_musique WHERE id_artiste = @id_artiste";
        SqlCommand cmdArtisteMusique = new SqlCommand(deleteArtisteMusiqueQuery, connexion);
        cmdArtisteMusique.Parameters.AddWithValue("@id_artiste", id_artiste);
        cmdArtisteMusique.ExecuteNonQuery();

        // Supprimer l'artiste
        string deleteArtisteQuery = "DELETE FROM Artiste WHERE id_artiste = @id_artiste";
        SqlCommand cmdArtiste = new SqlCommand(deleteArtisteQuery, connexion);
        cmdArtiste.Parameters.AddWithValue("@id_artiste", id_artiste);
        cmdArtiste.ExecuteNonQuery();

        close();
    }
}
