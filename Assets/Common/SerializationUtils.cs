using System.Collections.Generic;
using System.IO;

namespace Common
{
    public static class SerializationUtils
    {
        public static IEnumerable<string> ReadLines(string path)
        {
            using (var inpStm = new StreamReader(path))
                while (!inpStm.EndOfStream)
                    yield return inpStm.ReadLine();
        }

        public static IEnumerable<IDictionary<string, string>> ReadCsvLines(string path)
        {
            using (var stream = new StreamReader(path))
            {
                if (stream.EndOfStream)
                    yield break;

                var headerLine = stream.ReadLine();
                if (headerLine == null)
                    yield break;

                var headers = headerLine.Split(',');

                while (!stream.EndOfStream)
                {
                    var line = stream.ReadLine();
                    if (line == null)
                        yield break;
                    var values = line.Split(',');
                    var dic = new Dictionary<string, string>();
                    for (var i = 0; i < headers.Length; i++)
                        dic[headers[i]] = values[i];
                    yield return dic;
                }
            }
        }
    }
}