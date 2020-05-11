using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disp_WinForm
{
    class Kontragents_class
    {
        Macros macros = new Macros();
        public Kontragents_class()
        {
        }
        public Kontragents_class(
            int idKontragenti,
            string kontragenti_full_name,
            string kontragenti_short_name,
            int kontragenti_rekvezity,
            int kontragenti_user_creator,
            DateTime kontragenti_create_datetime,
            DateTime kontragenti_edit_datetime,
            string kontragenti_comment,
            string kontragenticol_kategory,
            int emails_idEmails,
            int phonebook_idPhonebook,
            int users_idUsers_manager,
            int emails_idEmails1,
            string kontragenticol_misto,
            string kontragenticol_vulitsa)
        {
            this.IdKontragenti = idKontragenti;
            this.Kontragenti_full_name = kontragenti_full_name;
            this.Kontragenti_short_name = kontragenti_short_name;
            this.Kontragenti_rekvezity = kontragenti_rekvezity;
            this.Kontragenti_user_creator = kontragenti_user_creator;
            this.Kontragenti_create_datetime = kontragenti_create_datetime;
            this.Kontragenti_edit_datetime = kontragenti_edit_datetime;
            this.Kontragenti_comment = kontragenti_comment;
            this.Kontragenticol_kategory = kontragenticol_kategory;
            this.Emails_idEmails = emails_idEmails;
            this.Phonebook_idPhonebook = phonebook_idPhonebook;
            this.Users_idUsers_manager = users_idUsers_manager;
            this.Emails_idEmails1 = emails_idEmails1;
            this.Kontragenticol_misto = kontragenticol_misto;
            this.Kontragenticol_vulitsa = kontragenticol_vulitsa;
        }
        public int IdKontragenti
        {
            get { return idKontragenti; }
            set { idKontragenti = value; }
        }
        private int idKontragenti;

        public string Kontragenti_full_name
        {
            get { return kontragenti_full_name; }
            set { kontragenti_full_name = value; }
        }
        private string kontragenti_full_name;

        public string Kontragenti_short_name
        {
            get { return kontragenti_short_name; }
            set { kontragenti_short_name = value; }
        }
        private string kontragenti_short_name;

        public int Kontragenti_rekvezity
        {
            get { return kontragenti_rekvezity; }
            set { kontragenti_rekvezity = value; }
        }
        private int kontragenti_rekvezity;

        public int Kontragenti_user_creator
        {
            get { return kontragenti_user_creator; }
            set { kontragenti_user_creator = value; }
        }
        private int kontragenti_user_creator;

        public DateTime Kontragenti_create_datetime
        {
            get { return kontragenti_create_datetime; }
            set { kontragenti_create_datetime = value; }
        }
        private DateTime kontragenti_create_datetime;

        public DateTime Kontragenti_edit_datetime
        {
            get { return kontragenti_edit_datetime; }
            set { kontragenti_edit_datetime = value; }
        }
        private DateTime kontragenti_edit_datetime;

        public string Kontragenti_comment
        {
            get { return kontragenti_comment; }
            set { kontragenti_comment = value; }
        }
        private string kontragenti_comment;

        public string Kontragenticol_kategory
        {
            get { return kontragenticol_kategory; }
            set { kontragenticol_kategory = value; }
        }
        private string kontragenticol_kategory;

        public int Emails_idEmails
        {
            get { return emails_idEmails; }
            set { emails_idEmails = value; }
        }
        private int emails_idEmails;

        public int Phonebook_idPhonebook
        {
            get { return phonebook_idPhonebook; }
            set { phonebook_idPhonebook = value; }
        }
        private int phonebook_idPhonebook;

        public int Users_idUsers_manager
        {
            get { return users_idUsers_manager; }
            set { users_idUsers_manager = value; }
        }
        private int users_idUsers_manager;

        public int Emails_idEmails1
        {
            get { return emails_idEmails1; }
            set { emails_idEmails1 = value; }
        }
        private int emails_idEmails1;

        public string Kontragenticol_misto
        {
            get { return kontragenticol_misto; }
            set { kontragenticol_misto = value; }
        }
        private string kontragenticol_misto;

        public string Kontragenticol_vulitsa
        {
            get { return kontragenticol_vulitsa; }
            set { kontragenticol_vulitsa = value; }
        }
        private string kontragenticol_vulitsa;

        static internal List<Kontragents_class> GetSongs()
        {
            if (Kontragents_class.AllSongs.Count == 0)
                Kontragents_class.AllSongs = Kontragents_class.InitializeKontragents();
            return Kontragents_class.AllSongs;
        }
        static private List<Kontragents_class> AllSongs = new List<Kontragents_class>();

        static private List<Kontragents_class> InitializeKontragents()
        {
            List<Kontragents_class> songs = new List<Kontragents_class>();
        


            songs.Add(new Kontragents_class(1, "АВТ Баварія", "АВТ Баварія", 1, 34, new DateTime(2007, 10, 21, 5, 42, 0), new DateTime(2007, 10, 21, 5, 42, 0), "", "Диллер/СТО", 1, 1, 34, 1, "Київ", "Semenivska"));
            songs.Add(new Kontragents_class(2, "АВТ Баварія", "АВТ Баварія", 1, 34, new DateTime(2007, 10, 21, 5, 42, 0), new DateTime(2007, 10, 21, 5, 42, 0), "", "Диллер/СТО", 1, 1, 34, 1, "Київ", "Semenivska"));
            return songs;
        }
    }
}
