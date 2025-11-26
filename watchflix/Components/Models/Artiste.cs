using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watchflix.Models
{ 
    public class Artiste
    {
        //attributs
        private int id;
        private string nom_artiste;


        //controller
        public Artiste(int Id, string Nom_artiste)
        {
            this.id = Id;
            this.Nom = Nom_artiste;
        }
        public Artiste(string Nom_artiste)
        {
            this.id = 0;
            this.Nom = Nom_artiste;
        }


        //accesseurs
        public int Id {get =>id;} 
        public string Nom { get =>nom_artiste; set => nom_artiste = value;}
    }
}