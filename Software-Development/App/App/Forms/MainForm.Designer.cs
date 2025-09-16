using System.Windows.Forms;

namespace App.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();

            this.Text = "Nonogram Puzzle";
            this.ClientSize = new System.Drawing.Size(1000, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Declare controls (no logic)
            this.gridPanel = new TableLayoutPanel();
            this.rowHintsPanel = new TableLayoutPanel();
            this.colHintsPanel = new TableLayoutPanel();
            this.rightPanel = new Panel();

            this.startButton = new Button();
            this.pauseButton = new Button();
            this.resetButton = new Button();
            this.submitButton = new Button();
            this.generateButton = new Button();
            this.hintButton = new Button();
            this.solveButton = new Button();

            this.difficultyComboBox = new ComboBox();
            this.timerLabel = new Label();

            this.ResumeLayout(false);
        }
    }
}
