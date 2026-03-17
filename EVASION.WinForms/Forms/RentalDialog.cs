using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EVASION.WinForms.DataAccess;
using EVASION.WinForms.Models;

namespace EVASION.WinForms.Forms;

public class RentalDialog : Form
{
    private readonly LookupRepository _lookupRepository = new();
    private readonly RentalRepository _rentalRepository = new();
    private readonly ComboBox _cmbClient = new();
    private readonly ComboBox _cmbVehicle = new();
    private readonly DateTimePicker _dtStart = new();
    private readonly DateTimePicker _dtEnd = new();
    private readonly Label _lblPriceInfo = new();

    public Rental Rental { get; private set; } = new();

    public RentalDialog(Rental? rental = null)
    {
        Rental = rental ?? new Rental();
        InitializeComponent();
        LoadLookups();
        LoadData();
    }

    private void InitializeComponent()
    {
        Text = Rental.Id == 0 ? "Ajouter une location" : "Modifier une location";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(560, 340);
        Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);

        _cmbClient.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbVehicle.DropDownStyle = ComboBoxStyle.DropDownList;
        _dtStart.Format = DateTimePickerFormat.Short;
        _dtEnd.Format = DateTimePickerFormat.Short;
        _lblPriceInfo.ForeColor = Color.DarkBlue;
        _lblPriceInfo.Font = new Font("Segoe UI", 9, FontStyle.Italic);

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(15),
            ColumnCount = 2,
            RowCount = 6
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

        AddRow(layout, 0, "Client :", _cmbClient);
        AddRow(layout, 1, "Véhicule :", _cmbVehicle);
        AddRow(layout, 2, "Date début :", _dtStart);
        AddRow(layout, 3, "Date fin :", _dtEnd);
        AddRow(layout, 4, "Tarifs :", _lblPriceInfo);

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

        _dtStart.ValueChanged += (_, _) => UpdateAvailableVehicles();
        _dtEnd.ValueChanged += (_, _) => UpdateAvailableVehicles();

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

    private void LoadLookups()
    {
        DataTable clients = _lookupRepository.GetClientLookup();
        _cmbClient.DataSource = clients;
        _cmbClient.DisplayMember = "client";
        _cmbClient.ValueMember = "codeclient";
    }

    private void UpdateAvailableVehicles()
    {
        try
        {
            DataTable vehicles = _rentalRepository.GetAvailableVehicles(_dtStart.Value.Date, _dtEnd.Value.Date);
            _cmbVehicle.DataSource = vehicles;
            _cmbVehicle.DisplayMember = "vehicule";
            _cmbVehicle.ValueMember = "immatriculation";

            // Afficher les tarifs
            if (vehicles.Rows.Count > 0)
            {
                DataRow row = vehicles.Rows[0];
                decimal prixJour = Convert.ToDecimal(row["prix_jour"]);
                decimal prixKm = Convert.ToDecimal(row["prix_km"]);
                _lblPriceInfo.Text = $"{prixJour}€/jour + {prixKm}€/km";
            }
        }
        catch
        {
            _cmbVehicle.DataSource = new DataTable();
            _lblPriceInfo.Text = "Aucun véhicule disponible";
        }
    }

    private void LoadData()
    {
        if (!string.IsNullOrWhiteSpace(Rental.CodeClient))
        {
            _cmbClient.SelectedValue = Rental.CodeClient;
        }

        _dtStart.Value = Rental.DateDebut == default ? DateTime.Today : Rental.DateDebut;
        _dtEnd.Value = Rental.DateFin == default ? DateTime.Today.AddDays(1) : Rental.DateFin;

        UpdateAvailableVehicles();

        if (!string.IsNullOrWhiteSpace(Rental.Immatriculation))
        {
            _cmbVehicle.SelectedValue = Rental.Immatriculation;
        }
    }

    private void Save_Click(object? sender, EventArgs e)
    {
        if (_cmbClient.SelectedValue is null)
        {
            MessageBox.Show("Veuillez sélectionner un client.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cmbVehicle.SelectedValue is null)
        {
            MessageBox.Show("Veuillez sélectionner un véhicule disponible.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_dtEnd.Value.Date < _dtStart.Value.Date)
        {
            MessageBox.Show("La date de fin doit être supérieure ou égale à la date de début.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_dtStart.Value.Date < DateTime.Today)
        {
            MessageBox.Show("La date de début ne peut pas être dans le passé.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Vérifier la disponibilité une dernière fois
        string immatriculation = _cmbVehicle.SelectedValue.ToString() ?? string.Empty;
        if (!_rentalRepository.IsVehicleAvailable(immatriculation, _dtStart.Value.Date, _dtEnd.Value.Date))
        {
            MessageBox.Show("Le véhicule n'est plus disponible pour cette période.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            UpdateAvailableVehicles();
            return;
        }

        Rental = new Rental
        {
            Id = Rental.Id,
            CodeClient = _cmbClient.SelectedValue.ToString() ?? string.Empty,
            Immatriculation = immatriculation,
            DateDebut = _dtStart.Value.Date,
            DateFin = _dtEnd.Value.Date
        };

        DialogResult = DialogResult.OK;
    }
}

    }
}