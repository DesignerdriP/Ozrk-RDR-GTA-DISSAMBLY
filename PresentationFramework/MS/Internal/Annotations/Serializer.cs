using System;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace MS.Internal.Annotations
{
	// Token: 0x020007CE RID: 1998
	internal class Serializer
	{
		// Token: 0x06007BAE RID: 31662 RVA: 0x0022BCCC File Offset: 0x00229ECC
		public Serializer(Type type)
		{
			Invariant.Assert(type != null);
			object[] customAttributes = type.GetCustomAttributes(false);
			foreach (object obj in customAttributes)
			{
				this._attribute = (obj as XmlRootAttribute);
				if (this._attribute != null)
				{
					break;
				}
			}
			Invariant.Assert(this._attribute != null, "Internal Serializer used for a type with no XmlRootAttribute.");
			this._ctor = type.GetConstructor(new Type[0]);
		}

		// Token: 0x06007BAF RID: 31663 RVA: 0x0022BD40 File Offset: 0x00229F40
		public void Serialize(XmlWriter writer, object obj)
		{
			Invariant.Assert(writer != null && obj != null);
			IXmlSerializable xmlSerializable = obj as IXmlSerializable;
			Invariant.Assert(xmlSerializable != null, "Internal Serializer used for a type that isn't IXmlSerializable.");
			writer.WriteStartElement(this._attribute.ElementName, this._attribute.Namespace);
			xmlSerializable.WriteXml(writer);
			writer.WriteEndElement();
		}

		// Token: 0x06007BB0 RID: 31664 RVA: 0x0022BD9C File Offset: 0x00229F9C
		public object Deserialize(XmlReader reader)
		{
			Invariant.Assert(reader != null);
			IXmlSerializable xmlSerializable = (IXmlSerializable)this._ctor.Invoke(new object[0]);
			if (reader.ReadState == ReadState.Initial)
			{
				reader.Read();
			}
			xmlSerializable.ReadXml(reader);
			return xmlSerializable;
		}

		// Token: 0x04003A31 RID: 14897
		private XmlRootAttribute _attribute;

		// Token: 0x04003A32 RID: 14898
		private ConstructorInfo _ctor;
	}
}
