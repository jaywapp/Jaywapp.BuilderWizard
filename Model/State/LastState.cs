using Jaywapp.BuilderWizard.Interface;
using System.Windows;

namespace Jaywapp.BuilderWizard.Model.State
{
    public class LastState : IWizardState
    {
        /// <inheritdoc/>
        public Visibility GetCancelVisibility() => Visibility.Collapsed;
        /// <inheritdoc/>
        public Visibility GetFinishVisibility() => Visibility.Visible;
        /// <inheritdoc/>
        public Visibility GetNextVisibility() => Visibility.Collapsed;
        /// <inheritdoc/>
        public Visibility GetPrevVisibility() => Visibility.Visible;
    }
}
