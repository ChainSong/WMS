using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Dao.RabbitMQ
{
    public class AssemblySerializeBinder : SerializationBinder
    {
        private readonly string _typeName;

        public AssemblySerializeBinder(string typeName)
        {
            _typeName = typeName;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            if (typeName == null) throw new ArgumentNullException("typeName");
            typeName = _typeName;
            var typeToDeserialize = Type.GetType(string.Format("{0}, {1}",
                typeName, assemblyName));
            return typeToDeserialize;
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = _typeName;
        }
    }
}
