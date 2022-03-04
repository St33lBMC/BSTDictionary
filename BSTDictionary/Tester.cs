/*
 * File name: Tester.cs
 * Author: St33l
 * Date: March 2022
 * Description: This program tests my BST implementation of a dictionary/map
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;

namespace BSTDictionary
{
    class Tester
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            for (int j = 0; j < 100000; j++)
            {
                //generate a dictionary of 100 ints
                var dict = new MyDict<int, string>();
                var testDict = new Dictionary<int, bool>();
                for (int i = 0; i < 100; i++)
                {
                    var num = rnd.Next(100);
                    dict.Add(num, "");
                    if(!testDict.ContainsKey(num))
                    {
                        testDict.Add(num, true);
                    }
                        
                }

                //remove up to 50 keys
                int removed_count = 0;
                for (int i = 0; i < 50; i++)
                {
                    var num = rnd.Next(100);
                    if (dict.KeyExists(num))
                    {
                        dict.Remove(num);
                        testDict[num] = false;
                        removed_count++;
                    }
                }

                //check against test dictionary
                foreach(KeyValuePair<int, bool> kv in testDict)
                {
                    if(kv.Value)
                    {
                        Debug.Assert(dict.KeyExists(kv.Key));
                    } else
                    {
                        Debug.Assert(!dict.KeyExists(kv.Key));
                    }
                }

            }

        }

    }
}
