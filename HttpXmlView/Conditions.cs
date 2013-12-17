// --------------------------------------------------------------------
// © Copyright 2013 Hewlett-Packard Development Company, L.P.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using HP.LR.VuGen.Snapshots.WebSnapshotBrowserControl.ViewEditors;
using HP.LR.VuGen.XmlViewer;
using ICSharpCode.Core;

namespace HttpXmlViewAddin
{
  public class XmlEditorNodeTypeCondition : IConditionEvaluator
  {
    private const string NodeTypeKey = "nodetype";

    public bool IsValid(object owner, ICSharpCode.Core.Condition condition)
    {

      if (!condition.Properties.Contains(NodeTypeKey))
        return false;

      string nodeTypeString = condition.Properties[NodeTypeKey];
      XmlEditor editor = owner as XmlEditor;
      if (editor == null)
        return false;

      XmlViewSingleContent view = LogicalTreeHelper.FindLogicalNode(editor, "xmlView") as XmlViewSingleContent; 
      
      if (view == null || view.ActiveViewer == null)
        return false;
      XmlNode targetNode = view.ActiveViewer.GetCaretNode();
      if (targetNode == null)
        return false;
      if (nodeTypeString.Equals("LeafElement"))
      {
        if (targetNode.NodeType == XmlNodeType.Element && targetNode.ChildNodes.Count == 1 && targetNode.FirstChild.NodeType == XmlNodeType.Text)
          return true;
        return false;
      }
      else
        return targetNode.NodeType.ToString().Equals(nodeTypeString);

    }
  }
}
