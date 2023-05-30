using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Meira.Interfaces;

namespace Meira.Utils
{
    internal class DefaultEmbedMessages : IDefaultEmbedMessages
    {
        public DiscordEmbedBuilder EmbedMessage { get; private set; } = null;

        /// <summary>
        /// Returns an embed message warning the user that the command is only available for administrators
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public async Task NonAdminMessage(InteractionContext ctx)
        {
            EmbedMessage
                .WithTitle("Access Denied")
                .WithDescription("You must be an administrator to use this command")
                .WithColor(DiscordColor.Red);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(EmbedMessage));
        }

        /// <summary>
        /// Returns an embed message warning the user doens't have enough permissions to use the command
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public async Task NoPermissionEnough(InteractionContext ctx)
        {
            EmbedMessage
                .WithTitle("Access Denied")
                .WithDescription("You don't have enough permissions to use this command")
                .WithColor(DiscordColor.Red);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(EmbedMessage));
        }
    }
}
