using FamilieLaissFlowJsHelper.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissFlowJsHelper.Models
{
    public class FlowJsPostChunkResponse
    {
        public FlowJsPostChunkResponse()
        {
            ErrorMessages = new List<string>();
        }

        public string FileName { get; set; }
        public string TargetFilename { get; set; }
        public long Size { get; set; }
        public enPostChunkStatus Status { get; set; }
        public List<string> FilenameChunks { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
