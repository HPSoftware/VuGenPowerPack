// --------------------------------------------------------------------
// © Copyright 2013 Hewlett-Packard Development Company, L.P.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using HP.LR.VuGen.Snapshots.WebSnapshotBrowserControl.ViewEditors;
using HP.LR.VuGen.XmlViewer;
using HP.Utt.Common;
using HP.Utt.UttCore;
using HP.Utt.UttDialog;

namespace HttpXmlViewAddin
{
  public class HighlightCommand : UttBaseCommand
  {
    public override void Run()
    {
      string clipboardText = Clipboard.GetText();
      if (clipboardText == null)
      {
        clipboardText = String.Empty;
      }
      InputDialog dialog = new InputDialog("Enter text", clipboardText, "Enter the value to search");
      if (dialog.ShowDialog() == CustomDialogResult.Ok)
      {
        XmlEditor editor = this.Owner as XmlEditor;
        if (editor == null)
          return;
        SingleDirectionData currentData = editor.SingleDirectionData;
        currentData.ShowAttributes = true;
        currentData.ShowValues = true;
        string tempXPath = string.Format("//*[text() = \"{0}\"]", dialog.InputString);
        if (XmlUtils.IsXPathValid(tempXPath))
        {
          XPathData xpath = new XPathData();
          xpath.XPath = tempXPath;
          currentData.HighlightedXPath = xpath;
        }
      }
    }
  }
}
