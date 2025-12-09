using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Repository
{
    public interface ICsvExportService
    {
        void ExportTimeRecords<T>(IEnumerable<T> data, string filePath, params string[] selectedProperties);
    }
}
