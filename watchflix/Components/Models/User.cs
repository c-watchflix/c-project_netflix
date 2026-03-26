using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace watchflix.Models
{ 
    public class User
    {
        //attributs
        public int id;
        public string nom;
        public string prenom;
        public string courriel;
        public string pseudo;
        public string mdp;
        public bool is_admin;


        //controller
        public User(int Id, string Nom, string Prenom, string Courriel, string Pseudo, string Mdp, bool Is_admin)
        {
            this.id = Id;
            this.Nom = Nom;
            this.Prenom = Prenom;
            this.Courriel = Courriel;
            this.Pseudo = Pseudo;
            this.Mdp = Mdp;
            this.Is_admin = Is_admin;
        }
        public User(string Nom, string Prenom, string Courriel, string Pseudo, string Mdp, bool Is_admin)
        {
            // this.id = 0;
            this.Nom = Nom;
            this.Prenom = Prenom;
            this.Courriel = Courriel;
            this.Pseudo = Pseudo;
            this.Mdp = Mdp;
            this.Is_admin = Is_admin;
        }


        //accesseurs
        public int Id {get =>id;} 
        public string Nom { get =>nom; set => nom = value;}
        public string Prenom { get =>prenom; set => prenom = value;}
        public string Courriel { get =>courriel; set => courriel = value;}
        public string Pseudo { get =>pseudo; set => pseudo = value;}
        public string Mdp { get =>mdp; set => mdp = value;}
        public bool Is_admin { get =>is_admin; set => is_admin = value;}

    }
}