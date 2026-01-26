# ⚡ SolarCharge

SolarCharge is a small self-hosted application designed to run on an always-on device. It integrates with your solar inverter and Tesla to manage EV charging intelligently - notifying you to start and stop charging based on your solar PV generation.

## 🧾 Overview

The system monitors your solar generation and notifies you to charge your Tesla only when there’s surplus power available, minimizing grid draw and saving you money. The solution is completely self-hosted so your data isn't being sent anywhere and is all contained within your local network.

## ✨ Features

- Notifies the user to start charging via Telegram when excess PV generation exceeds your configured threshold
- Notifies the user to stop charging via Telegram when energy pulled from the grid exceeds your configured threshold
- Your data never leaves your local network
- Fully self-hosted solution
- Lightweight and easy to run on a Raspberry Pi

## 🛠 Requirements

- Raspberry Pi (or other always-on device)
- Tesla account
- Fronius inverter

## 📦 Installation

### ✅ Prerequisites

Follow the instructions [here](https://core.telegram.org/bots/features#botfather) to set up your Telegram bot.

### ⚙️ Setup

- Copy the `docker-compose.yaml` from the `src` directory
- Replace the `Telegram__BotToken` environment variable in the `solar-charge-telegram` service with the Bot Token you retrieved above
- Send the message `/start` to the Telegram Bot. This will configure the bot to use that chat for future communications
- Navigate to `http://<IP_ADDRESS>:5000` and follow the instructions to log in to your Tesla account
  - Note: This uses Tesla's official authentication. SolarCharge never sees your Tesla credentials
