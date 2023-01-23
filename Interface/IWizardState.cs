using System.Windows;

namespace Jaywapp.Utility.BuilderWizard.Interface
{
    public interface IWizardState
    {
        /// <summary>
        /// Prev button visibility
        /// </summary>
        /// <returns></returns>
        Visibility GetPrevVisibility();
        /// <summary>
        /// Next button visibility
        /// </summary>
        /// <returns></returns>
        Visibility GetNextVisibility();
        /// <summary>
        /// Cancel button visibility
        /// </summary>
        /// <returns></returns>
        Visibility GetCancelVisibility();
        /// <summary>
        /// Finish button visibility
        /// </summary>
        /// <returns></returns>
        Visibility GetFinishVisibility();
    }
}
