using DSharpPlus.SlashCommands;

namespace Meira.Interfaces
{
    public interface IDefaultEmbedMessages
    {
        Task NonAdminMessage(InteractionContext ctx);
    }
}
