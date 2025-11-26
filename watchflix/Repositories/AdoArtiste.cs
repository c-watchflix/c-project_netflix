using System;
using System.Data.SqlClient;   
using System.Collections.Generic;
using System.Threading.Tasks;
namespace watchflix.Repositories;

public class AdoArtiste : Ado
{
    public static void GetAllArtistes()
    {
        open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connexion;
        cmd.CommandText = "SELECT * FROM Artiste";
        cmd.ExecuteNonQuery();
        close();
    }

    public static void GetOneById(int id_artiste)
    {
        open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connexion;
        cmd.CommandText = "SELECT * FROM Artiste WHERE id_artiste = @id_artiste";
        cmd.Parameters.AddWithValue("@id_artiste", id_artiste);
        cmd.ExecuteNonQuery();
        close();
    }
    public static void GetOneByName(string nom_artiste)
    {
        open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connexion;
        cmd.CommandText = "SELECT * FROM Artiste WHERE nom_artiste = @nom_artiste";
        cmd.Parameters.AddWithValue("@nom_artiste", nom_artiste);
        cmd.ExecuteNonQuery();
        close();
    }
    public static void DeleteArtiste(int id_artiste)
    {
        open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connexion;
        cmd.CommandText = "DELETE FROM Artiste WHERE id_artiste = @id_artiste";
        cmd.Parameters.AddWithValue("@id_artiste", id_artiste);
        cmd.ExecuteNonQuery();
        close();
    }

    
}
