using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EVASION.WinForms.DataAccess;
using EVASION.WinForms.Models;

namespace EVASION.WinForms.Forms;

public class MainForm : Form
{
    private readonly User _currentUser;
    private readonly ClientRepository _clientRepository = new();
    private readonly VehicleRepository _vehicleRepository = new();
    private readonly RentalRepository _rentalRepository = new();
    private readonly ReportRepository _reportRepository = new();

    private readonly TabControl _tabControl = new();

    private readonly DataGridView _clientsGrid = new();
    private readonly DataGridView _vehiclesGrid = new();
    private readonly DataGridView _rentalsGrid = new();
    private readonly DataGridView _reportsGrid = new();

    private readonly TextBox _txtSearchClients = new();
    private readonly TextBox _txtSearchVehicles = new();
    private readonly TextBox _txtSearchRentals = new();

    private readonly Label _lblClientCountValue = new();
    private readonly Label _lblVehicleCountValue = new();
    private readonly Label _lblActiveRentalsValue = new();
    private readonly Label _lblTotalRevenueValue = new();

    public MainForm(User currentUser)
    {
        _currentUser = currentUser;
        InitializeComponent();
        LoadClients();
        LoadVehicles();
        LoadRentals();
        LoadDashboardStatistics();
    }

    private void InitializeComponent()
    {
        Text = "EVASION - Tableau de bord";
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(1200, 700);
        Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);

        Panel headerPanel = new()
        {
            Dock = DockStyle.Top,
            Height = 60,
            Padding = new Padding(12),
            BackColor = Color.FromArgb(30, 64, 175)
        };

        Label titleLabel = new()
        {
            Dock = DockStyle.Fill,
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
            Text = $"EVASION - Gestion des locations | Utilisateur : {_currentUser.Username} ({_currentUser.Role})"
        };
        headerPanel.Controls.Add(titleLabel);

        Panel dashboardPanel = CreateDashboardPanel();

        _tabControl.Dock = DockStyle.Fill;
        _tabControl.TabPages.Add(CreateClientsTab());
        _tabControl.TabPages.Add(CreateVehiclesTab());
        _tabControl.TabPages.Add(CreateRentalsTab());
        _tabControl.TabPages.Add(CreateReportsTab());

        Controls.Add(_tabControl);
        Controls.Add(dashboardPanel);
        Controls.Add(headerPanel);
    }

    private Panel CreateDashboardPanel()
    {
        Panel panel = new()
        {
            Dock = DockStyle.Top,
            Height = 120,
            Padding = new Padding(12),
            BackColor = Color.FromArgb(245, 247, 250)
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 1
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

        layout.Controls.Add(CreateStatCard("Clients", _lblClientCountValue), 0, 0);
        layout.Controls.Add(CreateStatCard("Véhicules", _lblVehicleCountValue), 1, 0);
        layout.Controls.Add(CreateStatCard("Locations actives", _lblActiveRentalsValue), 2, 0);
        layout.Controls.Add(CreateStatCard("Chiffre d'affaires", _lblTotalRevenueValue), 3, 0);

        panel.Controls.Add(layout);
        return panel;
    }

    private static Panel CreateStatCard(string title, Label valueLabel)
    {
        Panel card = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = new Padding(6),
            Padding = new Padding(10)
        };

        Label titleLabel = new()
        {
            Text = title,
            Dock = DockStyle.Top,
            Height = 30,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point),
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(55, 65, 81)
        };

        valueLabel.Text = "0";
        valueLabel.Dock = DockStyle.Fill;
        valueLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
        valueLabel.TextAlign = ContentAlignment.MiddleCenter;
        valueLabel.ForeColor = Color.FromArgb(30, 64, 175);

        card.Controls.Add(valueLabel);
        card.Controls.Add(titleLabel);
        return card;
    }

    private TabPage CreateClientsTab()
    {
        return CreateManagementTab(
            "Clients",
            _txtSearchClients,
            SearchClients,
            _clientsGrid,
            CreateActionButton("Ajouter", AddClient_Click),
            CreateActionButton("Modifier", EditClient_Click),
            CreateActionButton("Supprimer", DeleteClient_Click));
    }

    private TabPage CreateVehiclesTab()
    {
        return CreateManagementTab(
            "Véhicules",
            _txtSearchVehicles,
            SearchVehicles,
            _vehiclesGrid,
            CreateActionButton("Ajouter", AddVehicle_Click),
            CreateActionButton("Modifier", EditVehicle_Click),
            CreateActionButton("Supprimer", DeleteVehicle_Click));
    }

    private TabPage CreateRentalsTab()
    {
        return CreateManagementTab(
            "Locations",
            _txtSearchRentals,
            SearchRentals,
            _rentalsGrid,
            CreateActionButton("Ajouter", AddRental_Click),
            CreateActionButton("Modifier", EditRental_Click),
            CreateActionButton("Supprimer", DeleteRental_Click),
            CreateActionButton("Retour véhicule", ReturnVehicle_Click));
    }

    private TabPage CreateReportsTab()
    {
        ConfigureGrid(_reportsGrid);

        FlowLayoutPanel buttonsPanel = new()
        {
            Dock = DockStyle.Top,
            Height = 120,
            Padding = new Padding(10),
            FlowDirection = FlowDirection.LeftToRight,
            AutoScroll = true
        };

        buttonsPanel.Controls.Add(CreateActionButton("Locations / véhicule", (_, _) => ExecuteReport(() => _reportRepository.TotalRentalsPerVehicle())));
        buttonsPanel.Controls.Add(CreateActionButton("Revenu / catégorie", (_, _) => ExecuteReport(() => _reportRepository.RevenuePerCategory())));
        buttonsPanel.Controls.Add(CreateActionButton("Véhicules les plus loués", (_, _) => ExecuteReport(() => _reportRepository.MostRentedVehicles())));
        buttonsPanel.Controls.Add(CreateActionButton("Locations par client", (_, _) => ExecuteReport(() => _reportRepository.RentalsByClient())));
        buttonsPanel.Controls.Add(CreateActionButton("Locations en attente", (_, _) => ExecuteReport(() => _reportRepository.GetPendingRentals())));
        buttonsPanel.Controls.Add(CreateActionButton("Clients fidèles", (_, _) => ExecuteReport(() => _reportRepository.GetLoyalClients())));
        buttonsPanel.Controls.Add(CreateActionButton("Utilisation véhicules", (_, _) => ExecuteReport(() => _reportRepository.GetVehicleUsageRate())));
        buttonsPanel.Controls.Add(CreateActionButton("Performance catégories", (_, _) => ExecuteReport(() => _reportRepository.GetCategoryPerformance())));
        buttonsPanel.Controls.Add(CreateActionButton("Revenu (période)", (_, _) => ShowRevenueByPeriod()));

        TabPage tab = new("Rapports");
        tab.Controls.Add(_reportsGrid);
        tab.Controls.Add(buttonsPanel);
        return tab;
    }

    private void ExecuteReport(Func<DataTable> reportFunc)
    {
        try
        {
            _reportsGrid.DataSource = reportFunc();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors de la génération du rapport : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ShowRevenueByPeriod()
    {
        using Form form = new()
        {
            Text = "Revenu par période",
            StartPosition = FormStartPosition.CenterParent,
            Width = 400,
            Height = 200,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(15),
            ColumnCount = 2,
            RowCount = 3
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

        DateTimePicker dtStart = new() { Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddDays(-30) };
        DateTimePicker dtEnd = new() { Format = DateTimePickerFormat.Short, Value = DateTime.Today };

        Label lblStart = new() { Text = "Date début :", Anchor = AnchorStyles.Left, AutoSize = true };
        Label lblEnd = new() { Text = "Date fin :", Anchor = AnchorStyles.Left, AutoSize = true };

        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        dtStart.Dock = DockStyle.Fill;
        dtEnd.Dock = DockStyle.Fill;

        layout.Controls.Add(lblStart, 0, 0);
        layout.Controls.Add(dtStart, 1, 0);
        layout.Controls.Add(lblEnd, 0, 1);
        layout.Controls.Add(dtEnd, 1, 1);

        FlowLayoutPanel btnPanel = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            AutoScroll = true
        };

        Button btnOk = new() { Text = "Afficher", Width = 100, Height = 36 };
        Button btnCancel = new() { Text = "Fermer", Width = 100, Height = 36 };

        btnOk.Click += (_, _) =>
        {
            try
            {
                _reportsGrid.DataSource = _reportRepository.GetRevenueByPeriod(dtStart.Value.Date, dtEnd.Value.Date);
                form.DialogResult = DialogResult.OK;
                form.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        btnCancel.Click += (_, _) => form.Close();

        btnPanel.Controls.Add(btnOk);
        btnPanel.Controls.Add(btnCancel);

        layout.Controls.Add(btnPanel, 1, 2);
        form.Controls.Add(layout);
        form.ShowDialog(this);
    }

    private TabPage CreateManagementTab(string title, TextBox searchBox, EventHandler searchHandler, DataGridView grid, params Button[] actionButtons)
    {
        ConfigureGrid(grid);

        Panel searchPanel = new()
        {
            Dock = DockStyle.Top,
            Height = 60,
            Padding = new Padding(12)
        };

        Label searchLabel = new()
        {
            Text = "Recherche :",
            AutoSize = true,
            Location = new Point(12, 18)
        };

        searchBox.Width = 320;
        searchBox.Location = new Point(110, 14);
        searchBox.KeyDown += (sender, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchHandler(sender, e);
            }
        };

        Button btnSearch = CreateActionButton("Rechercher", searchHandler);
        btnSearch.Location = new Point(450, 10);

        searchPanel.Controls.Add(searchLabel);
        searchPanel.Controls.Add(searchBox);
        searchPanel.Controls.Add(btnSearch);

        FlowLayoutPanel buttonsPanel = new()
        {
            Dock = DockStyle.Top,
            Height = 60,
            Padding = new Padding(10),
            FlowDirection = FlowDirection.LeftToRight
        };

        foreach (Button button in actionButtons)
        {
            buttonsPanel.Controls.Add(button);
        }

        TabPage tab = new(title);
        tab.Controls.Add(grid);
        tab.Controls.Add(buttonsPanel);
        tab.Controls.Add(searchPanel);
        return tab;
    }

    private static Button CreateActionButton(string text, EventHandler handler)
    {
        Button button = new()
        {
            Text = text,
            Width = 160,
            Height = 38,
            Margin = new Padding(6)
        };
        button.Click += handler;
        return button;
    }

    private static void ConfigureGrid(DataGridView grid)
    {
        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.MultiSelect = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.AllowUserToAddRows = false;
        grid.RowHeadersVisible = false;
        grid.BackgroundColor = Color.White;
    }

    private void LoadClients() => _clientsGrid.DataSource = _clientRepository.GetClients();

    private void LoadVehicles() => _vehiclesGrid.DataSource = _vehicleRepository.GetVehicles();

    private void LoadRentals() => _rentalsGrid.DataSource = _rentalRepository.GetLocations();

    private void LoadDashboardStatistics()
    {
        try
        {
            int totalClients = _reportRepository.GetClientCount();
            int totalVehicles = _reportRepository.GetVehicleCount();
            int activeRentals = _reportRepository.GetActiveRentalsCount();
            decimal totalRevenue = _reportRepository.GetTotalRevenue();

            _lblClientCountValue.Text = totalClients.ToString();
            _lblVehicleCountValue.Text = totalVehicles.ToString();
            _lblActiveRentalsValue.Text = activeRentals.ToString();
            _lblTotalRevenueValue.Text = $"{totalRevenue:N2} €";
        }
        catch (Exception)
        {
            _lblClientCountValue.Text = "-";
            _lblVehicleCountValue.Text = "-";
            _lblActiveRentalsValue.Text = "-";
            _lblTotalRevenueValue.Text = "-";
        }
    }

    private void SearchClients(object? sender, EventArgs e)
    {
        _clientsGrid.DataSource = string.IsNullOrWhiteSpace(_txtSearchClients.Text)
            ? _clientRepository.GetClients()
            : _clientRepository.SearchClient(_txtSearchClients.Text.Trim());
    }

    private void SearchVehicles(object? sender, EventArgs e)
    {
        _vehiclesGrid.DataSource = string.IsNullOrWhiteSpace(_txtSearchVehicles.Text)
            ? _vehicleRepository.GetVehicles()
            : _vehicleRepository.SearchVehicle(_txtSearchVehicles.Text.Trim());
    }

    private void SearchRentals(object? sender, EventArgs e)
    {
        _rentalsGrid.DataSource = string.IsNullOrWhiteSpace(_txtSearchRentals.Text)
            ? _rentalRepository.GetLocations()
            : _rentalRepository.SearchLocation(_txtSearchRentals.Text.Trim());
    }

    private void AddClient_Click(object? sender, EventArgs e)
    {
        try
        {
            using ClientDialog dialog = new();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _clientRepository.AddClient(dialog.Client);
                MessageBox.Show("Client ajouté avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadClients();
                LoadDashboardStatistics();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors de l'ajout du client : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void EditClient_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!TryGetSelectedRow(_clientsGrid, out DataGridViewRow row))
            {
                return;
            }

            Client client = new()
            {
                CodeClient = GetCellString(row, "codeclient"),
                Nom = GetCellString(row, "nom"),
                Prenom = GetCellString(row, "prenom"),
                Adresse = GetCellString(row, "adresse"),
                Telephone = GetCellString(row, "telephone"),
                TypeClient = GetCellString(row, "typeclient")
            };

            using ClientDialog dialog = new(client);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _clientRepository.UpdateClient(dialog.Client);
                MessageBox.Show("Client modifié avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadClients();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors de la modification du client : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DeleteClient_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!TryGetSelectedRow(_clientsGrid, out DataGridViewRow row))
            {
                return;
            }

            string codeClient = GetCellString(row, "codeclient");
            if (ConfirmDelete($"Supprimer le client {codeClient} ?"))
            {
                _clientRepository.DeleteClient(codeClient);
                MessageBox.Show("Client supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadClients();
                LoadDashboardStatistics();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors de la suppression du client : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AddVehicle_Click(object? sender, EventArgs e)
    {
        try
        {
            using VehicleDialog dialog = new();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _vehicleRepository.AddVehicle(dialog.Vehicle);
                MessageBox.Show("Véhicule ajouté avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadVehicles();
                LoadDashboardStatistics();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors de l'ajout du véhicule : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void EditVehicle_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!TryGetSelectedRow(_vehiclesGrid, out DataGridViewRow row))
            {
                return;
            }

            Vehicle vehicle = new()
            {
                Immatriculation = GetCellString(row, "immatriculation"),
                Categorie = GetCellString(row, "categorie"),
                Marque = GetCellString(row, "marque"),
                Modele = GetCellString(row, "modele"),
                Kilometrage = GetCellInt(row, "kilometrage")
            };

            using VehicleDialog dialog = new(vehicle);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _vehicleRepository.UpdateVehicle(dialog.Vehicle);
                MessageBox.Show("Véhicule modifié avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadVehicles();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors de la modification du véhicule : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DeleteVehicle_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!TryGetSelectedRow(_vehiclesGrid, out DataGridViewRow row))
            {
                return;
            }

            string immatriculation = GetCellString(row, "immatriculation");
            if (ConfirmDelete($"Supprimer le véhicule {immatriculation} ?"))
            {
                _vehicleRepository.DeleteVehicle(immatriculation);
                MessageBox.Show("Véhicule supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadVehicles();
                LoadDashboardStatistics();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors de la suppression du véhicule : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AddRental_Click(object? sender, EventArgs e)
    {
        try
        {
            using RentalDialog dialog = new();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _rentalRepository.AddLocation(dialog.Rental);
                MessageBox.Show("Location créée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRentals();
                LoadDashboardStatistics();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors de la création de la location : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void EditRental_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!TryGetSelectedRow(_rentalsGrid, out DataGridViewRow row))
            {
                return;
            }

            Rental rental = new()
            {
                Id = GetCellInt(row, "id"),
                CodeClient = GetCellString(row, "codeclient"),
                Immatriculation = GetCellString(row, "immatriculation"),
                DateDebut = GetCellDate(row, "datedebut"),
                DateFin = GetCellDate(row, "datefin")
            };

            using RentalDialog dialog = new(rental);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _rentalRepository.UpdateLocation(dialog.Rental);
                MessageBox.Show("Location modifiée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRentals();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors de la modification de la location : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DeleteRental_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!TryGetSelectedRow(_rentalsGrid, out DataGridViewRow row))
            {
                return;
            }

            int id = GetCellInt(row, "id");
            if (ConfirmDelete($"Supprimer la location #{id} ?"))
            {
                _rentalRepository.DeleteLocation(id);
                MessageBox.Show("Location supprimée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRentals();
                LoadDashboardStatistics();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors de la suppression de la location : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ReturnVehicle_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!TryGetSelectedRow(_rentalsGrid, out DataGridViewRow row))
            {
                return;
            }

            if (row.Cells["dateretour"].Value != DBNull.Value && row.Cells["dateretour"].Value is not null)
            {
                MessageBox.Show("Ce véhicule a déjà été retourné.", "Retour", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int id = GetCellInt(row, "id");
            using ReturnDialog dialog = new();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                decimal montant = _rentalRepository.ReturnVehicle(id, dialog.DateRetour, dialog.KmParcourus);
                LoadRentals();
                LoadDashboardStatistics();
                MessageBox.Show($"Retour enregistré. Montant = {montant:N2}€", "Retour véhicule", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors du retour du véhicule : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static bool TryGetSelectedRow(DataGridView grid, out DataGridViewRow row)
    {
        row = null!;
        if (grid.CurrentRow is null)
        {
            MessageBox.Show("Veuillez sélectionner une ligne.", "Sélection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        row = grid.CurrentRow;
        return true;
    }

    private static bool ConfirmDelete(string message)
    {
        return MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    }

    private static string GetCellString(DataGridViewRow row, string columnName)
    {
        return row.Cells[columnName].Value?.ToString() ?? string.Empty;
    }

    private static int GetCellInt(DataGridViewRow row, string columnName)
    {
        return Convert.ToInt32(row.Cells[columnName].Value);
    }

    private static DateTime GetCellDate(DataGridViewRow row, string columnName)
    {
        return Convert.ToDateTime(row.Cells[columnName].Value);
    }
}