using FamilieLaissFlowJsHelper.Enums;
using FamilieLaissFlowJsHelper.Extensions;
using FamilieLaissFlowJsHelper.Interfaces;
using FamilieLaissFlowJsHelper.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FamilieLaissFlowJsHelper.Repositories
{
    public class FlowJsRepository : iFlowJsRepository
    {
        public FlowJsPostChunkResponse PostChunk(HttpRequest request, string folderTempUpload)
        {
            return PostChunkBase(request, folderTempUpload, null);
        }

        public FlowJsPostChunkResponse PostChunk(HttpRequest request, string folderTempUpload, FlowJsValidationRules validationRules)
        {
            return PostChunkBase(request, folderTempUpload, validationRules);
        }

        public bool ChunkExists(string folderTempUpload, HttpRequest request)
        {
            var identifier = request.Query["flowIdentifier"].ToString();
            var chunkNumber = int.Parse(request.Query["flowChunkNumber"].ToString());
            var chunkFullPathName = GetChunkFilename(chunkNumber, identifier, folderTempUpload);
            return File.Exists(Path.Combine(folderTempUpload, chunkFullPathName));
        }

        private FlowJsPostChunkResponse PostChunkBase(HttpRequest request, string folderTempUpload, FlowJsValidationRules validationRules)
        {
            var chunk = new FlowJsChunk();
            var requestIsSane = chunk.ParseForm(request.Form.AsNameValueCollection());
            if (!requestIsSane)
            {
                var errResponse = new FlowJsPostChunkResponse();
                errResponse.Status = enPostChunkStatus.Error;
                errResponse.ErrorMessages.Add("damaged");
            }

            List<string> errorMessages = null;
            var file = request.Form.Files[0];

            var response = new FlowJsPostChunkResponse { FileName = chunk.FileName, Size = chunk.TotalSize };
            response.TargetFilename = chunk.TargetFilename + Path.GetExtension(chunk.FileName);

            //Validieren des Chuncks anhand der Validation Rules
            var chunkIsValid = true;
            if (validationRules != null)
                chunkIsValid = chunk.ValidateBusinessRules(validationRules, out errorMessages);

            //Wenn der Chuck nicht validiert werden konnte wird ein Fehler an den Client zurückgemeldet
            if (!chunkIsValid)
            {
                response.Status = enPostChunkStatus.Error;
                response.ErrorMessages = errorMessages;
                return response;
            }

            //Speichern des Chucks im temporären Upload-Folder
            var chunkFullPathName = GetChunkFilename(chunk.Number, chunk.Identifier, folderTempUpload);
            try
            {
                // create folder if it does not exist
                if (!Directory.Exists(folderTempUpload)) Directory.CreateDirectory(folderTempUpload);
                // save file
                var fileToSave = File.Create(chunkFullPathName);
                file.CopyTo(fileToSave);
                fileToSave.Close();
                fileToSave.Dispose();
            }
            catch (Exception)
            {
                throw;
            }

            //Überprüfen ob noch weitere Chucks hochgeladen werden müssen
            if (chunk.TotalChunks > chunk.Number)
            {
                response.Status = enPostChunkStatus.PartlyDone;
                return response;
            }

            //Es wurden alle Chuncks hochgeladen. Ermitteln aller Chunck Files
            //und schreiben der Namen dieser Files in das Array
            var fileArray = new List<string>();
            for (int i = 1, l = chunk.TotalChunks; i <= l; i++)
            {
                fileArray.Add("flow-" + chunk.Identifier + "." + i);
            }
            response.FilenameChunks = fileArray;

            response.Status = enPostChunkStatus.Done;
            return response;
        }

        //Ermittelt den Namen für das Chunkfile
        private string GetChunkFilename(int chunkNumber, string identifier, string folder)
        {
            return Path.Combine(folder, "flow-" + identifier + "." + chunkNumber);
        }
    }
}
