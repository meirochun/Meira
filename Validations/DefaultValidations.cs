using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Meira.Validations
{
    internal static class DefaultValidations
    {
        public static bool IsMeira(DiscordMember member)
        {
            // This is Meira's ID
            if (member.Id == 1107789924314402836)
            {
                return true;
            }
            return false;
        }

        public static bool IsAdministrator(DiscordMember member)
        {
            if (member.Permissions.HasPermission(Permissions.Administrator))
            {
                return true;
            }
            return false;
        }
    }
}
