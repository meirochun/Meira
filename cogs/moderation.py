import requests

from meira.default_responses import thread_default_responses as thread_responses
import discord
from discord import app_commands
from discord.ext import commands
from discord.ext.commands import Context

class Moderation(commands.Cog, name="moderation"):
    def __init__(self, client: commands.Bot) -> None:
        self.client = client

    @commands.command(name="clear", description="Apaga mensagens (permissão necessária)")
    @commands.has_permissions(manage_messages=True)
    @commands.bot_has_permissions(manage_messages=True)
    async def clear(self, ctx: Context, quantidade: int) -> None:
        clear_messages = await ctx.channel.purge(limit=quantidade, bulk = quantidade > 5)
        
        embed = discord.Embed(
            description=f"**{len(clear_messages)}** mensagens foram apagadas. \✅",
            color=discord.Color.green()
        )
        await ctx.send(embed=embed)

async def setup(client: commands.Bot) -> None:
    await client.add_cog(Moderation(client))