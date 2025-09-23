using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Common.Dtos
{
    public class FileResourceDto
    {
        public required string Url { get; set; }
        public required string FileName { get; set; }
        public string? ContentType { get; set; }
        public long SizeBytes { get; set; }
    }
}
