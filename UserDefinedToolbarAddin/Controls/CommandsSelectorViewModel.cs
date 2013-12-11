using HP.Utt.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UserDefinedToolbarAddin.Controls
{
  public class CommandsSelectorViewModel : UttDataHolder
  {

    private ObservableCollection<SearchItem> _unusedItems;
    private ObservableCollection<SearchItem> _usedItems;
    private ICollectionView _collectionView;

    public CommandsSelectorViewModel(List<SearchItem> allItems, List<string> displayedItemIds)
    {
      _unusedItems = new ObservableCollection<SearchItem>(allItems);
      _usedItems = new ObservableCollection<SearchItem>();
      _collectionView = CollectionViewSource.GetDefaultView(_unusedItems);
      _collectionView.Filter = new Predicate<object>(FilterPredicate); 

      foreach (string displayedItemId in displayedItemIds)
      {
        SearchItem foundItem = allItems.Find(x => x.Id == displayedItemId);
        if (foundItem != null)
        {
          _usedItems.Add(foundItem);
          _unusedItems.Remove(foundItem);
        }
      }

    }

    private bool FilterPredicate(Object item)
    {
      SearchItem searchItem = item as SearchItem;
      if (string.IsNullOrWhiteSpace(FilterText) || FilterText.Length < 2)
        return true;

      if (searchItem.DisplayString.Contains(FilterText))
        return true;

      return false;
    }

    private string _filterText;
    public string FilterText
    {
      get
      {
        return _filterText;
      }
      set
      {
        _filterText = value;
        _collectionView.Refresh();
        OnPropertyChanged("FilterText");
      }
    }

    public ObservableCollection<SearchItem> UnusedItems
    {
      get
      {
        return _unusedItems;
      }
    }

    public ObservableCollection<SearchItem> UsedItems
    {
      get
      {
        return _usedItems;
      }
    }


    internal List<string> GetUsedCommandsList()
    {
      List<string> result = new List<string>();
      foreach (SearchItem item in UsedItems)
      {
        result.Add(item.Id);
      }

      return result;
    }

    private DelegateCommand<SearchItem> _moveRightCommand;
    public DelegateCommand<SearchItem> MoveRightCommand
    {
      get
      {
        if (_moveRightCommand == null)
        {
          _moveRightCommand = new DelegateCommand<SearchItem>(LeftRightCanRun, MoveRightRun);
        }
        return _moveRightCommand;
      }
    }

    private void MoveRightRun(SearchItem item)
    {
      if (UnusedItems.Remove(item))
      {
        UsedItems.Insert(SelectedUsedIndex+1,item);
        SelectedUsedIndex = UsedItems.IndexOf(item);
      }
    }

    private bool LeftRightCanRun(SearchItem item)
    {
      return item != null;
    }

    private DelegateCommand<SearchItem> _moveLeftCommand;
    public DelegateCommand<SearchItem> MoveLeftCommand
    {
      get
      {
        if (_moveLeftCommand == null)
        {
          _moveLeftCommand = new DelegateCommand<SearchItem>(LeftRightCanRun, MoveLeftRun);
        }
        return _moveLeftCommand;
      }
    }

    private void MoveLeftRun(SearchItem item)
    {
      if (UsedItems.Remove(item))
      {
        UnusedItems.Add(item);
      }
    }


    private DelegateCommand<SearchItem> _moveUpCommand;
    public DelegateCommand<SearchItem> MoveUpCommand
    {
      get
      {
        if (_moveUpCommand == null)
        {
          _moveUpCommand = new DelegateCommand<SearchItem>(MoveUpCanRun, MoveUpRun);
        }
        return _moveUpCommand;
      }
    }

    private bool MoveUpCanRun(SearchItem item)
    {
      if (item == null) return false;
      if (_usedItems.IndexOf(item) == 0) return false;
      return true;
    }

    private void MoveUpRun(SearchItem item)
    {
      int index = _usedItems.IndexOf(item);
      _usedItems.RemoveAt(index);
      _usedItems.Insert(index - 1, item);
      SelectedUsedIndex = index - 1;

    }

    private DelegateCommand<SearchItem> _moveDownCommand;
    public DelegateCommand<SearchItem> MoveDownCommand
    {
      get
      {
        if (_moveDownCommand == null)
        {
          _moveDownCommand = new DelegateCommand<SearchItem>(MoveDownCanRun, MoveDownRun);
        }
        return _moveDownCommand;
      }
    }

    private bool MoveDownCanRun(SearchItem item)
    {
      if (item == null) return false;
      if (_usedItems.IndexOf(item) == (_usedItems.Count - 1)) return false;
      return true;

    }

    private void MoveDownRun(SearchItem item)
    {
      int index = _usedItems.IndexOf(item);
      _usedItems.RemoveAt(index);
      _usedItems.Insert(index + 1, item);
      SelectedUsedIndex = index + 1;
    }


    private int _selectedUnusedIndex = -1;
    public int SelectedUnusedIndex
    {
      get
      {
        return _selectedUnusedIndex;
      }
      set
      {
        _selectedUnusedIndex = value;
        OnPropertyChanged("SelectedUnusedIndex");
      }
    }

    private int _selectedUsedIndex = -1;
    public int SelectedUsedIndex
    {
      get
      {
        return _selectedUsedIndex;
      }
      set
      {
        _selectedUsedIndex = value;
        OnPropertyChanged("SelectedUsedIndex");
      }
    }


    internal List<SearchItem> GetUsedSearchItems()
    {
      List<SearchItem> result = new List<SearchItem>();
      foreach (SearchItem item in _usedItems)
      {
        result.Add(item);
      }

      return result;
    }
  }
}
