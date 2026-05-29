[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/Apa4hIya)
# 🛡️ Cybersecurity Awareness Chatbot

An intelligent, AI-powered cybersecurity assistant with voice capabilities, sentiment detection, and memory features. Built with WPF and C# for the College Management System.

## 📋 Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Technical Stack](#technical-stack)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Running the Application](#running-the-application)
- [Usage Guide](#usage-guide)
- [Voice Recording Setup](#voice-recording-setup)
- [Project Structure](#project-structure)
- [Features in Detail](#features-in-detail)
- [Troubleshooting](#troubleshooting)
- [Future Enhancements](#future-enhancements)
- [Contributing](#contributing)
- [License](#license)

## Overview

The Cybersecurity Awareness Chatbot is an interactive desktop application designed to educate users about online safety and cybersecurity best practices. It combines AI-powered conversation, sentiment analysis, memory retention, and voice capabilities to create an engaging learning experience.

### Key Capabilities
- **Intelligent Conversations**: Understands cybersecurity topics and provides relevant responses
- **Emotional Intelligence**: Detects user sentiment and adjusts responses accordingly
- **Memory System**: Remembers user preferences, name, and past conversations
- **Voice Integration**: Text-to-speech and speech recognition with custom voice recording support
- **ASCII Art Gallery**: Visual engagement with cybersecurity-themed ASCII art

## Features

### 1. 🤖 Smart Chatbot
- Keyword recognition for 10+ cybersecurity topics
- Context-aware follow-up responses
- Natural conversation flow management
- Personalized responses using user memory

### 2. 🎭 Sentiment Detection
- Identifies user emotions: Positive, Neutral, Worried, Frustrated, Curious
- Empathetic response adjustment
- Real-time sentiment visualization
- Mood tracking throughout conversation

### 3. 🧠 Memory System
- Remembers user's name
- Tracks topics of interest
- Records expressed concerns
- Maintains conversation context
- Persistent session memory

### 4. 🎤 Voice Capabilities
- Text-to-Speech with multiple voice options
- Speech recognition for voice input
- Custom voice recording support (WAV files)
- Adjustable speech rate and volume
- Fallback to system voices

### 5. 🎨 User Interface
- Modern dark theme design
- Real-time conversation display
- Sentiment analysis progress bars
- Session statistics tracking
- ASCII art gallery with navigation

### 6. 📊 Conversation Management
- Export chat history to text files
- Clear conversation option
- Session duration tracking
- Message count statistics
- Topic quick-access buttons

## Technical Stack

| Component | Technology |
|-----------|------------|
| **Framework** | .NET Framework 4.7.2+ / .NET Core |
| **UI Platform** | WPF (Windows Presentation Foundation) |
| **Programming Language** | C# |
| **Speech Synthesis** | System.Speech |
| **Speech Recognition** | System.Speech.Recognition |
| **Audio Support** | System.Media.SoundPlayer |
| **File Handling** | System.IO |
| **Dialog Services** | Microsoft.Win32 |

## Prerequisites

Before running this application, ensure you have:

### Required Software
- **Windows 10/11** (64-bit recommended)
- **Visual Studio 2019 or 2022** (any edition)
- **.NET Framework 4.7.2** or higher

### Required NuGet Packages
```xml
<PackageReference Include="System.Speech" Version="7.0.0" />
