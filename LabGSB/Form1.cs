using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabGSB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // fonction qui va vider notre application
        private void ViderControles(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                
                if (ctrl is TextBox)
                {
                    ((TextBox)ctrl).Clear();
                }
                else if (ctrl is RichTextBox)
                {
                    ((RichTextBox)ctrl).Clear();
                }
                else if (ctrl is ListBox)
                {
                    ((ListBox)ctrl).Items.Clear();
                }
                else if (ctrl is ComboBox)
                {
                    ((ComboBox)ctrl).SelectedIndex = -1;
                    ((ComboBox)ctrl).Text = "";
                }
                if (ctrl.HasChildren)
                {
                    ViderControles(ctrl);
                }
                else if (ctrl is RadioButton)
                {
                    ((RadioButton)ctrl).Checked = false;
                }
                else if (ctrl is CheckBox)
                {
                    ((CheckBox)ctrl).Checked = false;
                }
            }
        }



        // fonction qui va remplir nos combobox et listbox au chargement du formulaire
        private void Actualiser()
        {
            BD maBD = new BD();

            // On récupère la liste des matricules sans ordinateurs grâce à la méthode de la classe BD
            List<string> listeMatricules = maBD.RecupererMatriculeEmployesSansOrdinateur();

            // On récupére la liste des techniciens grâce à la méthode de la classe BD
            List<string> listeTechniciens = maBD.RecupererMatriculeTechniciens();

            // On récupère la liste des utilisateurs grâce à la méthode de la classe BD
            List<string> listeUtilisateurs = maBD.RecupererMatriculeUtilisateurs();

            // On récupère la liste des matériels grace à la méthode de la classe BD
            List<int> listeMateriels = maBD.RecupererIdMateriel();

            // On récupére la liste des infos des incidents non résolus
            List<String> listeIncidentsNonResolus = maBD.RecupererIncidentsNonResolues();

            // On récupére la liste des incidents non résolus pour la combobox
            List<int> listeIdIncidentsNonResolus = maBD.RecupererIdIncidentsNonResolus();

            // On récupére la liste des incidents en cours pour la combobox
            List<int> listeIdIncidentsEncours = maBD.RecupererIdIncidentsEnCours();

            // Netoyage des combobox
            ComboBoxPropri.Items.Clear();
            ComboBoxPoste.Items.Clear();
            comboBoxMateriel.Items.Clear();
            comboBoxIncidentsNonPrisEnCharge.Items.Clear();
            comboBoxResoudre.Items.Clear();
            comboBoxModifierEmployesTechnicien.Items.Clear();
            comboBoxModifierEmployesUtilisateur.Items.Clear();

            // Netoyage de la listbox
            listBoxIncidentsNonPrisEnCharge.Items.Clear();

            // On remplit la combobox des propriétaires
            foreach (string matricule in listeMatricules)
            {
                ComboBoxPropri.Items.Add(matricule);
            }

            if (ComboBoxPropri.Items.Count > 0)
                ComboBoxPropri.SelectedIndex = 0;

            // On remplit les combobox des matériels
            foreach (int materiels in listeMateriels)
            {
                ComboBoxPoste.Items.Add(materiels);
                comboBoxMateriel.Items.Add(materiels);
            }

            if (ComboBoxPoste.Items.Count > 0)
                ComboBoxPoste.SelectedIndex = 0;

            if (comboBoxMateriel.Items.Count > 0)
                comboBoxMateriel.SelectedIndex = 0;

            // On remplit la combobox des techniciens
            foreach (string technicien in listeTechniciens)
            {
                comboBoxModifierEmployesTechnicien.Items.Add(technicien);
            }


            // On remplit la combobox des utilisateurs
            foreach (string utilisateur in listeUtilisateurs)
            {
                comboBoxModifierEmployesUtilisateur.Items.Add(utilisateur);
            }


            // On remplit les combobox des incidents non résolus et en cours
            foreach (int idIncident in listeIdIncidentsNonResolus)
            {
                comboBoxIncidentsNonPrisEnCharge.Items.Add(idIncident);
            }

            foreach (int idIncident in listeIdIncidentsEncours)
            {
                comboBoxResoudre.Items.Add(idIncident);
            }

            if (comboBoxResoudre.Items.Count > 0)
                comboBoxResoudre.SelectedIndex = 0;

            if (comboBoxIncidentsNonPrisEnCharge.Items.Count > 0)
                comboBoxIncidentsNonPrisEnCharge.SelectedIndex = 0;

            // On remplit la listbox avec les infos des incidents non résolus
            foreach (string incident in listeIncidentsNonResolus)
            {
                string[] lignes = incident.Split('\n');

                foreach (string ligne in lignes)
                    listBoxIncidentsNonPrisEnCharge.Items.Add(ligne);

                // Ligne vide entre incidents
                listBoxIncidentsNonPrisEnCharge.Items.Add("");
            }

            // On réactive les éléments désactivés

            // Activer les champs techniciens
            radioButtonModifierBac.Enabled = true;
            radioButtonModifierBTS.Enabled = true;
            radioButtonModifierMaster.Enabled = true;
            comboBoxModifierEmployesTechnicien.Enabled = true;
            radioBac.Enabled = true;
            radioBTS.Enabled = true;
            radioMaster.Enabled = true;

            // Activer les textbox utilisateur
            textBoxModifierPrime.Enabled = true;
            textBoxModifierObjectif.Enabled = true;
            textBoxObjectifsUtilisateurs.Enabled = true;
            textBoxPrimeUtilisateur.Enabled = true;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // On a accés que à l'onglet de connexion au début
            for (int i = 1; i < tabControl1.TabPages.Count; i++)
            {
                tabControl1.TabPages[i].Enabled = false;
            }

            // Aller vers le premier onglet
            tabControl1.SelectedIndex = 0;
        }



        private void txtMDP_TextChanged(object sender, EventArgs e)
        {
        }

        // Les responsables crée un nouvel employés
        private void buttonCreerEmployes_Click(object sender, EventArgs e)
        {
            // on gère le rôle
            string role = "";

            BD maBD = new BD();

            int niveau = 0;

            // si l'employés est un technicien.
            if (radioButtonTechnicien.Checked)
            {
                role = "Technicien";

                if (radioBac.Checked) niveau = 1;
                else if (radioBTS.Checked) niveau = 2;
                else if (radioMaster.Checked) niveau = 3;
            }

            // si l'employés est un utilisateur
            else if (radioButtonUtilisateur.Checked)
            {
                role = "Utilisateur";

                // On désactive les radios des techiniciens
                radioBac.Enabled = false;
                radioBTS.Enabled = false;
                radioMaster.Enabled = false;
            }

            // On crée l’employé
            CEmployes unEmployes = new CEmployes(textBoxMatricule.Text, textBoxNom.Text, textBoxPrenom.Text, dateTimePickerE.Value.ToString("yyyy-MM-dd"), textBoxRegion.Text, txtBoxMDPE.Text, role);

            // Récupére l'ID de l'employé créé
            int idEmployeCree = maBD.InsererEmployer(unEmployes);

            // Ajouter les techniciens si besoin
            if (radioButtonTechnicien.Checked)
            {
                CTechniciens unTechniciens = new CTechniciens(niveau, idEmployeCree);
                maBD.InsertTechniciens(unTechniciens, idEmployeCree);
            }

            // Ajouter les utilisateurs si besoin
            else if (radioButtonUtilisateur.Checked)
            {
                string objectif = textBoxObjectifsUtilisateurs.Text;
                float prime = float.Parse(textBoxPrimeUtilisateur.Text);
                float budget = float.Parse(textBoxPrimeUtilisateur.Text);

                CUtilisateurs unUtilisateurs = new CUtilisateurs(objectif, prime, idEmployeCree);

                maBD.InsererUtilisateur(unUtilisateurs, idEmployeCree);
            }

            // MessageBox de confirmation
            DialogResult result = MessageBox.Show("Un nouvel employé a était crée"
            );

            // Actualiser
            Actualiser();
        }

        // Ajouter un matériel
        private void buttonAjouterMat_Click(object sender, EventArgs e)
        {
            BD maBD = new BD();

            // récupérer l'id du propriétaire sélectionné
            int idProprietaire = maBD.RecupererIdEmployeParMatricule(ComboBoxPropri.SelectedItem.ToString());

            // récupérer la mémoire en int
            int memoire = int.Parse(TextboxMemoire.Text);

            // Gestion de la garentie
            int garantie = 0;

            if (radioButtonGarantieOui.Checked)
            {
                garantie = 1;
            }
            else if (radioButtonGarantieNon.Checked)
            {
                garantie = 0;
            }

            // Gestion de l'achat ou location
            int achat = 0;

            if (radioButtonAchat.Checked)
            {
                achat = 1;
            }
            else if (radioButtonLoué.Checked)
            {
                garantie = 0;
            }

            // On crée le matériel
            CMateriels unMateriel = new CMateriels(TextboxProcesseur.Text, memoire, TextboxDisque.Text, TextboxLogiciels.Text,textBoxFournisseur.Text, dateTimePickerMat.Value.ToString("yyyy-MM-dd"), garantie, achat, idProprietaire);

            // on insère le matériel dans la BD
            maBD.InsererMateriel(unMateriel);

            // MessageBox de confirmation
            MessageBox.Show("Matériel ajouté avec succès !");

            // On actualise les combobox
            Actualiser();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBoxModifierNom_TextChanged(object sender, EventArgs e)
        {

        }

        private void TchargeInc_Click(object sender, EventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void textBoxModifierPrime_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonModifierBTS_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBoxDateMateriel_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBoxGarantie_Enter(object sender, EventArgs e)
        {

        }

        private void ListBoxInfosMateriel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void buttonConsulterMateriel_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void radioButtonGarantieOui_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBoxAchat_Enter(object sender, EventArgs e)
        {

        }

        private void ComboBoxPropri_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBoxModifierMatricule_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBoxIncidentsNonPrisEnCharge_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioBac_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonAchat_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void TextboxDisque_TextChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxMateriel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxTechnicienPrendreEnCharge_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        // Ce bouton va modifier un employé
        private void buttonModifierEmploye_Click(object sender, EventArgs e)
        {
            BD maBD = new BD();

            // On vérifie quel rôle est sélectionné et on modifie l'employé correspondant
            if (comboBoxModifierEmployesTechnicien.SelectedItem != null)
            {
                // d'abord on récupére l'id du technicien à modifier
                string matricule = comboBoxModifierEmployesTechnicien.SelectedItem.ToString();
                int idTechnicien = maBD.RecupererIdEmployeParMatricule(matricule);

                String roleE = "Technicien";
                int niveau = 0; 

                if (radioButtonModifierBac.Checked)
                    niveau = 1;
                else if (radioButtonModifierBTS.Checked) 
                    niveau = 2;
                else if (radioButtonModifierMaster.Checked) 
                    niveau = 3;

                // On crée l’employé
                CEmployes unEmployes = new CEmployes(idTechnicien, textBoxModifierMatricule.Text, textBoxModifierNom.Text, textBoxModifierPrenom.Text,dateTimePickerModifierDateEmbauche.Value.ToString("yyyy-MM-dd"),textBoxRegion.Text,roleE);

                // On crée le technicien
                CTechniciens unTechniciens = new CTechniciens(niveau, idTechnicien);

                // puis on modifie l'employé dans la bd
                maBD.ModifierEmploye(unEmployes);
                maBD.ModifierTechnicien(unTechniciens, idTechnicien);
            }

            else if (comboBoxModifierEmployesUtilisateur.SelectedItem != null)
            {
                // d'abord on récupére l'id de l'utilisateur à modifier
                string matricule = comboBoxModifierEmployesUtilisateur.SelectedItem.ToString();
                int idUtilisateur = maBD.RecupererIdEmployeParMatricule(matricule);

                String roleE = "Utilisateur";

                // On crée l’employé
                CEmployes unEmployes = new CEmployes(idUtilisateur, textBoxModifierMatricule.Text, textBoxModifierNom.Text, textBoxModifierPrenom.Text, dateTimePickerModifierDateEmbauche.Value.ToString("yyyy-MM-dd"), textBoxRegion.Text, roleE);


                // On crée l'utilisateur
                CUtilisateurs unUtilisateurs = new CUtilisateurs(textBoxModifierObjectif.Text, float.Parse(textBoxModifierPrime.Text), idUtilisateur);

                // puis on modifie l'employé dans la bd
                maBD.ModifierEmploye(unEmployes);
                maBD.ModifierUtilisateur(unUtilisateurs, idUtilisateur);
            }

            // On vide les sélections des combobox
            comboBoxModifierEmployesTechnicien.SelectedItem = null;
            comboBoxModifierEmployesUtilisateur.SelectedItem = null;
            comboBoxModifierEmployesTechnicien.Items.Clear();
            comboBoxModifierEmployesUtilisateur.Items.Clear();

            // MessageBox de confirmation
            DialogResult result = MessageBox.Show("L'employé a bien été modifié.");

            // On actualise les combobox
            Actualiser();
        }

     

        private void textBoxRegion_TextChanged(object sender, EventArgs e)
        {

        }

        private void Tstat_Click(object sender, EventArgs e)
        {

        }

        // Quand ce bouton est coché les autres deviennent inactifs
        private void radioButtonTechnicien_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTechnicien.Checked)
            {
                // Désactiver les champs utilisateur
                textBoxObjectifsUtilisateurs.Enabled = false;
                textBoxPrimeUtilisateur.Enabled = false;

                // Activer les radios technicien
                radioBac.Enabled = true;
                radioBTS.Enabled = true;
                radioMaster.Enabled = true;
            }
        }

        private void textBoxModifierRegion_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Tutilisateur_Click(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void textBoxObjectifsUtilisateurs_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePickerE_ValueChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonModifierTech_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ListBoxAvacement_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void TcreaEmp_Click(object sender, EventArgs e)
        {

        }

        // Quand ce bouton est coché les champs utilisateurs ne sont plus accesibles
        private void radioButtonUtilisateur_CheckedChanged(object sender, EventArgs e)
        {
            // Activer les champs utilisateur
            textBoxObjectifsUtilisateurs.Enabled = true;
            textBoxPrimeUtilisateur.Enabled = true;

            // Désactiver les radios technicien
            radioBac.Enabled = false;
            radioBTS.Enabled = false;
            radioMaster.Enabled = false;
        }

        // Un utilisateur déclare un incident sur un poste
        private void buttonDeclarerIncident_Click(object sender, EventArgs e)
        {
            BD maBD = new BD();

            // Récupérer l'id du propriétaire sélectionné
            int idProprietaire = CSessionUtilisateur.IdEmploye;

            // Récupérer l'id du poste concerné
            int idPoste = Convert.ToInt32(ComboBoxPoste.SelectedItem);

            // Date de déclaration de l'incident
            DateTime maintenant = DateTime.Now;

            // Date de l’incident
            DateTime dateIncident = maintenant.Date; // juste la date (00:00:00)

            // Heure de l'incident
            DateTime heureDebut = maintenant;

            // Heure de résolution de l'incident (null au début)
            DateTime? heureFin = null;


            // On crée l’incident
            CIncidents unIncident = new CIncidents("EN ATTENTE", dateIncident, heureDebut, heureFin, TextBoxDesc.Text, "Aucune", 0, idProprietaire, idPoste);

            // on insère l'incident dans la BD
            maBD.InsererIncident(unIncident);

            MessageBox.Show($"Incident déclaré patienté avant sa prise en charge");

            // On actualise les combobox
            Actualiser();
        }

        private void radioBTS_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void TextBoxMat_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void buttonSupprimerMateriel_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxIncidentsNonPrisEnCharge_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        // ce bouton va permetre à un utilisateur de suivre l'avancement des incidents qu'il a déclaré
        private void BtAvencement_Click(object sender, EventArgs e)
        {
            BD maBD = new BD();

            // Récupérer l'id du propriétaire sélectionné
            int idProprietaire = CSessionUtilisateur.IdEmploye;

            // On appel la fonction  pour récupérer les infos des incidents de l'utilisateur
            string infos = maBD.SuiviIncidentParIdUtilisateur(idProprietaire);

            // On affiche les infos dans la listbox
            string[] lignes = infos.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            listBoxInfosMat.Items.Clear(); // vider avant d'ajouter
            foreach (string ligne in lignes)
            {
                ListBoxAvacement.Items.Add(ligne);
            }
        }

        private void TextboxProcesseur_TextChanged(object sender, EventArgs e)
        {

        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        // Gestion de la connexion à la bd.
        private void connexion_Click(object sender, EventArgs e)
        {
            // Récupération et nettoyage du matricule
            string matricule = TextBoxMatConnexion.Text.Trim();
            byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(matricule);
            string hexCSharp = BitConverter.ToString(utf8Bytes).Replace("-", "");

            string motDePasse = TextBoxMDPConnexion.Text.Trim();

            BD maBD = new BD();

            // Connexion - retourne un ResultatConnexion
            ResultatConnexion res = maBD.Connexion(matricule, motDePasse);

            if (res == null)
            {
                MessageBox.Show("Identifiants incorrects.",
                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Stockage global en session
            CSessionUtilisateur.StartSession(res.IdEmploye, res.Role, matricule);

            MessageBox.Show($"Connexion réussie.",
                            "Bienvenue", MessageBoxButtons.OK, MessageBoxIcon.Information);


            // Désactiver tous les onglets
            foreach (TabPage tab in tabControl1.TabPages)
                tab.Enabled = false;

            Tconnexion.Enabled = true;

            // Selon le rôle l'employé a accès à certains onglets
            switch (res.Role)
            {
                case "Utilisateur":
                    Tutilisateur.Enabled = true;
                    break;

                case "Technicien":
                    TajtMat.Enabled = true;
                    TchargeInc.Enabled = true;
                    break;

                case "Responsable":
                    TcreaEmp.Enabled = true;
                    Tstat.Enabled = true;
                    break;
            }

            // Aller vers le premier onglet autorisé
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Enabled)
                {
                    tabControl1.SelectedTab = tab;
                    break;
                }
            }

            Actualiser();
        }


        private void txtBoxMDPE_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        // Les responsables affichent les statistiques des primes des utilisateurs
        private void buttonAfficherStatistiquesPrime_Click(object sender, EventArgs e)
        {

            BD maBD = new BD();
            if (radioButtonCroissant.Checked)
            {
                // créer une liste pour stocker les résultats
                List<string> resultats = maBD.ClasserUtilisateursParPrimeCroissant();

                // Vider la ListBox avant d'ajouter
                listBoxPrime.Items.Clear();

                foreach (string ligne in resultats)
                {
                    listBoxPrime.Items.Add(ligne);
                }
            }

            else if (radioButtonDecroissant.Checked)
            {
                // créer une liste pour stocker les résultats
                List<string> resultats = maBD.ClasserUtilisateursParPrimeDecroissant();

                // Vider la ListBox avant d'ajouter
                listBoxPrime.Items.Clear();

                foreach (string ligne in resultats)
                {
                    listBoxPrime.Items.Add(ligne);
                }
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void groupBoxModifierEmployer_Enter(object sender, EventArgs e)
        {

        }

        private void textBoxPrimeUtilisateur_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonModifierUtilisateur_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void TextboxLogiciels_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxMatricule_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void TajtMat_Click(object sender, EventArgs e)
        {

        }

        private void textBoxPrenom_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void TextboxMemoire_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonModifierBac_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBoxModifierPrenom_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void Tconnexion_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxRésoudre_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonGarantieNon_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void groupBoxRoleEmploye_Enter(object sender, EventArgs e)
        {

        }

        // Les techniciens prennent en charge un incident
        private void buttonPrendreEnCharge_Click(object sender, EventArgs e)
        {
            BD maBD = new BD();

            // Récupérer l'id du technicien qui va résoudre l'incident
            int idTechnicien = CSessionUtilisateur.IdEmploye;

            // Récupérer l'id de l'incident sélectionné
            int idIncident = Convert.ToInt32(comboBoxIncidentsNonPrisEnCharge.SelectedItem);



            maBD.ModifierIncidentPrendreEncharge(idIncident, idTechnicien, richTextBoxTypePriseEnCharge.Text);

            // MessageBox de confirmation
            DialogResult result = MessageBox.Show("L'incident a bien été pris en charge."
            );


            // On actualise les combobox
            Actualiser();
        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void ComboBoxPoste_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TextBoxDesc_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBoxDescriptionTravail_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonSupprimerEmployes_Click(object sender, EventArgs e)
        {
            BD maBD = new BD();

            // On vérifie quel rôle est sélectionné et on supprime l'employé correspondant
            if (comboBoxModifierEmployesTechnicien.SelectedItem != null)
            {
                string role = "Technicien";

                // d'abord on récupére l'id du technicien à supprimer
                string matricule = comboBoxModifierEmployesTechnicien.SelectedItem.ToString();
                int idTechnicien = maBD.RecupererIdEmployeParMatricule(matricule);

                // MessageBox de confirmation
                DialogResult result = MessageBox.Show(
                    "Êtes-vous sûr de vouloir supprimer ce technicien ?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    maBD.SupprimerTechinicien(idTechnicien);
                    MessageBox.Show("Le technicien a bien été supprimé.");
                }
            }

            else if (comboBoxModifierEmployesUtilisateur.SelectedItem != null)
            {
                string role = "Utilisateur";

                // d'abord on récupére l'id de l'utilisateur à supprimer
                string matricule = comboBoxModifierEmployesUtilisateur.SelectedItem.ToString();
                int idUtilisateur = maBD.RecupererIdEmployeParMatricule(matricule);

                // MessageBox de confirmation
                DialogResult result = MessageBox.Show(
                    "Êtes-vous sûr de vouloir supprimer cet utilisateur ?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    maBD.SupprimerUtilisateur(idUtilisateur);
                    MessageBox.Show("L'utilisateur a bien été supprimé.");
                }
            }

            // On vide les sélections des combobox
            comboBoxModifierEmployesTechnicien.SelectedItem = null;
            comboBoxModifierEmployesUtilisateur.SelectedItem = null;
            comboBoxModifierEmployesTechnicien.Items.Clear();
            comboBoxModifierEmployesUtilisateur.Items.Clear();

            // On actualise les combobox
            Actualiser();
        }

        // Cette fonction va permettre au technicien de résoudre un incident et de décrire le travail effectué
        private void buttonResoudre_Click(object sender, EventArgs e)
        {
            BD maBD = new BD();

            // Date de résolution de l'incident
            DateTime maintenant = DateTime.Now;

            // Heure de la résolution de l'incident
            DateTime heureFin = maintenant;


            // Récupérer l'id de l'incident sélectionné
            int idIncident = Convert.ToInt32(comboBoxResoudre.SelectedItem);

            maBD.ModifierIncidentResoudre(idIncident, richTextBoxDescriptionTravail.Text, heureFin);

            // MessageBox de confirmation
            DialogResult result = MessageBox.Show("Incident résolu avec succès."
            );

            // On actualise les combobox
            Actualiser();
        }

        // Quand on modifie un technicien ont désactive les textbox de l'utilisateur
        private void comboBoxModifierEmployes_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Activer les champs techniciens
            radioButtonModifierBac.Enabled = true;
            radioButtonModifierBTS.Enabled = true;
            radioButtonModifierMaster.Enabled = true;
            comboBoxModifierEmployesTechnicien.Enabled = true;

            // désactiver les textbox utilisateur
            textBoxModifierPrime.Enabled = false;
            textBoxModifierObjectif.Enabled = false;
        }

        private void radioButtonLoué_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void listBoxPrime_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBoxNom_TextChanged(object sender, EventArgs e)
        {

        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void label41_Click(object sender, EventArgs e)
        {

        }

        private void textBoxModifierObjectif_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonModifierMaster_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonAfficherDateEmbauche_Click(object sender, EventArgs e)
        {
            // les responsables affichent les statistiques des dates d'embauche des employés
            BD maBD = new BD();

            // si on choisit du plus ancien au plus récent
            if (radioButtonDateEmbaucheCroissant.Checked)
            {
                // créer une liste pour stocker les résultats
                List<string> resultats = maBD.ClasserEmployesParDateCroissant();

                // Vider la ListBox avant d'ajouter
                listBoxDateEmbauche.Items.Clear();

                // lire la liste et ajouter chaque ligne à la ListBox
                foreach (string ligne in resultats)

                {
                    listBoxDateEmbauche.Items.Add(ligne);
                }
            }

            // si on choisit du plus récent au plus ancien
            else if (radioButtonDateEmbaucheDecroissant.Checked)
            {
                // créer une liste pour stocker les résultats
                List<string> resultats = maBD.ClasserEmployesParDateDecroissant();
                // Vider la ListBox avant d'ajouter
                listBoxDateEmbauche.Items.Clear();
                // lire la liste et ajouter chaque ligne à la ListBox
                foreach (string ligne in resultats)
                {
                    listBoxDateEmbauche.Items.Add(ligne);
                }
            }

        }

        // bouton pour se déconnecter
        private void buttonDeconnecter_Click(object sender, EventArgs e)
        {
            

            // On a accès seulement à l'onglet de connexion
            for (int i = 1; i < tabControl1.TabPages.Count; i++)
            {
                tabControl1.TabPages[i].Enabled = false;
            }

            // Aller vers le premier onglet
            tabControl1.SelectedIndex = 0;

            MessageBox.Show("Vous êtes déconnecté");

            // Vider touts les controles
            ViderControles(tabControl1);


        }


        private void buttonSupprimerPoste_Click(object sender, EventArgs e)
        {
            BD maBD = new BD();

            if (ComboBoxPoste.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un matériel !");
                return;
            }

            int idMateriel = Convert.ToInt32(ComboBoxPoste.SelectedItem);

            // MessageBox de confirmation
            DialogResult result = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer ce matériel ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                maBD.SupprimerMateriel(idMateriel);
                MessageBox.Show("Le matériel a bien été supprimé.");

                comboBoxMateriel.Items.Clear();
                Actualiser();
            }

            else
            {
                MessageBox.Show("Suppression annulée.");
            }
        }

        // Cette fonction va afficher les infos du matériel sélectionné
        private void buttonInfosMateriel_Click(object sender, EventArgs e)
        {

            BD maBD = new BD();
            int idMateriel = Convert.ToInt32(ComboBoxPoste.SelectedItem);

            // On récupére les infos du matériel
            string infos = maBD.InfosMateriel(idMateriel);

            // On affiche les infos dans la listbox
            string[] lignes = infos.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            listBoxInfosMat.Items.Clear(); // vider avant d'ajouter
            foreach (string ligne in lignes)
            {
                listBoxInfosMat.Items.Add(ligne);
            }
        }

        private void label44_Click(object sender, EventArgs e)
        {

        }

        // Quand on choisi de modifier les utilisateurs ont désactive les techniciens
        private void comboBoxModifierEmployesUtilisateur_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Désactiver les champs techniciens
            radioButtonModifierBac.Enabled = false;
            radioButtonModifierBTS.Enabled = false;
            radioButtonModifierMaster.Enabled = false;

            // Activer les textbox utilisateur
            textBoxModifierPrime.Enabled = true;
            textBoxModifierObjectif.Enabled = true;
        }

        // Ce bouton affiche le temps moyen de résolution des incidents
        private void buttonTempsMoyenDeResolution_Click(object sender, EventArgs e)
        {
            BD maBD = new BD();



                // créer une liste pour stocker les résultats
                List<string> resultats = maBD.TempsMoyenResolutionParTechnicien();

                // Vider la ListBox avant d'ajouter
                listBoxTempsMoyenRésolution.Items.Clear();

                // lire la liste et ajouter chaque ligne à la ListBox
                foreach (string ligne in resultats)

                {
                listBoxTempsMoyenRésolution.Items.Add(ligne);
                }
            }

        }
}
