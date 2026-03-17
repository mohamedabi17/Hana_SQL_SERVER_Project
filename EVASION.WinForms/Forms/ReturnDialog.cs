using System.Drawing;
using System.Windows.Forms;

namespace EVASION.WinForms.Forms;

public class ReturnDialog : Form
{
    private readonly DateTimePicker _dtReturn = new();
    private readonly NumericUpDown _numKm = new();
    private readonly Label _lblEstimate = new();

    public DateTime DateRetour => _dtReturn.Value.Date;
    public int KmParcourus => Convert.ToInt32(_numKm.Value);

    public ReturnDialog()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Retour du véhicule";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(420, 280);
        Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);

        _dtReturn.Format = DateTimePickerFormat.Short;
        _dtReturn.Value = DateTime.Today;
        _dtReturn.MaxDate = DateTime.Today;

        _numKm.Maximum = 1000000;
        _numKm.Minimum = 0;
        _numKm.Dock = DockStyle.Fill;

        _lblEstimate.ForeColor = Color.DarkBlue;
        _lblEstimate.Font = new Font("Segoe UI", 9, FontStyle.Italic);
        _lblEstimate.Text = "Montant sera calculé à la validation";

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(15),
            ColumnCount = 2,
            RowCount = 4
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

        AddRow(layout, 0, "Date retour :", _dtReturn);
        AddRow(layout, 1, "KM parcourus :", _numKm);
        AddRow(layout, 2, "Estimation :", _lblEstimate);

        FlowLayoutPanel buttonPanel = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft
        };

        Button btnSave = new() { Text = "Valider", Width = 120, Height = 36 };
        Button btnCancel = new() { Text = "Annuler", Width = 120, Height = 36 };
        btnSave.Click += Save_Click;
        btnCancel.Click += (_, _) => DialogResult = DialogResult.Cancel;

        buttonPanel.Controls.Add(btnSave);
        buttonPanel.Controls.Add(btnCancel);
        layout.Controls.Add(buttonPanel, 1, 3);

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

    private void Save_Click(object? sender, EventArgs e)
    {
        if (_numKm.Value < 0)
        {
            MessageBox.Show("Le kilométrage ne peut pas être négatif.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_dtReturn.Value.Date > DateTime.Today)
        {
            MessageBox.Show("La date de retour ne peut pas être dans le futur.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        DialogResult = DialogResult.OK;
    }
}
