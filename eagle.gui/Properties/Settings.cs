using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Eagle.Properties
{
    internal sealed partial class Settings
    {
        public PathsSettings Paths { get; }
        public P2PSettings P2P { get; }
        public BrowserSettings Urls { get; }
        public ContractSettings Contracts { get; }
        public bool IsMainnet { get; private set; }

        public Settings()
        {
            if (NeedUpgrade)
            {
                Upgrade();
                NeedUpgrade = false;
                Save();
            }
            IConfigurationSection section = new ConfigurationBuilder().AddJsonFile("config.json").Build().GetSection("ApplicationConfiguration");
            this.IsMainnet = bool.Parse(section.GetSection("IsMainnet").Value);
            this.Paths = new PathsSettings(section.GetSection("Paths"), this.IsMainnet);
            this.P2P = new P2PSettings(section.GetSection("P2P"));
            this.Urls = this.IsMainnet ? new BrowserSettings(section.GetSection("UrlsMainnet")) : new BrowserSettings(section.GetSection("UrlsTestnet"));
            this.Contracts = new ContractSettings(section.GetSection("Contracts"));
        }
    }

    internal class PathsSettings
    {
        public string Chain { get; }
        public string CertCache { get; }

        public PathsSettings(IConfigurationSection section, bool isMainnet)
        {
            this.Chain = isMainnet ? "MainnetChain" : "Testnetchain" ;
            this.CertCache = section.GetSection("CertCache").Value;
        }
    }

    internal class P2PSettings
    {
        public ushort Port { get; }
        public ushort WsPort { get; }

        public P2PSettings(IConfigurationSection section)
        {
            this.Port = ushort.Parse(section.GetSection("Port").Value);
            this.WsPort = ushort.Parse(section.GetSection("WsPort").Value);
        }
    }

    internal class BrowserSettings
    {
        public string AddressUrl { get; }
        public string AssetUrl { get; }
        public string TransactionUrl { get; }

        public BrowserSettings(IConfigurationSection section)
        {
            this.AddressUrl = section.GetSection("AddressUrl").Value;
            this.AssetUrl = section.GetSection("AssetUrl").Value;
            this.TransactionUrl = section.GetSection("TransactionUrl").Value;
        }
    }

    internal class ContractSettings
    {
        public UInt160[] NEP5 { get; }

        public ContractSettings(IConfigurationSection section)
        {
            this.NEP5 = section.GetSection("NEP5").GetChildren().Select(p => UInt160.Parse(p.Value)).ToArray();
        }
    }
}
