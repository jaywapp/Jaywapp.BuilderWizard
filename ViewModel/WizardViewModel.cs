using Prism.Commands;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using Jaywapp.Utility.BuilderWizard.Interface;
using System;
using Jaywapp.Utility.BuilderWizard.Model.State;
using Jaywapp.Utility.BuilderWizard.Service;

namespace Jaywapp.Utility.BuilderWizard.ViewModel
{
    public partial class WizardViewModel : ReactiveObject
    {
        #region Internal Field
        private int _currentIndex = 1;
        private IReadOnlyList<IJayBuilderConfigView> _configViews;

        private ObservableAsPropertyHelper<IWizardState> _state;
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
        /// State
        /// </summary>
        public IWizardState State => _state.Value;
        
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
                .Select(idx => WizardStateFactory.Factory(idx, _configViews.Count))
                .ToProperty(this, x => x.State, out _state);

            // update current page
            indexChanges
                .Select(idx => _configViews.ElementAtOrDefault(idx - 1))
                .ToProperty(this, x => x.CurrentPage, out _currentPage);

            // If status property changed
            var stateChanges = this.WhenAnyValue(x => x.State);

            // update prev button visibility
            stateChanges
                .Select(state=> state.GetPrevVisibility())
                .ToProperty(this, x => x.PrevVisibility, out _prevVisibility);

            // update next button visibility
            stateChanges
                .Select(state => state.GetNextVisibility())
                .ToProperty(this, x => x.NextVisibility, out _nextVisibility);

            // update cancel button visibility
            stateChanges
                .Select(state => state.GetCancelVisibility())
                .ToProperty(this, x => x.CancelVisibility, out _cancelVisibility);

            // update finish button visibility
            stateChanges
                .Select(state => state.GetFinishVisibility())
                .ToProperty(this, x => x.FinishVisibility, out _finishVisibility);
        }
        #endregion

        #region Functions
        /// <summary>
        /// Return to the previous step.
        /// </summary>
        private void Prev()
        {
            if(State.GetType() == typeof(FirstState))
                return;

            CurrentIndex--;
        }

        /// <summary>
        /// Proceed to the next step.
        /// </summary>
        private void Next()
        {
            if (State.GetType() == typeof(LastState))
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
