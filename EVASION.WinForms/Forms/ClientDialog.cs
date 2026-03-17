using System.Drawing;
using System.Windows.Forms;
using EVASION.WinForms.Models;

namespace EVASION.WinForms.Forms;

public class ClientDialog : Form
{
    private readonly TextBox _txtCode = new();
    private readonly TextBox _txtNom = new();
    private readonly TextBox _txtPrenom = new();
    private readonly TextBox _txtAdresse = new();
    private readonly TextBox _txtTelephone = new();
    private readonly ComboBox _cmbType = new();

    public Client Client { get; private set; } = new();

    public ClientDialog(Client? client = null)
    {
        Client = client ?? new Client();
        InitializeComponent();
        LoadData();
    }

    private void InitializeComponent()
    {
        Text = Client.CodeClient == string.Empty ? "Ajouter un client" : "Modifier un client";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(520, 360);
        Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(15),
            ColumnCount = 2,
            RowCount = 7
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

        AddRow(layout, 0, "Code client :", _txtCode);
        AddRow(layout, 1, "Nom :", _txtNom);
        AddRow(layout, 2, "Prénom :", _txtPrenom);
        AddRow(layout, 3, "Adresse :", _txtAdresse);
        AddRow(layout, 4, "Téléphone :", _txtTelephone);

        _cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbType.Items.AddRange(["IND", "ORG"]);
        AddRow(layout, 5, "Type client :", _cmbType);

        FlowLayoutPanel buttonPanel = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft
        };

        Button btnSave = new() { Text = "Enregistrer", Width = 120, Height = 36 };
        Button btnCancel = new() { Text = "Annuler", Width = 120, Height = 36 };
        btnSave.Click += Save_Click;
        btnCancel.Click += (_, _) => DialogResult = DialogResult.Cancel;

        buttonPanel.Controls.Add(btnSave);
        buttonPanel.Controls.Add(btnCancel);
        layout.Controls.Add(buttonPanel, 1, 6);

        Controls.Add(layout);
        AcceptButton = btnSave;
        CancelButton = btnCancel;
    }

    private void AddRow(TableLayoutPanel layout, int row, string labelText, Control control)
    {
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        Label label = new() { Text = labelText, Anchor = AnchorStyles.Left, AutoSize = true };
        control.Dock = DockStyle.Fill;
        layout.Controls.Add(label, 0, row);
        layout.Controls.Add(control, 1, row);
    }

    private void LoadData()
    {
        _txtCode.Text = Client.CodeClient;
        _txtNom.Text = Client.Nom;
        _txtPrenom.Text = Client.Prenom;
        _txtAdresse.Text = Client.Adresse;
        _txtTelephone.Text = Client.Telephone;
        _cmbType.SelectedItem = string.IsNullOrWhiteSpace(Client.TypeClient) ? "IND" : Client.TypeClient;

        if (!string.IsNullOrWhiteSpace(Client.CodeClient))
        {
            _txtCode.ReadOnly = true;
            _txtCode.BackColor = Color.WhiteSmoke;
        }
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtCode.Text) ||
            string.IsNullOrWhiteSpace(_txtNom.Text) ||
            string.IsNullOrWhiteSpace(_txtPrenom.Text) ||
            string.IsNullOrWhiteSpace(_txtAdresse.Text) ||
            string.IsNullOrWhiteSpace(_txtTelephone.Text) ||
            _cmbType.SelectedItem is null)
        {
            MessageBox.Show("Veuillez remplir tous les champs.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Client = new Client
        {
            CodeClient = _txtCode.Text.Trim(),
            Nom = _txtNom.Text.Trim(),
            Prenom = _txtPrenom.Text.Trim(),
            Adresse = _txtAdresse.Text.Trim(),
            Telephone = _txtTelephone.Text.Trim(),
            TypeClient = _cmbType.SelectedItem.ToString() ?? "IND"
        };

        DialogResult = DialogResult.OK;
    }
}