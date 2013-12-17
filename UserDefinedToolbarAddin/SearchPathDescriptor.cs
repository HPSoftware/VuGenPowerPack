// --------------------------------------------------------------------
// © Copyright 2013 Hewlett-Packard Development Company, L.P.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserDefinedToolbarAddin
{
  internal class SearchPathDescriptor
  {
    public string Path
    {
      get;
      private set;
    }

    public string Category
    {
      get;
      private set;
    }

    public SearchPathDescriptor(string path, string category)
    {
      Path = path;
      Category = category;
    }
  }
}
