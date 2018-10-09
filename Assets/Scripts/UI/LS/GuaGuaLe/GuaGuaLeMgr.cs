using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GuaGuaLeMgr : Manager 
{
	 
	public List<GuaGuaLeData> GuaGuaLeDataList;

	List<List<int>> CheckList;

	const int Maxnum = 9;
	const int GetRewardNum = 3;

	public GuaGuaLeMgr()
	{
		GuaGuaLeDataList = new List<GuaGuaLeData>();
		CheckList = new List<List<int>>();
	}

	public override bool loadFromConfig () 
	{
		return base.readFromLocalConfigFile<GuaGuaLeData>(ConfigType.GuaGuaLe, GuaGuaLeDataList);
	}

	public int[] GetGuaGuaLeData(int Stone=0)
	{
		for(int z=0; z<CheckList.Count; z++)
		{
			CheckList[z].Clear();
		}
		CheckList.Clear();


		if(Stone == 0)
		{
			for(int i=0; i<Maxnum; i++)
			{
				int index = GetRandomNum(0, GuaGuaLeDataList.Count-1);
				if(CheckList.Count == 0)
				{
					CheckList.Add(new List<int>());
					CheckList[0].Add(GuaGuaLeDataList[index].MonsterId);
				}
				else
				{
					CheckGuaGuaLeDataList(index);
				}
			}
		}
		else 
		{
			CheckList.Add(new List<int>());
			for(int i=0; i<GetRewardNum; i++)
			{
				CheckList[0].Add(GetHeadID(Stone));
			}

			int totalcount = Maxnum-GetRewardNum;
			for(int i=0; i<totalcount; i++)
			{
				int index = GetRandomNum(0, GuaGuaLeDataList.Count-1);
				CheckGuaGuaLeDataList(index);
			}
		}


		int[] GetHeadArray = new int[9];
		int total = 0;
		for(int i=0; i<CheckList.Count; i++)
		{
			for(int j=0; j<CheckList[i].Count; j++)
			{
				GetHeadArray[total] = CheckList[i][j];
				total++;
			}
		}

		for(int i=0; i<10; i++)
		{
			int num1 = GetRandomNum(0,8);
			int num2 = GetRandomNum(0,8);
			int temp = GetHeadArray[num1];
			GetHeadArray[num1] = GetHeadArray[num2];
			GetHeadArray[num2] = temp;
        }
        
		return GetHeadArray;
    }
    
    void CheckGuaGuaLeDataList(int index)
	{
		for(int j=0; j<CheckList.Count; j++)
		{
			if(CheckList[j][0] == GuaGuaLeDataList[index].MonsterId )
			{
				if(CheckList[j].Count >= 2)
				{
					if(index + 1 < GuaGuaLeDataList.Count)
					{
						CheckGuaGuaLeDataList(index + 1);
					}
					else 
					{
						CheckGuaGuaLeDataList(0);
					}
					return;
				}
				else
				{
					CheckList[j].Add(GuaGuaLeDataList[index].MonsterId);
					return;
				}
			}
		}
		CheckList.Add(new List<int>());
		int lastindex = CheckList.Count-1; 
		CheckList[lastindex].Add(GuaGuaLeDataList[index].MonsterId);
	}

	int GetRandomNum(int min, int max)
	{
		System.Random rand = new System.Random(unchecked((int)DateTime.Now.Ticks));
		return rand.Next(min, max);
	}

	int GetHeadID(int stone)
	{
		for(int i=0; i<GuaGuaLeDataList.Count; i++)
		{
			if(GuaGuaLeDataList[i].Stone == stone)
				return GuaGuaLeDataList[i].MonsterId;
		}
		return 0;
	}
	
	public int GetStone(int headid)
	{
		for(int i=0; i<GuaGuaLeDataList.Count; i++)
		{
			if(GuaGuaLeDataList[i].MonsterId == headid)
				return GuaGuaLeDataList[i].Stone;
		}
		return 0;
	}
}
