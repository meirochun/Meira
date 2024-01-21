import discord

async def only_executable_in_threads(interaction: discord.Interaction, is_ephemeral: bool) -> None:
    await interaction.response.send_message(
        "Este comando só pode ser executado em threads",
        ephemeral=is_ephemeral)
    
async def locked_thread(interaction: discord.Interaction, is_ephemeral: bool) -> None:
    await interaction.response.send_message(
        "Thread trancada!",
        ephemeral=is_ephemeral)
    
async def unlocked_thread(interaction: discord.Interaction, is_ephemeral: bool) -> None:
    await interaction.response.send_message(
        "Thread destrancada!",
        ephemeral=is_ephemeral)
    
async def archived_thread(interaction: discord.Interaction, is_ephemeral: bool) -> None:
    await interaction.response.send_message(
        "Thread arquivada!",
        ephemeral=is_ephemeral)