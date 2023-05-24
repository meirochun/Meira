using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Meira.Interfaces;
using Meira.Validations;

namespace Meira.SlashCommands
{
    [RequirePermissions(Permissions.Administrator, ignoreDms: true)]
    internal class AdminSlashCommands : ApplicationCommandModule, IDefaultEmbedMessages
    {

        #region Ban Command

        [SlashCommand("ban", "Bans a user from server")]
        public async Task BanCommand(InteractionContext ctx,
            [Option("user", "The user you wants to ban")] DiscordUser user,
            [Option("days", "Days you want to delete messages from user you are banning")] long deleteMessageDays = 0,
            [Option("reason", "Reason for ban")] string reason = null)
        {
            await ctx.DeferAsync();

            if (ctx.Member.IsAdministrator())
            {
                var member = (DiscordMember)user;
                if (!member.IsMeira())
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

        #endregion Ban Command

        #region Kick Command

        [SlashCommand("kick", "Kicks a user from server")]
        public async Task KickCommand(InteractionContext ctx,
            [Option("user", "The user you wants to kick")] DiscordUser user)
        {
            await ctx.DeferAsync();

            if (ctx.Member.IsAdministrator())
            {
                var member = (DiscordMember)user;
                if (!member.IsMeira())
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

        #endregion Kick Command

        #region Timeout Command

        [SlashCommand("timeout", "Timeout a user")]
        public async Task TimeoutCommand(InteractionContext ctx,
            [Option("user", "The user you want to timeout")] DiscordUser user,
            [Option("duration", "Duration of the timeout in seconds")] long duration)
        {
            await ctx.DeferAsync();

            if (ctx.Member.IsAdministrator())
            {
                DiscordEmbedBuilder timeoutMessage = new();
                var timeDuration = DateTime.Now + TimeSpan.FromSeconds(duration);
                var member = (DiscordMember)user;

                if (member.IsAdministrator())
                {
                    // Message builder when is an administrator, owner or even Meira
                    timeoutMessage.Title = @$"You can't timeout {string.Format(member.IsOwner ? "the owner" : member.IsMeira() ? "Meira" : "an administrator")}!";
                    timeoutMessage.Color = DiscordColor.DarkRed;
                }
                else
                {
                    await member.TimeoutAsync(timeDuration);
                    timeoutMessage.Color = DiscordColor.Orange;
                    timeoutMessage.Title = member.Username + " has been timeout";
                    timeoutMessage.Description = "Duration " + TimeSpan.FromSeconds(duration).ToString();
                }

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(timeoutMessage));
            }
            else
            {
                await NonAdminMessage(ctx);
            }
        }

        #endregion Timeout Command

        #region Cancel Timeout Command

        [SlashCommand("cancel-timeout", "Cancel the timeout of a user")]
        public async Task CancelTimeout(InteractionContext ctx, [Option("user", "The user you want to cancel timeout")] DiscordUser user)
        {
            if (ctx.Member.IsAdministrator())
            {
                DiscordEmbedBuilder timeoutMessage = new();
                await ctx.DeferAsync();

                var member = (DiscordMember)user;

                if (member?.CommunicationDisabledUntil == null || member?.CommunicationDisabledUntil.Value < DateTime.Now)
                {
                    timeoutMessage.WithTitle(member.Username + " is already free");
                    timeoutMessage.WithColor(DiscordColor.SapGreen);
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(timeoutMessage));
                    return;
                }
                else
                {
                    // Message builder when is actually possible timeout
                    timeoutMessage.Title = $"{member.Username} is now free";
                    timeoutMessage.Color = DiscordColor.SapGreen;

                    // DateTime.Now minus 1 because in case of delay, the user won't get jumpscared by "YOU GOT TIMEOUT" big red text
                    await member.TimeoutAsync(DateTime.Now - TimeSpan.FromSeconds(1));
                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(timeoutMessage));
                    return;
                }
            }
            else
            {
                await NonAdminMessage(ctx);
            }
        }
        
        #endregion Cancel Timeout Command

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
