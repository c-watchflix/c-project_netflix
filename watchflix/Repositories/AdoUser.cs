using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using watchflix.Repositories;
using ap3_jintegration.classe;

namespace c_project_netflix.ado
{
    internal class AdoUser : Ado
    {
        public static void create(User user)
        {
            open();    //ouverture de la connexion a la bdd
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "INSERT INTO Utilisateur (nom, prenom, courriel, pseudo, mdp, is_admin) VALUES (@nom,@prenom,@courriel,@pseudo,@mdp,@is_admin)";
            cmd.Parameters.AddWithValue("@nom", user.Nom);
            cmd.Parameters.AddWithValue("@prenom",user.Prenom);
            cmd.Parameters.AddWithValue("@courriel",user.Courriel);
            cmd.Parameters.AddWithValue("@pseudo",user.Pseudo);
            cmd.Parameters.AddWithValue("@mdp",user.Mdp);
            cmd.Parameters.AddWithValue("@is_admin",user.Is_admin);
            cmd.ExecuteNonQuery();                  // pour executer la commande 
            close();   // fermeture de la connexion a la bdd 
        }

        public static List<User> getAll()
        {
            List<User> user = new List <User>();
            open();
            string query = $"SELECT * FROM Utilisateur";
            SqlCommand cmd = new SqlCommand(query, connexion);
            cmd.ExecuteNonQuery();
            
            SqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read()) 
            {
                user.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetBoolean(6)));
            }
            close();
            return user;
        }

        public static void getOne(int Id)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM Utilisateur WHERE id_utilisateur = @Id";
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.ExecuteNonQuery();
            close();
        }

        public static void update(User user)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "UPDATE Utilisateur SET nom = @nom, prenom = @prenom, courriel = @courriel, pseudo = @pseudo, mdp = @mdp, is_admin = @is_admin WHERE id_utilisateur = @Id";
            cmd.Parameters.AddWithValue("@Id", user.Id);
            cmd.Parameters.AddWithValue("@nom", user.Nom);
            cmd.Parameters.AddWithValue("@prenom", user.Prenom);
            cmd.Parameters.AddWithValue("@courriel", user.Courriel);
            cmd.Parameters.AddWithValue("@pseudo", user.Pseudo);
            cmd.Parameters.AddWithValue("@mdp", user.Mdp);
            cmd.Parameters.AddWithValue("@is_admin", user.Is_admin);
            cmd.ExecuteNonQuery();
            close();
        }

        public static void delete(User user)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "DELETE FROM user WHERE id_utilisateur = @Id";
            cmd.Parameters.AddWithValue("@Id", user.Id);
            cmd.ExecuteNonQuery();
            close();

        }


    }
}