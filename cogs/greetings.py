import discord
from discord.ext import commands

class Greetings(commands.Cog):
    def __init__(self, bot) -> None:
        self.bot = bot

    @commands.Cog.listener()
    async def on_member_join(self, member):
        channel = member.guild.system_channel
        if channel is not None:
            await channel.send(f"Bem-vindo {member.mention}!")

async def setup(bot) -> None:
    await bot.add_cog(Greetings(bot))