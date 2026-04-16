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

    internal class CUtilisateurs
    {
        int NumU;
        string ObjectifU;
        float PrimeU;
        int NumE;

        ///  Constructeur d'un utilisateur avec l'id

        public CUtilisateurs(int NumU, string ObjectifU, float PrimeU, int NumE)
        {
            this.NumU = NumU;
            this.ObjectifU = ObjectifU;
            this.PrimeU = PrimeU;
            this.NumE = NumE;
        }


        ///  Constructeur d'un employé sans l'id ( autoincrémenté )

        public CUtilisateurs(string ObjectifU, float PrimeU, int NumE)
        {
            this.ObjectifU = ObjectifU;
            this.PrimeU= PrimeU;
            this.NumE = NumE;
        }

        /// Get pour  les paramétres d'utilisateur
        public int getUtilisateurIdUtilisateur()
        {
            return this.NumU;
        }

        public int getUtilisateurIdEmploye()
        {
            return this.NumE;
        }

        public string getUtilisateurObjectif()
        {
            return this.ObjectifU;
        }

        public float getUtilisateurPrime()
        {
            return this.PrimeU;
        }
    }
}
