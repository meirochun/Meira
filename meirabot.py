import os
import sys
import json
import platform

import meira.tools.setup_log as meira_log

import discord
from discord.ext import commands
from dotenv import load_dotenv

load_dotenv()
DISCORD_BOT_TOKEN = os.getenv("DISCORD_BOT_TOKEN")

if not os.path.isfile(f"{os.path.realpath(os.path.dirname(__file__))}/config.json"):
    sys.exit("'config.json' não encontrado! Por favor, adicione-o e tente novamente.")
else:
    with open(f"{os.path.realpath(os.path.dirname(__file__))}/config.json") as file:
        config = json.load(file)

intents = discord.Intents.default()
intents.message_content = True
intents.members = True

class MeiraBot(commands.Bot):
    def __init__(self) -> None:
        super().__init__(
            command_prefix=commands.when_mentioned_or(config["prefix"]),
            case_insensitive=True,
            intents=intents,
            help_command=None,
        )
        self.logger = meira_log.logger
        self.config = config

    async def load_cogs(self) -> None:
        for file in os.listdir("./cogs"):
            if file.endswith(".py"):
                extension = file[:-3]
                try:
                    await self.load_extension(f"cogs.{extension}")
                    self.logger.info(f"Carregando extensão '{extension}'")
                except Exception as e:
                    exception = f"{type(e).__name__}: {e}"
                    self.logger.error(f"Não foi possível carregar a extensão {extension} devido a {exception}")

    async def setup_hook(self) -> None:
        await self.load_cogs()

    async def on_message(self, message: discord.Message) -> None:
        if message.author == self.user or message.author.bot:
            return
        await self.process_commands(message)

    async def on_ready(self) -> None:
        synced = await self.tree.sync()
        self.logger.info(f"Sistema operacional: {platform.system()} {platform.release()}")
        self.logger.info(f"Python: {platform.python_version()}")
        self.logger.info(f"Discord.py: {discord.__version__}")
        self.logger.info(f"Slash commands sincronizados: {str(len(synced))}.")
        self.logger.info("-----------------------------")
        self.logger.info(f"Meira online!")
        await bot.change_presence(
            activity=discord.CustomActivity(name="Meirando..."),
            status=discord.Status.online)

bot = MeiraBot()
bot.run(DISCORD_BOT_TOKEN)