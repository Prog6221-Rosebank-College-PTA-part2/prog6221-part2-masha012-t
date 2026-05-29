using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using Microsoft.Win32;

// These are the correct using statements for speech
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace IntegratedCollegeManagementSystem
{
    public partial class MainWindow : Window
    {
        private CybersecurityChatbot _chatbot;
        private DateTime _sessionStartTime;
        private int _messageCount;
        private string _lastResponse;

        // Voice Components
        private SpeechSynthesizer _synthesizer;
        private SpeechRecognitionEngine _recognizer;
        private bool _isListening;

        // ASCII Art Collection
        private List<AsciiArtItem> _asciiArts;
        private int _currentAsciiIndex;

        public MainWindow()
        {
            InitializeComponent();
            InitializeChatbot();
            InitializeVoice();
            InitializeAsciiArt();
            _sessionStartTime = DateTime.Now;

            // Update session timer
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => UpdateSessionDisplay();
            timer.Start();
        }

        private void InitializeChatbot()
        {
            _chatbot = new CybersecurityChatbot();
            _messageCount = 0;

            AddChatMessage("Assistant",
                "Welcome to Cybersecurity Awareness Assistant! 🔒\n\n" +
                "I can help you with:\n" +
                "• Creating strong passwords\n" +
                "• Recognizing phishing attempts\n" +
                "• Protecting against malware\n" +
                "• Maintaining online privacy\n\n" +
                "Try asking: 'How do I create a strong password?' or 'What is phishing?'",
                false);

            UpdateUserInfo();
            UpdateStatus("Ready to help with cybersecurity!");
        }

        private void InitializeVoice()
        {
            try
            {
                _synthesizer = new SpeechSynthesizer();

                // Populate voice options
                VoiceSelector.Items.Clear();
                foreach (var voice in _synthesizer.GetInstalledVoices())
                {
                    VoiceSelector.Items.Add(voice.VoiceInfo.Name);
                }

                if (VoiceSelector.Items.Count > 0)
                    VoiceSelector.SelectedIndex = 0;

                _synthesizer.Volume = 80;
                _synthesizer.Rate = 0;

                UpdateVoiceStatus("Voice ready");
            }
            catch (Exception ex)
            {
                UpdateVoiceStatus($"Voice unavailable: {ex.Message}");
            }
        }

        private void InitializeAsciiArt()
        {
            _asciiArts = new List<AsciiArtItem>
            {
                new AsciiArtItem { Title = "🛡️ SECURITY SHIELD", Art = @"
╔══════════════════════════════════════════════════════════╗
║                     🔒 SECURITY SHIELD 🔒               ║
║                   Protecting Your Digital World         ║
║              ┌─────────────────────────────────┐       ║
║              │      █████████████████████      │       ║
║              │      █░░░░░░░░░░░░░░░░░█      │       ║
║              │      █░█████████████░█      │       ║
║              │      █░█░░░░░░░░░░█░█      │       ║
║              │      █░█░███████░█░█      │       ║
║              │      █░░░░░░░░░░░░░█      │       ║
║              │      █████████████████      │       ║
║              └─────────────────────────────────┘       ║
╚══════════════════════════════════════════════════════════╝"},

                new AsciiArtItem { Title = "🤖 AI ASSISTANT", Art = @"
╔══════════════════════════════════════════════════════════╗
║                     🤖 AI ASSISTANT 🤖                  ║
║                  Your Intelligent Helper                ║
║                                                          ║
║                    ┌─────────────────┐                  ║
║                    │  ╭───────────╮  │                  ║
║                    │  │  👤 AI    │  │                  ║
║                    │  │  💬 I'm   │  │                  ║
║                    │  │  here to  │  │                  ║
║                    │  │  help!    │  │                  ║
║                    │  ╰───────────╯  │                  ║
║                    │   [#########]   │                  ║
║                    │   │  ○   ○  │   │                  ║
║                    │   │    ▼    │   │                  ║
║                    │   └─────────┘   │                  ║
║                    └─────────────────┘                  ║
╚══════════════════════════════════════════════════════════╝"},

                new AsciiArtItem { Title = "🔐 CYBER LOCK", Art = @"
╔══════════════════════════════════════════════════════════╗
║                    🔐 CYBER LOCK 🔐                      ║
║                    Stay Protected                       ║
║                                                          ║
║                       .---.                              ║
║                      /     \                             ║
║                      |     |                             ║
║                      |     |                             ║
║                      |     |                             ║
║                   .===========.                          ║
║                   |  |  |  |  |                          ║
║                   |  |  |  |  |                          ║
║                   '==========='                          ║
║                      |     |                             ║
║                      |     |                             ║
║                      '-----'                             ║
║                  🔒 LOCKED AND SECURE 🔒                 ║
╚══════════════════════════════════════════════════════════╝"},

                new AsciiArtItem { Title = "💻 CODE SECURITY", Art = @"
╔══════════════════════════════════════════════════════════╗
║                   💻 CODE SECURITY 💻                   ║
║                 Secure Development                     ║
║    ┌─────────────────────────────────────────┐         ║
║    │  function secureCode() {                 │         ║
║    │      let data = encrypt(userInput);     │         ║
║    │      if (validateInput(data)) {         │         ║
║    │          return sanitize(data);         │         ║
║    │      } else {                           │         ║
║    │          throw new SecurityError();     │         ║
║    │      }                                  │         ║
║    │  }                                       │         ║
║    └─────────────────────────────────────────┘         ║
║           🔒 Encryption  🔒 Validation                 ║
╚══════════════════════════════════════════════════════════╝"}
            };

            _currentAsciiIndex = 0;
            UpdateAsciiDisplay();
        }

        private void UpdateAsciiDisplay()
        {
            if (_asciiArts != null && _asciiArts.Count > 0)
            {
                var current = _asciiArts[_currentAsciiIndex];
                AsciiTitle.Text = current.Title;
                AsciiArtDisplay.Text = current.Art;
            }
        }

        private void PrevAscii_Click(object sender, RoutedEventArgs e)
        {
            _currentAsciiIndex--;
            if (_currentAsciiIndex < 0)
                _currentAsciiIndex = _asciiArts.Count - 1;
            UpdateAsciiDisplay();
        }

        private void NextAscii_Click(object sender, RoutedEventArgs e)
        {
            _currentAsciiIndex++;
            if (_currentAsciiIndex >= _asciiArts.Count)
                _currentAsciiIndex = 0;
            UpdateAsciiDisplay();
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !(Keyboard.Modifiers == ModifierKeys.Shift))
            {
                e.Handled = true;
                SendMessage();
            }
        }

        private void SendMessage()
        {
            string userInput = MessageInput.Text.Trim();

            if (string.IsNullOrEmpty(userInput))
                return;

            AddChatMessage("You", userInput, true);
            _messageCount++;
            UpdateMessageCount();

            // Get bot response
            string response = _chatbot.GetResponse(userInput);
            _lastResponse = response;
            AddChatMessage("Assistant", response, false);

            // Update UI
            UpdateUserInfo();
            UpdateSentimentDisplay();

            MessageInput.Clear();
            UpdateStatus("Response received");
        }

        private void AddChatMessage(string sender, string message, bool isUser)
        {
            Dispatcher.Invoke(() =>
            {
                ChatMessagesList.Items.Add(new ChatMessageItem
                {
                    Sender = sender,
                    Message = message,
                    IsUser = isUser,
                    TimeDisplay = DateTime.Now.ToString("HH:mm:ss")
                });
                ChatMessagesList.ScrollIntoView(ChatMessagesList.Items[ChatMessagesList.Items.Count - 1]);
            });
        }

        private void ClearChat_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Clear all chat messages?", "Clear Chat",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ChatMessagesList.Items.Clear();
                AddChatMessage("Assistant", "Chat cleared! How can I help you?", false);
                UpdateStatus("Chat cleared");
            }
        }

        private void SpeakLastResponse_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_lastResponse))
            {
                SpeakText(_lastResponse);
            }
            else
            {
                UpdateStatus("No response to speak yet");
            }
        }

        private void StartVoiceInput_Click(object sender, RoutedEventArgs e)
        {
            if (_isListening)
            {
                StopVoiceRecognition();
            }
            else
            {
                StartVoiceRecognition();
            }
        }

        private void StartVoiceRecognition()
        {
            try
            {
                if (_recognizer == null)
                {
                    _recognizer = new SpeechRecognitionEngine();
                    _recognizer.SetInputToDefaultAudioDevice();

                    // Create simple grammar
                    var choices = new Choices();
                    choices.Add(new string[] {
                        "hello", "help", "passwords", "phishing", "malware",
                        "privacy", "scams", "updates", "backups",
                        "tell me about passwords", "what is phishing",
                        "how to create strong password", "protect my privacy"
                    });

                    var grammar = new Grammar(new GrammarBuilder(choices));
                    _recognizer.LoadGrammar(grammar);
                    _recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
                }

                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
                _isListening = true;
                VoiceInputBtn.Content = "🎙️ Listening... Click to Stop";
                VoiceInputBtn.Background = new SolidColorBrush(Color.FromRgb(233, 69, 96));
                UpdateVoiceStatus("Listening for voice input...");
            }
            catch (Exception ex)
            {
                UpdateVoiceStatus($"Voice input error: {ex.Message}");
            }
        }

        private void StopVoiceRecognition()
        {
            if (_recognizer != null)
            {
                _recognizer.RecognizeAsyncStop();
                _isListening = false;
                VoiceInputBtn.Content = "🎙️ Start Voice Input";
                VoiceInputBtn.Background = new SolidColorBrush(Color.FromRgb(15, 52, 96));
                UpdateVoiceStatus("Voice input stopped");
            }
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string recognized = e.Result.Text;
            Dispatcher.Invoke(() =>
            {
                MessageInput.Text = recognized;
                UpdateStatus($"Recognized: {recognized}");
                SendMessage();
            });
        }

        private void SpeakText(string text)
        {
            try
            {
                if (_synthesizer != null && !string.IsNullOrEmpty(text))
                {
                    _synthesizer.SpeakAsync(text);
                    UpdateVoiceStatus("Speaking...");

                    // Reset status after speaking
                    var timer = new System.Windows.Threading.DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(3);
                    timer.Tick += (s, e) =>
                    {
                        timer.Stop();
                        UpdateVoiceStatus("Voice ready");
                    };
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                UpdateVoiceStatus($"Speech error: {ex.Message}");
            }
        }

        private void VoiceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (_synthesizer != null && VoiceSelector.SelectedItem != null)
                {
                    var selectedVoice = VoiceSelector.SelectedItem.ToString();
                    _synthesizer.SelectVoice(selectedVoice);
                    UpdateVoiceStatus($"Voice changed to {selectedVoice}");
                }
            }
            catch (Exception ex)
            {
                UpdateVoiceStatus($"Voice change failed: {ex.Message}");
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_synthesizer != null)
            {
                _synthesizer.Volume = (int)VolumeSlider.Value;
            }
        }

        private void RateSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_synthesizer != null)
            {
                _synthesizer.Rate = (int)RateSlider.Value;
            }
        }

        private void Topic_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                string topic = button.Content.ToString();
                // Remove emoji from topic
                string cleanTopic = System.Text.RegularExpressions.Regex.Replace(topic, @"[🔑🎣🦠👁️🚨🔄💾📶]", "").Trim();
                MessageInput.Text = $"Tell me about {cleanTopic.ToLower()}";
                SendMessage();
            }
        }

        private void HelpGuide_Click(object sender, RoutedEventArgs e)
        {
            string helpText =
                "🔐 CYBERSECURITY TOPICS\n\n" +
                "• Passwords - Create strong passwords, use password managers, enable 2FA\n" +
                "• Phishing - Recognize email scams, verify senders, don't click suspicious links\n" +
                "• Malware - Use antivirus, avoid untrusted downloads, regular backups\n" +
                "• Privacy - Review privacy settings, use VPN, check app permissions\n\n" +
                "💡 TIPS\n\n" +
                "• Ask 'Tell me more' for additional information\n" +
                "• Say 'I'm worried about...' for empathetic responses\n" +
                "• Tell me your name for personalized responses\n\n" +
                "🎤 VOICE COMMANDS\n\n" +
                "• Click microphone button and speak your question\n" +
                "• Click speaker button to hear responses\n" +
                "• Adjust voice speed and volume using sliders";

            MessageBox.Show(helpText, "Help Guide", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ResetMemory_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Reset all memory? This will forget your name and preferences.",
                "Reset Memory", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _chatbot.ResetMemory();
                UpdateUserInfo();
                UpdateStatus("Memory reset successfully");
                AddChatMessage("Assistant", "I've reset my memory. You can tell me your name again!", false);
            }
        }

        private void ExportChat_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Text Files (*.txt)|*.txt";
            dialog.DefaultExt = ".txt";
            dialog.FileName = $"ChatLog_{DateTime.Now:yyyyMMdd_HHmmss}";

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    using (var writer = new StreamWriter(dialog.FileName))
                    {
                        writer.WriteLine("CYBERSECURITY CHAT LOG");
                        writer.WriteLine($"Date: {DateTime.Now}");
                        writer.WriteLine(new string('=', 50));
                        writer.WriteLine();

                        foreach (ChatMessageItem msg in ChatMessagesList.Items)
                        {
                            writer.WriteLine($"[{msg.TimeDisplay}] {msg.Sender}:");
                            writer.WriteLine(msg.Message);
                            writer.WriteLine();
                        }
                    }

                    MessageBox.Show($"Chat exported to:\n{dialog.FileName}", "Export Successful",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateStatus("Chat exported");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Export failed: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void UpdateUserInfo()
        {
            Dispatcher.Invoke(() =>
            {
                string userName = _chatbot.UserMemory.ContainsKey("Name") ?
                    (_chatbot.UserMemory["Name"] as string ?? "Not set") : "Not set";
                UserNameDisplay.Text = userName;

                var interests = _chatbot.UserMemory.ContainsKey("Interests") ?
                    _chatbot.UserMemory["Interests"] as List<string> : null;
                UserInterestsDisplay.Text = (interests != null && interests.Count > 0) ?
                    string.Join(", ", interests) : "None";

                var concerns = _chatbot.UserMemory.ContainsKey("Concerns") ?
                    _chatbot.UserMemory["Concerns"] as List<string> : null;
                UserConcernsDisplay.Text = (concerns != null && concerns.Count > 0) ?
                    string.Join(", ", concerns) : "None";
            });
        }

        private void UpdateMessageCount()
        {
            Dispatcher.Invoke(() =>
            {
                MessageCountDisplay.Text = _messageCount.ToString();
            });
        }

        private void UpdateSessionDisplay()
        {
            Dispatcher.Invoke(() =>
            {
                var duration = DateTime.Now - _sessionStartTime;
                if (duration.TotalMinutes < 1)
                    SessionDurationDisplay.Text = $"{duration.Seconds} sec";
                else if (duration.TotalHours < 1)
                    SessionDurationDisplay.Text = $"{duration.Minutes} min";
                else
                    SessionDurationDisplay.Text = $"{duration.Hours}h {duration.Minutes}m";
            });
        }

        private void UpdateSentimentDisplay()
        {
            Dispatcher.Invoke(() =>
            {
                var stats = _chatbot.SentimentStatistics;

                PositiveProgress.Value = stats.PositivePercentage;
                NeutralProgress.Value = stats.NeutralPercentage;
                WorriedProgress.Value = stats.WorriedPercentage;
                CuriousProgress.Value = stats.CuriousPercentage;

                // Update mood badge
                double max = Math.Max(stats.PositivePercentage,
                               Math.Max(stats.NeutralPercentage,
                               Math.Max(stats.WorriedPercentage, stats.CuriousPercentage)));

                if (stats.PositivePercentage == max && max > 0)
                {
                    MoodText.Text = "Positive 😊";
                    MoodBadge.Background = new SolidColorBrush(Color.FromRgb(78, 205, 196));
                }
                else if (stats.WorriedPercentage == max && max > 0)
                {
                    MoodText.Text = "Worried 😟";
                    MoodBadge.Background = new SolidColorBrush(Color.FromRgb(255, 107, 107));
                }
                else if (stats.CuriousPercentage == max && max > 0)
                {
                    MoodText.Text = "Curious 🤔";
                    MoodBadge.Background = new SolidColorBrush(Color.FromRgb(255, 217, 61));
                }
                else
                {
                    MoodText.Text = "Neutral 😐";
                    MoodBadge.Background = new SolidColorBrush(Color.FromRgb(138, 155, 181));
                }
            });
        }

        private void UpdateStatus(string message)
        {
            Dispatcher.Invoke(() =>
            {
                StatusBarText.Text = $"✅ {message}";

                // Reset after 3 seconds
                var timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(3);
                timer.Tick += (s, e) =>
                {
                    timer.Stop();
                    if (StatusBarText.Text.Contains(message))
                        StatusBarText.Text = "✅ Ready";
                };
                timer.Start();
            });
        }

        private void UpdateVoiceStatus(string message)
        {
            Dispatcher.Invoke(() =>
            {
                VoiceStatus.Text = $"🔊 {message}";
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_recognizer != null)
            {
                _recognizer.RecognizeAsyncStop();
                _recognizer.Dispose();
            }
            if (_synthesizer != null)
            {
                _synthesizer.Dispose();
            }
            base.OnClosed(e);
        }
    }

    // Data Models
    public class ChatMessageItem
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public bool IsUser { get; set; }
        public string TimeDisplay { get; set; }

        public SolidColorBrush Background => IsUser ?
            new SolidColorBrush(Color.FromRgb(233, 69, 96)) :
            new SolidColorBrush(Color.FromRgb(15, 52, 96));

        public SolidColorBrush SenderColor => IsUser ?
            new SolidColorBrush(Colors.White) :
            new SolidColorBrush(Color.FromRgb(78, 205, 196));

        public HorizontalAlignment Alignment => IsUser ?
            HorizontalAlignment.Right :
            HorizontalAlignment.Left;
    }

    public class AsciiArtItem
    {
        public string Title { get; set; }
        public string Art { get; set; }
    }
}