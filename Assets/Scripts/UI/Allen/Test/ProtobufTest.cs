using UnityEngine;
using System;
using System.IO;
using System.Collections;
#if PROTOBUF
using Protobuf_DTO;
#endif

public class ProtobufTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		protobuf_ToDeserialize();
	}
	
	void protobuf_ToSerialize() {
		#if PROTOBUF
		Address add = new Address();
		add.Line1 = "Perking";
		add.Line2 = "LaMaTemper";

		Person per = new Person();
		per.Address = add;
		per.Id = 14;
		per.Name = "allen";

		using (FileStream fs = File.Open(DeviceInfo.PersisitFullPath("proto"), FileMode.Create)) {
			var ser = new MySerializer();
			ser.Serialize(fs, per);
		}
		#endif
	}

	void protobuf_ToDeserialize() {
		#if PROTOBUF
		Person per = new Person();
		if(File.Exists(DeviceInfo.PersisitFullPath("proto"))) {
			using (FileStream fs = File.OpenRead(DeviceInfo.PersisitFullPath("proto"))) {
				var ser = new MySerializer();

				ser.Deserialize(fs, per, typeof(Person));

				ConsoleEx.DebugLog(JsonFx.Json.JsonWriter.Serialize(per));
			}
		} else {
			AsyncTask.RunAsync( () => {protobuf_ToSerialize();} );
		}
		#endif
	}
}
