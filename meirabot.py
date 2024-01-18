import os
import sys
import json
import platform

import meira.tools.setup_log as meira_log

import discord
from discord.ext import commands
from dotenv import load_dotenv

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
        """
        Setup hook para carregar as extensões.
        """
        await self.load_cogs()

    async def on_message(self, message: discord.Message) -> None:
        """
        Evento chamado quando o bot recebe uma mensagem.
        Impede que o bot responda a si mesmo e a outros bots.
        """
        if message.author == self.user or message.author.bot:
            return
        await self.process_commands(message)

    async def on_ready(self) -> None:
        self.logger.info(f"Logado como {self.user.name}")
        self.logger.info(f"Sistema operacional: {platform.system()} {platform.release()}")
        self.logger.info(f"Python: {platform.python_version()}")
        self.logger.info(f"Discord.py: {discord.__version__}")
        self.logger.info("-----------------------------")
        synced = await self.tree.sync()
        self.logger.info(f"Slash commands sincronizados: {str(len(synced))}.")
        self.logger.info("-----------------------------")
        await bot.change_presence(activity=discord.Game(name="Meirando..."), status=discord.Status.idle)

load_dotenv()
bot = MeiraBot()
bot.run(os.getenv('TOKEN'))