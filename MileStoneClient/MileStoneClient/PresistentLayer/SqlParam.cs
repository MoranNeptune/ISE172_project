using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileStoneClient.PresistentLayer
{
    class SqlParam
    {
        private string name;
        private string data;

        public SqlParam(string name, string data)
        {
            this.name = name;
            this.data = data;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Value
        {
            get { return data; }
            set { data = value; }
        }
    }
}