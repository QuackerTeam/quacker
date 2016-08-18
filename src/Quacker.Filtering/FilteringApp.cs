using Quacker.Filtering.Interfaces;
using System;

namespace Quacker.Filtering
{
    public static class FilteringApp
    {
        public static void Initialize(Action<IFilteringConfiguration> configurationAction)
        {
            if (configurationAction == null)
                throw new ArgumentNullException(nameof(configurationAction));
            try
            {
                configurationAction(new FilteringConfiguration());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Uncaught error ocurred in initialization. See inner exception for further details.", ex);
            }
        }
    }
}