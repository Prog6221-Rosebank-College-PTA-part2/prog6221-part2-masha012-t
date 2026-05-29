using System;
using System.Windows.Media;

namespace IntegratedCollegeManagementSystem
{
    public class ChatMessage
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string Alignment { get; set; }
        public Brush Background { get; set; }
        public Brush SenderColor { get; set; }

        public ChatMessage(string sender, string message, bool isUser)
        {
            Sender = sender;
            Message = message;
            Timestamp = DateTime.Now;

            if (isUser)
            {
                Alignment = "Right";
                Background = new SolidColorBrush(Color.FromRgb(233, 69, 96));
                SenderColor = new SolidColorBrush(Colors.White);
            }
            else
            {
                Alignment = "Left";
                Background = new SolidColorBrush(Color.FromRgb(15, 52, 96));
                SenderColor = new SolidColorBrush(Color.FromRgb(78, 205, 196));
            }
        }

        public string FormattedTimestamp => Timestamp.ToString("HH:mm:ss");
    }

    public class SentimentStats
    {
        public int PositiveCount { get; set; }
        public int NeutralCount { get; set; }
        public int WorriedCount { get; set; }
        public int FrustratedCount { get; set; }
        public int CuriousCount { get; set; }

        public double PositivePercentage => GetPercentage(PositiveCount);
        public double NeutralPercentage => GetPercentage(NeutralCount);
        public double WorriedPercentage => GetPercentage(WorriedCount);
        public double FrustratedPercentage => GetPercentage(FrustratedCount);
        public double CuriousPercentage => GetPercentage(CuriousCount);  // Add this line

        private double GetPercentage(int count)
        {
            int total = PositiveCount + NeutralCount + WorriedCount + FrustratedCount + CuriousCount;
            return total == 0 ? 0 : (double)count / total * 100;
        }
    }
}