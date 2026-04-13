//Final Submission Luyakha Ntshobane (ST10485641)
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace CyberSecurityChatbot
{
    public partial class MainWindow : Window
    {
        // --- 1. DELEGATE DEFINITION ---
        // This delegate defines the signature for any method that handles bot responses
        public delegate void ResponseHandler(string message);

        // --- 2. MEMORY VARIABLES ---
        private string userName = "";
        private string userMood = "neutral";
        private string lastTopic = "";

        // --- 3. GENERIC COLLECTIONS (Lists for Randomness) ---
        private List<string> phishingTips = new List<string>
        {
            "Check the sender's email address for slight misspellings.",
            "Never click 'Update Account' links in an unexpected email.",
            "Hover your mouse over a link to see where it actually leads.",
            "Be suspicious of emails that create a false sense of urgency."
        };

        public MainWindow()
        {
            InitializeComponent();
            DisplayWelcomeMessage();
        }

        private void DisplayWelcomeMessage()
        {
            // ASCII Art Requirement (Task 1 Translation)
            string asciiArt = "  ______      __               \n / ____/_  __/ /_  ___  ____ _\n/ /   / / / / __ \\/ _ \\/ __ `/\n/ /___/ /_/ / /_/ /  __/ /_/ / \n\\____/\\__, /_.___/\\___/\\__, /  \n     /____/           /____/   \n";
            ChatDisplay.Text = asciiArt + "\nBot: Welcome! I am your Cyber-Security bot. What is your name?\n\n";
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = UserInput.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            ChatDisplay.Text += $"You: {input}\n";

            // --- 4. USING THE DELEGATE ---
            // We point the delegate to our 'OutputToChat' method
            ResponseHandler handler = new ResponseHandler(OutputToChat);

            string botResult = ProcessLogic(input.ToLower());
            handler(botResult); // Executing the delegate

            UserInput.Clear();
            ChatDisplay.ScrollToEnd();
        }

        // The method the delegate calls
        private void OutputToChat(string message)
        {
            ChatDisplay.Text += $"Bot: {message}\n\n";
        }

        private string ProcessLogic(string input)
        {
            // MEMORY: Capture Name
            if (string.IsNullOrEmpty(userName))
            {
                userName = UserInput.Text;
                return $"Hello {userName}! I've noted your name. How are you feeling about your digital safety today?";
            }

            // SENTIMENT DETECTION
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("anxious"))
            {
                userMood = "anxious";
                return "I can hear that you're feeling a bit uneasy. Don't worry, staying informed is the best defense! What topic should we secure first: Passwords or Scams?";
            }

            // KEYWORD RECOGNITION & RANDOM RESPONSES
            if (input.Contains("password"))
            {
                lastTopic = "password";
                return "Passwords should be like toothbrushes: choose a good one, don't share it, and change it regularly! Want another tip?";
            }

            if (input.Contains("phishing") || input.Contains("scam"))
            {
                lastTopic = "phishing";
                Random rng = new Random();
                return phishingTips[rng.Next(phishingTips.Count)]; // Random Selection
            }

            // CONVERSATION FLOW (Follow-up)
            if (input.Contains("more") || input.Contains("another") || input.Contains("yes"))
            {
                if (lastTopic == "password") return "Use a passphrase! 'Purple-Cow-Jumps-High!' is stronger than 'Password123'.";
                if (lastTopic == "phishing") return "Always check for 'HTTPS' in the URL, though even that isn't 100% proof anymore.";
            }

            // DEFAULT (Error Handling)
            return "I'm not quite sure about that. Try asking me about 'passwords' or 'phishing tips'.";
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ChatDisplay.Clear();
            userName = "";
            userMood = "neutral";
            DisplayWelcomeMessage();
        }
    }
}
