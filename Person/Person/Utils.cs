using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Person
{
    class Utils
    {
        public static float  getFirstFloat(string messge)
        {
            string[] words = messge.Split(new String[] { " ", ":" }, StringSplitOptions.RemoveEmptyEntries);
            Debug.Assert(words.Length > 0); 

            foreach(var word in words)
            {
                bool bFind = IsFloatOrInt(word);
                if (!bFind) continue;
                float val = float.Parse(word);
                return val; 
            }
            return 0.0f; 
        }

        public static  bool IsFloatOrInt(string value)
        {
            int intValue;
            float floatValue;
            return Int32.TryParse(value, out intValue) || float.TryParse(value, out floatValue);
        }

        public static void getFloadArray(string szInfo, int W, int H, ref polygon poly)
        {
            string[] words = szInfo.Split(' ');
            poly.x_nodes = new float[Constant.MAX_POLY_NODE];
            poly.y_nodes = new float[Constant.MAX_POLY_NODE]; 
            int cnt = 0;
            int i = 0;
            int nLen = words.Length; 
            while (i < nLen)
            {
                float test;
                while (true)
                {
                    bool res = float.TryParse(words[i], out test);
                    if (!res) i++;
                    else break;
                 }
                
               float.TryParse(words[i], out poly.x_nodes[cnt]);
                poly.x_nodes[cnt] *= W; 
                i++;
                if (i >= nLen) break; 
                float.TryParse(words[i], out poly.y_nodes[cnt]);
                poly.y_nodes[cnt] *= H;
                i++;  cnt++; 
            }
            poly.nNodes = cnt; 
            
        }
    }
}
