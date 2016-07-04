using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageEater.Lib.Models
{

    public enum FiveGramIx:int
    {
        L2=0,
        L1=1,
        C=2,
        R1=3,
        R2=4
    }

   public class FiveGram:INGram
    {
        [BsonConstructor]
        public FiveGram()
        {
            this.tokens = new List<string>();
        }

        [BsonIgnoreAttribute]
        private List<string> tokens;

        [BsonDefaultValue(0)]
        public long Count;

       [BsonDefaultValue("")]
        public string L2 { get
            {
                return this.tokens[(int)FiveGramIx.L2];
            }
        }

        [BsonDefaultValue("")]
        public string L1
        {
            get
            {
                return this.tokens[(int)FiveGramIx.L1];
            }
        }
        [BsonDefaultValue("")]
        public string C
        {
            get
            {
                return this.tokens[(int)FiveGramIx.C];
            }
        }
        [BsonDefaultValue("")]
        public string R1
        {
            get
            {
                return this.tokens[(int)FiveGramIx.R1];
            }
        }
        [BsonDefaultValue("")]
        public string R2
        {
            get
            {
                return this.tokens[(int)FiveGramIx.R2];
            }
        }
    }
}
