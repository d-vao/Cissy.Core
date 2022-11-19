using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Text;
using Cissy.Http;
using Cissy;

namespace Cissy.Configuration
{
    internal class RemoteConfig : IModel
    {
        public string body;
        public DateTime lastModiTime;
        public DateTime lastRequestTime;
    }
    public class DatabaseFileInfo : IFileInfo
    {
        private string _FileUrl;
        private byte[] _fileContent;
        private DateTime _lastModified;
        private bool _exists;

        public DatabaseFileInfo(string fileUrl)
        {
            _FileUrl = fileUrl;
            GetFile(fileUrl);
        }
        public bool Exists => _exists;

        public bool IsDirectory => false;

        public DateTimeOffset LastModified => _lastModified;

        public long Length
        {
            get
            {
                using (var stream = new MemoryStream(_fileContent))
                {
                    return stream.Length;
                }
            }
        }

        public string Name => Path.GetFileName(_FileUrl);

        public string PhysicalPath => null;

        public Stream CreateReadStream()
        {
            return new MemoryStream(_fileContent);
        }

        private void GetFile(string fileUrl)
        {
            try
            {
                RemoteConfig RemoteConfig = default(RemoteConfig);
                Actor.Public.Get(fileUrl, m =>
                {
                    RemoteConfig = m.JsonToModel<RemoteConfig>();
                });
                if (RemoteConfig.IsNotNull())
                {
                    _exists = true;
                    _lastModified = RemoteConfig.lastModiTime;
                    _fileContent = Convert.FromBase64String(RemoteConfig.body);
                }
            }
            catch
            {

            }
            //var query = $@"SELECT Content, LastModified FROM Views WHERE Location = @Path;
            //        UPDATE Views SET LastRequested = GetUtcDate() WHERE Location = @Path";
            //try
            //{
            //    using (var conn = new SqlConnection(connection))
            //    using (var cmd = new SqlCommand(query, conn))
            //    {
            //        cmd.Parameters.AddWithValue("@Path", fileUrl);
            //        conn.Open();
            //        using (var reader = cmd.ExecuteReader())
            //        {
            //            _exists = reader.HasRows;
            //            if (_exists)
            //            {
            //                reader.Read();
            //                _fileContent = Encoding.UTF8.GetBytes(reader["Content"].ToString());
            //                _lastModified = Convert.ToDateTime(reader["LastModified"]);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // if something went wrong, Exists will be false
            //}
        }
    }
}
