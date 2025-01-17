using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OfferteMercatoLibero
{
    [XmlRoot(ElementName = "ListaOfferteMercatoLibero", Namespace = "http://www.acquirenteunico.it/schemas/SII_AU/OffertaRetail/01")]
    public class ListaOfferteMercatoLibero
    {
        [XmlElement(ElementName = "offerta")]
        public List<Offerta> Offerte { get; set; }
    }

    public class Offerta
    {
        [XmlElement(ElementName = "IdentificativiOfferta")]
        public IdentificativiOfferta IdentificativiOfferta { get; set; }

        [XmlElement(ElementName = "DettaglioOfferta")]
        public DettaglioOfferta DettaglioOfferta { get; set; }

        [XmlElement(ElementName = "RiferimentiPrezzoEnergia")]
        public RiferimentiPrezzoEnergia RiferimentiPrezzoEnergia { get; set; }

        [XmlElement(ElementName = "ValiditaOfferta")]
        public ValiditaOfferta ValiditaOfferta { get; set; }

        [XmlElement(ElementName = "MetodoPagamento")]
        public MetodoPagamento MetodoPagamento { get; set; }

        [XmlElement(ElementName = "TipoPrezzo")]
        public TipoPrezzo TipoPrezzo { get; set; }

        [XmlElement(ElementName = "Dispacciamento")]
        public List<Dispacciamento> Dispacciamenti { get; set; }

        [XmlElement(ElementName = "ComponenteImpresa")]
        public List<ComponenteImpresa> ComponentiImpresa { get; set; }

        [XmlElement(ElementName = "CondizioniContrattuali")]
        public CondizioniContrattuali CondizioniContrattuali { get; set; }

        [XmlElement(ElementName = "ZoneOfferta")]
        public ZoneOfferta ZoneOfferta { get; set; }

        public string PrezzoEnergia
        {
            get
            {
                System.Text.StringBuilder prezzi = new();

                foreach (var component in ComponentiImpresa)
                {
                    prezzi.Append("\n").Append("• ").Append(component.Nome);

                    foreach (var intervalloprezzi in component.IntervalloPrezzi)
                    {
                        if (intervalloprezzi.Prezzo > 0)
                        {
                            prezzi.Append(" ==> ");
                            if (intervalloprezzi.FasciaComponente == "01")
                                prezzi.Append("F1: ");
                            else if (intervalloprezzi.FasciaComponente == "02")
                                prezzi.Append("F2: ");
                            else if (intervalloprezzi.FasciaComponente == "03")
                                prezzi.Append("F3: ");
                            else if (intervalloprezzi.FasciaComponente == "04")
                                prezzi.Append("F4: ");
                            else if (intervalloprezzi.FasciaComponente == "05")
                                prezzi.Append("F5: ");
                            else if (intervalloprezzi.FasciaComponente == "06")
                                prezzi.Append("F6: ");
                            else if (intervalloprezzi.FasciaComponente == "07")
                                prezzi.Append("F7: ");
                            else if (intervalloprezzi.FasciaComponente == "08")
                                prezzi.Append("F8: ");
                            else if (intervalloprezzi.FasciaComponente == "91")
                                prezzi.Append("F2+F3: ");
                            else if (intervalloprezzi.FasciaComponente == "92")
                                prezzi.Append("F1+F3: ");
                            else if (intervalloprezzi.FasciaComponente == "93")
                                prezzi.Append("F1+F2: ");

                            prezzi.Append(intervalloprezzi.Prezzo).Append(" ").Append(intervalloprezzi.UnitaMisuraLabel).Append(" ");
                        }
                    }
                }
                return prezzi.ToString();
            }
        }

        public decimal StimaPrezzo
        {
            get
            {
                decimal costo = 0;
                decimal totaleKw = 7500;
                foreach (var component in ComponentiImpresa)
                {
                    foreach (var intervalloprezzi in component.IntervalloPrezzi)
                    {
                        if (intervalloprezzi.Prezzo > 0)
                        {
                            switch (intervalloprezzi.UnitaMisura)
                            {
                                case "01":
                                    costo += intervalloprezzi.Prezzo;
                                    break;
                                case "02":
                                    costo += intervalloprezzi.Prezzo * totaleKw;
                                    break;
                                case "03":
                                    costo += intervalloprezzi.Prezzo * totaleKw;
                                    break;
                                case "04":
                                    //costo += intervalloprezzi.Prezzo;
                                    break;
                                case "05":
                                    costo += intervalloprezzi.Prezzo;
                                    break;
                            }
                            /* 01:€Anno 02:€kW 03:€kWh 04:€Sm3 05:€ */
                        }
                    }
                }
                return Math.Round(costo, 2);
            }
        }
        public string ValiditaOffertaRange
        {
            get
            {
                return "string.Empty";
            }

        }
    }
    public class IdentificativiOfferta
    {
        [XmlElement(ElementName = "PIVA_UTENTE")]
        public string PivaUtente { get; set; }

        [XmlElement(ElementName = "COD_OFFERTA")]
        public string CodOfferta { get; set; }
    }
    public class DettaglioOfferta
    {
        [XmlElement(ElementName = "TIPO_MERCATO")]
        public string TipoMercato { get; set; }

        [XmlElement(ElementName = "OFFERTA_SINGOLA")]
        public string OffertaSingola { get; set; }

        [XmlElement(ElementName = "TIPO_CLIENTE")]
        public string TipoCliente { get; set; }

        [XmlElement(ElementName = "TIPO_OFFERTA")]
        public string TipoOfferta { get; set; }

        [XmlElement(ElementName = "TIPOLOGIA_ATT_CONTR")]
        public string TipologiaAttContr { get; set; }

        [XmlElement(ElementName = "NOME_OFFERTA")]
        public string NomeOfferta { get; set; }

        [XmlElement(ElementName = "DESCRIZIONE")]
        public string Descrizione { get; set; }

        [XmlElement(ElementName = "DURATA")]
        public int Durata { get; set; }

        [XmlElement(ElementName = "GARANZIE")]
        public string Garanzie { get; set; }

        [XmlElement(ElementName = "ModalitaAttivazione")]
        public ModalitaAttivazione ModalitaAttivazione { get; set; }

        [XmlElement(ElementName = "Contatti")]
        public Contatti Contatti { get; set; }
    }
    public class ModalitaAttivazione
    {
        [XmlElement(ElementName = "MODALITA")]
        public string Modalita { get; set; }
    }
    public class Contatti
    {
        [XmlElement(ElementName = "TELEFONO")]
        public string Telefono { get; set; }

        [XmlElement(ElementName = "URL_SITO_VENDITORE")]
        public string UrlSitoVenditore { get; set; }
    }
    public class RiferimentiPrezzoEnergia
    {
        [XmlElement(ElementName = "IDX_PREZZO_ENERGIA")]
        public int IdxPrezzoEnergia { get; set; }
    }
    public class ValiditaOfferta
    {
        [XmlElement(ElementName = "DATA_INIZIO")]
        public string DataInizio { get; set; }

        [XmlElement(ElementName = "DATA_FINE")]
        public string DataFine { get; set; }
    }
    public class MetodoPagamento
    {
        [XmlElement(ElementName = "MODALITA_PAGAMENTO")]
        public string ModalitaPagamento { get; set; }
    }
    public class TipoPrezzo
    {
        [XmlElement(ElementName = "TIPOLOGIA_FASCE")]
        public string TipologiaFasce { get; set; }
    }
    public class Dispacciamento
    {
        [XmlElement(ElementName = "TIPO_DISPACCIAMENTO")]
        public string TipoDispacciamento { get; set; }

        [XmlElement(ElementName = "NOME")]
        public string Nome { get; set; }
    }
    public class ComponenteImpresa
    {
        [XmlElement(ElementName = "NOME")]
        public string Nome { get; set; }

        [XmlElement(ElementName = "DESCRIZIONE")]
        public string Descrizione { get; set; }

        [XmlElement(ElementName = "TIPOLOGIA")]
        public string Tipologia { get; set; }
        /* 01: STANDARD 02:OPZIONALE */

        [XmlElement(ElementName = "MACROAREA")]
        public string Macroarea { get; set; }
        /*
         01:Commercializzazione quota fissa
         02:Commercializzazione quota energia
         04:Prezzo quota energia
         05: Una Tantum
         06: FER/Energia Verde 
         */

        [XmlElement(ElementName = "IntervalloPrezzi")]
        public List<IntervalloPrezzi> IntervalloPrezzi { get; set; }
    }
    public class IntervalloPrezzi
    {
        [XmlElement(ElementName = "FASCIA_COMPONENTE")]
        public string FasciaComponente { get; set; }
        /*
01: monorario/F1
02: F2
03: F3
04: F4
05: F5
06: F6
07:Peak
08:OffPeak
91: F2+F3
92: F1+F3
93: F1+F2         
         */

        [XmlElement(ElementName = "PREZZO")]
        public decimal Prezzo { get; set; }

        [XmlElement(ElementName = "UNITA_MISURA")]
        public string UnitaMisura { get; set; }
        public string UnitaMisuraLabel
        {
            get
            {
                return UnitaMisura switch
                {
                    "01" => "€/Anno",
                    "02" => "€/kW",
                    "03" => "€/kWh",
                    "04" => "€/Sm3",
                    "05" => "€",
                    _ => throw new NotImplementedException()
                };
            }
        }
        /* 01:€Anno 02:€kW 03:€kWh 04:€Sm3 05:€ */
    }
    public class ZoneOfferta
    {
        [XmlElement("REGIONE")]
        public List<string> Regioni { get; set; }
        [XmlElement("PROVINCIA")]
        public List<string> Provincia { get; set; }
        [XmlElement("COMUNE")]
        public List<string> Comune { get; set; }
    }
    public class CondizioniContrattuali
    {
        [XmlElement(ElementName = "TIPOLOGIA_CONDIZIONE")]
        public string TipologiaCondizione { get; set; }

        [XmlElement(ElementName = "DESCRIZIONE")]
        public string Descrizione { get; set; }

        [XmlElement(ElementName = "LIMITANTE")]
        public string Limitante { get; set; }
    }
}
