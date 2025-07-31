# SolarCharge

SolarCharge is a small self-hosted application designed to run on an always on device. It integrates with your solar inverter and Tesla to manage EV charging intelligently — starting and stopping charge sessions based on your solar PV generation.

## Overview

The system monitors your solar generation and charges your Tesla only when there’s surplus power available, minimizing grid draw and saving you money. The solution is completely self-hosted so your data isn't being sent anywhere and is all contained within your local network.

## Features

- Automatically starts Tesla charging when excess PV generation exceeds a defined threshold
- Stops charging when solar generation falls below the threshold
- Compatible with common solar inverters (see supported list)
- Support for notifications through Telegram
- Fully self-hosted solution
- Lightweight and easy to run on a Raspberry Pi

## Requirements

- Raspberry Pi (or other always-on device)
- Tesla account with API access
- Solar inverter with network-accessible real-time data

## Installation

### Prerequisites

* If you plan on using the telegram bot functionality to provide notifications, then you'll need to get a Bot Token for your Telegram Bot. Follow the instructions [here](https://core.telegram.org/bots/features#botfather) to get started.

### Setup

* Copy the `docker-compose.yaml` from the `src` directory
* Replace the `Telegram__BotToken` environment variable in the `solar-charge-telegram` service to be the Bot Token you retrieved above
* Send the message `/start` to the Telegram Bot. This will configure the bot to use that chat for future communications
* Navigate to `http://<IP_ADDRESS>:5000` and follow the instructions to login to your Tesla account
  * Note this uses Tesla's official authentication, SolarCharge never see's your Tesla credentials