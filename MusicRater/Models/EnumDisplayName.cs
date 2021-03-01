using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MusicRater.Models
{
    public static class EnumDisplayName
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            DisplayAttribute displayname = enumValue.GetType()
                                            .GetMember(enumValue.ToString())
                                            .First()
                                            .GetCustomAttribute<DisplayAttribute>();
            if(displayname == null)
            {
                return enumValue.ToString();
            }
            else
            {
                return displayname.GetName();
            }
        }
    }
}
