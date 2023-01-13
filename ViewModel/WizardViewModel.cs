using Prism.Commands;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using Jaywapp.BuilderWizard.Interface;
using System;

namespace Jaywapp.BuilderWizard.ViewModel
{
    public partial class WizardViewModel : ReactiveObject
    {
        #region Internal Field
        private int _currentIndex = 1;
        private IReadOnlyList<IJayBuilderConfigView> _configViews;

        private ObservableAsPropertyHelper<eWizardStatus> _status;
        private ObservableAsPropertyHelper<IJayBuilderConfigView> _currentPage;
        private ObservableAsPropertyHelper<Visibility> _prevVisibility;
        private ObservableAsPropertyHelper<Visibility> _nextVisibility;
        private ObservableAsPropertyHelper<Visibility> _finishVisibility;
        private ObservableAsPropertyHelper<Visibility> _cancelVisibility;
        #endregion

        #region Event
        /// <summary>
        /// Finish Event
        /// </summary>
        public EventHandler Finished;
        /// <summary>
        /// Cancel Event
        /// </summary>
        public EventHandler Canceled;
        #endregion

        #region Commands
        /// <summary>
        /// Prev Command
        /// </summary>
        public DelegateCommand PrevCommand { get; private set; }
        /// <summary>
        /// Next Command
        /// </summary>
        public DelegateCommand NextCommand { get; private set; }
        /// <summary>
        /// Finish Command
        /// </summary>
        public DelegateCommand FinishCommand { get; private set; }
        /// <summary>
        /// Cancel Command
        /// </summary>
        public DelegateCommand CancelCommand { get; private set; }
        #endregion

        #region Properties
        /// <summary>
        /// Current Index
        /// </summary>
        public int CurrentIndex
        {
            get => _currentIndex;
            set => this.RaiseAndSetIfChanged(ref _currentIndex, value);
        }

        /// <summary>
        /// Current Status
        /// </summary>
        public eWizardStatus Status => _status.Value;
        
        /// <summary>
        /// Current Page
        /// </summary>
        public IJayBuilderConfigView CurrentPage => _currentPage.Value;

        /// <summary>
        /// Previous Button Visibility
        /// </summary>
        public Visibility PrevVisibility => _prevVisibility.Value;
        /// <summary>
        /// Next Button Visibility
        /// </summary>
        public Visibility NextVisibility => _nextVisibility.Value;
        /// <summary>
        /// Finish Button Visibility
        /// </summary>
        public Visibility FinishVisibility => _finishVisibility.Value;
        /// <summary>
        /// Cancel Button Visibility
        /// </summary>
        public Visibility CancelVisibility => _cancelVisibility.Value;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configViews"></param>
        public WizardViewModel(IEnumerable<IJayBuilderConfigView> configViews)
        {
            _configViews = configViews.ToList();

            // initialize commands
            PrevCommand = new DelegateCommand(Prev);
            NextCommand = new DelegateCommand(Next);
            FinishCommand = new DelegateCommand(Finish);
            CancelCommand = new DelegateCommand(Cancel);

            // If index property changed
            var indexChanges = this.WhenAnyValue(x => x.CurrentIndex);

            // update status
            indexChanges
                .Select(idx => GetStatus(idx, _configViews.Count))
                .ToProperty(this, x => x.Status, out _status);

            // update current page
            indexChanges
                .Select(idx => _configViews.ElementAtOrDefault(idx - 1))
                .ToProperty(this, x => x.CurrentPage, out _currentPage);

            // If status property changed
            var statusChanges = this.WhenAnyValue(x => x.Status);

            // update prev button visibility
            statusChanges
                .Select(status => status == eWizardStatus.FirstPage ? Visibility.Collapsed : Visibility.Visible)
                .ToProperty(this, x => x.PrevVisibility, out _prevVisibility);

            // update next button visibility
            statusChanges
                .Select(status => status == eWizardStatus.LastPage ? Visibility.Collapsed : Visibility.Visible)
                .ToProperty(this, x => x.NextVisibility, out _nextVisibility);

            // update cancel button visibility
            statusChanges
                .Select(status => status == eWizardStatus.LastPage ? Visibility.Collapsed : Visibility.Visible)
                .ToProperty(this, x => x.CancelVisibility, out _cancelVisibility);

            // update finish button visibility
            statusChanges
                .Select(status => status == eWizardStatus.LastPage ? Visibility.Visible : Visibility.Collapsed)
                .ToProperty(this, x => x.FinishVisibility, out _finishVisibility);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Determine the status by looking at the <paramref name="index"/>, <paramref name="max"/> values.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static eWizardStatus GetStatus(int index, int max)
        {
            if (index == 1)
                return eWizardStatus.FirstPage;
            else if (index == max)
                return eWizardStatus.LastPage;

            return eWizardStatus.MiddlePage;
        }

        /// <summary>
        /// Return to the previous step.
        /// </summary>
        private void Prev()
        {
            if (Status == eWizardStatus.FirstPage)
                return;

            CurrentIndex--;
        }

        /// <summary>
        /// Proceed to the next step.
        /// </summary>
        private void Next()
        {
            if (Status == eWizardStatus.LastPage)
                return;

            CurrentIndex++;
        }

        /// <summary>
        /// Finish all steps.
        /// </summary>
        private void Finish() => Finished?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Cancel all steps.
        /// </summary>
        private void Cancel() => Canceled?.Invoke(this, EventArgs.Empty);
        #endregion
    }
}
