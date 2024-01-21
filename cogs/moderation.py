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

    @app_commands.command(name="create_thread", description="Cria uma thread")
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

    @app_commands.command(name="lock_thread", description="Trava uma thread")
    async def lock_thread(self, interaction: discord.Interaction) -> None:
        if interaction.channel.type != discord.ChannelType.public_thread:
            await thread_responses.only_executable_in_threads(interaction, True)
            return
        await interaction.channel.edit(locked=True)
        await thread_responses.locked_thread(interaction, False)
    
    @app_commands.command(name="unlock_thread", description="Destrava uma thread")
    async def unlock_thread(self, interaction: discord.Interaction) -> None:
        if interaction.channel.type != discord.ChannelType.public_thread:
            await thread_responses.only_executable_in_threads(interaction, True)
            return
        await interaction.channel.edit(locked=False)
        await thread_responses.unlocked_thread(interaction, False)

    @app_commands.command(name="archive_thread", description="Arquiva uma thread")
    async def archive_thread(self, interaction: discord.Interaction) -> None:
        if interaction.channel.type != discord.ChannelType.public_thread:
            await thread_responses.only_executable_in_threads(interaction, True)
            return
        await thread_responses.archived_thread(interaction, False)
        await interaction.channel.edit(archived=True)

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

async def setup(client: commands.Bot) -> None:
    await client.add_cog(Moderation(client))