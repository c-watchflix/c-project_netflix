using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watchflix.Models
{ 
    public class Film
    {
        //attributs
        public int id;
        public string? titre;
        private string? pegi;
        private string? jacquette;
        private string? synopsys;
        private string? bande_annonce;
        private string? realisateur;
        private string? fond;
        private string? dte_sortie;
        private string? duree;



        //controller
        public Film(int Id, string Titre, string? Pegi, string? Jacquette, string? Resume, string? Bande_annonce, string? Realisateur, string? Fond, string? Dte_sortie, string? Duree)
        {
            this.id = Id;
            this.titre = Titre;
            this.duree = Duree;
            this.dte_sortie = Dte_sortie;
            this.pegi = Pegi;
            this.jacquette = Jacquette;
            this.synopsys = Resume;
            this.bande_annonce = Bande_annonce;
            this.realisateur = Realisateur;
            this.fond = Fond;
        }

        public Film(string Titre, string? Pegi, string? Jacquette, string? Resume, string? Bande_annonce, string? Realisateur, string? Fond, string? Dte_sortie, string? Duree)
        {
            this.id = 0;
            this.titre = Titre;
            this.duree = Duree;
            this.dte_sortie = Dte_sortie;
            this.pegi = Pegi;
            this.jacquette = Jacquette;
            this.synopsys = Resume;
            this.bande_annonce = Bande_annonce;
            this.realisateur = Realisateur;
            this.fond = Fond;
        }

        public Film(int id, string titre)
        {
            this.id = id;
            this.titre = titre;
        }

        public Film(){}


        //accesseurs
        public int Id {get =>id;} 
        public string? Titre { get =>titre; set => titre = value;}
        public string? Duree { get =>duree; set => duree = value;}
        public string? Dte_sortie { get =>dte_sortie; set => dte_sortie = value;}
        public string Pegi { get =>pegi; set => pegi = value;}
        public string Jacquette { get =>jacquette; set => jacquette = value;}
        public string Synopsys { get =>synopsys; set => synopsys = value;}
        public string Bande_annonce { get =>bande_annonce; set => bande_annonce = value;}
        public string Realisateur { get =>realisateur; set => realisateur = value;}  
        public string Fond { get =>fond; set => fond = value;}  
    }
}