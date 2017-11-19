using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public interface ISerialization
    {
        byte[] Serialize<T>(T item);
        T Deserialize<T>(byte[] data);
    }

}
