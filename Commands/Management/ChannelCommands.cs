using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Meira.Utils;

namespace Meira.Commands
{
    [SlashCommandPermissions(Permissions.ManageChannels)]
    [SlashCommandGroup("channels", "Group of channel commands")]
    internal class ChannelCommands : ApplicationCommandModule
    {
        private readonly DefaultEmbedMessages _defaultEmbedMessages = new();

        [RequirePermissions(Permissions.ManageChannels)]
        [SlashCommand("new-category", "Creates a new category")]
        public async Task NewCategoryCommand(InteractionContext ctx, [Option("name", "The name of the new category.")] string name)
        {
            var channel = await ctx.Guild.CreateChannelCategoryAsync(name);
            await ctx.CreateResponseAsync(
                InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().WithContent($"Created category {channel.Mention}"));
        }

        [RequirePermissions(Permissions.ManageChannels)]
        [SlashCommand("new-text", "Creates a new Text channel")]
        public async Task NewTextCommand(InteractionContext ctx, [Option("name", "The name of the new channel.")] string name,
                                                                 [Option("parent", "parent")] DiscordChannel parent = null,
                                                                 [Option("topic", "The topic of the new channel.")] string topic = "",
                                                                 [Option("nsfw", "The channel is nsfw")] bool? isNSFW = null,
                                                                 [Option("dsadas", "The position of the new channel.")] long position = 0)
        {
            var channel = await ctx.Guild.CreateTextChannelAsync(name, parent, topic, nsfw: isNSFW, position: (int?)position);
            await ctx.CreateResponseAsync(
                InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().WithContent($"Created channel {channel.Mention}"));
        }

        [RequirePermissions(Permissions.ManageChannels)]
        [SlashCommand("new-vc", "Creates a new Voice Chat channel")]
        public async Task NewVCCommand(InteractionContext ctx, [Option("name", "The name of the new channel.")] string name,
                                                               [Option("limit", "Quantity of slots to user")] long userLimit = 0)
        {
            var channel = await ctx.Guild.CreateVoiceChannelAsync(name, user_limit: (int?)userLimit);
            await ctx.CreateResponseAsync(
                InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().WithContent($"Created channel {channel.Mention}"));
        }

        [RequirePermissions(Permissions.ManageChannels)]
        [SlashCommand("delete-category", "Deletes a category")]
        public async Task DeleteCategoryCommand(InteractionContext ctx, [Option("category", "The category to delete.")] DiscordChannel category)
        {
            var channelType = category.Type;
            if (channelType != ChannelType.Category)
            {
                await ctx.CreateResponseAsync(
                    InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent($"Channel {category.Name} is not a category"));
                return;
            }

            await category.DeleteAsync();
            await ctx.CreateResponseAsync(
                InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().WithContent($"Deleted category {category.Name}"));
        }

        [SlashCommand("delete-text", "Deletes a text channel")]
        public async Task DeleteTextCommand(InteractionContext ctx, [Option("channel", "The channel to delete.")] DiscordChannel channel)
        {
            var channelType = channel.Type;
            if (channelType != ChannelType.Text)
            {
                await ctx.CreateResponseAsync(
                    InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent($"Channel {channel.Name} is not a text channel"));
                return;
            }

            await ctx.CreateResponseAsync($"{channel.Name} deleted.");
        }

        [SlashCommand("delete-vc", "Deletes a voice chat channel")]
        public async Task DeleteVCCommand(InteractionContext ctx, [Option("channel", "The channel to delete.")] DiscordChannel channel)
        {
            var channelType = channel.Type;
            if (channelType != ChannelType.Voice)
            {
                await ctx.CreateResponseAsync(
                        InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent($"Channel {channel.Name} is not a voice channel"));
                return;
            }

            await channel.DeleteAsync();
            await ctx.CreateResponseAsync(
                InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().WithContent($"Deleted channel {channel.Name}"));
        }

        [SlashCommand("edit-textchannel", "Edits a text-channel configuration")]
        public async Task EditTextChannel(InteractionContext ctx)
        {

        }

        [SlashCommand("modal", "modal example")]
        public async Task Modal(InteractionContext ctx)
        {
            var components = new DiscordComponent[]
            {
                new TextInputComponent("Username", "usernameTextbox", "Type your username"),
                new DiscordChannelSelectComponent("modal-channel-select", "Select the channel...")
            };

            var modal = new DiscordInteractionResponseBuilder()
                .WithTitle("Test Modal")
                .WithCustomId("modalExample")
                .AddComponents(components);

            await ctx.CreateResponseAsync(InteractionResponseType.Modal, modal);
        }
    }
}
