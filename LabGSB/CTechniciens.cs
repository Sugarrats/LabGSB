using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabGSB
{
    /// <Résumé>
    ///  Cette classe va gérer la création des techniciens et les actions possibles avec cette classe
    /// </Résumé>

    internal class CTechniciens
    {
        int id_tech;
        int nivFormation;
        int NumE;

        public CTechniciens(int id_tech, int nivFormation, int NumE)
        {
            this.id_tech = id_tech;
            this.nivFormation = nivFormation;
            this.NumE = NumE;
        }

        //Sans ID
        public CTechniciens(int nivFormation, int numE)
        {
            this.nivFormation = nivFormation;
            NumE = numE;
        }

        public int getId_tech()
        {
            return id_tech;
        }

        public int getNivFormation()
        {
            return nivFormation;
        }

        public int getTechnicienIdEmploye()
        {
            return this.NumE;
        }

    }
}
