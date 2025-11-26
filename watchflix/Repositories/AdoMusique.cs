using System;
using System.Data.SqlClient;   
using System.Collections.Generic;
using System.Threading.Tasks;

namespace watchflix.Repositories;

public class AdoMusique : Ado
{
    public static void GetAllMusique()
    {
        open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connexion;
        cmd.CommandText = "SELECT * FROM Musique";
        cmd.ExecuteNonQuery();
        close();
    }

    public static void GetOneById(int id_musique)
    {
        open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connexion;
        cmd.CommandText = "SELECT * FROM Musique WHERE id_musique = @id_musique";
        cmd.Parameters.AddWithValue("@id_musique", id_musique);
        cmd.ExecuteNonQuery();
        close();
    }

    public static void GetOneByName(string titre_musique)
    {
        open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connexion;
        cmd.CommandText = "SELECT * FROM Musique WHERE titre_musique = @titre_musique";
        cmd.Parameters.AddWithValue("@titre_musique", titre_musique);
        cmd.ExecuteNonQuery();
        close();
    }

    public static void DeleteMusique(int id_musique)
    {
        open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connexion;
        cmd.CommandText = "DELETE FROM Musique WHERE id_musique = @id_musique";
        cmd.Parameters.AddWithValue("@id_musique", id_musique);
        cmd.ExecuteNonQuery();
        close();
    }
}
