using FamilieLaissFlowJsHelper.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissFlowJsHelper.Interfaces
{
    public interface iFlowJsRepository
    {
        FlowJsPostChunkResponse PostChunk(HttpRequest request, string folderTempUpload);

        FlowJsPostChunkResponse PostChunk(HttpRequest request, string folderTempUpload, FlowJsValidationRules validationRules);

        bool ChunkExists(string folderTempUpload, HttpRequest request);
    }
}
