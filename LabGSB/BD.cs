/// Gére le hashage des mots de passe
using BCrypt.Net;
/// Gére l'interaction avec la base de donnée
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabGSB
{
    /// <Résumé>
    ///  Cette classe va gérer l'interaction avec la base de donnée
    /// </Résumé>

    internal class BD
    {
        public ResultatConnexion Connexion(string matricule, string motDePasse)
        {
            matricule = matricule.Trim();
            motDePasse = motDePasse.Trim();

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none; Charset=utf8mb4;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                SELECT Id_employes, Mot_de_passe, Role
                FROM employes
                WHERE TRIM(Matricule) = @matricule
            ";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@matricule", MySqlDbType.VarChar, 50).Value = matricule;

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                // Aucun employé avec ce matricule
                                return null;
                            }

                            int id = reader.GetInt32("Id_employes");
                            string hashBD = reader.GetString("Mot_de_passe");
                            string role = reader.GetString("Role");

                            // Vérification du mot de passe hashé
                            if (!BCrypt.Net.BCrypt.Verify(motDePasse, hashBD))
                            {
                                return null;
                            }

                            // Retourner l'objet contenant l'ID + rôle
                            return new ResultatConnexion
                            {
                                IdEmploye = id,
                                Role = role
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion à la base de données :\n" + ex.Message,
                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }



        // Cette fonction va récupérer l'id d'un employé à partir de son matricule
        public int RecupererIdEmployeParMatricule(string matricule)
        {

            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            {
                cmd.CommandText = "SELECT Id_employes FROM employes WHERE Matricule = @matricule;";
                cmd.Parameters.AddWithValue("@matricule", matricule);

                // Récupération de l'ID généré
                object resultat = cmd.ExecuteScalar();

                // Si il y a un employés on renvoi l'id
                if (resultat != null && resultat != DBNull.Value)
                {
                    return Convert.ToInt32(resultat);
                }

                // Si aucun employé trouvé, on renvoit 0
                else
                {
                    // Retourne null si aucun employé trouvé
                    return 0;
                }

                conn.Close();
            }
        }


        // Cette requête va insérer un nouvel employé dans la base de donnée
        public int InsererEmployer(CEmployes unEmploye)
        {
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            string MdpSecurise = BCrypt.Net.BCrypt.HashPassword(unEmploye.getMdpE());

            MySqlCommand cmd = conn.CreateCommand();
            {
                cmd.CommandText = "INSERT INTO employes(Matricule, Nom, Prenom, Date_Embauche, Regions, Mot_de_passe, Role) " +
                                  "VALUES (@Matricule, @Nom, @Prenom, @Date_embauche, @Regions, @Mot_de_passe, @Role); " +
                                  "SELECT LAST_INSERT_ID();";

                cmd.Parameters.AddWithValue("@Matricule", unEmploye.getMatriculeE());
                cmd.Parameters.AddWithValue("@Nom", unEmploye.getNomE());
                cmd.Parameters.AddWithValue("@Prenom", unEmploye.getPrenomE());
                cmd.Parameters.AddWithValue("@Date_embauche", unEmploye.getDateEmbaucheE());
                cmd.Parameters.AddWithValue("@Regions", unEmploye.getRegionsE());
                cmd.Parameters.AddWithValue("@Mot_de_passe", MdpSecurise);
                cmd.Parameters.AddWithValue("@Role", unEmploye.getRoleE());

                // Récupération de l'ID généré
                int id = Convert.ToInt32(cmd.ExecuteScalar());

                conn.Close();

                return id;
            }
        }

        // Insert pour un nouveau technicien
        public void InsertTechniciens(CTechniciens unTechniciens, int idEmploye)
        {
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            {
                cmd.CommandText = "INSERT INTO technicien (Niveau_formation, Id_employes) " +
                                  "VALUES (@Niveau, @id_employes)";

                cmd.Parameters.AddWithValue("@Niveau", unTechniciens.getNivFormation());
                cmd.Parameters.AddWithValue("@id_employes", idEmploye);

                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }

        //Insert pour un nouvel utilisateur
        public void InsererUtilisateur(CUtilisateurs unUtilisateurs, int idEmploye)
        {
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            {
                cmd.CommandText = "INSERT INTO utilisateurs (Objectif, Prime, Id_employes) " +
                                  "VALUES (@Objectif, @Prime, @Id_employes)";

                cmd.Parameters.AddWithValue("@Objectif", unUtilisateurs.getUtilisateurObjectif());
                cmd.Parameters.AddWithValue("@Prime", unUtilisateurs.getUtilisateurPrime());
                cmd.Parameters.AddWithValue("@Id_employes", idEmploye);

                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }



        // Insert pour du nouveau matériel
        public void InsererMateriel(CMateriels unMateriel)
        {
            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            {
                // On écrit le texte de la commande
                cmd.CommandText = "INSERT INTO materiel(Processeur, Memoire, Disque, Logiciel_, Fournisseur, Date_achat_ou_loc, Garantie, Acheter, Id_employes) VALUES (@Processeur, @Memoire, @Disque, @Logiciel_, @Fournisseur, @Date_achat_ou_loc, @Garantie, @Acheter, @Id_employes)";

                // on récupére ce qu'il nous faut avec la méthode get
                cmd.Parameters.AddWithValue("@Processeur", unMateriel.getProcesseur());
                cmd.Parameters.AddWithValue("@Memoire", unMateriel.getMemoire());
                cmd.Parameters.AddWithValue("@Disque", unMateriel.getDisque());
                cmd.Parameters.AddWithValue("@Logiciel_", unMateriel.getLogiciel());
                cmd.Parameters.AddWithValue("@Fournisseur", unMateriel.getFournisseur());
                cmd.Parameters.AddWithValue("@Date_achat_ou_loc", unMateriel.getdate_achat_loc());
                cmd.Parameters.AddWithValue("@Garantie", unMateriel.getGarantie());
                cmd.Parameters.AddWithValue("@Acheter", unMateriel.getAchat());
                cmd.Parameters.AddWithValue("@Id_employes", unMateriel.getId_e());

                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }

        // Insert pour un nouvel incident
        public void InsererIncident(CIncidents unIncidents)
        {
            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            {
                // On écrit le texte de la commande
                cmd.CommandText = "INSERT INTO incident(Etat, Date_, HeureDeb, HeureFin, Descr, Type_priseen_charge, id_technicien, id_utilisateur, id_materiel) VALUES (@Etat, @Date_, @HeureDeb, @HeureFin, @Descr, @Type_priseen_charge, @id_technicien, @id_utilisateur, @id_materiel)";

                // on récupére ce qu'il nous faut avec la méthode get
                cmd.Parameters.AddWithValue("@Etat", unIncidents.getEtat());
                cmd.Parameters.AddWithValue("@Date_", unIncidents.getDate());
                cmd.Parameters.AddWithValue("@HeureDeb", unIncidents.getHeureDeb());
                cmd.Parameters.AddWithValue("@HeureFin", unIncidents.getHeureFin());
                cmd.Parameters.AddWithValue("@Descr", unIncidents.getDescr());
                cmd.Parameters.AddWithValue("@Type_priseen_charge", unIncidents.getTypeTravail());
                cmd.Parameters.AddWithValue("@id_technicien", unIncidents.getId_tech());
                cmd.Parameters.AddWithValue("@id_utilisateur", unIncidents.getId_uti());
                cmd.Parameters.AddWithValue("@id_materiel", unIncidents.getId_mat());
                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }

        // Cette fonction va supprimer un employé de la base de donnée
        public void SupprimerEmploye(string Matricule)
        {
            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            {
                // On écrit le texte de la commande
                cmd.CommandText = "DELETE  FROM employes WHERE Matricule = @Matricule";

                // on récupére ce qu'il nous faut avec la méthode get ( ici l'id de l'employé )
                cmd.Parameters.AddWithValue("@Matricule", Matricule);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        // Cette fonction va supprimer un utilisateur de la base de donnée
        public void SupprimerUtilisateur(int IdEmployes)
        {
            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // D'abord on supprime l'utilisateur de la table utilisateurs
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.CommandText = "DELETE FROM utilisateurs WHERE Id_employes = @Id_employes";
                    cmd.Parameters.AddWithValue("@Id_employes", IdEmployes);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    // puis l'employé de la table employes
                    MySqlCommand cmd2 = new MySqlCommand();
                    cmd2.Connection = conn;
                    cmd2.Transaction = transaction;
                    cmd2.CommandText = "DELETE FROM employes WHERE Id_employes = @Id_employes";
                    cmd2.Parameters.AddWithValue("@Id_employes", IdEmployes);
                    cmd2.ExecuteNonQuery();
                    cmd2.Parameters.Clear();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Erreur lors de la suppression : " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion à la base : " + ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }



        // Cette fonction va supprimer un technicien de la base de donnée
        public void SupprimerTechinicien(int IdEmployes)
        {
            // D'abord on supprime le technicien de la table technicien
            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();


            MySqlCommand cmd = conn.CreateCommand();
            {
                // On écrit le texte de la commande
                cmd.CommandText = "DELETE  FROM technicien WHERE Id_employes = @Id_employes";

                // on supprime ce qu'il nous faut avec la méthode get ( ici l'id de l'employé )
                cmd.Parameters.AddWithValue("@Id_employes", IdEmployes);
                cmd.ExecuteNonQuery();
            }

            // Ensuite on supprime l'employé de la table employes
            MySqlCommand cmd2 = conn.CreateCommand();
            {
                // On écrit le texte de la commande
                cmd2.CommandText = "DELETE FROM employes WHERE Id_employes = @Id_employes";

                // on supprime ce qu'il nous faut avec la méthode get ( ici l'id de l'employé )
                cmd2.Parameters.AddWithValue("@Id_employes", IdEmployes);
                cmd2.ExecuteNonQuery();
            }

            conn.Close();
        }

        // Cette fonction va supprimer un responsable de la base de donnée
        public void SupprimerResponsable(int IdEmployes)
        {
            // D'abord on supprime le responsable de la table responsable
            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();


            MySqlCommand cmd = conn.CreateCommand();
            {
                // On écrit le texte de la commande
                cmd.CommandText = "DELETE  FROM responsables WHERE Id_employes = @Id_employes";

                // on supprime ce qu'il nous faut avec la méthode get ( ici l'id de l'employé )
                cmd.Parameters.AddWithValue("@Id_employes", IdEmployes);
                cmd.ExecuteNonQuery();
            }

            // Ensuite on supprime l'employé de la table employes
            MySqlCommand cmd2 = conn.CreateCommand();
            {
                // On écrit le texte de la commande
                cmd2.CommandText = "DELETE FROM employes WHERE Id_employes = @Id_employes";

                // on supprime ce qu'il nous faut avec la méthode get ( ici l'id de l'employé )
                cmd2.Parameters.AddWithValue("@Id_employes", IdEmployes);
                cmd2.ExecuteNonQuery();
            }

            conn.Close();
        }

        // Cette fonction va supprimer un matériel de la base de donnée
        public void SupprimerMateriel(int idMateriel)
        {
            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            {
                // On écrit le texte de la commande
                cmd.CommandText = "DELETE  FROM materiel WHERE Id_materiel = @Id_materiel";

                // on récupére ce qu'il nous faut.
                cmd.Parameters.AddWithValue("@Id_materiel", idMateriel);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }


        // Cette fonction va modifier un incident quand il est pris en charge par un technicien
        public void ModifierIncidentPrendreEncharge(int Id_incident, int Id_technicien, string typePriseEncharge)
        {
            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;


            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            {
                // On écrit le texte de la commande
                cmd.CommandText = "UPDATE incident SET Id_technicien =  @Id_technicien, Etat = @Etat, Type_priseen_charge = @typePriseEncharge WHERE Id_incident = @Id_incident";

                cmd.Parameters.AddWithValue("@Id_technicien", Id_technicien);
                cmd.Parameters.AddWithValue("@Etat", "En cours");
                cmd.Parameters.AddWithValue("@typePriseEncharge", typePriseEncharge);
                cmd.Parameters.AddWithValue("@Id_incident", Id_incident);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        // Cette fonction va modifier un incident quand il est terminé par un technicien
        public void ModifierIncidentResoudre(int Id_incident, string descriptionTravail, DateTime HeureFin)
        {
            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            {
                // On écrit le texte de la commande
                cmd.CommandText = "UPDATE incident SET Etat = @Etat, Descr = @descriptionTravail, HeureFin =  @HeureFin WHERE Id_incident = @Id_incident";

                // on récupére ce qu'il nous f
                cmd.Parameters.AddWithValue("@Etat", "Résolu");
                cmd.Parameters.AddWithValue("@descriptionTravail", descriptionTravail);
                cmd.Parameters.AddWithValue("@Id_incident", Id_incident);
                cmd.Parameters.AddWithValue("@HeureFin", HeureFin);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        // Cette fonction va modifier un employés
        public bool ModifierEmploye(CEmployes unEmploye)
        {
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();

                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"UPDATE employes
                            SET Matricule = @Matricule,
                                Nom = @Nom,
                                Prenom = @Prenom,
                                Date_Embauche = @Date_embauche,
                                Regions = @Regions,
                                Role = @Role
                            WHERE Id_employes = @Id_employes";

                // on récupére ce qu'il nous faut avec la méthode get
                cmd.Parameters.AddWithValue("@Id_employes", unEmploye.getIdEmploye());
                cmd.Parameters.AddWithValue("@Matricule", unEmploye.getMatriculeE());
                cmd.Parameters.AddWithValue("@Nom", unEmploye.getNomE());
                cmd.Parameters.AddWithValue("@Prenom", unEmploye.getPrenomE());
                cmd.Parameters.AddWithValue("@Date_embauche", unEmploye.getDateEmbaucheE());
                cmd.Parameters.AddWithValue("@Regions", unEmploye.getRegionsE());
                cmd.Parameters.AddWithValue("@Role", unEmploye.getRoleE());

                int lignesAffectees = cmd.ExecuteNonQuery();
                conn.Close();

                return lignesAffectees > 0;
            }

            // Gestion des exceptions
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la modification : " + ex.Message);
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                return false;
            }
        }


        // Cette fonction va modifier les informations d'un utilisateur
        public void ModifierUtilisateur(CUtilisateurs unUtilisateurs, int idEmploye)
        {
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            {
                cmd.CommandText = @"UPDATE utilisateurs
                            SET Objectif = @Objectif,
                                Prime = @Prime
                            WHERE Id_employes = @Id_employes";

                cmd.Parameters.AddWithValue("@Objectif", unUtilisateurs.getUtilisateurObjectif());
                cmd.Parameters.AddWithValue("@Prime", unUtilisateurs.getUtilisateurPrime());
                cmd.Parameters.AddWithValue("@Id_employes", idEmploye);

                cmd.ExecuteNonQuery();
            }

            conn.Close();

        }

        // Cette fonction va modifier les informations d'un techinicien
        public void ModifierTechnicien(CTechniciens unTechniciens, int idEmploye)
        {
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            {
                cmd.CommandText = @"UPDATE technicien
                            SET Niveau_formation = @Niveau_formation
                            WHERE Id_employes = @Id_employes";

                cmd.Parameters.AddWithValue("@Niveau_formation", unTechniciens.getNivFormation());
                cmd.Parameters.AddWithValue("@Id_employes", idEmploye);

                cmd.ExecuteNonQuery();
            }

            conn.Close();

        }

        // Cette fonction va créer une liste avec les employes qui on le rôle technicien
        public List<string> RecupererMatriculeTechniciens()
        {
            // On crée une liste pour stocker les matricules des employés
            List<string> listeMatriculeTechniciens = new List<string>();

            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            string requete = "SELECT Matricule FROM employes WHERE Role = 'Technicien'";
            MySqlCommand cmd = new MySqlCommand(requete, conn);

            // Exécution de la commande 
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // on ajoute chaque matricule à la liste
                    listeMatriculeTechniciens.Add(reader["Matricule"].ToString());
                }
            }

            conn.Close();

            return listeMatriculeTechniciens;
        }

        // Cette fonction va créer une liste avec les employes qui on le rôle utilisateur
        public List<string> RecupererMatriculeUtilisateurs()
        {
            // On crée une liste pour stocker les matricules des employés
            List<string> listeMatriculeUtilisateeurs = new List<string>();

            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            string requete = "SELECT Matricule FROM employes WHERE Role = 'Utilisateur'";
            MySqlCommand cmd = new MySqlCommand(requete, conn);

            // Exécution de la commande 
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // on ajoute chaque matricule à la liste
                    listeMatriculeUtilisateeurs.Add(reader["Matricule"].ToString());
                }
            }

            conn.Close();

            return listeMatriculeUtilisateeurs;
        }


        // Cette fonction va créer une liste avec les employes sans ordinateur on pourra l'utiliser pour les combo box
        public List<string> RecupererMatriculeEmployesSansOrdinateur()
        {
            // On crée une liste pour stocker les matricules des employés
            List<string> listeMatricule_employes = new List<string>();

            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            string requete = "SELECT e.Matricule FROM employes e WHERE e.Id_employes NOT IN (SELECT m.Id_employes FROM materiel m)";
            MySqlCommand cmd = new MySqlCommand(requete, conn);

            // Exécution de la commande 
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // on ajoute chaque matricule à la liste
                    listeMatricule_employes.Add(reader["Matricule"].ToString());
                }
            }

            conn.Close();

            return listeMatricule_employes;
        }

        // Cette fonction va créer une liste avec les id du matériel on pourra l'utiliser pour les combo box
        public List<int> RecupererIdMateriel()
        {
            // On crée une liste pour stocker les id du matériel
            List<int> listeId_materiel = new List<int>();

            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            string requete = "SELECT Id_materiel FROM materiel";
            MySqlCommand cmd = new MySqlCommand(requete, conn);

            // Exécution de la commande 
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // on ajoute chaque matériel à la liste
                    listeId_materiel.Add(reader.GetInt32("Id_materiel"));
                }
            }

            conn.Close();

            return listeId_materiel;
        }

        // Cette fonction va créer une liste avec les id des incidents non résolus on pourra l'utiliser pour les combo box
        public List<int> RecupererIdIncidentsNonResolus()
        {
            // On crée une liste pour stocker les id des incidents non résolus
            List<int> listeIdIncidentsNonResolus = new List<int>();

            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            string requete = "SELECT Id_incident FROM incident WHERE etat = 'En attente'";
            MySqlCommand cmd = new MySqlCommand(requete, conn);

            // Exécution de la commande 
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // on ajoute chaque matériel à la liste
                    listeIdIncidentsNonResolus.Add(reader.GetInt32("Id_incident"));
                }
            }

            conn.Close();

            return listeIdIncidentsNonResolus;
        }

        // Cette fonction va créer une liste avec les id des incidents en cours on pourra l'utiliser pour les combo box
        public List<int> RecupererIdIncidentsEnCours()
        {
            // On crée une liste pour stocker les id des incidents non résolus
            List<int> listeIdIncidentsEnCours = new List<int>();

            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            string requete = "SELECT Id_incident FROM incident WHERE Etat = 'En cours'";
            MySqlCommand cmd = new MySqlCommand(requete, conn);

            // Exécution de la commande 
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // on ajoute chaque matériel à la liste
                    listeIdIncidentsEnCours.Add(reader.GetInt32("Id_incident"));
                }
            }

            conn.Close();

            return listeIdIncidentsEnCours;
        }


        // Cette fonction va créer une liste avec les informations des incidents en attente on pourra l'utiliser pour les combo box
        public List<String> RecupererIncidentsNonResolues()
        {
            // On crée une liste pour stocker les informations des utilisateurs
            List<string> listeIncidentsNonPrisEnCharge = new List<string>();

            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            // On crée la requête pour récupérer les informations de l'incident
            string requete = "SELECT Id_incident, Descr, HeureDeb, HeureFin FROM incident WHERE Etat = 'En attente'";
            MySqlCommand cmd = new MySqlCommand(requete, conn);

            // On crée une liste pour stocker les informations de l'incident
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string infos = $"--- Incidents non pris en charge ---\n" +
                                   $"Id de l'incident : {reader["Id_incident"]} \n" +
                                   $"Description de l'incident : {reader["Descr"]}\n" +
                                   $"Incident déclarée à : {reader["HeureDeb"]} \n" +
                                   $"Incident résolu à : {reader["HeureFin"]} \n";

                    listeIncidentsNonPrisEnCharge.Add(infos);
                }
            }

            conn.Close();

            return listeIncidentsNonPrisEnCharge;
        }

        // Cette fonction va créer une liste qui va classer les utilisateurs par prime ( décroissant ) ( utile pour les statistiques )
        public List<string> ClasserUtilisateursParPrimeDecroissant()
        {
            // On crée une liste pour stocker les informations des utilisateurs
            List<string> listeUtilisateurs = new List<string>();

            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            string requete = "SELECT e.Nom, e.Prenom, u.Prime FROM employes e, utilisateurs u WHERE e.Id_employes = u.Id_employes\r\nORDER BY u.Prime DESC;";
            MySqlCommand cmd = new MySqlCommand(requete, conn);

            // Exécution de la commande 
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // on ajoute les informations pour la liste
                    string nom = reader.GetString("Nom");
                    string prenom = reader.GetString("Prenom");
                    decimal prime = reader.GetDecimal("Prime");

                    listeUtilisateurs.Add($"{nom} {prenom} a une prime de {prime}€");
                }
            }

            conn.Close();

            return listeUtilisateurs;
        }
        // Cette fonction va créer une liste qui va classer les utilisateurs par prime ( croissant ) ( utile pour les statistiques )
        public List<string> ClasserUtilisateursParPrimeCroissant()
        {
            // On crée une liste pour stocker les informations des utilisateurs
            List<string> listeUtilisateurs = new List<string>();

            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            string requete = "SELECT e.Nom, e.Prenom, u.Prime\r\nFROM employes e, utilisateurs u\r\nWHERE e.Id_employes = u.Id_employes\r\nORDER BY u.Prime ASC;";
            MySqlCommand cmd = new MySqlCommand(requete, conn);

            // Exécution de la commande 
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // on ajoute les informations pour la liste
                    string nom = reader.GetString("Nom");
                    string prenom = reader.GetString("Prenom");
                    decimal prime = reader.GetDecimal("Prime");

                    listeUtilisateurs.Add($"{nom} {prenom} a une prime de {prime}€");
                }
            }

            conn.Close();

            return listeUtilisateurs;
        }

        // Cette fonction va créer une liste qui va afficher le temps moyen de résolution des incidents par technicien
        public List<string> TempsMoyenResolutionParTechnicien()
        {
        List<string> listeTechniciens = new List<string>();

        // une méthode de connexion à la base de donnée projet_bd
        MySqlConnection conn;

        string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
        conn = new MySqlConnection(connStr);

        conn.Open();

        string requete =
            "SELECT e.Nom, e.Prenom, " +
            "SEC_TO_TIME(AVG(TIMESTAMPDIFF(SECOND, i.HeureDeb, i.HeureFin))) AS TempsMoyen " +
            "FROM incident i, technicien t, employes e " +
            "WHERE e.Id_employes = t.Id_employes " +
            "AND t.Id_employes = i.Id_technicien " +
            "AND i.Etat = 'Résolu' " +
            "AND i.HeureDeb IS NOT NULL " +
            "AND i.HeureFin IS NOT NULL " +
            "GROUP BY t.Id_technicien;";

        MySqlCommand cmd = new MySqlCommand(requete, conn);

        // Exécution de la commande 
        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                // on ajoute les informations pour la liste
                string nom = reader.GetString("Nom");
                string prenom = reader.GetString("Prenom");
                string tempsMoyen = reader.GetString("TempsMoyen");

                listeTechniciens.Add($"{nom} {prenom} a un temps moyen de résolution de {tempsMoyen}");
            }
        }

        conn.Close();

        return listeTechniciens;
    }


        // Cette fonction va créer une liste qui va classer les employés par date d'embauche ( du plus ancien au plus récent )
        public List<string> ClasserEmployesParDateCroissant()
        {
            // On crée une liste pour stocker les informations des utilisateurs
            List<string> listeEmployes = new List<string>();

            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            string requete = "SELECT e.Nom, e.Prenom, e.Date_embauche FROM employes e ORDER BY e.Date_embauche ASC;";
            MySqlCommand cmd = new MySqlCommand(requete, conn);

            // Exécution de la commande 
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // on ajoute les informations pour la liste
                    string nom = reader.GetString("Nom");
                    string prenom = reader.GetString("Prenom");
                    DateTime date = reader.GetDateTime("Date_embauche");

                    listeEmployes.Add($"{nom} {prenom} a était embauché le {date}");
                }
            }

            conn.Close();

            // retour de notre liste d'employés
            return listeEmployes;
        }

        // Cette fonction va créer une liste qui va classer les employés par date d'embauche ( du plus récent au plus ancien )
        public List<string> ClasserEmployesParDateDecroissant()
        {
            // On crée une liste pour stocker les informations des utilisateurs
            List<string> listeEmployes = new List<string>();

            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            string requete = "SELECT e.Nom, e.Prenom, e.Date_embauche FROM employes e ORDER BY e.Date_embauche DESC;";
            MySqlCommand cmd = new MySqlCommand(requete, conn);

            // Exécution de la commande 
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // on ajoute les informations pour la liste
                    string nom = reader.GetString("Nom");
                    string prenom = reader.GetString("Prenom");
                    DateTime date = reader.GetDateTime("Date_embauche");

                    listeEmployes.Add($"{nom} {prenom} a était embauché le {date}");
                }
            }

            conn.Close();

            // retour de notre liste d'employés
            return listeEmployes;
        }

        // Cette fonction va récupérer les informations d'un matériel à partir de son id
        public string InfosMateriel(int idMateriel)
        {

            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            // On crée la requête pour récupérer les informations du matériel
            string requete = "SELECT * FROM materiel WHERE Id_materiel = @Id_materiel";
            MySqlCommand cmd = new MySqlCommand(requete, conn);
            cmd.Parameters.AddWithValue("@Id_materiel", idMateriel);

            // On crée une liste pour stocker les informations du matériel
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string infos = $"--- Informations sur le matériel ---\n" +
                                   $"Processeur : {reader["Processeur"]}\n" +
                                   $"Mémoire : {reader["Memoire"]}\n" +
                                   $"Disque : {reader["Disque"]}\n" +
                                   $"Logiciels : {reader["Logiciel_"]}\n" +
                                   $"Fournisseur : {reader["Fournisseur"]}\n" +
                                   $"Date d'achat/location : {Convert.ToDateTime(reader["Date_achat_ou_loc"]):yyyy-MM-dd}\n" +
                                   $"Garantie : {((bool)reader["Garantie"] ? "Oui" : "Non")}\n" +
                                   $"Achat : {((bool)reader["Acheter"] ? "Oui" : "Non")}\n" +
                                   $"ID Employé : {reader["Id_employes"]}";

                    // Retourne les informations du matériel
                    return infos;
                }
            }
            // Si il n'y a pas de matériel avec cet id on retourne un message d'erreur
            return "Aucun matériel trouvé pour cet ID.";
        }

        // Cette fonction va suivre l'avencement de l'incident selon l'id de la personne qui l'a crée
        public string SuiviIncidentParIdUtilisateur(int idUtilisateur)
        {
            // une méthode de connexion à la base de donnée projet_bd
            MySqlConnection conn;

            string connStr = "Server=127.0.0.1; Database=projet_bd; Uid=root; Password=; SslMode=none;";
            conn = new MySqlConnection(connStr);

            conn.Open();

            // On crée la requête pour récupérer les informations de l'incident
            string requete = "SELECT Etat, Type_priseen_charge, HeureDeb, HeureFin FROM incident WHERE Id_utilisateur = @idUtilisateur";
            MySqlCommand cmd = new MySqlCommand(requete, conn);
            cmd.Parameters.AddWithValue("@idUtilisateur", idUtilisateur);

            // On crée une liste pour stocker les informations de l'incident
            StringBuilder infos = new StringBuilder();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                int compteur = 1;

                while (reader.Read())
                {
                    infos.AppendLine($"--- Incident n°{compteur} ---");
                    infos.AppendLine($"Etat de l'incident : {reader["Etat"]}");
                    infos.AppendLine($"Type de prise en charge : {reader["Type_priseen_charge"]}");
                    infos.AppendLine($"Incident déclaré à : {reader["HeureDeb"]}");
                    infos.AppendLine($"Incident résolu à : {reader["HeureFin"]}");
                    infos.AppendLine("");

                    compteur++;
                }


                conn.Close();

                if (infos.Length == 0)
                    return "Aucun incident trouvé pour cet utilisateur.";

                return infos.ToString();
            }
        }
    }
}