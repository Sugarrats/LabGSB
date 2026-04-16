using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabGSB
{
    /// <Résumé>
    ///  Cette classe vagérer la session de chaque utilisateur connecté
    /// </Résumé>
    public static class CSessionUtilisateur
    {

        public static int IdEmploye;
        public static string Role;
        public static string Matricule;

        // Initialise la session
        public static void StartSession(int idEmploye, string role, string matricule)
        {
            IdEmploye = idEmploye;
            Role = role;
            Matricule = matricule;
        }
    }

    
}
