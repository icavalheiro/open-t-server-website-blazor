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
    }

    public class NavMenuService : INavMenuService
    {
        public event Action OnUpdate;
        public IEnumerable<NavMenuEntry> Entries => _entries;
        private List<NavMenuEntry> _entries;

        public NavMenuService(IConfiguration configuration)
        {
            _entries = new List<NavMenuEntry>();
            _entries.Add(new NavMenuEntry()
            {

            });
        }
    }
}
