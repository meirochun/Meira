using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Meira.Interfaces;

namespace Meira.Utils
{
    internal class DefaultEmbedMessages : IDefaultEmbedMessages
    {
        /// <summary>
        /// Returns an embed message warning the user that the command is only available for administrators
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public async Task NonAdminMessage(InteractionContext ctx)
        {
            var nonAdminMessage = new DiscordEmbedBuilder()
            {
                Title = "Access Denied",
                Description = "You must be an administrator to use this command",
                Color = DiscordColor.Red
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(nonAdminMessage));
        }
    }
}
