using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HistoriansGuild.Helpers;
using LibGit2Sharp;
using System;
using System.Diagnostics;

namespace HistoriansGuild.ViewModels.Repositories
{
    public partial class RepositoryViewModel : ViewModelBase
    {
        [ObservableProperty]
        private HistoryViewModel historyViewModel;

        [ObservableProperty]
        private StagingViewModel stagingViewModel;

        private readonly Repository repository;
        private readonly GitRepositoryWatcher watcher;

        public RepositoryViewModel(Repository repository) 
        {
            this.repository = repository;
            watcher = new GitRepositoryWatcher(repository, UpdateUI);

            HistoryViewModel = new HistoryViewModel(this.repository);
            StagingViewModel = new StagingViewModel(this.repository);
        }

        void UpdateUI()
        {
            HistoryViewModel = new HistoryViewModel(this.repository);
            StagingViewModel = new StagingViewModel(this.repository);
        }

        ~RepositoryViewModel()
        {
            watcher.Dispose();
        }

        [RelayCommand]
        public void Push()
        {
            var options = new PushOptions
            {
                CredentialsProvider = new LibGit2Sharp.Handlers.CredentialsHandler(
                    (url, user, cred) => new UsernamePasswordCredentials
                    {
                        Username = repository.Config.Get<string>(["user", "name"]).Value,
                        Password = repository.Config.Get<string>(["user", "password"]).Value,
                    }),
                OnNegotiationCompletedBeforePush = (push) =>
                {
                    Debugger.Log(0, "Repository", $"Push negotiation completed: {push}\n");
                    return true;
                },
                OnPackBuilderProgress = (stage, current, total) =>
                {
                    Debugger.Log(0, "Repository", $"Pack builder progress: {stage} {current}/{total}\n");
                    return true;
                },
                OnPushTransferProgress = (current, total, bytes) =>
                {
                    Debugger.Log(0, "Repository", $"Push transfer progress: {current}/{total} {bytes}\n");
                    return true;
                },
                OnPushStatusError = (error) =>
                {
                    Debugger.Log(0, "Repository", $"Push error: {error}\n");
                },
            };

            repository.Network.Push(repository.Head.TrackedBranch, options);
        }

        [RelayCommand]
        public void Pull()
        {
            var signature = repository.Config.BuildSignature(DateTimeOffset.Now);
            Commands.Pull(repository, signature, new PullOptions());
        }
    }
}
