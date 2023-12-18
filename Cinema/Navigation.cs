using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Cinema
{
    public static class Navigation
    {
        public static Frame frame;

        public static void SetPage(Page page)
        {
            frame.Content = page;
        }
    }
}
