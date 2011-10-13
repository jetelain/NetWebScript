using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace NetWebScript.Debug.Server
{
    internal class ProgramIdentity
    {
        private readonly int port;
        private readonly String hostname;
        private readonly IPAddress[] addresses;
        private readonly String pathPrefix;

        internal ProgramIdentity(Uri uri)
        {
            port = uri.Port;
            pathPrefix = uri.LocalPath;

            IPAddress address;
            if (IPAddress.TryParse(uri.Host, out address))
            {
                if (IPAddress.IsLoopback(address))
                {
                    addresses = new IPAddress[] { IPAddress.Loopback, IPAddress.IPv6Loopback };
                }
                else
                {
                    addresses = new IPAddress[] { address };
                }
            }
            else
            {
                IPAddress[] uriAddresses = Dns.GetHostAddresses(uri.DnsSafeHost);
                if (uriAddresses.FirstOrDefault(IPAddress.IsLoopback) != null)
                {
                    addresses = new IPAddress[] { IPAddress.Loopback, IPAddress.IPv6Loopback };
                }
                else
                {
                    hostname = uri.Host;
                }
            }
        }

        internal bool IsPartOfProgram(Uri uri)
        {
            if (uri.Port != port)
            {
                return false;
            }

            if (!uri.LocalPath.StartsWith(pathPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (hostname != null)
            {
                if (uri.Host != hostname)
                {
                    return false;
                }
                return true;
            }

            IPAddress[] uriAddresses = Dns.GetHostAddresses(uri.DnsSafeHost);

            foreach (IPAddress a in addresses)
            {
                foreach (IPAddress b in uriAddresses)
                {
                    if (a.Equals(b))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
 

    }
}
