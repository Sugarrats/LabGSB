using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabGSB
{
    /// <Résumé>
    ///  Cette classe va gérer l'état des incidents
    /// </Résumé>

    internal class CIncidents
    {
        int id_incid;
        int id_techn;
        int id_uti;
        int id_mater;

        string etat;
        string descr;
        string Tprise_charge;

        DateTime date;
        DateTime HDebut;

        // Le point d'interrogation indique que la variable peut être nulle
        DateTime? HFin;
            
        public CIncidents(int id_incid, string etat, DateTime date, DateTime HDebut, DateTime? HFin, string descr, string Tprise_charge, int id_techn, int id_uti, int id_mater)
        {
            this.id_incid = id_incid;
            this.etat = "En attente";  // valeur par défaut
            this.date = date;
            this.HDebut = HDebut;
            this.HFin = HFin;
            this.descr = descr;
            this.Tprise_charge = Tprise_charge;
            this.id_techn = 0;  // valeur par défaut
            this.id_uti = id_uti;
            this.id_mater = id_mater;
        }

        public CIncidents(string etat, DateTime date, DateTime HDebut, DateTime? HFin, string descr, string Tprise_charge, int id_techn, int id_uti, int id_mater)
        {
            this.etat = "En attente"; // valeur par défaut
            this.date = date;
            this.HDebut = HDebut;
            this.HFin = HFin;
            this.descr = descr;
            this.Tprise_charge = Tprise_charge;
            this.id_techn = 0; // valeur par défaut
            this.id_uti = id_uti;
            this.id_mater = id_mater;
        }

        ///get des informations.
        ///
        public string getEtat()
        {
            return etat;
        }
        public int getId_incid()
        {
            return id_incid;
        }

        public DateTime getDate()
        {
            return date;
        }

        public DateTime getHeureDeb()
        {
            return HDebut;
        }

        public DateTime? getHeureFin()
        {
            return HFin;
        }

        public string getDescr()
        {
            return descr;
        }

        public string getTypeTravail()
        {
            return Tprise_charge;
        }

        public int getId_tech()
        {
            return id_techn;
        }

        public int getId_uti()
        {
            return id_uti;
        }

        public int getId_mat()
        {
            return id_mater;
        }


        // Méthodes pour modifier l'état de l'incident
        public void priseCharge(int id_techn)
        {
            this.etat = "En cours";
            this.id_techn = id_techn;
        }

        public void resousIncident(int id_techn)
        {
            this.etat = "Resolu";
            this.id_techn = id_techn;
        }
    }
}
