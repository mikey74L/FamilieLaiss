using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissFlowJsHelper.Models
{
    public class FlowJsValidationRules
    {
        public FlowJsValidationRules()
        {
            AcceptedExtensions = new List<string>();
        }



        public long? MaxFileSize { get; set; }
        public string MaxFileSizeMessage { get; set; }

        public List<string> AcceptedExtensions { get; set; }
        public string AcceptedExtensionsMessage { get; set; }
    }
}
