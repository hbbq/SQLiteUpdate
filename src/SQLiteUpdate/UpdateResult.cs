using System.Collections.Generic;

namespace SQLiteUpdate
{

    public class UpdateResult
    {

        public bool Successful { get; set; }
        public List<UpdateScript> ExecutedScripts { get; set; }

    }

}
