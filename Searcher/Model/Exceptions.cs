using System;

namespace Searcher.Model
{
    public class SearcherException : Exception
    {
        public SearcherException(string message)
            : base(message)
        {
        }
    }

    public class UnrecognizedFileTypeException : SearcherException
    {
        public UnrecognizedFileTypeException()
            : base("Unrecognized File Type")
        {
        }
    }

    public class NoFileSelectedException : SearcherException
    {
        public NoFileSelectedException()
            : base("No File Selected")
        {
        }
    }

}
