using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationB.Management.Repositories
{
    public interface IConfigurationReaderServices
    {
        T GetValue<T>(string key);    }
}
