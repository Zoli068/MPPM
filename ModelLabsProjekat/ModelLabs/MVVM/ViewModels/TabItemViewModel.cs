using MVVM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.ViewModels
{
    public class TabItemViewModel
    {
        public string Title { get; }
        public BindableBase ContentViewModel { get; }

        public TabItemViewModel(string title, BindableBase content)
        {
            Title = title;
            ContentViewModel = content;
        }
    }
}
