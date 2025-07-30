# solar-charge

## Installation

### Prerequisites

* If you plan on using the telegram bot functionality to provide notifications, then you'll need to get a Bot Token for your Telegram Bot. Follow the instructions [here](https://core.telegram.org/bots/features#botfather) to get started.

### Setup

* Copy the `docker-compose.yaml` from the `src` directory.
* Replace the `Telegram__BotToken` environment variable in the `solar-charge-telegram` service to be the Bot Token you retrieved above.
* Send the message `/start` to the Telegram Bot. This will configure the bot to use that chat for future communications.
