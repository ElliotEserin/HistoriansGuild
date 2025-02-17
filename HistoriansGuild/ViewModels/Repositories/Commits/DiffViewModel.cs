using Avalonia.Controls.Documents;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using LibGit2Sharp;

namespace HistoriansGuild.ViewModels.Repositories.Commits
{
    public partial class DiffViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string filename;

        [ObservableProperty]
        private string status;

        [ObservableProperty]
        private InlineCollection diffInlines;

        public DiffViewModel(PatchEntryChanges diff)
        {
            Filename = diff.Path;
            Status = diff.Status.ToString();
            diffInlines = ParseDiff(diff);
        }

        public InlineCollection ParseDiff(PatchEntryChanges diff)
        {
            var inlines = new InlineCollection();

            var lines = diff.Patch.Split('\n');

            foreach (var line in lines)
            {
                //Skip metadata lines
                if (line.StartsWith("---") || line.StartsWith("+++") || line.StartsWith("@@"))
                {
                    continue;
                }
                //Removed lines
                else if (line.StartsWith('-'))
                {
                    inlines.Add(new Run(line + "\n")
                    {
                        Foreground = Brushes.LightCoral,
                    });
                }
                //Added lines
                else if (line.StartsWith('+'))
                {
                    inlines.Add(new Run(line + "\n")
                    {
                        Foreground = Brushes.LightGreen,
                    });
                }
                //Unchanged lines
                else
                {
                    inlines.Add(new Run(line + "\n"));
                }
            }
            return inlines;
        }
    }
}
