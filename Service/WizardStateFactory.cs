using Jaywapp.BuilderWizard.Interface;
using Jaywapp.BuilderWizard.Model.State;

namespace Jaywapp.BuilderWizard.Service
{
    public static class WizardStateFactory
    {
        /// <summary>
        /// Determine the state by looking at the <paramref name="index"/>, <paramref name="max"/> values.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static IWizardState Factory(int index, int max)
        {
            if (index == 1)
                return new FirstState();
            else if (index == max)
                return new LastState();

            return new MiddleState();
        }
    }
}
