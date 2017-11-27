using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Common
{
    [XmlInclude(typeof(Command))]
    [XmlInclude(typeof(User))]
    [XmlInclude(typeof(Guid))]
    [XmlInclude(typeof(Message))]
    [XmlInclude(typeof(Conversation))]
    [XmlInclude(typeof(List<Conversation>))]
    [XmlInclude(typeof(Sentence))]
    public class Command
    {
        public PackageTypeEnum Type;
        public Object Content;
    }
}
