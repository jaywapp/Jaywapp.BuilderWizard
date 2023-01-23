using Jaywapp.Utility.BuilderWizard.Interface;
using System.Windows;

namespace Jaywapp.Utility.BuilderWizard.Model.State
{
    public class FirstState : IWizardState
    {
        /// <inheritdoc/>
        public Visibility GetCancelVisibility() => Visibility.Visible;
        /// <inheritdoc/>
        public Visibility GetFinishVisibility() => Visibility.Collapsed;
        /// <inheritdoc/>
        public Visibility GetNextVisibility() => Visibility.Visible;
        /// <inheritdoc/>
        public Visibility GetPrevVisibility() => Visibility.Collapsed;
    }
}
