using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using watchflix.Models; 

namespace watchflix.Repositories
{
    internal class AdoFilm : Ado
    {
        // --- 1. LECTURE (Sécurisée avec using) ---
        public static List<Film> getAll()
        {
            List<Film> films = new List<Film>();
            
            // Le bloc using gère l'ouverture ET la fermeture automatique de la connexion
            using (SqlConnection localConnexion = new SqlConnection(cs))
            {
                localConnexion.Open();
                
                string query = "SELECT id_film, titre_film, duree_film, date_sortie, pegi, jacquette, synopsys, bande_annonce, realisateur, fond FROM Film";
                
                using (SqlCommand cmd = new SqlCommand(query, localConnexion))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        films.Add(new Film(
                            reader.GetInt32(0), // 0: id
                            reader.GetString(1), // 1: titre
                            
                            reader.IsDBNull(4) ? "N/A" : reader.GetString(4), // Pegi
                            reader.IsDBNull(5) ? "" : reader.GetString(5),    // Jacquette
                            reader.IsDBNull(6) ? "" : reader.GetString(6),    // Synopsis
                            reader.IsDBNull(7) ? "" : reader.GetString(7),    // Bande annonce
                            reader.IsDBNull(8) ? "" : reader.GetString(8),    // Realisateur
                            reader.IsDBNull(9) ? "" : reader.GetString(9),    // Fond
                            
                            reader.IsDBNull(3) ? "" : reader.GetValue(3).ToString(), // Date sortie
                            reader.IsDBNull(2) ? "" : reader.GetValue(2).ToString()  // Duree
                        ));
                    }
                }
            } // <--- La connexion est refermée proprement et instantanément ici
            
            return films;
        }

        // --- NOUVELLE MÉTHODE : RÉCUPÉRER LES CATÉGORIES DE CHAQUE FILM ---
        public static Dictionary<int, List<string>> GetCategoriesParFilm()
        {
            var dico = new Dictionary<int, List<string>>();
            
            using (SqlConnection localConnexion = new SqlConnection(cs))
            {
                localConnexion.Open();
                
                string query = "SELECT cf.id_film, c.libelle FROM categorie_film cf INNER JOIN Categorie c ON cf.id_categorie = c.id_categorie";
                
                using (SqlCommand cmd = new SqlCommand(query, localConnexion))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
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
                }
            }
            
            return dico;
        }

        // --- 2. ANCIENNES MÉTHODES (Adaptées) ---

        public static List<Film> getOneById(int id)
        {
            List<Film> film = new List<Film>();
            using (SqlConnection localConnexion = new SqlConnection(cs))
            {
                localConnexion.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Film WHERE id_film = @Id", localConnexion))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) { 
                            film.Add(new Film(reader.GetInt32(0), reader.GetString(1))); 
                        }
                    }
                }
            }
            return film;
        }

        public static List<Film> getOneByName(string titre)
        {
            List<Film> films = new List<Film>();
            using (SqlConnection localConnexion = new SqlConnection(cs))
            {
                localConnexion.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Film WHERE titre_film LIKE @Titre", localConnexion))
                {
                    cmd.Parameters.AddWithValue("@Titre", "%" + titre + "%");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            films.Add(new Film(reader.GetInt32(0), reader.GetString(1)));
                        }
                    }
                }
            }
            return films;
        }

        public static void delete(int Id_film)
        {
            using (SqlConnection localConnexion = new SqlConnection(cs))
            {
                localConnexion.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = localConnexion;

                    cmd.CommandText = "DELETE FROM film_musique WHERE id_film = @Id_film";
                    cmd.Parameters.AddWithValue("@Id_film", Id_film);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE am FROM artiste_musique am INNER JOIN Musique m ON m.id_musique = am.id_musique LEFT JOIN film_musique fm ON fm.id_musique = m.id_musique WHERE fm.id_musique IS NULL";
                    cmd.Parameters.Clear();
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM Musique WHERE NOT EXISTS (SELECT 1 FROM film_musique film WHERE film.id_musique = Musique.id_musique)";
                    cmd.Parameters.Clear();
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM categorie_film WHERE id_film = @Id_film";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Id_film", Id_film);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM Film WHERE id_film = @Id_film";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Id_film", Id_film);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void addMusicToFilm(int Id_film, int Id_musique)
        {
            using (SqlConnection localConnexion = new SqlConnection(cs))
            {
                localConnexion.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO film_musique VALUES (@Id_film, @Id_musique)", localConnexion))
                {
                    cmd.Parameters.AddWithValue("@Id_film", Id_film);
                    cmd.Parameters.AddWithValue("@Id_musique", Id_musique);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void addCategorieToFilm(int Id_film, int Id_categorie)
        {
            using (SqlConnection localConnexion = new SqlConnection(cs))
            {
                localConnexion.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO categorie_film VALUES (@Id_film, @Id_categorie)", localConnexion))
                {
                    cmd.Parameters.AddWithValue("@Id_film", Id_film);
                    cmd.Parameters.AddWithValue("@Id_categorie", Id_categorie);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // --- 3. ÉCRITURE DANS LA BDD ---
        public static async Task<bool> addFilmWhithApi(MovieDetails filmApi)
        {
            using (SqlConnection localConnexion = new SqlConnection(cs))
            {
                await localConnexion.OpenAsync();

                // A. Vérif doublon
                string queryCheck = "SELECT COUNT(*) FROM Film WHERE titre_film = @Titre";
                using (SqlCommand cmdCheck = new SqlCommand(queryCheck, localConnexion))
                {
                    cmdCheck.Parameters.AddWithValue("@Titre", filmApi.Title);
                    int count = (int)(await cmdCheck.ExecuteScalarAsync() ?? 0);
                    if (count > 0) return false; // LE FILM EXISTE DÉJÀ
                }

                // B. Insertion Film
                string queryInsert = @"
                    INSERT INTO Film (titre_film, duree_film, date_sortie, pegi, jacquette, synopsys, bande_annonce, realisateur, fond)
                    OUTPUT INSERTED.id_film 
                    VALUES (@Titre, @Duree, @Annee, @Pegi, @Poster, @Synop, @Youtube, @Real, @Fond)";

                int newFilmId = 0;

                using (SqlCommand cmdInsert = new SqlCommand(queryInsert, localConnexion))
                {
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

                    newFilmId = (int)(await cmdInsert.ExecuteScalarAsync() ?? 0);
                }

                // C. Catégories
                foreach (var genre in filmApi.Genres)
                {
                    string checkCat = "SELECT id_categorie FROM Categorie WHERE libelle = @NomCat";
                    int idCategorie = 0;

                    using (SqlCommand cmdCatCheck = new SqlCommand(checkCat, localConnexion))
                    {
                        cmdCatCheck.Parameters.AddWithValue("@NomCat", genre.Name);
                        object? result = await cmdCatCheck.ExecuteScalarAsync();
                        
                        if (result != null)
                        {
                            idCategorie = (int)result;
                        }
                        else
                        {
                            string insertCat = "INSERT INTO Categorie (libelle) OUTPUT INSERTED.id_categorie VALUES (@NomCat)";
                            using (SqlCommand cmdCatInsert = new SqlCommand(insertCat, localConnexion))
                            {
                                cmdCatInsert.Parameters.AddWithValue("@NomCat", genre.Name);
                                idCategorie = (int)(await cmdCatInsert.ExecuteScalarAsync() ?? 0);
                            }
                        }
                    }

                    string insertLien = "INSERT INTO categorie_film (id_film, id_categorie) VALUES (@IdFilm, @IdCat)";
                    using (SqlCommand cmdLien = new SqlCommand(insertLien, localConnexion))
                    {
                        cmdLien.Parameters.AddWithValue("@IdFilm", newFilmId);
                        cmdLien.Parameters.AddWithValue("@IdCat", idCategorie);
                        await cmdLien.ExecuteNonQueryAsync();
                    }
                }

                return true; 
            }
        }
    }
}