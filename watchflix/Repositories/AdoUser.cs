using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using watchflix.Repositories;
using watchflix.Models;

namespace watchflix.Repositories
{
    internal class AdoUser : Ado
    {
        public static void create(User user)
        {
            open();    //ouverture de la connexion a la bdd
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;

            // Hachage du mot de passe avant l'insertion
            string hashedPassword = watchflix.Services.Authentification.HashPassword(user.Mdp);

            cmd.CommandText = "INSERT INTO Utilisateur (nom, prenom, courriel, pseudo, mdp, is_admin) VALUES (@nom,@prenom,@courriel,@pseudo,@mdp,@is_admin)";
            cmd.Parameters.AddWithValue("@nom", user.Nom);
            cmd.Parameters.AddWithValue("@prenom", user.Prenom);
            cmd.Parameters.AddWithValue("@courriel", user.Courriel);
            cmd.Parameters.AddWithValue("@pseudo", user.Pseudo);
            cmd.Parameters.AddWithValue("@mdp", hashedPassword);
            cmd.Parameters.AddWithValue("@is_admin", user.Is_admin);
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

        public static List<User> getOneById(int Id)
        {
            List<User> user = new List <User>();
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM Utilisateur WHERE id_utilisateur = @Id";
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.ExecuteNonQuery();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                user.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetBoolean(6)));
            }

            close();
            return user;           
        }

        public static List<User> getOneByName(string Nom)
        {
            List<User> users = new List<User>();
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM Utilisateur WHERE nom LIKE '%' + @Nom + '%'";
            cmd.Parameters.AddWithValue("@Nom", Nom);
            cmd.ExecuteNonQuery();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetBoolean(6)));
            }

            close();
            return users;
        }

        public static void update(User user)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;

            // Hachage du mot de passe avant la mise à jour
            string hashedPassword = watchflix.Services.Authentification.HashPassword(user.Mdp);

            cmd.CommandText = "UPDATE Utilisateur SET nom = @nom, prenom = @prenom, courriel = @courriel, pseudo = @pseudo, mdp = @mdp, is_admin = @is_admin WHERE id_utilisateur = @Id";
            cmd.Parameters.AddWithValue("@Id", user.Id);
            cmd.Parameters.AddWithValue("@nom", user.Nom);
            cmd.Parameters.AddWithValue("@prenom", user.Prenom);
            cmd.Parameters.AddWithValue("@courriel", user.Courriel);
            cmd.Parameters.AddWithValue("@pseudo", user.Pseudo);
            cmd.Parameters.AddWithValue("@mdp", hashedPassword);
            cmd.Parameters.AddWithValue("@is_admin", user.Is_admin);
            cmd.ExecuteNonQuery();
            close();
        }

        public static void delete(int id)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "DELETE FROM utilisateur WHERE id_utilisateur = @Id";
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
            close();

        }

        public static User Authenticate(string pseudo, string mdp)
        {
            User user = null;
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM Utilisateur WHERE pseudo = @pseudo AND mdp = @mdp";
            cmd.Parameters.AddWithValue("@pseudo", pseudo);
            cmd.Parameters.AddWithValue("@mdp", mdp);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                user = new User(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetBoolean(6)
                );
            }

            close();
            return user;
        }
    }
}