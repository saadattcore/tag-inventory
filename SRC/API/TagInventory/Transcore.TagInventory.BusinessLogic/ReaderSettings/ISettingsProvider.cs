using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.BusinessLogic
{
    public interface ISettingsProvider
    {
        void SetAttenuation(int value);

        void RestartReader();
    }
}
