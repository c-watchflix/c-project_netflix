using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watchflix.Repositories
{
    public abstract class Ado
    {
        protected static SqlConnection connexion;

        public static void open()
        {
            string cs = $"Server=172.19.0.7;Database=watchflix;User Id=user_watchflix;Password=pwd_watchflix;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            //string cs = $"Data Source = LAPTOP-9HK09LIE; Initial Catalog = bd_ap3; Integrated Security = True";
            try
            {
                connexion = new SqlConnection(cs);
                connexion.Open();
                Console.WriteLine("Connexion ouverte");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
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
