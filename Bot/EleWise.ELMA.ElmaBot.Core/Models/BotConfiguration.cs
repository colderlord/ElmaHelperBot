namespace EleWise.ELMA.ElmaBot.Core.Models
{
    public sealed class BotConfiguration
    {
        public string BotToken { get; set; }

        public string Socks5Host { get; set; }

        public int Socks5Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }
    }
}
