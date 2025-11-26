using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watchflix.Repositories
{
    public class AdoCatégories : Ado
    {
        public static void CreateCategorie(Categorie uneCategorie)
        {
            open();
            //string query = $"INSERT INTO categorie(nom_categorie) VALUES('{uneCategorie.Nom}')";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText  = "INSERT INTO categorie(nom_categorie) VALUES(@nom_categorie)";
            cmd.Parameters.AddWithValue("@nom_categorie", uneCategorie.Nom_categorie);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            close();
        }

        public static void getAll()
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM categorie";
            cmd.ExecuteNonQuery();
            close();
        }
        public static void getOneByID(int id_categorie)
        {
            open();
             SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM categorie WHERE @id_categorie=id_categorie";
            cmd.Parameters.AddWithValue("@id_categorie", id_categorie);
            cmd.ExecuteNonQuery();
            close();
        }

        public static void getOneByName(Categorie uneCategorie)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM categorie WHERE @nom_categorie=nom_categorie";
            cmd.Parameters.AddWithValue("@nom_categorie", uneCategorie.Nom_categorie);
            cmd.ExecuteNonQuery();
            close();
        }

        public static void updateCategorie(Categorie uneCategorie, int id_categorie)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "UPDATE categorie SET @nom_categorie=nom_categorie WHERE @id_categorie=id_categorie";
            cmd.Parameters.AddWithValue("@id_categorie", id_categorie);
            cmd.Parameters.AddWithValue("@nom_categorie", uneCategorie.Nom_categorie);
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
}