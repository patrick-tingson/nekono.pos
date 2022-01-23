using System;
using System.Collections.Generic;
using System.Text;

namespace Nekono.App.Pos.Models
{
    public enum MenuItemType
    {
        Sales,
        Receipts,
        Settings,
        Logout
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
