using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GoogleDomainsDdnsSvc
{
    public class DomainsSection : ConfigurationSection
    {
        [ConfigurationProperty("domains", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(DomainsCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public DomainsCollection Domains
        {
            get
            {
                DomainsCollection domainsCollection =
                    (DomainsCollection)base["domains"];
                return domainsCollection;
            }
        }

    }

    public class DomainsCollection : ConfigurationElementCollection
    {
        public DomainsCollection()
        {
            DomainConfigElement domain = (DomainConfigElement)CreateNewElement();
            Add(domain);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DomainConfigElement();
        }

        protected override Object GetElementKey(ConfigurationElement domain)
        {
            return ((DomainConfigElement)domain).UserName;
        }

        public DomainConfigElement this[int index]
        {
            get
            {
                return (DomainConfigElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public DomainConfigElement this[string HostName]
        {
            get
            {
                return (DomainConfigElement)BaseGet(HostName);
            }
        }

        public int IndexOf(DomainConfigElement domain)
        {
            return BaseIndexOf(domain);
        }

        public void Add(DomainConfigElement domain)
        {
            BaseAdd(domain);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(DomainConfigElement domain)
        {
            if (BaseIndexOf(domain) >= 0)
                BaseRemove(domain.HostName);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string hostname)
        {
            BaseRemove(hostname);
        }

        public void Clear()
        {
            BaseClear();
        }
    }

    public class DomainConfigElement : ConfigurationElement
    {
        public DomainConfigElement(string hostname, string username, string password, double longDelay = 86400000, double shortDelay = 600000)
        {
            HostName = hostname;
            UserName = username;
            Password = password;
            LongDelay = longDelay;
            ShortDelay = shortDelay;
        }

        public DomainConfigElement()
        {
        }

        public readonly Timer Timer = new Timer();

        [ConfigurationProperty("hostname", IsRequired = true, IsKey = true)]
        public string HostName
        {
            get
            {
                return (string)this["hostname"];
            }
            set
            {
                this["hostname"] = value;
            }
        }

        [ConfigurationProperty("username", IsRequired = true)]
        public string UserName
        {
            get
            {
                return (string)this["username"];
            }
            set
            {
                this["username"] = value;
            }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
            set
            {
                this["password"] = value;
            }
        }

        [ConfigurationProperty("longDelay", DefaultValue = (double)86400000, IsRequired = false)]
        public double LongDelay
        {
            get
            {
                return (double)this["longDelay"];
            }
            set
            {
                this["longDelay"] = value;
            }
        }

        [ConfigurationProperty("shortDelay", DefaultValue = (double)600000, IsRequired = false)]
        public double ShortDelay
        {
            get
            {
                return (double)this["shortDelay"];
            }
            set
            {
                this["shortDelay"] = value;
            }
        }

        public override string ToString()
        {
            return HostName;
        }
    }
}
