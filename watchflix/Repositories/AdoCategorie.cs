using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using watchflix.Models;

namespace watchflix.Repositories; 
    public class AdoCategorie : Ado
    {
        public static void CreateCategorie(Categorie uneCategorie)
        {
            open();
            //string query = $"INSERT INTO categorie(nom_categorie) VALUES('{uneCategorie.Nom}')";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText  = "INSERT INTO categorie(nom_categorie) VALUES(@nom_categorie)";
            cmd.Parameters.AddWithValue("@nom_categorie", uneCategorie.Libelle);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            close();
        }

        public static List<Categorie> getAll()
        {
            List<Models.Categorie> categories = new List<Models.Categorie>();
            open();
            string query = $"SELECT * FROM Categorie";
            SqlCommand cmd = new SqlCommand(query, connexion);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                categories.Add(new Categorie(reader.GetInt32(0), reader.GetString(1)));
            }

            close();
            return categories;

        }
        public static List<Categorie> getOneByID(int id_categorie)
        {
            List<Models.Categorie> categories = new List<Models.Categorie>();
            open();
            string query = "SELECT * FROM categorie WHERE @id_categorie=id_categorie";
            SqlCommand cmd = new SqlCommand(query, connexion);
            cmd.Parameters.AddWithValue("@id_categorie", id_categorie);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                categories.Add(new Categorie(reader.GetInt32(0), reader.GetString(1)));
            }

            close();
            return categories;

        }
        public static List<Categorie> getOneByLibelle(string Libelle)
        {
            List<Models.Categorie> categories = new List<Models.Categorie>();
            open();
            string query = "SELECT * FROM categorie WHERE libelle LIKE '%' + @libelle + '%'";
            SqlCommand cmd = new SqlCommand(query, connexion);
            cmd.Parameters.AddWithValue("@libelle", Libelle);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                categories.Add(new Categorie(reader.GetInt32(0), reader.GetString(1)));
            }

            close();
            return categories;

        }
        public static void updateCategorie(Categorie uneCategorie, int id_categorie)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "UPDATE categorie SET @nom_categorie=nom_categorie WHERE @id_categorie=id_categorie";
            cmd.Parameters.AddWithValue("@id_categorie", id_categorie);
            cmd.Parameters.AddWithValue("@nom_categorie", uneCategorie.Libelle);
            cmd.ExecuteNonQuery();
            close();
        }

        public static void deleteCategorie(int id_categorie)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM categorie WHERE @id_categorie = id_categorie";
            cmd.Parameters.AddWithValue("@id_categorie", id_categorie);
            cmd.ExecuteNonQuery();
            close();
        }
    }