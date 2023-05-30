using DSharpPlus;
using DSharpPlus.Entities;

namespace Meira.Validations
{
    internal static class DefaultValidations
    {
        /// <summary>
        /// Extension method to check if Guild's member is Meira (bot).
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool IsMeira(this DiscordMember member)
        {
            // This is Meira's ID
            if (member.Id == 1107789924314402836)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Extension method to check if Guild's member is an administrator or has the permission.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool IsAdministrator(this DiscordMember member)
        {
            if (member.Permissions.HasPermission(Permissions.Administrator))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Extension method to check if Guild's member can manage channels or has the permission.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool CanManageChannels(this DiscordMember member)
        {
            if (member.Permissions.HasPermission(Permissions.ManageChannels))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Extension method to check if Guild's member can manage roles or has the permission.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool CanManageRoles(this DiscordMember member)
        {
            if (member.Permissions.HasPermission(Permissions.ManageRoles))
            {
                return true;
            }
            return false;
        }
    }
}