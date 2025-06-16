namespace App.Forms
{
    partial class MainForm
    {
        private System.Windows.Forms.Panel gridPanel;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button pauseButton; // ✅ Toegevoegd
        private System.Windows.Forms.Label timerLabel;
        private System.Windows.Forms.Timer timerControl;

        private System.Windows.Forms.ComboBox difficultyComboBox;
        private System.Windows.Forms.Button generateButton;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            gridPanel = new Panel();
            resetButton = new Button();
            submitButton = new Button();
            startButton = new Button();
            pauseButton = new Button(); // ✅ Toegevoegd
            timerLabel = new Label();
            timerControl = new System.Windows.Forms.Timer(components);

            difficultyComboBox = new ComboBox();
            generateButton = new Button();

            SuspendLayout();

            // gridPanel
            gridPanel.Location = new Point(20, 60);
            gridPanel.Name = "gridPanel";
            gridPanel.Size = new Size(400, 400);
            gridPanel.TabIndex = 0;

            // resetButton
            resetButton.Location = new Point(450, 60);
            resetButton.Name = "resetButton";
            resetButton.Size = new Size(100, 30);
            resetButton.TabIndex = 1;
            resetButton.Text = "Reset";
            resetButton.Click += ResetButton_Click;

            // submitButton
            submitButton.Location = new Point(450, 100);
            submitButton.Name = "submitButton";
            submitButton.Size = new Size(100, 30);
            submitButton.TabIndex = 2;
            submitButton.Text = "Submit";
            submitButton.Click += SubmitButton_Click;

            // startButton
            startButton.Location = new Point(450, 140);
            startButton.Name = "startButton";
            startButton.Size = new Size(100, 30);
            startButton.TabIndex = 3;
            startButton.Text = "Start";
            startButton.Click += StartButton_Click;

            // ✅ pauseButton
            pauseButton.Location = new Point(450, 180);
            pauseButton.Name = "pauseButton";
            pauseButton.Size = new Size(100, 30);
            pauseButton.TabIndex = 4;
            pauseButton.Text = "Pause";
            pauseButton.Click += PauseButton_Click; // ✅ Essentieel

            // timerLabel
            timerLabel.Location = new Point(450, 20);
            timerLabel.Name = "timerLabel";
            timerLabel.Size = new Size(100, 20);
            timerLabel.TabIndex = 5;
            timerLabel.Text = "Time: 00:00";

            // difficultyComboBox
            difficultyComboBox.Location = new Point(20, 480);
            difficultyComboBox.Name = "difficultyComboBox";
            difficultyComboBox.Size = new Size(120, 21);
            difficultyComboBox.Items.AddRange(new object[] { "Easy", "Medium", "Hard" });
            difficultyComboBox.SelectedIndex = 0;

            // generateButton
            generateButton.Location = new Point(160, 478);
            generateButton.Name = "generateButton";
            generateButton.Size = new Size(120, 25);
            generateButton.Text = "Genereer puzzel";
            generateButton.Click += GenerateButton_Click;

            // timerControl
            timerControl.Interval = 1000;
            timerControl.Tick += TimerControl_Tick;

            // MainForm
            ClientSize = new Size(756, 673);
            Controls.Add(gridPanel);
            Controls.Add(resetButton);
            Controls.Add(submitButton);
            Controls.Add(startButton);
            Controls.Add(pauseButton); // ✅ toegevoegd aan form
            Controls.Add(timerLabel);
            Controls.Add(difficultyComboBox);
            Controls.Add(generateButton);
            Name = "MainForm";
            Text = "Nonogram Puzzle";
            ResumeLayout(false);
        }

        private System.ComponentModel.IContainer components;
    }
}
