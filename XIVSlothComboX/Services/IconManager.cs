using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dalamud.Game;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using Lumina.Data.Files;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace XIVSlothComboX.Services;

﻿using Action = Action;


public class IconManager : IDisposable {
    private bool disposed;
    private readonly Dictionary<(uint, bool), IDalamudTextureWrap> iconTextures = new();
    private readonly Dictionary<uint, ushort> actionCustomIcons = new() {
            
    };


    
    
    public IconManager() {
     
    }

    public void Dispose() {
        disposed = true;
        var c = 0;
        foreach (var texture in iconTextures.Values.Where(texture => texture != null)) {
            c++;
            texture.Dispose();
        }

        iconTextures.Clear();
        
    }
        
    private void LoadIconTexture(uint iconId, bool hq = false) {
        Task.Run(() => {
            try {

                var gameIconLookup = new GameIconLookup(iconId);

                var dalamudTextureWrap = Service.TextureProvider.GetFromGameIcon(gameIconLookup).RentAsync().Result;


                var tex = dalamudTextureWrap;

                if (tex.ImGuiHandle != IntPtr.Zero) {
                    this.iconTextures[(iconId, hq)] = tex;
                } else {
                    tex.Dispose();
                }
            } catch (Exception ex) {
            }
        });
    }
        
    public TexFile GetIcon(uint iconId, bool hq = false) => this.GetIcon(Service.DataManager.Language, iconId, hq);

    /// <summary>
    /// Get a <see cref="T:Lumina.Data.Files.TexFile" /> containing the icon with the given ID, of the given language.
    /// </summary>
    /// <param name="iconLanguage">The requested language.</param>
    /// <param name="iconId">The icon ID.</param>
    /// <returns>The <see cref="T:Lumina.Data.Files.TexFile" /> containing the icon.</returns>
    public TexFile GetIcon(ClientLanguage iconLanguage, uint iconId, bool hq = false)
    {
        string type;
        switch (iconLanguage)
        {
            case ClientLanguage.Japanese:
                type = "ja/";
                break;
            case ClientLanguage.English:
                type = "en/";
                break;
            case ClientLanguage.German:
                type = "de/";
                break;
            case ClientLanguage.French:
                type = "fr/";
                break;

            default:
                type = "/";
                break;
        }
        return this.GetIcon(type, iconId, hq);
    }
        
    public TexFile GetIcon(string type, uint iconId, bool hq = false)
    {
        if (type == null)
            type = string.Empty;
        if (type.Length > 0 && !type.EndsWith("/"))
            type += "/";
            
        var formatStr = $"ui/icon/{{0:D3}}000/{(hq?"hq/":"")}{{1}}{{2:D6}}.tex";
        TexFile file = Service.DataManager.GetFile<TexFile>(string.Format(formatStr, (object) (iconId / 1000), (object) type, (object) iconId));
        return file != null || type.Length <= 0 ? file : Service.DataManager.GetFile<TexFile>(string.Format(formatStr, (object) (iconId / 1000), (object) string.Empty, (object) iconId));
    }
        

    public IDalamudTextureWrap? GetActionIcon(Action action) {
        return GetIconTexture(actionCustomIcons.ContainsKey(action.RowId) ? actionCustomIcons[action.RowId] : action.Icon);
    }

    public ushort GetActionIconId(Action action) {
        return actionCustomIcons.ContainsKey(action.RowId) ? actionCustomIcons[action.RowId] : action.Icon;
    }

    public IDalamudTextureWrap? GetIconTexture(uint iconId, bool hq = false) {
        if (this.disposed) 
            return null;
        if (this.iconTextures.ContainsKey((iconId, hq))) 
            return this.iconTextures[(iconId, hq)];
        this.iconTextures.Add((iconId, hq), null);
        LoadIconTexture(iconId, hq);
        return this.iconTextures[(iconId, hq)];
    }
}