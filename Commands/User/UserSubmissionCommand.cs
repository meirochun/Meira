using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Meira.Commands
{
    public class UserSubmissionCommand : ApplicationCommandModule
    {
        [SlashCommand("coinflip", "Coin flips (there is 1 in a 500k chance to get a draw)")]
        public async Task Coinflip(InteractionContext ctx)
        {
            await ctx.DeferAsync();
            const int min = 1;
            const int max = 500001;
            const string title = "Coinflip";

            var embed = new DiscordEmbedBuilder();

            var random = new Random();
            int randomNumber = random.Next(min, max);

            if (randomNumber == 727)
            {
                embed.WithTitle(title)
                    .WithDescription("Draw! The coin is in vertical!")
                    .WithColor(DiscordColor.Azure);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
                return;
            }
            
            if (randomNumber %2 == 0)
            {
                embed.WithTitle(title)
                    .WithDescription($"The side is heads")
                    .WithColor(DiscordColor.Green);
            }
            else
            {
                embed.WithTitle(title)
                    .WithDescription($"The side is tails")
                    .WithColor(DiscordColor.Green);
            }

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
        }
    }
}
