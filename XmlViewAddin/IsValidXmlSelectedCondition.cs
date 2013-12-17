// --------------------------------------------------------------------
// © Copyright 2013 Hewlett-Packard Development Company, L.P.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.Core;

namespace XmlViewAddin
{
  public class IsValidXmlSelectedCondition:IConditionEvaluator
  {
    public bool IsValid(object owner, Condition condition)
    {
      return OpenXmlViewCommand.IsValidXml(OpenXmlViewCommand.GetSelectedString());
    }
  }
}
