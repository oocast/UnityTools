using UnityEngine;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

public class SpecStore : MonoBehaviour, ISerializationCallbackReceiver
{
    public string storeName;
    public System.Type elementBaseType;
    public string filePath;

    [XmlElement(typeof(Skill))]
    [XmlElement(typeof(AOESkill))]
    public List<SpecObject> storeArray = new List<SpecObject>();

    [SerializeField]
    [HideInInspector]
    private string serializedStoreArray;
    private bool isDirty = false;

    public void OnBeforeSerialize()
    {
        SerializeStoreArray();
    }

    public void OnAfterDeserialize()
    {
        DeserializeStoreArray();
    }

    public void SerializeStoreArray()
    {
        if (storeArray.Count > 0 && !storeArray[0].GetType().Equals(typeof(SpecObject)))
        {
            var serializer = new XmlSerializer(typeof(List<SpecObject>));
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, storeArray);
                serializedStoreArray = stringWriter.ToString();
            }
        }
    }

    public void DeserializeStoreArray()
    {
        if (System.Threading.Thread.CurrentThread.ManagedThreadId == 1 && GUI.changed)
        {
            SetDirty(true);
        }
        else
        {
            SetDirty(false);
        }

        if (serializedStoreArray != null && serializedStoreArray.Length > 10 && isDirty == false)
        {
            var serializer = new XmlSerializer(typeof(List<SpecObject>));
            using (var stringReader = new StringReader(serializedStoreArray))
            {
                storeArray = serializer.Deserialize(stringReader) as List<SpecObject>;
            }
        }
    }

    public void ClearSerialization()
    {
        serializedStoreArray = "";
    }

    public void SetDirty(bool dirty)
    {
        isDirty = dirty;
    }
}
