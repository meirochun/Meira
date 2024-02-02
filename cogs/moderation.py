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

    @app_commands.command(name="add_song", description="OWNER ONLY COMMAND")
    @app_commands.describe(
        nome = "Nome da música",
        link = "Link da música"
    )
    async def add_song(self,
                       interaction: discord.Interaction,
                       nome: str,
                       link: str) -> None:
        await interaction.response.defer()

        if interaction.user.id != 267410788996743168:
            await interaction.response.send_message("Você não tem permissão para usar este comando.", ephemeral=True)
            return

        await interaction.followup.send("Adicionando música...")

        try:
            data = { "name": f"{nome}", "link": f"{link}" }
            url = "http://localhost:3000/add-song"
            res = await requests.post(url, data=data)

            if res.status_code == 200:
                await interaction.followup.send("Música adicionada com sucesso!")
            else:
                await interaction.followup.send("Ocorreu um erro ao adicionar a música.")
        except Exception as e:
            await interaction.followup.send(f"Ocorreu um erro ao adicionar a música: {e}")

async def setup(client: commands.Bot) -> None:
    await client.add_cog(Moderation(client))