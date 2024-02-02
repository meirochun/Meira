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
                await message.channel.send(os.getenv("SUINEMA"));
            except:
                await message.channel.send(f"Suinema expirou 🥲");

async def setup(bot) -> None:
    await bot.add_cog(Events(bot))