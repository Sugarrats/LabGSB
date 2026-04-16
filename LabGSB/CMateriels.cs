using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabGSB
{
    /// <Résumé>
    ///  Cette classe va gérer la création du matériel
    /// </Résumé>

    internal class CMateriels
    {
        string processeur;
        string disque;
        string logiciel;
        string fournisseur;
        string date_achat_loc;

        int memoire;
        int garantie;
        int id_mat;
        int id_e;
        int achat;

        //Avec ID
        public CMateriels(int id_mat, string processeur, int memoire, string disque, string logiciel, string fournisseur, string date_achat_loc, int garantie, int achat, int id_e)
        {
            this.id_mat = id_mat;
            this.processeur = processeur;
            this.memoire = memoire;
            this.disque = disque;
            this.logiciel = logiciel;
            this.fournisseur = fournisseur;
            this.date_achat_loc = date_achat_loc;
            this.garantie = garantie;
            this.achat = achat;
            this.id_e = id_e;
        }

        //Sans ID
        public CMateriels(string processeur, int memoire, string disque, string logiciel, string fournisseur, string date_achat_loc, int garantie, int achat, int id_e)
        {
            this.processeur = processeur;
            this.memoire = memoire;
            this.disque = disque;
            this.logiciel = logiciel;
            this.fournisseur = fournisseur;
            this.date_achat_loc = date_achat_loc;
            this.garantie = garantie;
            this.achat = achat;
            this.id_e = id_e;
        }

        /// Get pour  les paramétres materiels
        public int getIdMat()
        {
            return id_mat;
        }

        public string getProcesseur()
        {
            return processeur;
        }

        public int getMemoire()
        {
            return memoire;
        }

        public string getDisque()
        {
            return disque;
        }

        public string getLogiciel()
        {
            return logiciel;
        }

        public string getFournisseur()
        {
            return fournisseur;
        }

        public string getdate_achat_loc()
        {
            return date_achat_loc;
        }

        public int getGarantie()
        {
            return garantie;
        }

        public int getAchat()
        {
            return achat;
        }

        public int getId_e()
        {
            return id_e;
        }

    }
}