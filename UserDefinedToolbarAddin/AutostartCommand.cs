using HP.Utt.UttCore;
using ICSharpCode.SharpDevelop.Gui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UserDefinedToolbarAddin
{
  public class AutostartCommand : UttBaseCommand
  {
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
      ToolBar toolbar = new ToolBar();
      DockPanel.SetDock(toolbar, Dock.Top);
      mainWindowDockPanel.Children.Insert(insertionIndex, toolbar);
      

    }
  }
}
