using EvernoteCloneWPF.Model;
using EvernoteCloneWPF.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteCloneWPF.ViewModel.VM
{
    public class NotesVM
    {
        public NotesVM()
        {
			NewNoteCommand = new NewNoteCommand(this);
			NewNotebookCommand = new NewNotebookCommand(this);
        }
        public ObservableCollection<Notebook> Notebooks { get; set; }
		private Notebook selectedNotebook;

		public Notebook SelectedNotebook
		{
			get { return selectedNotebook; }
			set 
			{ 
				selectedNotebook = value; 
				//TODO: get notes
			}
		}

		public ObservableCollection<Note> Notes { get; set; }
		public NewNoteCommand NewNoteCommand { get; set; }
		public NewNotebookCommand NewNotebookCommand { get; set; }

	}
}
