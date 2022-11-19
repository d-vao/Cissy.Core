using Microsoft.Extensions.Primitives;
using System;
using Cissy.Http;
using Cissy;

namespace Cissy.Configuration
{
    public class DatabaseChangeToken : IChangeToken
    {
        private string _fileUrl;

        public DatabaseChangeToken(string fileUrl)
        {
            _fileUrl = fileUrl;
        }

        public bool ActiveChangeCallbacks => false;

        public bool HasChanged
        {
            get
            {
                try
                {
                    RemoteConfig RemoteConfig = default(RemoteConfig);
                    Actor.Public.Get(_fileUrl, m =>
                          {
                              RemoteConfig = m.JsonToModel<RemoteConfig>();
                          });
                    if (RemoteConfig.IsNotNull())
                    {
                        return RemoteConfig.lastModiTime > RemoteConfig.lastRequestTime;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public IDisposable RegisterChangeCallback(Action<object> callback, object state) => EmptyDisposable.Instance;
    }

    internal class EmptyDisposable : IDisposable
    {
        public static EmptyDisposable Instance { get; } = new EmptyDisposable();
        private EmptyDisposable() { }
        public void Dispose() { }
    }
}