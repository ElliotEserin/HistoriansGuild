using Avalonia.Controls.Documents;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using LibGit2Sharp;

namespace HistoriansGuild.ViewModels.Repositories.Commits
{
    public partial class DiffViewModel : ViewModelBase
    {
        [ObservableProperty]
        private InlineCollection diffInlines;

        private PatchEntryChanges diff;

        public DiffViewModel(PatchEntryChanges diff)
        {
            this.diff = diff;
            diffInlines = ParseDiff();
        }

        public InlineCollection ParseDiff()
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
