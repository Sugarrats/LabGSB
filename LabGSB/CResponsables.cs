using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabGSB
{
    /// <Résumé>
    ///  Cette classe va gérer la création des Responsables et les actions possibles avec cette classe
    /// </Résumé>

    internal class CResponsables
    {
        int NumR;
        string RegionR;
        int NumE;


        ///  Constructeur d'un responsables avec l'id

        public CResponsables(int NumR, string RegionR, int NumE)
        {
            this.NumR = NumR;
            this.RegionR = RegionR;
            this.NumE = NumE;
        }

        ///  Constructeur d'un responsable sans l'id ( autoincrémenté )

        public CResponsables(string RegionR, int NumE)
        {
            this.RegionR = RegionR;
            this.NumE = NumE;
        }

        // fonction pour récupérer le numéro d'employé du responsable
        public int getResponsableIdEmploye()
        {
            return this.NumE;
        }

    }
}
