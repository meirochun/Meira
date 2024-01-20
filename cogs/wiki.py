import discord
from discord import app_commands
from discord.ext import commands
import aiohttp

WIKI_ENDPOINT = "https://pt.wikipedia.org/w/api.php"
NUMBER_OF_SENTENCES = 3

class Wiki(commands.Cog, name="wiki"):
    def __init__(self, client: commands.Bot) -> None:
        self.client = client
    
    @app_commands.command(description="Pesquisa na Wikipédia")
    @app_commands.describe(
        tema = "Tema da pesquisa"
    )
    async def busca(self, interaction: discord.Interaction, tema: str) -> None:
        await interaction.response.defer()
        params = {
            "action": "query",
            "prop": "extracts",
            "titles": tema,
            "srsearch": tema,
            "exsentences": NUMBER_OF_SENTENCES,
            "list": "search",
            "explaintext": "true",
            "format": "json",
            "redirects": "true"
        }
        headers = { "User-Agent": "MeiraBot/1.0" }

        async with aiohttp.ClientSession() as session:
            async with session.get(WIKI_ENDPOINT, params=params, headers=headers) as response:
                if response.status == 200:
                    data = await response.json()
                    pages = data['query']['pages']
                    page = pages[list(pages.keys())[0]]
                    try:
                        extract = page['extract']
                        await interaction.followup.send(extract)
                    except:
                        await interaction.followup.send("Nenhum resultado encontrado.")
                else:
                    await interaction.followup.send("Não foi possível realizar a pesquisa.", ephemeral = True)

async def setup(client: commands.Bot) -> None:
    await client.add_cog(Wiki(client))