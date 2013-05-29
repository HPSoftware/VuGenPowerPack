using System;
using HP.Utt.UttCore;
using HP.Utt.CodeEditorUtils;
using ICSharpCode.SharpDevelop.Editor;
using System.Text;
using System.Xml;
using HP.Utt.UttDialog;
using HP.LR.VuGen.XmlViewer;
using System.Xml.Linq;

namespace XmlViewAddin
{
  public class OpenXmlViewCommand : UttBaseWpfCommand
  {
    public static bool IsValidXml(string xml)
    {
      if (string.IsNullOrWhiteSpace(xml))
      {
        return false;
      }

      XmlDocument doc = new XmlDocument();
      try
      {
        doc.LoadXml(xml);
      }
      catch
      {
        return false;
      }

      return true;
    }

    public static string GetSelectedString()
    {
      ITextEditor editor = UttCodeEditor.GetActiveTextEditor();
      if (editor == null)
        return null;
      string[] lines = editor.SelectedText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
      StringBuilder result = new StringBuilder();
      foreach (string line in lines)
      {
        string proccessedLine = line.Trim();
        //Remove the leading and trailing " - this is VuGen specific code
        if (proccessedLine.StartsWith("\""))
        {
          proccessedLine = proccessedLine.Remove(0, 1);
        }
        if (proccessedLine.EndsWith("\""))
        {
          proccessedLine = proccessedLine.Remove(proccessedLine.Length - 1, 1);
        }
        proccessedLine = proccessedLine.Replace("\\", "");

        result.Append(proccessedLine);
      }

      return result.ToString();
    }

    public override void Run()
    {
      string xml = GetSelectedString();
      if (IsValidXml(xml))
      {
        ShowXmlDialog(xml);
      }
    }

    private void ShowXmlDialog(string xml)
    {
      CustomDialog dialog = new CustomDialog();
      XmlViewSingleContent content = new XmlViewSingleContent();
      SingleDirectionData data = new SingleDirectionData();
      content.DataContext = data;
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xml);
      data.Document = doc;
      dialog.Content = content;
      dialog.MaxWidth = 800;
      dialog.AddOkButton();
      dialog.AddButton("reformat", Reformat, System.Windows.Input.Key.R, System.Windows.Input.ModifierKeys.Alt, "Reformat");
      dialog.Show();
    }

    private void Reformat(CustomDialog dialog)
    {
      ITextEditor editor = UttCodeEditor.GetActiveTextEditor();
      string selectedText = editor.SelectedText;
      string[] xmlLines = FormatXml(GetSelectedString()).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
      StringBuilder formattedText = new StringBuilder();
      foreach (string line in xmlLines)
      {
        formattedText.AppendLine("\""+line.Replace("\"", "\\\"")+"\"");
      }

      formattedText.Remove(formattedText.Length - Environment.NewLine.Length, Environment.NewLine.Length);

      if (!selectedText.StartsWith("\""))
      {
        formattedText.Insert(0, "\"" + Environment.NewLine);
      }

      if (!selectedText.EndsWith("\""))
      {
        formattedText.Append(Environment.NewLine+"\"");
      }

      editor.Document.Replace(editor.SelectionStart, editor.SelectionLength, formattedText.ToString());
      dialog.Close();
    }

    private string FormatXml(String xml)
    {
      try
      {
        XDocument doc = XDocument.Parse(xml);
        return doc.ToString();
      }
      catch (Exception)
      {
        return xml;
      }
    }
  }
}
