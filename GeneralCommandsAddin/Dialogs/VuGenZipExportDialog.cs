// --------------------------------------------------------------------
// © Copyright 2013 Hewlett-Packard Development Company, L.P.
//--------------------------------------------------------------------

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using HP.LR.VuGen.ServiceCore.Data.ProjectSystem;
using HP.LR.VuGen.ServiceCore.Interfaces;
using HP.LR.VuGen.ServiceCore;
using HP.LR.Vugen.BackEnd.Extensibility.Methods;
using HP.LR.VuGen.Extensibility.Methods.Parameters.Specific;
using HP.Utt.ZipDialogs;
using HP.Utt.Common;
using HP.Utt.ProjectSystem.Persistence;
using ICSharpCode.SharpDevelop.Gui;
using HP.LR.Vugen.Common.StringResources;

namespace GeneralCommandsAddin.Dialogs
{
  class VuGenZipExportDialog : ZipExportDialog
  {
    private IVuGenScript _script;
    private ZipScriptCustomControl _customControl;
    private ZipScriptCustomControlViewModel _customControlViewModel;

    public VuGenZipExportDialog(IVuGenScript script, ZipExportDialogSettings settings)
      : base(settings)
    {
      _script = script;
      _customControl = new ZipScriptCustomControl();
      _customControlViewModel = new ZipScriptCustomControlViewModel(_script);
      _customControl.DataContext = _customControlViewModel;
      _customControlViewModel.ExportAllFiles = true;
      SetUserControl(_customControl);
    }

    protected override string PrepareTempFolderToZip(string selectedPath, string zipFile)
    {
      //create temp folder
      string newScriptName = Path.GetFileNameWithoutExtension(zipFile);
      string tempFolder = Path.Combine(Path.GetTempPath(), Keywords.VuGen_Keywords_ProductName, newScriptName);
      if (Directory.Exists(tempFolder))
        UttFileSystemUtils.DeleteDirectory(tempFolder);
      UttFileSystemUtils.CreateDirectory(tempFolder);

      //do export
      string loadData = Path.Combine(tempFolder, newScriptName + Keywords.VuGen_Keywords_ScriptExtension);
      UttPersistenceToken newToken = new UttPersistenceToken(loadData, tempFolder);
      WorkbenchSingleton.SafeThreadCall(ExportScript, _script, newToken);

      WorkbenchSingleton.SafeThreadCall(FilterFiles, _script, tempFolder);

      return tempFolder;
    }

    private void FilterFiles(IVuGenScript script, string tempFolder)
    {
      //filter if we only want to zip runtime files
      if (_customControlViewModel.ZipFiles == ZipFilesEnum.RuntimeFiles)
        FilteForRuntimeSetting(_script, tempFolder);

      //filter protocol specified excluded folders and files
      FilteExcludedFiles(_script, tempFolder);
    }

    private void ExportScript(IVuGenScript script, UttPersistenceToken token)
    {
      VuGenServiceManager.GetService<IVuGenProjectService>().Export(script, token);
    }

    private void FilteForRuntimeSetting(IVuGenScript script, string folder)
    {
      IVuGenProjectService projectService = VuGenServiceManager.GetService<IVuGenProjectService>();
      //get runtime files
      List<string> runtimeFiles = new List<string>();
      ReadOnlyCollection<string> localFiles = projectService.GetScriptLocalFiles(folder);
      foreach (string file in localFiles)
        runtimeFiles.Add(Path.GetFileName(file));

      //delete all non-runtime files
      List<string> files = UttFileSystemUtils.GetFileList(folder, "*.*", true);
      foreach (string file in files)
      {
        string fileName = Path.GetFileName(file);
        if (!runtimeFiles.Contains(fileName))
        {
          File.SetAttributes(file, FileAttributes.Normal);
          File.Delete(file);
        }
      }

      //delete empty folders
      string[] directories = Directory.GetDirectories(folder, "*.*", SearchOption.AllDirectories);
      foreach (string directory in directories)
      {
        if (!Directory.EnumerateFileSystemEntries(directory).Any())
          UttFileSystemUtils.DeleteDirectory(directory);
      }
    }

    private void FilteExcludedFiles(IVuGenScript script, string folder)
    {
      //TODO : This is a very bad candidate for method extension, we should consider other solutions, eg adding virtual properties in BaseProtocol.
      GetExcludedFilesForZipParameters parameters = new GetExcludedFilesForZipParameters(script);
      ProtocolExtensionContext context = ProtocolExtensionContext.GetGetExcludedFilesForZipContext(parameters);
      context.OnBefore();

      if (parameters.ExcludedFolders != null && parameters.ExcludedFolders.Count > 0)
      {
        foreach (string excludeFolder in parameters.ExcludedFolders)
        {
          UttFileSystemUtils.DeleteDirectory(Path.Combine(folder, excludeFolder));
        }
      }
      if (parameters.ExcludedFiles != null && parameters.ExcludedFiles.Count > 0)
      {
        foreach (string excludeFile in parameters.ExcludedFiles)
        {
          UttFileSystemUtils.DeleteFile(excludeFile);
        }
      }
    }
  }
}
