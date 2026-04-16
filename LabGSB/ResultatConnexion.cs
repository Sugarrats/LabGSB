using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabGSB
{
    internal class ResultatConnexion
    {
        /// <Résumé>
        ///  Cette classe va permettre de garder les informations après la connexion
        ///  Elle va récupérer l'id  le rôle et le matricule de l'employé connecté
        /// </Résumé>

       public int IdEmploye { get; set; }
       public string Role { get; set; }
       public string Matricule { get; set; }

    }
}
