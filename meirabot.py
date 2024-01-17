import discord
from discord import app_commands
from dotenv import load_dotenv
import os

class client(discord.Client):
    def __init__(self):
        super().__init__(intents=discord.Intents.default())
        self.synced = False

    async def on_ready(self):
        if not self.synced:
            await tree.sync()
            self.synced = True
        print('Meira rodando!')

aclient = client()
tree = app_commands.CommandTree(aclient)

@tree.command(name = 'ping', description = 'Verifica se o bot está funcionando.')
async def ping(interaction: discord.Interaction):
    await interaction.response.send_message('Pong!', ephemeral = True)


load_dotenv()
aclient.run(os.getenv('TOKEN'))