using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UnityClassLibrary1;

namespace SampleWebApp.Controllers
{
    [Serializable]
    public class SampleController : ApiController
    {
        private static List<List<int>> itemNum =
            new List<List<int>>();

        int categoryNum = 2;
        int categoryOne = 9;

        SampleController()
        {
            for(int i = 0; i < categoryNum; i++)
            {
                List<int> itemNum_ = new List<int>();
                for(int k = 0; k < categoryOne; k++)
                {
                    itemNum_.Add(0);
                }

                itemNum.Add(itemNum_);
            }
        }

        public List<List<int>> Get()
        {
            return itemNum;
        }

        public void Post(List<List<int>> itemNum_)
        {
            itemNum = itemNum_;
        }
    }
}


