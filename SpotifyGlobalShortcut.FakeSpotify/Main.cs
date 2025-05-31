using System.Text;

namespace SpotifyGlobalShortcut.FakeSpotify
{
    public partial class Main : Form
    {
        private readonly StringBuilder currentCombo = new();

        public Main()
        {
            InitializeComponent();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            Log($"Key down: {e.KeyCode} ({e.Modifiers})");
            if (!currentCombo.ToString().Contains(e.KeyCode.ToString()))
            {
                if (currentCombo.Length > 0)
                    currentCombo.Append(" + ");
                currentCombo.Append(e.KeyCode);
            }
            e.Handled = true;
        }

        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            Log($"Key up: {currentCombo}");
            currentCombo.Clear();
        }

        private void Main_Activated(object sender, EventArgs e)
        {
            Log("Focus acquired");
        }

        private void Log(string message)
        {
            logBox.AppendText($"{DateTime.Now:HH:mm:ss} - {message}{Environment.NewLine}");
            logBox.SelectionStart = logBox.Text.Length;
            logBox.ScrollToCaret();
        }
    }
}
