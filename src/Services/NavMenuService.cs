using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TibiaWebsite.Services
{
    public class NavMenuEntry
    {
        public class Item
        {
            public string Name { get; set; }
            public string Href { get; set; }
        }

        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsOpened { get; set; }
        public List<Item> Items { get; set; }
    }

    public interface INavMenuService
    {
        IEnumerable<NavMenuEntry> Entries { get; }
        event Action OnUpdate;
        void AddOrUpdate(NavMenuEntry entry);
    }

    public class NavMenuService : INavMenuService
    {
        public event Action OnUpdate;
        public IEnumerable<NavMenuEntry> Entries => _entries;
        private List<NavMenuEntry> _entries;

        public NavMenuService(IConfiguration configuration)
        {
            _entries = new List<NavMenuEntry>();

            //News
            _entries.Add(new NavMenuEntry()
            {
                Name = configuration.GetValue<string>("Server:Labels:NavMenu:News"),
                Icon = configuration.GetValue<string>("Server:Labels:NavMenu:NewsIcon"),
                IsOpened = true,
                Items = new List<NavMenuEntry.Item>()
                {
                    new NavMenuEntry.Item() {
                        Name = configuration.GetValue<string>("Server:Labels:NavMenu:LatestNews"),
                        Href = "/news"
                    },
                    new NavMenuEntry.Item() {
                        Name = configuration.GetValue<string>("Server:Labels:NavMenu:NewsArchive"),
                        Href = "/news-archive"
                    }
                }
            });

            //Community
            _entries.Add(new NavMenuEntry()
            {
                Name = configuration.GetValue<string>("Server:Labels:NavMenu:Community"),
                Icon = configuration.GetValue<string>("Server:Labels:NavMenu:CommunityIcon"),
                IsOpened = true,
                Items = new List<NavMenuEntry.Item>()
                {
                    new NavMenuEntry.Item() {
                        Name = configuration.GetValue<string>("Server:Labels:NavMenu:Characters"),
                        Href = "/characters"
                    },
                    new NavMenuEntry.Item() {
                        Name = configuration.GetValue<string>("Server:Labels:NavMenu:Highscores"),
                        Href = "/highscores"
                    },
                    new NavMenuEntry.Item() {
                        Name = configuration.GetValue<string>("Server:Labels:NavMenu:Guilds"),
                        Href = "/guilds"
                    },
                }
            });

            //Forum
            _entries.Add(new NavMenuEntry()
            {
                Name = configuration.GetValue<string>("Server:Labels:NavMenu:Forum"),
                Icon = configuration.GetValue<string>("Server:Labels:NavMenu:ForumIcon"),
                Items = new List<NavMenuEntry.Item>()
                {
                    new NavMenuEntry.Item() {
                        Name = configuration.GetValue<string>("Server:Labels:NavMenu:ServerForum"),
                        Href = "/forum"
                    },
                }
            });

            //Account
            _entries.Add(new NavMenuEntry()
            {
                Name = configuration.GetValue<string>("Server:Labels:NavMenu:Account"),
                Icon = configuration.GetValue<string>("Server:Labels:NavMenu:AccountIcon"),
                Items = new List<NavMenuEntry.Item>()
                {
                    new NavMenuEntry.Item() {
                        Name = configuration.GetValue<string>("Server:Labels:NavMenu:AccountManagement"),
                        Href = "/account"
                    },
                    new NavMenuEntry.Item() {
                        Name = configuration.GetValue<string>("Server:Labels:NavMenu:DownloadClient"),
                        Href = "/download"
                    },
                    new NavMenuEntry.Item() {
                        Name = configuration.GetValue<string>("Server:Labels:NavMenu:LostAccount"),
                        Href = "/lost-acount"
                    },
                }
            });

            //Library
            _entries.Add(new NavMenuEntry()
            {
                Name = configuration.GetValue<string>("Server:Labels:NavMenu:Library"),
                Icon = configuration.GetValue<string>("Server:Labels:NavMenu:LibraryIcon"),
                IsOpened = true,
                Items = new List<NavMenuEntry.Item>()
                {
                    new NavMenuEntry.Item() {
                        Name = configuration.GetValue<string>("Server:Labels:NavMenu:ExperienceTable"),
                        Href = "/experience-table"
                    },
                    new NavMenuEntry.Item() {
                        Name = configuration.GetValue<string>("Server:Labels:NavMenu:ServerInfo"),
                        Href = "/info"
                    }
                }
            });

            //Support
            _entries.Add(new NavMenuEntry()
            {
                Name = configuration.GetValue<string>("Server:Labels:NavMenu:Support"),
                Icon = configuration.GetValue<string>("Server:Labels:NavMenu:SupportIcon"),
                Items = new List<NavMenuEntry.Item>()
                {
                    new NavMenuEntry.Item() {
                        Name = configuration.GetValue<string>("Server:Labels:NavMenu:Rules"),
                        Href = "/rules"
                    },
                    new NavMenuEntry.Item() {
                        Name = configuration.GetValue<string>("Server:Labels:NavMenu:ContactDetails"),
                        Href = "/contact"
                    }
                }
            });

            //Shop
            _entries.Add(new NavMenuEntry()
            {
                Name = configuration.GetValue<string>("Server:Labels:NavMenu:Shop"),
                Icon = configuration.GetValue<string>("Server:Labels:NavMenu:ShopIcon"),
                IsOpened = true,
                Items = new List<NavMenuEntry.Item>()
                {
                    new NavMenuEntry.Item() {
                        Name = configuration.GetValue<string>("Server:Labels:NavMenu:Donate"),
                        Href = "/donate"
                    },
                    new NavMenuEntry.Item() {
                        Name = configuration.GetValue<string>("Server:Labels:NavMenu:WebShop"),
                        Href = "/shop"
                    }
                }
            });
        }

        public void AddOrUpdate(NavMenuEntry entry)
        {
            if (!_entries.Contains(entry))
            {
                _entries.Add(entry);
            }
            //else no needed, if the list already contains it
            //nothing needs to be done

            OnUpdate?.Invoke();
        }
    }
}
