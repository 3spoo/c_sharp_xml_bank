using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleAppBank
{
    internal class Program
    {
        const string FILE_REGIONI = "REGIONI.xml";
        const string FILE_FILIALI = "FILIALI.xml";
        const string FILE_CLIENTI = "CLIENTI.xml";
        const string FILE_CONTI = "CONTI.xml";
        const string FILE_CONTI_INTESTATARI = "CONTI_INTESTATARI.xml";
        const string FILE_MOVIMENTI = "MOVIMENTI.xml";

        static void Main(string[] args)
        {
            // RICHIESTA N.1
            XmlDocument xd_movivemti = new XmlDocument();
            xd_movivemti.Load(FILE_MOVIMENTI);

            Dictionary<string, string> movimento = getMovimentoDiImportoMaggiore(xd_movivemti);
            Console.WriteLine("RICHIESTA 1: \n" + movimento["DATA"] + ", " + movimento["ORARIO"] + ", " + movimento["IBAN"] + ", " + movimento["IMPORTO"] + ", " + movimento["CAUSALE"] + ", " + movimento["CANALE"]);

            // RICHIESTA N.2
            XmlDocument xd_conti = new XmlDocument();
            xd_conti.Load(FILE_CONTI);

            XmlDocument xd_filiali = new XmlDocument();
            xd_filiali.Load(FILE_FILIALI);

            movimento = getContoAndFiliale(xd_conti, xd_filiali, movimento);
            Console.WriteLine("RICHIESTA 2: \n" + movimento["TIPO_CONTO"] + ", " + movimento["DENOMINAZIONE"] + ", " + movimento["SIGLA_PROVINCIA"]);

            // RICHIESTA N. 3
            XmlDocument xd_regioni = new XmlDocument();
            xd_regioni.Load(FILE_REGIONI);

            movimento = getRegione(xd_regioni, movimento);
            Console.WriteLine("RICHIESTA 3: \n" + movimento["NOME_REGIONE"]);

            // RICHIESTA N. 4
            XmlDocument xd_intestatari = new XmlDocument();
            xd_intestatari.Load(FILE_CONTI_INTESTATARI);

            XmlDocument xd_client = new XmlDocument();
            xd_client.Load(FILE_CLIENTI);

            List<Dictionary<string, string>> intestatari = getIntestatari(xd_intestatari, xd_client, movimento);
            Console.WriteLine("RICHIESTA 4: ");
            foreach(Dictionary<string, string> tmp in intestatari)
            {
                Console.WriteLine(tmp["NOME"] + ", " + tmp["COGNOME"] + ", " + tmp["ETA"] + ", " + tmp["RUOLO"]);
            }
        }

        public static int getEtaByString(string data, string formato = "yyyy-MM-dd")
        {
            if (!DateTime.TryParseExact(data, formato, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataNascita))
            {
                return -1;
            }

            DateTime oggi = DateTime.Today;
            int eta = oggi.Year - dataNascita.Year;

            if (dataNascita.Date > oggi.AddYears(-eta))
            {
                eta--;
            }

            return eta;
        }

        public static Dictionary<string, string> getMovimentoDiImportoMaggiore(XmlDocument xd)
        {
            Dictionary<string, string> movimento_max = new Dictionary<string, string>();
            decimal importo_max = 0;

            foreach (XmlNode movimento in xd.DocumentElement.ChildNodes)
            {
                if (Convert.ToDecimal(movimento["IMPORTO"].InnerText) > importo_max)
                {
                    importo_max = Convert.ToDecimal(movimento["IMPORTO"].InnerText);

                    movimento_max = new Dictionary<string, string>
                    {
                        {"DATA",  movimento["DATA"].InnerText},
                        {"ORARIO",  movimento["ORARIO"].InnerText},
                        {"IBAN",  movimento["IBAN"].InnerText},
                        {"IMPORTO",  movimento["IMPORTO"].InnerText},
                        {"CAUSALE",  movimento["CAUSALE"].InnerText},
                        {"CANALE",  movimento["CANALE"].InnerText},
                    };
                }
            }

            return movimento_max;
        }

        public static Dictionary<string, string> getContoAndFiliale(XmlDocument xd_conti, XmlDocument xd_filiali, Dictionary<string, string> movimento)
        {
            string id = "_";

            foreach (XmlNode conto in xd_conti.DocumentElement.ChildNodes)
            {
                if (conto["IBAN"].InnerText.Equals(movimento["IBAN"]))
                {
                    movimento.Add("TIPO_CONTO", conto["TIPO_CONTO"].InnerText);
                    id = conto["ID_FILIALE"].InnerText;
                    break;
                }
            }

            foreach (XmlNode filiale in xd_filiali.DocumentElement.ChildNodes)
            {
                if (filiale["ID_FILIALE"].InnerText.Equals(id))
                {
                    movimento.Add("DENOMINAZIONE", filiale["DENOMINAZIONE"].InnerText);
                    movimento.Add("SIGLA_PROVINCIA", filiale["SIGLA_PROVINCIA"].InnerText);
                    movimento.Add("CODICE_REGIONE", filiale["CODICE_REGIONE"].InnerText);
                    break;
                }
            }

            return movimento;
        }

        public static Dictionary<string, string> getRegione(XmlDocument xd_regione, Dictionary<string, string> movimento)
        {
            foreach (XmlNode regione in xd_regione.DocumentElement.ChildNodes)
            {
                if (regione["CODICE_REGIONE"].InnerText.Equals(movimento["CODICE_REGIONE"]))
                {
                    movimento.Add("NOME_REGIONE", regione["NOME_REGIONE"].InnerText);
                    break;
                }
            }

            return movimento;
        }

        public static List<Dictionary<string, string>> getIntestatari(XmlDocument xd_intestatari, XmlDocument xd_clienti, Dictionary<string, string> movimento)
        {
            List<Dictionary<string, string>> intestatari = 
                new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> tmp_intestatari = 
                new List<Dictionary<string, string>>();

            foreach (XmlNode intestatario in xd_intestatari.DocumentElement.ChildNodes)
            {
                if (intestatario["IBAN_CONTO"].InnerText.Equals(movimento["IBAN"]))
                {
                    tmp_intestatari.Add(new Dictionary<string, string>
                        {
                            {"ID_CLIENTE", intestatario["ID_CLIENTE"].InnerText},
                            {"RUOLO", intestatario["RUOLO"].InnerText},
                        });
                }
            }

            foreach (XmlNode cliente in xd_clienti.DocumentElement.ChildNodes)
            {
                foreach (Dictionary<string, string> tmp in tmp_intestatari)
                {
                    if (cliente["ID_CLIENTE"].InnerText.Equals(tmp["ID_CLIENTE"]))
                    {
                        intestatari.Add(new Dictionary<string, string>
                            {
                                {"NOME", cliente["NOME"].InnerText},
                                {"COGNOME", cliente["COGNOME"].InnerText},
                                {"ETA", Convert.ToString(getEtaByString(cliente["DATA_NASCITA"].InnerText))},
                                {"RUOLO", tmp["RUOLO"]},
                            });

                    }
                }
            }

            return intestatari;
        }
    }
}
