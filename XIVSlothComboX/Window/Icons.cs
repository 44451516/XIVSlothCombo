using Dalamud.Interface.Internal;
using ECommons.DalamudServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using XIVSlothComboX.Services;

namespace XIVSlothComboX.Window
{
    internal static class Icons
    {
        public static IDalamudTextureWrap? GetJobIcon(uint jobId)
        {
            if (jobId == 0 || jobId > 42)
                return null;
            // Service.TextureProvider.
            var gameIconLookup = new GameIconLookup(62100 + jobId);
            return Service.TextureProvider.GetFromGameIcon(gameIconLookup).GetWrapOrEmpty();
        }
    }
}