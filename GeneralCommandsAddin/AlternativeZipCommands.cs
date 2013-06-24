using GeneralCommandsAddin.Dialogs;
using HP.LR.VuGen.ServiceCore;
using HP.LR.VuGen.ServiceCore.Data.ProjectSystem;
using HP.LR.VuGen.ServiceCore.Interfaces;
using HP.Utt.UttCore;
using HP.Utt.UttDialog;
using HP.Utt.ZipDialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GeneralCommandsAddin
{
  public class ExportToZipCommand : UttMenuCommand
  {
    public static ZipExportDialogSettings GetDefaultExportToZipSettings(IVuGenScript script)
    {
      ZipExportDialogSettings settings = new ZipExportDialogSettings();
      settings.DialogTitle = "Export to zip";
      settings.ExportItemLabel = "Script folder:";
      settings.ExportAction = ExportActionEnum.Zip;
      settings.AllowSwitchExportAction = false;
      settings.DefaultExportItemPath = script.LocalPath;
      settings.IsExportItemReadOnly = true;
      settings.CompressionLevel = Ionic.Zlib.CompressionLevel.BestSpeed;
      settings.DefaultExportToPath = Path.Combine(Path.GetDirectoryName(script.LocalPath), script.DisplayName + ".zip");
      return settings;
    }

    public override void Run()
    {
      IVuGenScript script = VuGenServiceManager.GetService<IVuGenProjectService>().GetActiveScript();
      if (script == null)
      {
        UttDialogMessageService.Instance.ShowWarning("Export to Zip operation is available for the currently loaded script only");
      }
      else
      {
        ZipExportDialogSettings settings = GetDefaultExportToZipSettings(script);
        using (VuGenZipExportDialog dialog = new VuGenZipExportDialog(script, settings))
        {
          if (dialog.ShowDialog() == CustomDialogResult.Ok)
          {
            Process.Start("explorer.exe", "/select," + dialog.ExportZipFile);
          }
        }
      }
    }
  }
}
