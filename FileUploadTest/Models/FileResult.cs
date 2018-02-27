using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileUploadTest.Models
{
    public class FileResult
    {
        public IEnumerable<string> FileNames { get; set; }
        public string ClassID { get; set; }
        public string Token { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime UpdatedTimestamp { get; set; }
        public string DownloadLink { get; set; }
        public IEnumerable<string> ContentTypes { get; set; }
        public IEnumerable<string> Names { get; set; }
    }
}