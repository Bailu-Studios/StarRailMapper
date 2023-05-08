using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace StarRailMapper.Core.Models
{
    internal class HttpJson
    {
        public int retcode;
        public string message;
        public Json data;
    }
    internal class Json
    {
        public AllInfo[] list;
    }
    internal class AllInfo
    {
        public int id;
        public string name;
        public Info[] children;
    }
    internal class Info
    {
        public int id;
        public string name;
        public InfoData[] list;
    }
    internal class InfoData
    {
        public int content_id;
        public string title;
        public string icon;
    }
}
