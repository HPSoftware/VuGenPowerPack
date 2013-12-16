using HP.Utt.UttCore;
using HP.Utt.UttDialog;
using ICSharpCode.SharpDevelop.Gui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UserDefinedToolbarAddin.Controls;

namespace UserDefinedToolbarAddin
{
  public class AutostartCommand : UttBaseCommand
  {
    private static ToolBar _toolbar;
    public override void Run()
    {

      DockPanel mainWindowDockPanel = WorkbenchSingleton.MainWindow.FindName("dockPanel") as DockPanel;

      // Find the insertion index - under the toolbar
      int insertionIndex = 1;
      for (int i = 0; i < mainWindowDockPanel.Children.Count; i++)
      {
        if (mainWindowDockPanel.Children[i] is System.Windows.Controls.ToolBar)
        {
          insertionIndex = i + 1;
          break;
        }
      }
      _toolbar = new ToolBar();
      DockPanel.SetDock(_toolbar, Dock.Top);
      mainWindowDockPanel.Children.Insert(insertionIndex, _toolbar);

      CustomDialog dialog = new HP.Utt.UttDialog.CustomDialog("UserDefinedToolbarAddin.SelectorDialog");
      List<string> usedCommands = dialog.PersistenceData.GetValue<List<string>>("", ShowSelectorDialogCommand.UsedCommandsListKey, new List<string>());
      List<SearchItem> items = SearchItemBuilder.BuildSearchItems();
      CommandsSelectorViewModel viewModel = new CommandsSelectorViewModel(items, usedCommands);
      UpdateToolbar(viewModel.GetUsedSearchItems());
      dialog.Close();
    }

    public static void UpdateToolbar(List<SearchItem> items)
    {
      _toolbar.Items.Clear();
      foreach (SearchItem item in items)
      {
        Button button = new Button();
        ImageSource icon = item.Icon;
        string tooltip = item.Shortcut;
        if (item.Codon != null && item.Codon.Properties.Get("tooltip") != null)
        {
          tooltip = item.Codon.Properties.Get("tooltip").ToString() + " " + tooltip;
        }

        button.ToolTip = tooltip;

        if (icon != null)
        {
          Image image = new Image();
          image.Source = icon;
          image.Width = 17;
          image.Height = 17;
          image.Stretch = Stretch.None;
          button.Content = image;
        }
        else
        {
          Label label = new Label();    
          label.Content = item.DisplayString;
          label.Padding = new Thickness(0);
          label.VerticalContentAlignment = VerticalAlignment.Center;
          button.Content = label;
        }

        button.Command = new DelegateCommand<SearchItem>(ButtonCanExecute, ButtonExecute);
        button.CommandParameter = item;
        button.Tag = item;
        _toolbar.Items.Add(button);
      }

      if (items.Count > 0)
      {
        _toolbar.Visibility = Visibility.Visible;
      }
      else
      {
        _toolbar.Visibility = Visibility.Collapsed;
      }
    }

    static void ButtonExecute(SearchItem item)
    {
        item.Activator();
    }

    static bool ButtonCanExecute(SearchItem item)
    {
      if (item.Activator == null)
        return false;

      if (item.IsVisible == false)
        return false;

      if (item.IsEnabled == false)
        return false;

      if (item.Command != null && !item.Command.CanExecute(item.CommandParameter))
        return false;

      return true;
    }



  }
}
