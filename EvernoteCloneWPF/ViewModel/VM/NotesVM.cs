using EvernoteCloneWPF.Model;
using EvernoteCloneWPF.ViewModel.Commands;
using EvernoteCloneWPF.ViewModel.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteCloneWPF.ViewModel.VM
{
    public class NotesVM : INotifyPropertyChanged
    {
        private Notebook selectedNotebook;
        public ObservableCollection<Notebook> Notebooks { get; set; }
        public ObservableCollection<Note> Notes { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
        public NewNotebookCommand NewNotebookCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public NotesVM()
        {
			NewNoteCommand = new NewNoteCommand(this);
			NewNotebookCommand = new NewNotebookCommand(this);

			
			Notebooks = new ObservableCollection<Notebook>();
			Notes = new ObservableCollection<Note>();
			GetNotebooks();
        }

        public Notebook SelectedNotebook
		{
			get { return selectedNotebook; }
			set 
			{ 
				selectedNotebook = value;
                OnPropertyChanged("SelectedNotebook");
                GetNotes();
            }
		}

		private void GetNotebooks()
		{
			var notebooks = DatabaseHelper.Read<Notebook>();
			Notebooks.Clear();

            foreach (var item in notebooks)
            {
				Notebooks.Add(item);
            }
        }

        private void GetNotes()
        {
            if (SelectedNotebook != null)
            {
                var notes = DatabaseHelper.Read<Note>().Where(n => n.NotebookId == SelectedNotebook.Id).ToList();
                Notes.Clear();

                foreach (var item in notes)
                {
                    Notes.Add(item);
                }
            }
            
        }

        public void CreateNotebook()
        {
            Notebook newNotebook = new Notebook
            {
                Name = "Notebook"
            };

            DatabaseHelper.Insert(newNotebook);
            GetNotebooks();
        }

        public void CreateNotes(int notebookId)
        {
            Note newNote = new Note
            {
                Title = $"Note for {DateTime.Now.ToString()}",
                NotebookId = notebookId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            DatabaseHelper.Insert(newNote);
            GetNotes();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
