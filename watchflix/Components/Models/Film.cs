namespace watchflix.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Titre { get; set; } = "";
        public string Duree { get; set; } = ""; // On stocke en string pour simplifier l'affichage (ex: "01:48:00")
        public string Annee { get; set; } = "";
        public string Pegi { get; set; } = "";
        public string Jacquette { get; set; } = ""; // Contiendra l'URL de l'image API
        public string Synopsis { get; set; } = "";
        public string BandeAnnonce { get; set; } = ""; // Contiendra l'URL Youtube
        public string Realisateur { get; set; } = "";
        public string Fond { get; set; } = ""; // Contiendra l'URL du fond API

        // Constructeur vide nécessaire pour certaines opérations
        public Film() { }

        // Constructeur complet pour la lecture depuis la BDD
        public Film(int id, string titre, string duree, string annee, string pegi, string jacquette, string synopsis, string bandeAnnonce, string realisateur, string fond)
        {
            Id = id;
            Titre = titre;
            Duree = duree;
            Annee = annee;
            Pegi = pegi;
            Jacquette = jacquette;
            Synopsis = synopsis;
            BandeAnnonce = bandeAnnonce;
            Realisateur = realisateur;
            Fond = fond;
        }
    }
}