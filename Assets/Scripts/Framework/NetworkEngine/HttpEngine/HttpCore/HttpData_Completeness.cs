using System;
using System.Collections.Generic;

//for storage , this class is element
public class HttpDataNoElement
{
    public int no;
    public int act;

    public HttpDataNoElement() { }

    public HttpDataNoElement(int No, int Act)
    {
        no = No;
        act = Act;
    }
}
// 
public class HttpData_No : DataObject
{
    public const bool AES_ENCRYPT = true;
    public HttpDataNoElement[] httpNo;

    public HttpData_No() { }

    public static void load(DataPersistManager persist, out Dictionary<int, int> HttpDataNo)
    {
        HttpDataNo = new Dictionary<int, int>();

        HttpData_No db = persist.ReadFromLocalFileSystem(DataType.HTTP_COMPLETE, AES_ENCRYPT) as HttpData_No;

        if (db != null)
        {
            foreach (HttpDataNoElement element in db.httpNo)
            {
                HttpDataNo.Add(element.act, element.no);
            }
        }
    }


    public void save(DataPersistManager persist, HttpDataNoElement[] data)
    {
        if (data != null)
        {
            httpNo = data;
            mType = DataType.HTTP_COMPLETE;
            persist.WriteToLocalFileSystem(this, AES_ENCRYPT);
        }
    }
}

// we should keep the singleton
public class HttpData_Completeness : ICore
{
    //------------ const vars -----------------
    private const int MAX_NO = 9999;
    // Becasue we need to read local file, wo must keep one DataPersistManager instance.
    private DataPersistManager persistManager;
        
    private Dictionary<int, int> HttpDataNo;

    public HttpData_Completeness(DataPersistManager persist)
    {
        persistManager = persist;
        HttpData_No.load(persistManager, out HttpDataNo);
    }

	public int getHttpRequestNo(HttpRequest request)
    {
        int No = 0;
        if (request != null)
        {
            if (HttpDataNo.TryGetValue(request.Act, out No))
            {
                //
            }
            else
            {
                No = 1;
                HttpDataNo.Add(request.Act, No);
            }
        }

        return No;
    }

	public void incHttpRequestNo(HttpRequest request)
    {
        int No = 0;
        if (request != null)
        {
            if (HttpDataNo.TryGetValue(request.Act, out No))
            {
                No = (++No) % MAX_NO;
                HttpDataNo[request.Act] = No;
            }
            else
            {
                No = 1;
                HttpDataNo.Add(request.Act, No);
            }
        }
    }

	void ICore.Dispose() {

        List<HttpDataNoElement> list = new List<HttpDataNoElement>();
        foreach (int key in HttpDataNo.Keys)
        {
            list.Add(new HttpDataNoElement(HttpDataNo[key], key));
        }
        if (list.Count > 0)
        {
            HttpData_No completness = new HttpData_No();
            completness.save(persistManager, list.ToArray());
        }
    }

	void ICore.Reset() {
		List<HttpDataNoElement> list = new List<HttpDataNoElement>();
		foreach (int key in HttpDataNo.Keys)
			list.Add(new HttpDataNoElement(HttpDataNo[key], key));

		if (list.Count > 0) {
			HttpData_No completness = new HttpData_No();
			completness.save(persistManager, list.ToArray());
		}

		if (HttpDataNo != null) {
            HttpDataNo.Clear();
            HttpDataNo = null;
        }

        persistManager = null;
    }


	void ICore.OnLogin(Object obj)
    {
        throw new NotImplementedException();
    }
}
