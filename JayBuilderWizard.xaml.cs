using System;
using System.Windows;
using Jaywapp.BuilderWizard.ViewModel;
using Jaywapp.BuilderWizard.Interface;

namespace Jaywapp.BuilderWizard
{
    public partial class JayBuilderWizard : Window
    {
        #region Properties
        /// <summary>
        /// Builder to create through wizard
        /// </summary>
        public IJayBuilder Builder { get; private set; }
        /// <summary>
        /// ViewModel associated with WizardView
        /// </summary>
        public WizardViewModel WizardViewModel { get; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title"></param>
        /// <param name="builder"></param>
        /// <param name="configViews"></param>
        public JayBuilderWizard(string title, IJayBuilder builder, params IJayBuilderConfigView[] configViews)
        {
            Builder = builder;

            WizardViewModel = new WizardViewModel(configViews);
            WizardViewModel.Finished += OnFinished;
            WizardViewModel.Canceled += OnCanceled;

            foreach (var configView in configViews)
                configView.ViewModel.Builder = builder;

            InitializeComponent();

            _window.Title = title;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="builder"></param>
        /// <param name="configViews"></param>
        public JayBuilderWizard(string title, double width, double height, IJayBuilder builder, params IJayBuilderConfigView[] configViews)
            : this(title, builder, configViews)
        {
            _window.Width = width;
            _window.Height = height;
        }
        #endregion

        #region Functions
        /// <summary>
        ///  <see cref="WizardViewModel.Canceled"/> callback method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCanceled(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>
        ///  <see cref="WizardViewModel.Finished"/> callback method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFinished(object sender, EventArgs e)
        {
            Builder = WizardViewModel.CurrentPage.ViewModel.Builder;
            DialogResult = true;
            Close();
        }
        #endregion
    }
}
