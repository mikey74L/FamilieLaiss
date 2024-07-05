using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FamilieLaissFlowJsHelper.Models
{
    public class FlowJsChunk
    {
        #region Private Methods
        private string CleanIdentifier(string identifier)
        {
            identifier = Regex.Replace(identifier, "/[^0-9A-Za-z_-]/g", "");
            return identifier;
        }
        #endregion

        #region Internal Methods
        internal bool ParseForm(NameValueCollection form)
        {
            try
            {
                if (string.IsNullOrEmpty(form["flowIdentifier"]) || string.IsNullOrEmpty(form["flowFilename"]))
                    return false;

                Number = int.Parse(form["flowChunkNumber"]);
                Size = long.Parse(form["flowChunkSize"]);
                TotalSize = long.Parse(form["flowTotalSize"]);
                Identifier = CleanIdentifier(form["flowIdentifier"]);
                FileName = form["flowFilename"];
                TotalChunks = int.Parse(form["flowTotalChunks"]);
                TargetFilename = form["flowTargetname"];
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        internal bool ValidateBusinessRules(FlowJsValidationRules rules, out List<string> errorMessages)
        {
            errorMessages = new List<string>();
            if (rules.MaxFileSize.HasValue && TotalSize > rules.MaxFileSize.Value)
                errorMessages.Add(rules.MaxFileSizeMessage ?? "File size --> File is to big");

            if (rules.AcceptedExtensions.Count > 0 && rules.AcceptedExtensions.SingleOrDefault(x => x.ToLower() == FileName.Split('.').Last().ToLower()) == null)
                errorMessages.Add(rules.AcceptedExtensionsMessage ?? $"Wronng file extension \"{FileName.Split('.').Last().ToLower()}\"");

            return errorMessages.Count == 0;
        }
        #endregion

        #region Public Properties
        public int Number { get; set; }
        public long Size { get; set; }
        public long TotalSize { get; set; }
        public string Identifier { get; set; }
        public string FileName { get; set; }
        public int TotalChunks { get; set; }
        public string TargetFilename { get; set; }
        #endregion
    }
}
