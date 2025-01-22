using EnergyApp.src;
using OfferteMercatoLibero;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace EnergyApp
{
    public partial class MainPage : ContentPage
    {
        public ListaOfferteMercatoLibero? offerteAsync;
        private string? TipoCliente
        {
            get
            {
                return TipoClientePicker?.SelectedItem?.ToString()?.Substring(0, 2)!;
            }
        }
        private string? TipoOfferta
        {
            get
            {
                return TipoOffertaPicker?.SelectedItem?.ToString()?.Substring(0, 2)!;
            }
        }
        private string? TipoMercato
        {
            get
            {
                return TipoMercatoPicker?.SelectedItem?.ToString()?.Substring(0, 2)!;
            }
        }
        private string? FilterNomeOfferta
        {
            get
            {
                return (NomeOffertaTextBox.Text is not null && NomeOffertaTextBox.Text.Length > 3) ? NomeOffertaTextBox.Text : null;
            }
        }
        public MainPage()
        {
            InitializeComponent();

            string fileName = FetchXml(DateTime.Now).Result;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = FetchXml(DateTime.Now.AddDays(-1)).Result;
            }

            offerteAsync = ReadFromFileAsync(fileName);
            if (offerteAsync == null) { return; }

            TipoClientePicker.SelectedIndex = 0;
            TipoOffertaPicker.SelectedIndex = 0;
            TipoMercatoPicker.SelectedIndex = 0;

            Reload();
        }
        private IEnumerable<Offerta>? FilterData(ListaOfferteMercatoLibero? lista, string? TipoCliente, string? TipoOfferta, string? TipoMercato, string? FilterNomeOfferta, ZoneOfferta? zonaOfferta)
        {
            if (lista == null) { return null; }

            var x = lista.Offerte.Where(
                                        x => x.DettaglioOfferta.TipoCliente == TipoCliente
                                     && x.DettaglioOfferta.TipoOfferta == TipoOfferta
                                     && x.DettaglioOfferta.TipoMercato == TipoMercato
                                     //&& (x.ZoneOfferta is null)
                                     && (x.ZoneOfferta is not null && (x.ZoneOfferta.Comune.Contains("10091")
                                     || x.ZoneOfferta.Provincia.Contains("001")
                                     || x.ZoneOfferta.Regioni.Contains("01")))
                                     );

            if (FilterNomeOfferta is not null && FilterNomeOfferta.Length > 0)
            {
                var res = x.Where(y => y.DettaglioOfferta.NomeOfferta.Contains(FilterNomeOfferta, StringComparison.CurrentCultureIgnoreCase)
                                 || y.IdentificativiOfferta.CodOfferta.Contains(FilterNomeOfferta, StringComparison.CurrentCultureIgnoreCase));
                return res;
            }
            return x;
        }
        private void Reload()
        {
            if (offerteAsync == null)
            {
                return;
            }
            var x = FilterData(offerteAsync, TipoCliente, TipoOfferta, TipoMercato, FilterNomeOfferta, null);
            if (offerteAsync != null)
            {
                switch (OrdinaOffertaPicker.SelectedIndex)
                {
                    case 0:
                    case -1:
                        OfferteCollectionView.ItemsSource = new ObservableCollection<Offerta>(x.OrderBy(x => x.StimaPrezzo));
                        break;
                    case 1:
                        OfferteCollectionView.ItemsSource = new ObservableCollection<Offerta>(x.OrderByDescending(x => x.ValiditaOfferta.DataInizio));
                        break;
                }
            }
        }
        private async Task<string> FetchXml(DateTime d)
        {
            XmlFetcher fetcher = new XmlFetcher();
            string xmlFile = string.Empty;
            try
            {
                xmlFile = await fetcher.GetXmlAsync(d.Year, d.Month, d.Day);

                Console.WriteLine("XML Data retrieved successfully:");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return xmlFile;
        }
        public static ListaOfferteMercatoLibero? ReadFromFileAsync(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            using (StreamReader reader = new StreamReader(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ListaOfferteMercatoLibero));
                return (ListaOfferteMercatoLibero)serializer.Deserialize(reader)!;
            }
        }
        private void TipoClientePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reload();
        }
        private void TipoOffertaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reload();
        }
        private void TipoMercatoPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reload();
        }
        private void NomeOffertaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Reload();
        }
        private void OrdinaOffertaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reload();
        }
    }
}
