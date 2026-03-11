using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using watchflix.Models; 

namespace watchflix.Repositories
{
    internal class AdoFilm : Ado
    {
        // --- 1. LECTURE ---
        public static List<Film> getAll()
        {
            List<Film> films = new List<Film>();
            open();
            
            // Ordre SQL : 
            // 0=id, 1=titre, 2=duree, 3=date, 4=pegi, 5=jacquette, 6=synopsis, 7=bande_annonce, 8=realisateur, 9=fond
            string query = "SELECT id_film, titre_film, duree_film, date_sortie, pegi, jacquette, synopsys, bande_annonce, realisateur, fond FROM Film";
            
            SqlCommand cmd = new SqlCommand(query, connexion);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                // On passe les variables dans L'ORDRE EXACT du constructeur de Film.cs :
                // Constructeur : Film(Id, Titre, Pegi, Jacquette, Resume, Bande_annonce, Realisateur, Fond, Dte_sortie, Duree)
                films.Add(new Film(
                    reader.GetInt32(0), // 0: id
                    reader.GetString(1), // 1: titre
                    
                    reader.IsDBNull(4) ? "N/A" : reader.GetString(4), // Pegi (Index 4 dans le SQL)
                    reader.IsDBNull(5) ? "" : reader.GetString(5),    // Jacquette (Index 5 dans le SQL) <-- L'IMAGE EST ICI !
                    reader.IsDBNull(6) ? "" : reader.GetString(6),    // Synopsis (Index 6 dans le SQL)
                    reader.IsDBNull(7) ? "" : reader.GetString(7),    // Bande annonce (Index 7 dans le SQL)
                    reader.IsDBNull(8) ? "" : reader.GetString(8),    // Realisateur (Index 8 dans le SQL)
                    reader.IsDBNull(9) ? "" : reader.GetString(9),    // Fond (Index 9 dans le SQL)
                    
                    // J'utilise GetValue().ToString() au cas où la BDD renvoie une Date ou un String, ça marchera dans les deux cas sans planter.
                    reader.IsDBNull(3) ? "" : reader.GetValue(3).ToString(), // Date sortie (Index 3 dans le SQL)
                    reader.IsDBNull(2) ? "" : reader.GetValue(2).ToString()  // Duree (Index 2 dans le SQL)
                ));
            }
            close();
            return films;
        }

        // --- NOUVELLE MÉTHODE : RÉCUPÉRER LES CATÉGORIES DE CHAQUE FILM ---
        public static Dictionary<int, List<string>> GetCategoriesParFilm()
        {
            var dico = new Dictionary<int, List<string>>();
            open();
            
            // On croise la table de liaison (categorie_film) avec la table Categorie
            string query = "SELECT cf.id_film, c.libelle FROM categorie_film cf INNER JOIN Categorie c ON cf.id_categorie = c.id_categorie";
            
            SqlCommand cmd = new SqlCommand(query, connexion);
            SqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                int idFilm = reader.GetInt32(0);
                string libelle = reader.GetString(1);
                
                if (!dico.ContainsKey(idFilm))
                {
                    dico[idFilm] = new List<string>();
                }
                dico[idFilm].Add(libelle);
            }
            close();
            return dico;
        }

        // --- 2. ANCIENNES MÉTHODES ---

        public static List<Film> getOneById(int id)
        {
            List<Film> film = new List<Film>();
            open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Film WHERE id_film = @Id", connexion);
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) { 
                film.Add(new Film(reader.GetInt32(0), reader.GetString(1))); 
            }
            close();
            return film;
        }

        public static List<Film> getOneByName(string titre)
        {
            List<Film> films = new List<Film>();
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "SELECT * FROM Film WHERE titre_film LIKE @Titre";
            cmd.Parameters.AddWithValue("@Titre", "%" + titre + "%");
            
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                films.Add(new Film(reader.GetInt32(0), reader.GetString(1)));
            }
            close();
            return films;
        }

        public static void delete(int Id_film)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;

            //casser lelien entre le film et ses musiques
            cmd.CommandText = "DELETE FROM film_musique WHERE id_film = @Id_film";
            cmd.Parameters.Clear(); //évite les bug silencieux
            cmd.Parameters.AddWithValue("@Id_film", Id_film);
            cmd.ExecuteNonQuery();

            //casser lelilen entre les musiques et les artistes
            cmd.CommandText = "DELETE am FROM artiste_musique am INNER JOIN Musique m ON m.id_musique = am.id_musique LEFT JOIN film_musique fm ON fm.id_musique = m.id_musique WHERE fm.id_musique IS NULL";
            cmd.Parameters.Clear();
            cmd.ExecuteNonQuery();

            //supp les musiques orphelines
            cmd.CommandText = "DELETE FROM Musique WHERE NOT EXISTS (SELECT 1 FROM film_musique film WHERE film.id_musique = Musique.id_musique)";
            cmd.Parameters.Clear();
            cmd.ExecuteNonQuery();

            //casser le lien entre le film et ses catégories
            cmd.CommandText = "DELETE FROM categorie_film WHERE id_film = @Id_film";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Id_film", Id_film);
            cmd.ExecuteNonQuery();

            // supp le film
            cmd.CommandText = "DELETE FROM Film WHERE id_film = @Id_film";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Id_film", Id_film);
            cmd.ExecuteNonQuery();
            
            close();
        }

        public static void addMusicToFilm(int Id_film, int Id_musique)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "INSERT INTO film_musique VALUES (@Id_film, @Id_musique)";
            cmd.Parameters.AddWithValue("@Id_film", Id_film);
            cmd.Parameters.AddWithValue("@Id_musique", Id_musique);
            cmd.ExecuteNonQuery();
            close();
        }

        public static void addCategorieToFilm(int Id_film, int Id_categorie)
        {
            open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connexion;
            cmd.CommandText = "INSERT INTO categorie_film VALUES (@Id_film, @Id_categorie)";
            cmd.Parameters.AddWithValue("@Id_film", Id_film);
            cmd.Parameters.AddWithValue("@Id_categorie", Id_categorie);
            cmd.ExecuteNonQuery();
            close();
        }

        // --- 3. ÉCRITURE DANS LA BDD ---
        public static async Task<bool> addFilmWhithApi(MovieDetails filmApi)
        {
            try 
            {
                open(); 

                // A. Vérif doublon
                string queryCheck = "SELECT COUNT(*) FROM Film WHERE titre_film = @Titre";
                SqlCommand cmdCheck = new SqlCommand(queryCheck, connexion);
                cmdCheck.Parameters.AddWithValue("@Titre", filmApi.Title);
                
                int count = (int)(await cmdCheck.ExecuteScalarAsync() ?? 0);
                
                if (count > 0) 
                { 
                    close(); 
                    return false; // <--- LE FILM EXISTE DÉJÀ : ON RETOURNE FAUX
                }

                // B. Insertion Film
                string queryInsert = @"
                    INSERT INTO Film (titre_film, duree_film, date_sortie, pegi, jacquette, synopsys, bande_annonce, realisateur, fond)
                    OUTPUT INSERTED.id_film 
                    VALUES (@Titre, @Duree, @Annee, @Pegi, @Poster, @Synop, @Youtube, @Real, @Fond)";

                SqlCommand cmdInsert = new SqlCommand(queryInsert, connexion);
                
                // Conversions
                string dateSortie = string.IsNullOrEmpty(filmApi.AnneeSortie) ? "2000-01-01" : $"{filmApi.AnneeSortie}-01-01";
                TimeSpan ts = TimeSpan.FromMinutes(filmApi.Runtime);
                string dureeStr = string.Format("{0:00}:{1:00}:00", (int)ts.TotalHours, ts.Minutes);

                string urlYoutube = filmApi.YoutubeKey != null ? $"https://www.youtube.com/watch?v={filmApi.YoutubeKey}" : "";
                
                cmdInsert.Parameters.AddWithValue("@Titre", filmApi.Title);
                cmdInsert.Parameters.AddWithValue("@Duree", dureeStr);
                cmdInsert.Parameters.AddWithValue("@Annee", DateTime.Parse(dateSortie));
                cmdInsert.Parameters.AddWithValue("@Pegi", filmApi.Pegi);
                cmdInsert.Parameters.AddWithValue("@Poster", filmApi.FullPosterUrl); 
                cmdInsert.Parameters.AddWithValue("@Synop", filmApi.Overview ?? "");
                cmdInsert.Parameters.AddWithValue("@Youtube", urlYoutube);
                cmdInsert.Parameters.AddWithValue("@Real", filmApi.Realisateur ?? "Inconnu");
                cmdInsert.Parameters.AddWithValue("@Fond", filmApi.FullBackdropUrl);

                int newFilmId = (int)(await cmdInsert.ExecuteScalarAsync() ?? 0);

                // C. Catégories
                foreach (var genre in filmApi.Genres)
                {
                    string checkCat = "SELECT id_categorie FROM Categorie WHERE libelle = @NomCat";
                    SqlCommand cmdCatCheck = new SqlCommand(checkCat, connexion);
                    cmdCatCheck.Parameters.AddWithValue("@NomCat", genre.Name);
                    
                    object? result = await cmdCatCheck.ExecuteScalarAsync();
                    int idCategorie;

                    if (result != null)
                    {
                        idCategorie = (int)result;
                    }
                    else
                    {
                        string insertCat = "INSERT INTO Categorie (libelle) OUTPUT INSERTED.id_categorie VALUES (@NomCat)";
                        SqlCommand cmdCatInsert = new SqlCommand(insertCat, connexion);
                        cmdCatInsert.Parameters.AddWithValue("@NomCat", genre.Name);
                        idCategorie = (int)(await cmdCatInsert.ExecuteScalarAsync() ?? 0);
                    }

                    string insertLien = "INSERT INTO categorie_film (id_film, id_categorie) VALUES (@IdFilm, @IdCat)";
                    SqlCommand cmdLien = new SqlCommand(insertLien, connexion);
                    cmdLien.Parameters.AddWithValue("@IdFilm", newFilmId);
                    cmdLien.Parameters.AddWithValue("@IdCat", idCategorie);
                    await cmdLien.ExecuteNonQueryAsync();
                }

                return true; // <--- TOUT EST OK : ON RETOURNE VRAI
            }
            finally
            {
                close();
            }
        }
    }
}