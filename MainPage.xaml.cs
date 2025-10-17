using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;

namespace Dice_Roller
{
    public partial class MainPage : ContentPage
    {
        Random random = new Random();

        ObservableCollection<string> rollHistory = new ObservableCollection<string>();

        public MainPage()
        {
            InitializeComponent();
            HistoryList.ItemsSource = rollHistory;
        }

        private void OnRollDiceClicked(object sender, EventArgs e)
        {
            RollDice();
        }

        private async void RollDice()
        {
            int roll1 = random.Next(1, 7);
            int roll2 = random.Next(1, 7);
            int sum = roll1 + roll2;

            // Update images
            dice1image.Source = $"dice{roll1}image.png";
            dice2image.Source = $"dice{roll2}image.png";

            // Update result text
            ResultLabel.Text = $"You rolled {roll1} + {roll2} = {sum}";

            // Add to history
            rollHistory.Insert(0, $"{DateTime.Now:T} → {roll1} + {roll2} = {sum}");

            // Limit history length
            if (rollHistory.Count > 20)
                rollHistory.RemoveAt(rollHistory.Count - 1);


            var rollData = new
            {
                Roll1 = roll1,
                Roll2 = roll2,
                Sum = sum,
                Timestamp = DateTime.Now
            };

            var json = JsonSerializer.Serialize(rollData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            try
            {
                await client.PostAsync("http://192.168.10.177:5045/api/diceroll", content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending roll to API: {ex.Message}");
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Accelerometer.Start(SensorSpeed.Game);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Accelerometer.Stop();
            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            double totalAcceleration = Math.Abs(data.Acceleration.X) +
                                       Math.Abs(data.Acceleration.Y) +
                                       Math.Abs(data.Acceleration.Z);

            if (totalAcceleration > 3) // tweak threshold
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    RollDice();
                });
            }
        }

    }
}
