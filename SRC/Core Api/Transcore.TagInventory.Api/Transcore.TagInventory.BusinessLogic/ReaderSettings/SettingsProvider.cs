using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Transcore.TagInventory.BusinessLogic
{
    public class SettingsProvider : ISettingsProvider
    {
        public void RestartReader()
        {
            throw new NotImplementedException();
        }

        public void SetAttenuation(int value)
        {
            MessageQueue attenuationQue = new MessageQueue();

            attenuationQue.Path = @".\private$\Attenuation";

            if (!MessageQueue.Exists(@".\private$\Attenuation"))
            {
                MessageQueue.Create(attenuationQue.Path);
            }

            attenuationQue.Formatter = new XmlMessageFormatter(new Type[] { typeof(int) });

            attenuationQue.Send(value);

            attenuationQue.Close();           

        }

        private void AttenuationQue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
