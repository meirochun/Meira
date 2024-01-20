import os
import discord
from datetime import datetime
from discord import app_commands
from discord.ext import commands

class About(commands.Cog, name='sobre'):
    def __init__(self, client: commands.Bot) -> None:
        self.client = client

    @app_commands.command(description="Informações sobre o bot")
    async def sobre(self, interaction: discord.Interaction) -> None:
        await interaction.response.defer()
        embed = discord.Embed(
            title="Meira (Bot)",
            description="Meira é um bot (possívelmente genérico) de open source.",
            color=discord.Color.blue()
        )
        embed.add_field(name="GitHub", value="https://github.com/meirochun/meira", inline = True)
        embed.add_field(name="Versão", value=os.getenv("meira_version"), inline = True)

        owner_avatar_url = await get_owner_avatar(self.client, os.getenv("owner_id"))
        embed.set_footer(text=f"Desenvolvido por meirochou - {datetime.now().year}",
            icon_url = owner_avatar_url)

        await interaction.followup.send(embed=embed)

async def get_owner_avatar(client: commands.Bot, owner_id: str) -> str:
    owner = await client.fetch_user(int(owner_id))
    return owner.avatar.url

async def setup(client: commands.Bot) -> None:
    await client.add_cog(About(client))