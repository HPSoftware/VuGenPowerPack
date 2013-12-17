// --------------------------------------------------------------------
// © Copyright 2013 Hewlett-Packard Development Company, L.P.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.Core;

namespace UserDefinedToolbarAddin
{
  public class SearchPathDoozer : IDoozer
  {
    public bool HandleConditions
    {
      get { return false; }
    }

    public object BuildItem(BuildItemArgs args)
    {
      Codon codon = args.Codon;
      return new SearchPathDescriptor(codon.Properties["path"], codon.Properties["category"]);
    }
  }
}
