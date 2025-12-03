using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using watchflix.Models;

namespace watchflix.Repositories;

public class AdoMusique : Ado
{
    public static List<Musique> GetAllMusique()
    {
        open();
        List<Musique> musiques = new List<Musique>();
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM Musique";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                musiques.Add(new Musique(
                    reader.GetInt32(0), // id_musique
                    reader.GetString(1), // titre_musique
                    reader.GetTimeSpan(2), // duree_musique
                    reader.GetString(3), // album
                    reader.GetString(4)  // couverture
                ));
            }
            reader.Close();
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Erreur SQL : {ex.Message}");
        }
        finally
        {
            close();
        }
        return musiques;
    }

    public static Musique GetOneById(int id_musique)
    {
        open();
        Musique musique = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM Musique WHERE id_musique = @id_musique";
            cmd.Parameters.AddWithValue("@id_musique", id_musique);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                musique = new Musique(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetTimeSpan(2),
                    reader.GetString(3),
                    reader.GetString(4)
                );
            }
            reader.Close();
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Erreur SQL : {ex.Message}");
        }
        finally
        {
            close();
        }
        return musique;
    }

    public static Musique GetOneByName(string titre_musique)
    {
        open();
        Musique musique = null;
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM Musique WHERE titre_musique = @titre_musique";
            cmd.Parameters.AddWithValue("@titre_musique", titre_musique);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                musique = new Musique(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetTimeSpan(2),
                    reader.GetString(3),
                    reader.GetString(4)
                );
            }
            reader.Close();
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Erreur SQL : {ex.Message}");
        }
        finally
        {
            close();
        }
        return musique;
    }

    public static void DeleteMusique(int id_musique)
    {
        open();
        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "DELETE FROM Musique WHERE id_musique = @id_musique";
            cmd.Parameters.AddWithValue("@id_musique", id_musique);
            cmd.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Erreur SQL : {ex.Message}");
        }
        finally
        {
            close();
        }
    }
}
