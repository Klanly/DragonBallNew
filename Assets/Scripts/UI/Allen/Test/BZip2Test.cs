using UnityEngine;
using System.Collections;
using Framework;

public class BZip2Test : MonoBehaviour {

	void OnGUI() {
		if(GUI.Button(new Rect(10,10,100,100), @"BZip2 Decompress")) {
			decompress();
		}

        if(GUI.Button(new Rect(130,10,100,100), "Generate Battle")) {
			compress();
		}

		if(GUI.Button(new Rect(310,10,100,100), "GZip Compress")) {
			//ConsoleEx.DebugLog("compress : " + GZip.Compress("zhangyuntaozhangyuntao"));
			decompressGzip();
		}

		if(GUI.Button(new Rect(310,220,100,100), "Deflate Decompress")) {
			decompressDeflate();
		}

		if(GUI.Button(new Rect(130,220,100,100), "Deflate Decompress")) {
			decompressDeflateSh();
		}
	}

	void decompress() {
		string compressed = @"rZTBjpswEIbfxWe6msEhCZwaRT1EatVD91KteqBg0mhdg4xJG0W8e21jgtlCyra9eTzjmfnmH\/lKapWqpibJGwxIzr42x4MoSpI8fQlIqtSu\/HzqjJwVg6E9jyz9TpIrScVRXvZCkQQCouzl05WIT88nzu2xNqdDTpIQMYqCzn7Pzky7sQ1GAbChLwNMtalkEdDNRGzWyJ3SzYSg2+HOEZBKlhWT6kIS3UHKeZEqltuWRaM7RsBVqKPO7CBy9lM7TC51qZiNkSVntug63GyhtcP4G37Y0j\/wx+vl\/Ou7\/HSGH2f4Y+rz48CPAz+07durET\/Nnpm0z3tj789dj4eJmu39XtzK7My4rApuhu4i1CH9KsJvm+gSmr68Bt6J7Ju+esBoeBtOtEQnWvLa7NLAwzZ6UdfUqpuiMKhoS0hlX0d68sVJpPxm9WPHlR6jZFl5ZtKR6UJZWasBvJLMc2khjqXshJnc7o+V9mHsrMd+J03FW5YPTSdzltbKCdMPZAXoc4DPEYPPEYPPAfc5jF7LQNb\/AoKzICNBaOSDGOsGAtv7IAs5vL\/LcIQjCnyVHKMtEw3ns\/rQkT7U1wfh\/2ANX9LrsXDZllkJ+sCRUNZz+7Fm1V2YAGwCF6aX7sdJCJPLfAik\/QU=";
		ConsoleEx.DebugLog(" decompress : " + DeflateEx.Decompress(compressed));
	}

	void compress() {
		//string uncompressed = @"[-1,0,2,401,401,0,2,401,0,2,401,0,2,404,0,2,403,0,2,404,5,5,7]";
		string uncompressed = @"[-1,0,2,401,401,401,401,5,5,7]";

        ConsoleEx.DebugLog("compress : " + DeflateEx.Compress(uncompressed));

		string uncompressed2 = @"{""status"":-1,""debugInfo"":[],""attAoYi"":[],""defAoYi"":[],""attTeam"":{""angryCnt"":0,""team"":[{""nSkill"":[{""skillId"":21155,""skillLevel"":1},{""skillId"":21073,""skillLevel"":1}],""aSkill"":[{""skillId"":25037,""skillLevel"":1}],""curAtt"":200,""level"":1,""property"":5,""allfated"":0,""num"":10142,""pveIndex"":0}],""type"":0,""roleId"":262780},""defTeam"":{""angryCnt"":0,""team"":[{""nSkill"":[{""skillId"":21083,""skillLevel"":1},{""skillId"":21096,""skillLevel"":1}],""aSkill"":[{""skillId"":25067,""skillLevel"":1}],""curAtt"":230,""level"":1,""property"":1,""allfated"":0,""num"":10193,""pveIndex"":1}],""type"":1,""roleId"":0}}@{""attacker"":0,""attackerCurAtt"":200,""defenseCurAtt"":230,""attTeamAngry"":5,""defTeamAngry"":20,""status"":0,""debugInfo"":[],""defense"":1}@{""attackerEnch"":1.15,""status"":2,""attackerCurAtt"":230,""defenseCurAtt"":195,""defenseEnch"":0.85,""debugInfo"":[]}@{""suffer"":1,""startAtt"":57,""finalAtt"":57,""curAtt"":143,""recoverAngry"":0,""costAngry"":5,""preAngry"":0,""category"":1,""skillId"":25037,""skillOp"":119,""skillType"":0,""curAngry"":0,""Mul"":1,""caster"":0,""status"":401}@{""suffer"":0,""startAtt"":90,""finalAtt"":90,""curAtt"":140,""recoverAngry"":0,""costAngry"":20,""preAngry"":0,""category"":1,""skillId"":25067,""skillOp"":119,""skillType"":0,""curAngry"":0,""Mul"":1,""caster"":1,""status"":401}@{""suffer"":1,""startAtt"":35,""finalAtt"":35,""curAtt"":108,""recoverAngry"":0,""costAngry"":0,""preAngry"":0,""category"":1,""skillId"":21155,""skillOp"":2,""skillType"":1,""curAngry"":0,""Mul"":1,""caster"":0,""status"":401,""debugInfo"":null}@{""suffer"":0,""startAtt"":30,""finalAtt"":30,""curAtt"":110,""recoverAngry"":0,""costAngry"":0,""preAngry"":0,""category"":1,""skillId"":21083,""skillOp"":2,""skillType"":1,""curAngry"":0,""Mul"":1,""caster"":1,""status"":401}@{""suffer"":0,""startAtt"":108,""status"":5,""finalAtt"":108,""curAtt"":2}@{""suffer"":1,""startAtt"":108,""status"":5,""finalAtt"":108,""curAtt"":0}@{""status"":7,""winner"":""att""}";
        string test = DeflateEx.Compress(uncompressed2);
        ConsoleEx.DebugLog("compress : " + test);

        ConsoleEx.DebugLog("Decompess : " + DeflateEx.Decompress(test));

	}

	void decompressGzip () {
		string compressed = @"H4sIAAAAAAAAA0spzcpMTCnNTIHSAJAYGC0QAAAA";
		ConsoleEx.DebugLog("decompress : " + GZip.Decompress(compressed));
	}

	void decompressDeflate () {
		string t = @"Hco7DoAgEEXRvbwak/kwwrAVYmxsiFR+KuPeJZb35NYH97G2DYU1EccwsrfzQqmYnEVdkmUmM3ZXBDi5UCTPlGnWGPm3ZOMwnmU4qWAJ6DsK8C4f";
		ConsoleEx.DebugLog("decompress : " + Deflate.Decompress(t));
	}

	void decompressDeflateSh () {
		string t = @"Hco7DoAgEEXRvbwak/kwwrAVYmxsiFR+KuPeJZb35NYH97G2DYU1EccwsrfzQqmYnEVdkmUmM3ZXBDi5UCTPlGnWGPm3ZOMwnmU4qWAJ6DsK8C4f";
		ConsoleEx.DebugLog("decompress : " + DeflateEx.Decompress(t));
	}
}
