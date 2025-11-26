using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ap3_jintegration.classe
{ 
    public class Film
    {
        //attributs
        private int id;
        private string titre;
        private TimeOnly duree;
        private DateOnly dte_sortie;
        private string pegi;
        private string jacquette;
        private string resume;
        private string distribution;
        private string bande_annonce;
        private string realisateur;


        //controller
        public Film(int Id, string Titre, TimeOnly Duree, DateOnly Dte_sortie, string Pegi, string Jacquette, string Resume, string Distribution, string Bande_annonce, string Realisateur)
        {
            this.id = Id;
            this.titre = Titre;
            this.duree = Duree;
            this.dte_sortie = Dte_sortie;
            this.pegi = Pegi;
            this.jacquette = Jacquette;
            this.resume = Resume;
            this.distribution = Distribution;
            this.bande_annonce = Bande_annonce;
            this.realisateur = Realisateur;
        }

        public Film(string Titre, TimeOnly Duree, DateOnly Dte_sortie, string Pegi, string Jacquette, string Resume, string Distribution, string Bande_annonce, string Realisateur)
        {
            this.id = 0;
            this.titre = Titre;
            this.duree = Duree;
            this.dte_sortie = Dte_sortie;
            this.pegi = Pegi;
            this.jacquette = Jacquette;
            this.resume = Resume;
            this.distribution = Distribution;
            this.bande_annonce = Bande_annonce;
            this.realisateur = Realisateur;
        }


        //accesseurs
        public int Id {get =>id;} 
        public string Titre { get =>titre; set => titre = value;}
        public TimeOnly Duree { get =>duree; set => duree = value;}
        public DateOnly Dte_sortie { get =>dte_sortie; set => dte_sortie = value;}
        public string Pegi { get =>pegi; set => pegi = value;}
        public string Jacquette { get =>jacquette; set => jacquette = value;}
        public string Resume { get =>resume; set => resume = value;}
        public string Distribution { get =>distribution; set => distribution = value;}
        public string Bande_annonce { get =>bande_annonce; set => bande_annonce = value;}
        public string Realisateur { get =>realisateur; set => realisateur = value;}    
    }
}