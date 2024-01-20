from time import sleep
import discord
from discord import app_commands
from discord.ext import commands
from discord.ext.commands import Context

class Moderation(commands.Cog, name="moderation"):
    def __init__(self, client: commands.Bot) -> None:
        self.client = client

    @commands.command()
    async def clear(self, ctx: Context, quantidade: int) -> None:
        clear_messages = await ctx.channel.purge(limit=quantidade, bulk = quantidade > 5)
        
        embed = discord.Embed(
            description=f"**{len(clear_messages)}** mensagens foram apagadas. \💀",
            color=discord.Color.green()
        )
        await ctx.send(embed=embed)

    @app_commands.command()
    @app_commands.describe(
        nome = "Nome da thread",
        mensagem_id = "ID da mensagem que será enviada para a thread"
    )
    async def create_thread(self, interaction: discord.Interaction, nome: str = "", mensagem_id: str = "") -> None:
        await interaction.response.defer()

        if mensagem_id != "":
            mensagem = await interaction.channel.fetch_message(int(mensagem_id))
            nome = f"Thread ({mensagem.id})"
            created_thread = await interaction.channel.create_thread(name=nome, message=mensagem)
            await interaction.followup.send(f"Thread criada: {created_thread.mention}")
        else:
            created_thread = await interaction.channel.create_thread(
                name=nome,
                type=discord.ChannelType.public_thread)
            await interaction.followup.send(f"Thread criada: {created_thread.mention}", ephemeral=True)

async def setup(client: commands.Bot) -> None:
    await client.add_cog(Moderation(client))