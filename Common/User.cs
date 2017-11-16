using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Common
{
    [XmlInclude(typeof(User))]
    [XmlInclude(typeof(Guid))]
    public class User
    {
        public Guid Id;
        public string NickName;
        public string Password;
    }
}
