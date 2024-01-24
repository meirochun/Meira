import os

import discord
from discord.ext import commands
from discord import app_commands
from meira.default_responses import thread_default_responses as thread_responses

class Thread(commands.Cog):
    def __init__(self, bot) -> None:
        self.bot = bot

    threads = app_commands.Group(name="thread", description="Comandos para gerenciar threads")

    @threads.command(name="create_thread", description="Cria uma thread")
    @app_commands.describe(
        nome = "Nome da thread",
        mensagem = "Mensagem para iniciar a thread",
        privado = "É uma thread privada?",
    )
    async def create_thread(self,
                interaction: discord.Interaction,
                nome: str,
                mensagem: str = "",
                privado: bool = False) -> None:
        visibilidade = discord.ChannelType.public_thread
        if privado == True:
            visibilidade = discord.ChannelType.private_thread

        if mensagem != "":
            created_thread = await interaction.channel.create_thread(
                name=nome + f" ({interaction.user.id})",
                type=visibilidade)
            await created_thread.send(mensagem)
        else:
            created_thread = await interaction.channel.create_thread(
                name=nome + f" ({interaction.user.id})",
                type=visibilidade)
        
        if privado is True:
                await thread_responses.private_thread_created(interaction, created_thread, privado)
        await thread_responses.public_thread_created(interaction, created_thread, privado)

    @threads.command(name="lock_thread", description="Trava uma thread")
    async def lock_thread(self, interaction: discord.Interaction) -> None:
        if interaction.channel.type != discord.ChannelType.public_thread:
            await thread_responses.only_executable_in_threads(interaction, True)
            return
        await interaction.channel.edit(locked=True)
        await thread_responses.locked_thread(interaction, False)
    
    @threads.command(name="unlock_thread", description="Destrava uma thread")
    async def unlock_thread(self, interaction: discord.Interaction) -> None:
        if interaction.channel.type != discord.ChannelType.public_thread:
            await thread_responses.only_executable_in_threads(interaction, True)
            return
        await interaction.channel.edit(locked=False)
        await thread_responses.unlocked_thread(interaction, False)

    @threads.command(name="archive_thread", description="Arquiva uma thread")
    async def archive_thread(self, interaction: discord.Interaction) -> None:
        if interaction.channel.type != discord.ChannelType.public_thread:
            await thread_responses.only_executable_in_threads(interaction, True)
            return
        await thread_responses.archived_thread(interaction, False)
        await interaction.channel.edit(archived=True)

async def setup(bot) -> None:
    await bot.add_cog(Thread(bot))