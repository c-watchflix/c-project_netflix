using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watchflix.Repositories
{
    public abstract class Ado
    {
        // AJOUT DE "Pooling=false;" POUR EMPÊCHER LES CONNEXIONS FANTÔMES DE SATURER LE SERVEUR
        public static string cs = "Data Source=sql.reseau-labo.fr;Initial Catalog=watchflix;User ID=user_watchflix;Password=pwd_watchflix;TrustServerCertificate=True;Pooling=false;";
        
        protected static SqlConnection connexion;

        public static void open()
        {
            try
            {
                connexion = new SqlConnection(cs);
                connexion.Open();
                Console.WriteLine("Connexion ouverte");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("erreur !!! on est dans le catch -> message : " + ex.Message);
            }
        }

        protected static void close()
        {
            if (connexion != null)
            {
                connexion.Close();
                Console.WriteLine("Connexion fermée");
            }
        }
    }
}