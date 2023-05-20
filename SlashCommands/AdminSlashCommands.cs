using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Meira.Interfaces;

namespace Meira.SlashCommands
{
    [RequirePermissions(Permissions.Administrator, ignoreDms: true)]
    public class AdminSlashCommands : ApplicationCommandModule, IDefaultEmbedMessages
    {
        [SlashCommand("ban", "Bans a user from server")]
        public async Task BanCommand(InteractionContext ctx,
            [Option("user", "The user you wants to ban")] DiscordUser user,
            [Option("days", "Days you want to delete messages from user you are banning")] long deleteMessageDays = 0,
            [Option("reason", "Reason for ban")] string reason = null)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                var member = (DiscordMember)user;
                if (!CheckIsMeira(member))
                {
                    await ctx.Guild.BanMemberAsync(member, (int)deleteMessageDays, reason);

                    var banMessage = new DiscordMessageBuilder()
                        .AddEmbed(new DiscordEmbedBuilder()
                        .WithTitle("Banned user " + member.Username)
                        .WithDescription("Reason " + reason)
                        .WithFooter("User ID: " + member.Id)
                        .WithColor(DiscordColor.Red));

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(banMessage.Embed));
                }
                else
                {
                    var meiraMessage = new DiscordEmbedBuilder()
                    {
                        Title = "Access denied",
                        Description = "You cannot force me ban myself! 😡", 
                        Color = DiscordColor.Red
                    };

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(meiraMessage));
                }
            }
            else
            {
                await NonAdminMessage(ctx);
            }
        }

        [SlashCommand("kick", "Kicks a user from server")]
        public async Task KickCommand(InteractionContext ctx,
            [Option("user", "The user you wants to kick")] DiscordUser user)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                var member = (DiscordMember)user;
                if (!CheckIsMeira(member))
                {
                    await member.RemoveAsync();

                    var kickMessage = new DiscordMessageBuilder()
                            .AddEmbed(new DiscordEmbedBuilder()
                            .WithTitle(member.Username + "got kicked.")
                            .WithFooter("User ID: " + member.Id)
                            .WithColor(DiscordColor.Red));

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(kickMessage.Embed));
                }
                else
                {
                    var meiraMessage = new DiscordEmbedBuilder()
                    {
                        Title = "Access denied",
                        Description = "You cannot force me kick myself out! 😡",
                        Color = DiscordColor.Red
                    };

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(meiraMessage));
                }
            }
            else
            {
                await NonAdminMessage(ctx);
            }
        }

        private static bool CheckIsMeira(DiscordMember member)
        {
            // This is Meira's ID
            if (member.Id == 1107789924314402836)
            {
                return true;
            }
            return false;
        }

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
