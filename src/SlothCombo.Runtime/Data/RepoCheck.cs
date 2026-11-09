using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlothCombo.Runtime.Services;

namespace SlothCombo.Data
{
    public class RepoCheck
    {
        public string? InstalledFromUrl { get; set; }
    }

    public static class RepoCheckFunctions
    {
        public static RepoCheck? FetchCurrentRepo()
        {
            var f = new FileInfo(Service.Interface.AssemblyLocation);
            var manifest = Path.Join(f.DirectoryName, "SlothCombo.json");
            
            if (File.Exists(manifest))
            {
                RepoCheck? repo = JsonConvert.DeserializeObject<RepoCheck>(File.ReadAllText(manifest));
                return repo;
            }
            else
            {
                return null;
            }

        }

        public static bool IsFromSlothRepo()
        {
            RepoCheck? repo = FetchCurrentRepo();
            if (repo is null) return false;

            if (repo.InstalledFromUrl is null) return false;

            if (repo.InstalledFromUrl == "https://raw.githubusercontent.com/Nik-Potokar/MyDalamudPlugins/main/pluginmaster.json")
                return true;
            else
                return false;
        }
    }
}
