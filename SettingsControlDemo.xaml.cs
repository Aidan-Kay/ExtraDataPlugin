using System.Windows.Controls;

namespace AidanKay.ExtraDataPlugin
{
    /// <summary>
    /// Logique d'interaction pour SettingsControlDemo.xaml
    /// </summary>
    public partial class SettingsControlDemo : UserControl
    {
        public ExtraDataPlugin Plugin { get; }

        public SettingsControlDemo()
        {
            InitializeComponent();
        }

        public SettingsControlDemo(ExtraDataPlugin plugin) : this()
        {
            this.Plugin = plugin;
        }


    }
}
