using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Shared.Entities
{
    public class ValuesOfComb
    {
        public int id {get;set;}
        public int valueOfComb { get;set;}
        public string value { get;set;}

        public ValuesOfComb(int id , int valueOfComb ,string value) 
        {
            this.id = id;
            this.valueOfComb = valueOfComb;
            this.value = value;
        }
    }
}
