using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LabGSB
{
    /// <Résumé>
    ///  Cette classe va gérer la création des employés et les actions possibles avec cette classe
    /// </Résumé>

    internal class CEmployes
    {
        int NumE;
        string MatriculeE;
        string NomE;
        string PrenomE;
        string Date_EmbaucheE;
        string RegionsE;
        string MdpE;
        string roleE;

        ///  Constructeur d'un employé avec l'id

        public CEmployes(int NumE, string MatriculeE, string NomE, string PrenomE, string Date_EmbaucheE, string RegionsE, string MdpE, string roleE)
        {
            this.NumE = NumE;
            this.MatriculeE = MatriculeE;
            this.NomE = NomE;
            this.PrenomE = PrenomE;
            this.Date_EmbaucheE = Date_EmbaucheE;
            this.RegionsE = RegionsE;
            this.MdpE = MdpE;
            this.roleE = roleE;
        }


        ///  Constructeur d'un employé sans l'id ( autoincrémenté )

        public CEmployes(string MatriculeE, string NomE, string PrenomE, string Date_EmbaucheE, string RegionsE, string MdpE, string roleE)
        {
            this.MatriculeE = MatriculeE;
            this.NomE = NomE;
            this.PrenomE = PrenomE;
            this.Date_EmbaucheE = Date_EmbaucheE;
            this.RegionsE = RegionsE;
            this.MdpE = MdpE;
            this.roleE = roleE;
        }

        // Constructeur d'un employé sans le mot de passe ( pour affichage )
        public CEmployes(int NumE, string MatriculeE, string NomE, string PrenomE,string Date_EmbaucheE, string RegionsE, string roleE)
        {
            this.NumE = NumE;
            this.MatriculeE = MatriculeE;
            this.NomE = NomE;
            this.PrenomE = PrenomE;
            this.Date_EmbaucheE = Date_EmbaucheE;
            this.RegionsE = RegionsE;
            this.roleE = roleE;
        }

        /// Get pour  les paramétres d'employé 

        public int getIdEmploye()
        {
            return this.NumE;
        }

        public string getMatriculeE()
        {
            return this.MatriculeE;
        }
        public int getNumE()
        {
            return this.NumE;
        }

        public string getNomE()
        {
            return this.NomE;
        }

        public string getPrenomE()
        {
            return this.PrenomE;
        }

        public string getDateEmbaucheE()
        {
            return this.Date_EmbaucheE;
        }

        public string getRegionsE()
        {
            return this.RegionsE;
        }

        public string getMdpE()
        {
            return this.MdpE;
        }

        public string getRoleE()
        {
            return this.roleE;
        }

    }
}
