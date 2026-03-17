using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EVASION.WinForms.DataAccess;
using EVASION.WinForms.Models;

namespace EVASION.WinForms.Forms;

public class VehicleDialog : Form
{
    private readonly LookupRepository _lookupRepository = new();
    private readonly TextBox _txtImmatriculation = new();
    private readonly ComboBox _cmbCategorie = new();
    private readonly TextBox _txtMarque = new();
    private readonly TextBox _txtModele = new();
    private readonly NumericUpDown _numKilometrage = new();

    public Vehicle Vehicle { get; private set; } = new();

    public VehicleDialog(Vehicle? vehicle = null)
    {
        Vehicle = vehicle ?? new Vehicle();
        InitializeComponent();
        LoadCategories();
        LoadData();
    }

    private void InitializeComponent()
    {
        Text = Vehicle.Immatriculation == string.Empty ? "Ajouter un véhicule" : "Modifier un véhicule";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(520, 320);
        Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(15),
            ColumnCount = 2,
            RowCount = 6
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

        AddRow(layout, 0, "Immatriculation :", _txtImmatriculation);

        _cmbCategorie.DropDownStyle = ComboBoxStyle.DropDownList;
        AddRow(layout, 1, "Catégorie :", _cmbCategorie);
        AddRow(layout, 2, "Marque :", _txtMarque);
        AddRow(layout, 3, "Modèle :", _txtModele);

        _numKilometrage.Maximum = 10000000;
        _numKilometrage.Dock = DockStyle.Fill;
        AddRow(layout, 4, "Kilométrage :", _numKilometrage);

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
        layout.Controls.Add(buttonPanel, 1, 5);

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

    private void LoadCategories()
    {
        DataTable categories = _lookupRepository.GetTariffCategories();
        _cmbCategorie.DataSource = categories;
        _cmbCategorie.DisplayMember = "categorie";
        _cmbCategorie.ValueMember = "categorie";
    }

    private void LoadData()
    {
        _txtImmatriculation.Text = Vehicle.Immatriculation;
        _txtMarque.Text = Vehicle.Marque;
        _txtModele.Text = Vehicle.Modele;
        _numKilometrage.Value = Vehicle.Kilometrage;

        if (!string.IsNullOrWhiteSpace(Vehicle.Categorie))
        {
            _cmbCategorie.SelectedValue = Vehicle.Categorie;
        }

        if (!string.IsNullOrWhiteSpace(Vehicle.Immatriculation))
        {
            _txtImmatriculation.ReadOnly = true;
            _txtImmatriculation.BackColor = Color.WhiteSmoke;
        }
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtImmatriculation.Text) ||
            string.IsNullOrWhiteSpace(_txtMarque.Text) ||
            string.IsNullOrWhiteSpace(_txtModele.Text) ||
            _cmbCategorie.SelectedValue is null)
        {
            MessageBox.Show("Veuillez remplir tous les champs.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Vehicle = new Vehicle
        {
            Immatriculation = _txtImmatriculation.Text.Trim(),
            Categorie = _cmbCategorie.SelectedValue.ToString() ?? string.Empty,
            Marque = _txtMarque.Text.Trim(),
            Modele = _txtModele.Text.Trim(),
            Kilometrage = Convert.ToInt32(_numKilometrage.Value)
        };

        DialogResult = DialogResult.OK;
    }
}