using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace watchflix.Models
{ 
    public class Categorie
    {
        //attributs
        private int id;
        private string libelle;


        //controller
        public Categorie(int Id, string Libelle)
        {
            this.id = Id;
            this.libelle = Libelle;
        }
        public Categorie(string Libelle)
        {
            this.id = 0;
            this.libelle = Libelle;
        }


        //accesseurs
        public int Id {get =>id;} 
        public string Libelle { get =>libelle; set => libelle = value;}
    }
}