using UnityEngine;
using System.Collections.Generic;
using JsonFx.Json;
using System.IO;
using System.Text;

namespace UEditor {
	
    public class FileMD5Item {
        public string FileName;
        public string MD5;
        public string Size;

        public FileMD5Item() { }

        public FileMD5Item(string name, string md5, string size) { 
            FileName = name;
            MD5 = md5;
            Size = size;
        }
    }

    public class FileMD5Col {
        public FileMD5Item[] fileInfo;

        private List<FileMD5Item> toBeSaved = new List<FileMD5Item>();
        public void addItem(FileMD5Item item) {
            if(item != null)
                toBeSaved.Add(item);
        }
        public void End() {
            fileInfo = toBeSaved.ToArray();
            toBeSaved.Clear();
        }
    }

	public class PrefabCol{
		public string FileName;
		public FileMD5Item[] FileInfo;

		private List<FileMD5Item> toBeSaved = new List<FileMD5Item>();

		public PrefabCol(){

		}

		public PrefabCol(string name){
			FileName = name;
		}

		public void addItem(FileMD5Item item) {
			if(item != null)
				toBeSaved.Add(item);
		}

		public void End() {
			FileInfo = toBeSaved.ToArray();
			toBeSaved.Clear();
		}
	}

	public class PrefabInfo{
		public PrefabCol[] Files;

		private List<PrefabCol> toBeSaved = new List<PrefabCol>();
		public void addItem(PrefabCol item) {
			if(item != null)
				toBeSaved.Add(item);
		}
		public void End() {
			Files = toBeSaved.ToArray();
			toBeSaved.Clear();
		}

	}
	
    public class VersionNumItem {
        public string FileName;
        public int Num;
        public string Size;

        public VersionNumItem() { }

        public VersionNumItem(string fn, int ver , string size) { 
            FileName = fn;
			Num = ver;
            Size = size;
        }
    }

    public class VersionNum {
		public string Game;
		public string Platform;
		public string Version;
		public VersionNumItem[] Files;

        private List<VersionNumItem> toBeSaved = new List<VersionNumItem>();

        public void addItem(VersionNumItem Item) {
            if(Item != null)
                toBeSaved.Add(Item);
        }

		public VersionNum(){

		}
		public VersionNum(string game,string platform,string version){
			this.Game = game;
			this.Platform = platform;
			this.Version = version;
		}

        public void End() {
			Files = toBeSaved.ToArray();

            toBeSaved.Clear();
        }

    }

    public class IO {

        public static bool SaveToFile(System.Object save, string path, FileMode mode = FileMode.Create){
            string outdata = JsonWriter.Serialize(save);
            if (outdata == null || outdata == string.Empty)
                return false;

            using (StreamWriter sw = new StreamWriter(File.Open(path, mode)))
            {
                sw.Write(outdata);
            }
            return true;
        }

        public static bool SaveToFile<T>(T[] save, string path, FileMode mode = FileMode.Create) where T : class {
            if (save == null)
                return false;

            StringBuilder sb = new StringBuilder();

            foreach(T obj in save) {
                sb.Append(JsonWriter.Serialize(obj) + "\n");
            }

            using (StreamWriter sw = new StreamWriter(File.Open(path, mode)))
            {
                sw.Write(sb.ToString());
            }
            return true;
        }

        public static T LoadFromFile<T>(string path) where T : class {
            T obj = default(T);
            string jsonText = null;

            try {
                using (StreamReader sr = new StreamReader(path))
                {
                    // Read and display lines from the file until the end of the file is reached.
                    jsonText = sr.ReadToEnd();
                }
                obj = JsonReader.Deserialize(jsonText, typeof(T)) as T;
            } catch(System.Exception ex) {
                Debug.LogWarning(ex.ToString());
            }

            return obj;
        }

    }
}