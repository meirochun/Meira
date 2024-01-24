import os
from datetime import datetime

import discord
from discord import app_commands
from discord.ext import commands
from discord.ext.commands import Context

class About(commands.Cog, name='sobre'):
    def __init__(self, client: commands.Bot) -> None:
        self.client = client

    @app_commands.command(name="help", description="Mostra os comandos disponíveis")
    async def help(self, interaction: discord.Interaction) -> None:
        slash_commands = self.get_app_commands()
        embed = discord.Embed(
            title="Listinha de comandos",
            description="Prefixo: *m!*",
            color=discord.Color.blue()
        )
        embed.add_field(name="Comandos com prefixo", value="", inline=False)
        for command in self.client.commands:
            embed.add_field(name=f"m! {command.name}", value=command.description, inline=False)

        embed.add_field(name="----------------", value="")
        
        embed.add_field(name="Comandos com barra", value="", inline=False)
        for slash_command in slash_commands:
            embed.add_field(name=f"/{slash_command.name}", value=slash_command.description, inline=False)

        await interaction.response.send_message(embed=embed, ephemeral=True, delete_after=60)

    @app_commands.command(description="Informações sobre o bot")
    async def sobre(self, interaction: discord.Interaction) -> None:
        await interaction.response.defer()
        embed = discord.Embed(
            title="Meira (Bot)",
            description="Meira é um bot (possívelmente genérico) open source.",
            color=discord.Color.blue()
        )
        embed.add_field(name="Prefixo", value="m!")
        embed.add_field(name="Versão", value=os.getenv("MEIRA_VERSION"))
        owner_avatar_url = await get_owner_avatar(self.client)
        embed.set_footer(
            text=f"Desenvolvido por meirochou | 2023-{datetime.now().year}",
            icon_url = owner_avatar_url
        )

        await interaction.followup.send(embed=embed)

    @commands.command(name="ping", description="Retorna o ping do bot")
    async def ping(self, ctx: Context):
        if ctx.author.bot:
            return

        await ctx.reply(
            f"Pong! {round(ctx.bot.latency * 1000)}ms.",
            delete_after=10,
            ephemeral=True)

async def get_owner_avatar(client: commands.Bot) -> str:
    owner_id = os.getenv("OWNER_ID")
    owner = await client.fetch_user(int(owner_id))
    return owner.avatar.url

async def setup(client: commands.Bot) -> None:
    await client.add_cog(About(client))