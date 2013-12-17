// --------------------------------------------------------------------
// © Copyright 2013 Hewlett-Packard Development Company, L.P.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.Core;
using System.Windows.Input;
using ICSharpCode.Core.Presentation;
using System.Threading;
using System.Collections;
using System.Windows.Controls;

namespace UserDefinedToolbarAddin
{
  public static class SearchItemBuilder
  {
    private static string SDSearchPath = "/SharpDevelop/Workbench/UserDefinedToolbar";
    public static List<SearchItem> BuildSearchItems()
    {
      List<SearchItem> result = new List<SearchItem>();
      if (!AddInTree.ExistsTreeNode(SDSearchPath))
        return result;

      List<SearchPathDescriptor> descriptors = AddInTree.BuildItems<SearchPathDescriptor>(SDSearchPath, null);
      foreach (SearchPathDescriptor descriptor in descriptors)
      {
        List<SearchItem> descriptorList = BuildSearchItems(descriptor);
        result.AddRange(descriptorList);
      }
      return result;
    }

    private static List<SearchItem> BuildSearchItems(SearchPathDescriptor descriptor)
    {
      List<SearchItem> result = new List<SearchItem>();
      AddInTreeNode node = AddInTree.GetTreeNode(descriptor.Path);
      foreach (Codon codon in node.Codons)
      {
        result.AddRange(HandleMenuCodon(descriptor, codon, descriptor.Path, new List<string>()));
      }
      return result;
    }

    private static List<SearchItem> HandleMenuCodon(SearchPathDescriptor descriptor, Codon codon, string path, List<string> pathChain)
    {

      List<SearchItem> result = new List<SearchItem>();

      if (codon.Name != "MenuItem")
        return result;

      List<string> nextPathChain = new List<string>(pathChain);
      string nextPath = string.Format("{0}/{1}", path, codon.Id);
      if (codon.Properties.Contains("type") && codon.Properties["type"] == "Menu")
      {
        nextPathChain.Add(AddNextPathChainString(codon));
        if (AddInTree.ExistsTreeNode(nextPath))
        {
          AddInTreeNode node = AddInTree.GetTreeNode(nextPath);
          foreach (Codon innerCodon in node.Codons)
          {
            result.AddRange(HandleMenuCodon(descriptor, innerCodon, nextPath, nextPathChain));
          }
        }
      }

      if (!codon.Properties.Contains("type") || (codon.Properties.Contains("type") && (codon.Properties["type"] == "Item" || codon.Properties["type"] == "Command")))
      {
        HandleItemMenuType(descriptor, codon, nextPath, nextPathChain, result);
      }

      if (codon.Properties.Contains("type") && codon.Properties["type"] == "Builder")
      {
        try
        {
          IMenuItemBuilder builder = codon.AddIn.CreateObject(codon.Properties["class"]) as IMenuItemBuilder;
          if (builder == null)
            return result;
          ICollection collection = builder.BuildItems(codon, null);
          foreach (MenuItem menuItem in collection)
          {
            SearchItem item = GenerateSearchItem(descriptor, codon, nextPath, nextPathChain);
            item.Label = menuItem.Header.ToString();
            item.Command = menuItem.Command;
            item.CommandParameter = menuItem.CommandParameter;
            item.Shortcut = menuItem.InputGestureText;

            if (String.IsNullOrWhiteSpace(item.CommandTypeString) && item.Command == null)
              continue;

            item.Id = path + "/" + item.Label;
            result.Add(item);
          }
        }
        catch (Exception ex)
        {
          LoggingService.Warn(String.Format("Could not load search item from builder at {0} - {1}", codon.Id, codon.AddIn.FileName), ex);
        }
      }
      return result;



    }

    private static void HandleItemMenuType(SearchPathDescriptor descriptor, Codon codon, string path, List<string> nextPathChain, List<SearchItem> result)
    {
      SearchItem item = GenerateSearchItem(descriptor, codon, path, nextPathChain);
      item.Label = AddNextPathChainString(codon);
      if (codon.Properties.Contains("shortcut"))
      {
        KeyGesture kg = MenuService.ParseShortcut(codon.Properties["shortcut"]);
        item.Shortcut = string.Format("({0})", kg.GetDisplayStringForCulture(Thread.CurrentThread.CurrentUICulture));
      }
      item.CommandTypeString = codon.Properties["class"];
      result.Add(item);
    }

    private static SearchItem GenerateSearchItem(SearchPathDescriptor descriptor, Codon codon, string path, List<string> nextPathChain)
    {
      SearchItem item = new SearchItem();
      item.Codon = codon;
      string commandName = codon.Properties["command"];
      if (!string.IsNullOrEmpty(commandName))
        item.Command = MenuService.GetRegisteredCommand(codon.AddIn, commandName);

      item.PathChain.AddRange(nextPathChain);
      item.Category = descriptor.Category;
      item.Id = path;
      return item;
    }

    private static string AddNextPathChainString(Codon codon)
    {
      if (codon.Properties.Contains("label"))
        return StringParser.Parse(codon.Properties["label"]).Replace("&", "");
      else
        return codon.Id;
    }
  }
}
