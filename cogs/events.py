import os
import discord
from discord.ext import commands

class Events(commands.Cog):
    def __init__(self, bot) -> None:
        self.bot = bot

    @commands.Cog.listener()
    async def on_message(self, message):
        if message.author == self.bot.user or message.author.bot:
            return
        
        if "damn" in message.content:
            await message.channel.send("<:DAMN:1198085777997967481>");

        if "suinema" in message.content:
            try:
                with open(os.getenv("SUINEMA_PATH_IMG"), 'rb') as file:
                    file_content = discord.File(file)
                    await message.channel.send(file=file_content);
            except Exception as ex:
                await message.channel.send(f"Erro: {ex}");

async def setup(bot) -> None:
    await bot.add_cog(Events(bot))