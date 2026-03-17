using System.Drawing;
using System.Windows.Forms;
using EVASION.WinForms.DataAccess;
using EVASION.WinForms.Models;

namespace EVASION.WinForms.Forms;

public class LoginForm : Form
{
    private readonly AuthRepository _authRepository = new();
    private readonly TextBox _txtUsername = new();
    private readonly TextBox _txtPassword = new();
    private readonly Button _btnLogin = new();

    public LoginForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "EVASION - Authentification";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(460, 280);
        Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);

        Label lblTitle = new()
        {
            Text = "Connexion à l'application EVASION",
            Dock = DockStyle.Top,
            Height = 55,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point)
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20),
            ColumnCount = 2,
            RowCount = 3
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

        Label lblUsername = new() { Text = "Nom d'utilisateur :", Anchor = AnchorStyles.Left, AutoSize = true };
        Label lblPassword = new() { Text = "Mot de passe :", Anchor = AnchorStyles.Left, AutoSize = true };

        _txtUsername.Dock = DockStyle.Fill;
        _txtPassword.Dock = DockStyle.Fill;
        _txtPassword.UseSystemPasswordChar = true;

        _btnLogin.Text = "Se connecter";
        _btnLogin.Width = 150;
        _btnLogin.Height = 38;
        _btnLogin.Anchor = AnchorStyles.Right;
        _btnLogin.Click += Login_Click;

        layout.Controls.Add(lblUsername, 0, 0);
        layout.Controls.Add(_txtUsername, 1, 0);
        layout.Controls.Add(lblPassword, 0, 1);
        layout.Controls.Add(_txtPassword, 1, 1);
        layout.Controls.Add(_btnLogin, 1, 2);

        Controls.Add(layout);
        Controls.Add(lblTitle);

        AcceptButton = _btnLogin;
    }

    private void Login_Click(object? sender, EventArgs e)
    {
        try
        {
            User? user = _authRepository.Authenticate(_txtUsername.Text.Trim(), _txtPassword.Text.Trim());

            if (user is null)
            {
                MessageBox.Show("Identifiants invalides.", "Connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Hide();
            using MainForm mainForm = new(user);
            mainForm.ShowDialog(this);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors de la connexion : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}