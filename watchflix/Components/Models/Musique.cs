using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watchflix.Components.Models
{
    public class Musique
    {
        // Attributs
        private int id;
        private string titre;
        private TimeSpan duree;
        private string album;
        private string couverture;

        // Constructeurs
        public Musique(int Id, string Titre, TimeSpan Duree, string Album, string Couverture)
        {
            this.id = Id;
            this.Titre = Titre;
            this.Duree = Duree;
            this.Album = Album;
            this.Couverture = Couverture;
        }

        public Musique(string Titre, TimeSpan Duree, string Album, string Couverture)
        {
            this.id = 0;
            this.Titre = Titre;
            this.Duree = Duree;
            this.Album = Album;
            this.Couverture = Couverture;
        }

        // Accesseurs
        public int Id { get => id; }
        public string Titre { get => titre; set => titre = value; }
        public TimeSpan Duree { get => duree; set => duree = value; }
        public string Album { get => album; set => album = value; }
        public string Couverture { get => couverture; set => couverture = value; }
    }
}
