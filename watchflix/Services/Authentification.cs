using watchflix.Models;
using watchflix.Repositories;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace watchflix.Services
{
    public class Authentification
    {
        private User? _currentUser;
        private readonly ProtectedSessionStorage _sessionStorage;

        public Authentification(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public bool IsAuthenticated => _currentUser != null;

        public void SetCurrentUser(User? user)
        {
            if (user == null)
            {
                Debug.WriteLine("Tentative de définir un utilisateur null.");
                return;
            }

            _currentUser = user;
        }

        public User? GetCurrentUser()
        {
            return _currentUser;
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public User? Login(string pseudo, string mdp, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Hachage du mot de passe avant l'authentification
            string hashedPassword = HashPassword(mdp);

            // Journalisation des informations d'entrée
            Debug.WriteLine($"Tentative de connexion avec Pseudo: {pseudo}, Mot de passe haché: {hashedPassword}");

            // Appelle la méthode Authenticate dans AdoUser
            User? user = AdoUser.Authenticate(pseudo, hashedPassword);

            if (user == null)
            {
                Debug.WriteLine("Échec de l'authentification : Pseudo ou mot de passe incorrect.");
                errorMessage = "Pseudo ou mot de passe incorrect.";
                return null;
            }

            SetCurrentUser(user);
            return user;
        }

        public async Task<(User? user, string errorMessage)> LoginAsync(string pseudo, string mdp)
        {
        return await Task.Run(() =>
        {
            string errorMessage;
            User? user = Login(pseudo, mdp, out errorMessage);
            return (user, errorMessage);
        });
    }

        public async Task PersistUserAsync(User user)
        {
            await _sessionStorage.SetAsync("CurrentUser", user);
            SetCurrentUser(user);
        }

        public async Task RestoreUserAsync()
        {
            var result = await _sessionStorage.GetAsync<User>("CurrentUser");
            if (result.Success && result.Value != null)
            {
                SetCurrentUser(result.Value);
            }
        }
    }
}