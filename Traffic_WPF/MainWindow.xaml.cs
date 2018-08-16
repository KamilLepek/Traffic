using System;
using System.Windows;
using System.Windows.Controls;
using Traffic.Graphics;
using Traffic.Physics;
using Traffic.Utilities;

namespace Traffic_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(CarAmountTextBox.Text) && !string.IsNullOrWhiteSpace(VerticalLinesAmountTextBox.Text) &&
                !string.IsNullOrWhiteSpace(HorizontalLinesAmountTextBox.Text))
            {
                int verticalLines = Convert.ToInt32(CarAmountTextBox.Text);
                int horizontalLines = Convert.ToInt32(HorizontalLinesAmountTextBox.Text);
                int desiredNumberOfVehicles = Convert.ToInt32(VerticalLinesAmountTextBox.Text);
                this.Hide();
                var simulationController = new SimulationController(horizontalLines, verticalLines, desiredNumberOfVehicles);
                simulationController.InitSimulation();
                var graphicsController = new GraphicsController(simulationController.World, simulationController.PerformSimulationTick);
                graphicsController.Run(Constants.TicksPerSecond);
                InitializeComponent();
                CarAmountTextBox.Text = "";
                VerticalLinesAmountTextBox.Text = "";
                HorizontalLinesAmountTextBox.Text = "";
                this.Show();
            }
            else
            {
                MessageBox.Show("Don't leave white space");
            }
        }

        private void CarAmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = CarAmountTextBox.Text;
            foreach (char c in input)
            {
                if (char.IsNumber(c) == false)
                {
                    MessageBox.Show("Please enter a valid number");
                    CarAmountTextBox.Text = "";
                }
            }
        }
    }
}
