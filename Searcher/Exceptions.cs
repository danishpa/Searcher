using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searcher
{
    public class SearcherException : Exception { }

    public class UnrecognizedFileTypeException : SearcherException { }

}
