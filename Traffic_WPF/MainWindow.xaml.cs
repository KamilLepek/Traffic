using System;
using System.Windows;
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
            this.InitializeComponent();
        }

        public void Clear()
        {
            this.CarAmountTextBox.Text = "";
            this.VerticalLinesAmountTextBox.Text = "";
            this.HorizontalLinesAmountTextBox.Text = "";
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            int verticalLines, horizontalLines, desiredNumberOfVehicles;
            try
            {
                verticalLines = Convert.ToInt32(this.VerticalLinesAmountTextBox.Text);
                if (verticalLines > Constants.MaxAmountOfLines)
                {
                    MessageBox.Show("Max amount of vertical lines is: " + Constants.MaxAmountOfLines);
                    this.VerticalLinesAmountTextBox.Text = Constants.MaxAmountOfLines.ToString();
                    return;
                }
                horizontalLines = Convert.ToInt32(this.HorizontalLinesAmountTextBox.Text);
                if (horizontalLines > Constants.MaxAmountOfLines)
                {
                    MessageBox.Show("Max amount of horizontal lines is: " + Constants.MaxAmountOfLines);
                    this.HorizontalLinesAmountTextBox.Text = Constants.MaxAmountOfLines.ToString();
                    return;
                }
                desiredNumberOfVehicles = Convert.ToInt32(this.CarAmountTextBox.Text);
                if (desiredNumberOfVehicles > horizontalLines * verticalLines * Constants.CarAmountNormalizationConstant)
                {
                    MessageBox.Show("For given amount of vertical and horizontal lines max amount of cars is: " +
                        horizontalLines * verticalLines * Constants.CarAmountNormalizationConstant);
                    this.CarAmountTextBox.Text = (horizontalLines * verticalLines * Constants.CarAmountNormalizationConstant).ToString();
                    return;
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
                this.Clear();
                return;
            }
            this.Hide();
            var simulationController = new SimulationController(horizontalLines, verticalLines, desiredNumberOfVehicles);
            simulationController.InitSimulation();
            var graphicsController = new GraphicsController(simulationController.World, simulationController.PerformSimulationTick);
            graphicsController.Run(Constants.TicksPerSecond);
            this.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window win2 = new Window();
            win2.Height = Constants.HelpWindowHeight;
            win2.Width = Constants.HelpWindowWidth;
            win2.ResizeMode = ResizeMode.NoResize;
            win2.Title = "Help";
            win2.Content = "Placeholder for description...";
            win2.Show();
        }
    }
}
