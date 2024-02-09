using System.Collections.Generic;

namespace _Sources.Interfaces
{
    public interface IDictSerializable
    {
        void Deserialize(Dictionary<string, string> dictionary);
    }
}