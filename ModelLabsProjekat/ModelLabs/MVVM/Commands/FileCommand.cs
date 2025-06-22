using Microsoft.Win32;

namespace MVVM.Commands
{
    public static class FileCommand
    {
        public static string SelectFile(string title,string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = title;
            openFileDialog.Filter = filter;

            bool? dialogResponse = openFileDialog.ShowDialog();

            if (dialogResponse == true)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}