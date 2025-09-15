using System;
using System.Drawing;
using System.Windows.Forms;

namespace App.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panel1;
        private Button startButton;
        private Button pauseButton;
        private Button resetButton;
        private Button submitButton;
        private Button generateButton;
        private Button showSolutionButton;
        private ComboBox difficultyComboBox;
        private Label timerLabel;
        private System.Windows.Forms.Timer timerControl;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            panel1 = new Panel();
            startButton = new Button();
            pauseButton = new Button();
            resetButton = new Button();
            submitButton = new Button();
            generateButton = new Button();
            showSolutionButton = new Button();
            difficultyComboBox = new ComboBox();
            timerLabel = new Label();
            timerControl = new System.Windows.Forms.Timer(components);

            SuspendLayout();

            // Start button
            startButton.Location = new Point(450, 60);
            startButton.Size = new Size(100, 30);
            startButton.Text = "Start";
            startButton.Click += StartButton_Click;

            // Pause button
            pauseButton.Location = new Point(450, 100);
            pauseButton.Size = new Size(100, 30);
            pauseButton.Text = "Pause";
            pauseButton.Click += PauseButton_Click;

            // Reset button
            resetButton.Location = new Point(450, 140);
            resetButton.Size = new Size(100, 30);
            resetButton.Text = "Reset";
            resetButton.Click += ResetButton_Click;

            // Submit button
            submitButton.Location = new Point(450, 180);
            submitButton.Size = new Size(100, 30);
            submitButton.Text = "Submit";
            submitButton.Click += SubmitButton_Click;

            // Generate button
            generateButton.Location = new Point(160, 480);
            generateButton.Size = new Size(120, 25);
            generateButton.Text = "Genereer puzzel";
            generateButton.Click += GenerateButton_Click;

            // Show Solution button
            showSolutionButton.Location = new Point(300, 480);
            showSolutionButton.Size = new Size(120, 25);
            showSolutionButton.Text = "Show Solution";
            showSolutionButton.Click += ShowSolutionButton_Click;

            // Difficulty ComboBox
            difficultyComboBox.Location = new Point(20, 480);
            difficultyComboBox.Size = new Size(120, 21);
            difficultyComboBox.Items.AddRange(new object[] { "Easy", "Medium", "Hard" });
            difficultyComboBox.SelectedIndex = 0;

            // Timer Label
            timerLabel.Location = new Point(450, 20);
            timerLabel.Size = new Size(100, 20);
            timerLabel.Text = "Time: 00:00";

            // Timer
            timerControl.Interval = 1000;
            timerControl.Tick += TimerControl_Tick;

            // MainForm
            ClientSize = new Size(756, 673);
            Controls.Add(startButton);
            Controls.Add(pauseButton);
            Controls.Add(resetButton);
            Controls.Add(submitButton);
            Controls.Add(generateButton);
            Controls.Add(showSolutionButton);
            Controls.Add(difficultyComboBox);
            Controls.Add(timerLabel);
            Text = "Nonogram Puzzle";

            ResumeLayout(false);
        }
    }
}
