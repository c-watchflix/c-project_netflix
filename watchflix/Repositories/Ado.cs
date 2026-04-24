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
    
        protected static string? cs;
        protected static SqlConnection? connexion;

        // Initialisation UNIQUE au démarrage
        public static void Init(IConfiguration configuration)
        {
            cs = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(cs))
            {
                throw new Exception("ConnectionString 'Default' non trouvée !");
            }
        }

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
 