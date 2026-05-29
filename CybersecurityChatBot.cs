using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace IntegratedCollegeManagementSystem
{
    public class CybersecurityChatbot
    {
        private Dictionary<string, List<string>> _keywordResponses;
        private Dictionary<string, List<string>> _followUpResponses;
        private List<string> _greetings;
        private List<string> _farewells;
        private List<string> _defaultResponses;
        private List<string> _encouragementResponses;

        public Dictionary<string, object> UserMemory { get; private set; }
        public string LastTopic { get; set; }
        public List<string> ConversationHistory { get; private set; }
        public SentimentStats SentimentStatistics { get; private set; }

        public CybersecurityChatbot()
        {
            InitializeMemory();
            InitializeResponses();
            InitializeSentimentStats();
        }

        private void InitializeMemory()
        {
            UserMemory = new Dictionary<string, object>
            {
                { "Name", null },
                { "Interests", new List<string>() },
                { "Concerns", new List<string>() },
                { "LastTopic", null },
                { "ConversationHistory", new List<Tuple<string, string, DateTime>>() }
            };
            ConversationHistory = new List<string>();
        }

        private void InitializeSentimentStats()
        {
            SentimentStatistics = new SentimentStats();
        }

        private void InitializeResponses()
        {
            _keywordResponses = new Dictionary<string, List<string>>
            {
                { "password", new List<string> {
                    "🔐 Passwords should be at least 12 characters long with a mix of uppercase, lowercase, numbers, and symbols.",
                    "💡 Did you know? Using a password manager can help you create and store strong, unique passwords for each account.",
                    "🛡️ Enable two-factor authentication (2FA) whenever possible - it adds an extra layer of security beyond your password.",
                    "Never use the same password across multiple accounts. If one gets compromised, others remain safe!"
                }},
                { "phishing", new List<string> {
                    "🎣 Phishing attacks try to trick you into revealing sensitive information. Always verify email senders before clicking links!",
                    "⚠️ Watch for red flags: urgent language, spelling errors, suspicious links, or requests for personal information.",
                    "📧 Never click on links in unsolicited emails. Instead, type the website address directly into your browser.",
                    "Legitimate companies never ask for passwords or credit card details via email!"
                }},
                { "malware", new List<string> {
                    "🦠 Malware includes viruses, ransomware, and spyware. Keep your antivirus software updated!",
                    "💻 Avoid downloading software from untrusted sources, and be cautious with email attachments.",
                    "🔄 Regular backups are your best defense against ransomware attacks.",
                    "Windows Defender is good, but consider additional anti-malware tools for comprehensive protection."
                }},
                { "privacy", new List<string> {
                    "👁️ Review your social media privacy settings regularly - you might be sharing more than you realize!",
                    "🔒 Use a VPN on public Wi-Fi to encrypt your internet traffic and protect your privacy.",
                    "📱 Check app permissions on your phone - many apps request access they don't actually need.",
                    "Consider using privacy-focused browsers like Firefox or Brave with tracking protection enabled."
                }},
                { "scam", new List<string> {
                    "🚨 If something sounds too good to be true, it probably is! Scammers prey on urgency and emotion.",
                    "📞 Never give personal information over the phone unless you initiated the call to a verified number.",
                    "💵 Legitimate organizations will never ask for payment via gift cards or wire transfers.",
                    "The 'Nigerian Prince' scam is still active - be skeptical of unexpected large sums of money."
                }},
                { "update", new List<string> {
                    "🔄 Software updates often include critical security patches. Don't delay installing them!",
                    "⚙️ Enable automatic updates when possible to ensure you're always protected.",
                    "📱 This applies to all devices: computers, phones, tablets, and even smart home devices.",
                    "Outdated plugins like Java and Flash are common attack vectors - remove if not needed."
                }},
                { "backup", new List<string> {
                    "💾 Follow the 3-2-1 backup rule: 3 copies, 2 different media types, 1 off-site backup.",
                    "☁️ Cloud backups are convenient, but ensure they're encrypted for sensitive data.",
                    "⏰ Test your backups regularly - a backup is only useful if you can restore from it!",
                    "Automated backup solutions like Backblaze or Carbonite make regular backups effortless."
                }},
                { "wifi", new List<string> {
                    "📶 Public Wi-Fi is convenient but risky. Avoid accessing sensitive accounts on unsecured networks.",
                    "🔐 Look for 'HTTPS' in the URL - it indicates encrypted communication with the website.",
                    "🏠 Secure your home Wi-Fi with WPA3 or WPA2 encryption and a strong administrator password.",
                    "Disable WPS (Wi-Fi Protected Setup) on your router - it has known security vulnerabilities."
                }},
                { "social media", new List<string> {
                    "📱 Think before you post! Oversharing can help attackers gather information for social engineering.",
                    "👤 Use different profile pictures for different platforms to make it harder to connect your accounts.",
                    "🔒 Enable login alerts and review connected apps on your social media accounts.",
                    "Avoid posting your location or travel plans in real-time - share after you've left."
                }},
                { "email", new List<string> {
                    "📧 Check the sender's email address carefully - scammers often use addresses similar to legitimate ones.",
                    "🔍 Hover over links before clicking to see the actual destination URL.",
                    "📎 Be especially cautious with unexpected attachments, even from people you know.",
                    "Enable 2FA on your email account - it's often the key to resetting passwords for other services."
                }}
            };

            _followUpResponses = new Dictionary<string, List<string>>
            {
                { "more", new List<string> {
                    "I can tell you more! {0}",
                    "Here's another important point: {0}",
                    "Let me add to that: {0}"
                }},
                { "explain", new List<string> {
                    "Let me break that down: {0}",
                    "In simple terms, {0}",
                    "To explain further: {0}"
                }},
                { "example", new List<string> {
                    "For example: {0}",
                    "Here's a real-world scenario: {0}",
                    "Consider this situation: {0}"
                }}
            };

            _greetings = new List<string>
            {
                "Hello! 👋 I'm your Cybersecurity Awareness Assistant. How can I help you stay safe online today?",
                "Hi there! 🛡️ Ready to learn about cybersecurity? What would you like to know?",
                "Welcome! 🔒 I'm here to help you navigate the digital world safely. Ask me anything about cybersecurity!"
            };

            _farewells = new List<string>
            {
                "Stay safe online! 🔐 Remember, cybersecurity is everyone's responsibility.",
                "Goodbye! 🛡️ Keep practicing good security habits!",
                "Take care! 💻 Feel free to return if you have more cybersecurity questions."
            };

            _defaultResponses = new List<string>
            {
                "I'm not sure I understand. Could you rephrase that? I specialize in cybersecurity topics like passwords, phishing, malware, and privacy.",
                "Hmm, I'm not following. 🤔 Try asking me about passwords, phishing, malware, online privacy, or how to spot scams!",
                "I want to help, but I didn't quite catch that. Could you ask about a specific cybersecurity topic?",
                "That's an interesting point, but I'm specialized in cybersecurity. Ask me about protecting yourself online!"
            };

            _encouragementResponses = new List<string>
            {
                "Great question! 💪 ",
                "That's an important topic! 📚 ",
                "Excellent thinking! 🌟 ",
                "I'm glad you asked! 👍 ",
                "You're asking the right questions! 🎯 "
            };
        }

        public string DetectSentiment(string text)
        {
            string textLower = text.ToLower();

            // Simple keyword-based sentiment detection
            if (textLower.Contains("worried") || textLower.Contains("scared") ||
                textLower.Contains("afraid") || textLower.Contains("nervous") ||
                textLower.Contains("anxious"))
            {
                SentimentStatistics.WorriedCount++;
                return "worried";
            }
            else if (textLower.Contains("frustrated") || textLower.Contains("annoyed") ||
                     textLower.Contains("angry") || textLower.Contains("upset"))
            {
                SentimentStatistics.FrustratedCount++;
                return "frustrated";
            }
            else if (textLower.Contains("curious") || textLower.Contains("interested") ||
                     textLower.Contains("want to learn") || textLower.Contains("tell me"))
            {
                SentimentStatistics.CuriousCount++;
                return "curious";
            }
            else if (textLower.Contains("happy") || textLower.Contains("great") ||
                     textLower.Contains("good") || textLower.Contains("thanks") ||
                     textLower.Contains("thank you"))
            {
                SentimentStatistics.PositiveCount++;
                return "positive";
            }
            else
            {
                SentimentStatistics.NeutralCount++;
                return "neutral";
            }
        }

        public string AdjustResponseBySentiment(string response, string sentiment)
        {
            switch (sentiment)
            {
                case "worried":
                    return "It's completely understandable to feel worried about this. 😟 Let me reassure you: " + response +
                           " Remember, being aware is the first step to staying safe!";
                case "frustrated":
                    return "I hear your frustration. Cybersecurity can be overwhelming, but I'm here to help! 💪 " + response +
                           " Take a deep breath - we'll figure this out together.";
                case "curious":
                    return "I love your curiosity! 🌟 " + response + " Keep asking great questions!";
                case "positive":
                    return GetRandomFromList(_encouragementResponses) + response;
                default:
                    return response;
            }
        }

        public List<string> RecognizeKeywords(string userInput)
        {
            string userInputLower = userInput.ToLower();
            var detectedTopics = new List<string>();

            foreach (var keyword in _keywordResponses.Keys)
            {
                if (userInputLower.Contains(keyword))
                {
                    detectedTopics.Add(keyword);
                }
            }

            // Check for common variations
            if ((userInputLower.Contains("pass") || userInputLower.Contains("code")) &&
                (userInputLower.Contains("word") || userInputLower.Contains("phrase")))
            {
                if (!detectedTopics.Contains("password"))
                    detectedTopics.Add("password");
            }
            if (userInputLower.Contains("virus") || userInputLower.Contains("ransomware") ||
                userInputLower.Contains("trojan"))
            {
                if (!detectedTopics.Contains("malware"))
                    detectedTopics.Add("malware");
            }

            return detectedTopics.Distinct().ToList();
        }

        public string HandleFollowUp(string userInput)
        {
            string userInputLower = userInput.ToLower();

            if (!string.IsNullOrEmpty(LastTopic) && _keywordResponses.ContainsKey(LastTopic))
            {
                string responseType = null;

                if (userInputLower.Contains("more") || userInputLower.Contains("another") ||
                    userInputLower.Contains("additional") || userInputLower.Contains("else"))
                {
                    responseType = "more";
                }
                else if (userInputLower.Contains("explain") || userInputLower.Contains("understand") ||
                         userInputLower.Contains("clarify") || userInputLower.Contains("detail"))
                {
                    responseType = "explain";
                }
                else if (userInputLower.Contains("example") || userInputLower.Contains("instance") ||
                         userInputLower.Contains("scenario"))
                {
                    responseType = "example";
                }

                if (responseType != null && _followUpResponses.ContainsKey(responseType))
                {
                    var availableResponses = _keywordResponses[LastTopic];
                    var random = new Random();
                    string currentResponse = availableResponses[random.Next(availableResponses.Count)];

                    // Remove emojis for cleaner integration
                    string cleanResponse = Regex.Replace(currentResponse, @"[🔐💡🛡️🎣⚠️📧🦠💻🔄👁️🔒📱🚨📞💵⚙️📻💾☁️⏰📶🏠📱👤]", "");
                    cleanResponse = cleanResponse.Trim();

                    var templates = _followUpResponses[responseType];
                    string template = templates[random.Next(templates.Count)];
                    return string.Format(template, cleanResponse);
                }
            }

            return null;
        }

        public string RememberUserInfo(string userInput)
        {
            string userInputLower = userInput.ToLower();

            // Remember name
            if (userInputLower.Contains("my name is") || userInputLower.Contains("i'm") ||
                userInputLower.Contains("i am"))
            {
                var match = Regex.Match(userInputLower, @"(?:my name is|i'm|i am)\s+([a-z]+)");
                if (match.Success && UserMemory["Name"] == null)
                {
                    string name = char.ToUpper(match.Groups[1].Value[0]) + match.Groups[1].Value.Substring(1);
                    UserMemory["Name"] = name;
                    return $"Nice to meet you, {name}! I'll remember that. 😊";
                }
            }

            // Remember interests
            if (userInputLower.Contains("interested in") || userInputLower.Contains("want to learn about") ||
                userInputLower.Contains("curious about"))
            {
                foreach (var topic in _keywordResponses.Keys)
                {
                    if (userInputLower.Contains(topic))
                    {
                        var interests = UserMemory["Interests"] as List<string>;
                        if (!interests.Contains(topic))
                        {
                            interests.Add(topic);
                            return $"Great! I'll remember that you're interested in {topic}. It's an important cybersecurity topic! 📚";
                        }
                        break;
                    }
                }
            }

            // Remember concerns based on sentiment
            string sentiment = DetectSentiment(userInput);
            if (sentiment == "worried" || sentiment == "frustrated")
            {
                foreach (var topic in _keywordResponses.Keys)
                {
                    if (userInputLower.Contains(topic))
                    {
                        var concerns = UserMemory["Concerns"] as List<string>;
                        if (!concerns.Contains(topic))
                        {
                            concerns.Add(topic);
                            return $"I understand your concern about {topic}. Many people feel the same way. Let me help you stay protected! 🛡️";
                        }
                        break;
                    }
                }
            }

            return null;
        }

        public string GetResponse(string userInput)
        {
            userInput = userInput.Trim();

            if (string.IsNullOrWhiteSpace(userInput))
                return "Please type a message. I'm here to help with cybersecurity! 🔒";

            // Store in conversation history
            var history = UserMemory["ConversationHistory"] as List<Tuple<string, string, DateTime>>;
            history.Add(Tuple.Create("user", userInput, DateTime.Now));
            ConversationHistory.Add($"User: {userInput}");

            // Detect sentiment
            string sentiment = DetectSentiment(userInput);

            // Check for greetings
            if (userInput.ToLower().Contains("hello") || userInput.ToLower().Contains("hi") ||
                userInput.ToLower().Contains("hey") || userInput.ToLower().Contains("greetings"))
            {
                string response = GetRandomFromList(_greetings);
                if (UserMemory["Name"] != null)
                    response = $"Welcome back, {UserMemory["Name"]}! " + response;
                return response;
            }

            // Check for farewells
            if (userInput.ToLower().Contains("bye") || userInput.ToLower().Contains("goodbye") ||
                userInput.ToLower().Contains("exit") || userInput.ToLower().Contains("quit") ||
                userInput.ToLower().Contains("see you"))
            {
                return GetRandomFromList(_farewells);
            }

            // Check for memory recall
            string memoryResponse = RememberUserInfo(userInput);
            if (memoryResponse != null)
                return memoryResponse;

            // Check for follow-up questions
            string followUp = HandleFollowUp(userInput);
            if (followUp != null)
            {
                return AdjustResponseBySentiment(followUp, sentiment);
            }

            // Recognize keywords
            var topics = RecognizeKeywords(userInput);

            if (topics.Count > 0)
            {
                // Store last topic for follow-ups
                LastTopic = topics[0];
                UserMemory["LastTopic"] = LastTopic;

                // Get response for detected topic
                var random = new Random();
                string response = GetRandomFromList(_keywordResponses[topics[0]]);

                // Add personalization if user name is known
                if (UserMemory["Name"] != null)
                {
                    response = $"{UserMemory["Name"]}, " + response.ToLower();
                }

                // Adjust by sentiment
                string adjustedResponse = AdjustResponseBySentiment(response, sentiment);

                // Store bot response in history
                history.Add(Tuple.Create("bot", adjustedResponse, DateTime.Now));
                ConversationHistory.Add($"Bot: {adjustedResponse}");

                return adjustedResponse;
            }

            // Default response for unknown input
            string defaultResponse = GetRandomFromList(_defaultResponses);
            if (UserMemory["Name"] != null)
                defaultResponse = $"{UserMemory["Name"]}, {defaultResponse.ToLower()}";

            history.Add(Tuple.Create("bot", defaultResponse, DateTime.Now));
            ConversationHistory.Add($"Bot: {defaultResponse}");

            return defaultResponse;
        }

        private string GetRandomFromList(List<string> list)
        {
            var random = new Random();
            return list[random.Next(list.Count)];
        }

        public void ResetMemory()
        {
            InitializeMemory();
            LastTopic = null;
            InitializeSentimentStats();
        }
    }
}