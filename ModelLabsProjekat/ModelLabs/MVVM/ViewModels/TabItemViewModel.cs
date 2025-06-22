using MVVM.Helpers;

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
