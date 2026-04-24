using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watchflix.Models
{
    public class Musique
    {
        // Attributs
        private int id;
        private string titre;
        private TimeSpan? duree;
        private string? album;
        private string? couverture;
        private string? idYoutube;
        private string? lien; 

        // Constructeurs
        public Musique(int id, string titre, TimeSpan duree, string album, string couverture, string idYoutube, string lien)
        {
            this.id = id;
            this.titre = titre;
            this.duree = duree;
            this.album = album;
            this.couverture = couverture;
            this.idYoutube = idYoutube;
            this.lien = lien;
        }

        public Musique(string titre, TimeSpan duree, string album, string couverture, string idYoutube, string lien)
        {
            this.titre = titre;
            this.duree = duree;
            this.album = album;
            this.couverture = couverture;
            this.idYoutube = idYoutube;
            this.lien = lien;
        }

        public Musique(int id, string titre)
        {
            this.id = id;
            this.titre = titre;
        }

        // Accesseurs
        public int Id { get => id; }
        public string Titre { get => titre; set => titre = value; }
        public TimeSpan? Duree { get => duree; set => duree = value; }
        public string Album { get => album; set => album = value; }
        public string Couverture { get => couverture; set => couverture = value; }
        public string IdYoutube { get => idYoutube; set => idYoutube = value; }
        public string Lien { get => lien; set => lien = value; }
    }
}