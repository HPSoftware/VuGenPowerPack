using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using HP.LR.VuGen.ServiceCore.Data.ProjectSystem;
using HP.Utt.Common;
using HP.Utt.ZipDialogs;

namespace GeneralCommandsAddin.Dialogs
{
  enum ZipFilesEnum
  {
    AllFiles,
    RuntimeFiles
  }

  class ZipScriptCustomControlViewModel : UttDataHolder
  {
    private IVuGenScript _script;

    public ZipScriptCustomControlViewModel(IVuGenScript script)
    {
      _script = script;
    }

    private bool _exportRuntimeFiles;
    public bool ExportRuntimeFiles
    {
      get
      {
          return _exportRuntimeFiles;
      }
      set
      {
        _exportRuntimeFiles = value;
        OnPropertyChanged("ExportRuntimeFiles");
      }
    }

    private bool _exportAllFiles;
    public bool ExportAllFiles
    {
      get
      {
        return _exportAllFiles;
      }
      set
      {
        _exportAllFiles = value;
        OnPropertyChanged("ExportAllFiles");
      }
    }

    public ZipFilesEnum ZipFiles
    {
      get
      {
        if (ExportAllFiles)
          return ZipFilesEnum.AllFiles;
        else
          return ZipFilesEnum.RuntimeFiles;
      }
    }
  }
}
