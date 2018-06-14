using ConfigurationB.Management.Entities;
using System.Collections.Generic;

namespace ConfigurationB.MVC.Models
{
    public class ConfigurationViewModel
    {
        public List<ConfigurationItem> ConfigurationItems { get; set; }

        public ConfigurationViewModel()
        {
            ConfigurationItems = new List<ConfigurationItem>();
        }
    }
}