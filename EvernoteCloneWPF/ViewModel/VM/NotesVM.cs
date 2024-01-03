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
using System.Windows;

namespace EvernoteCloneWPF.ViewModel.VM
{
    public class NotesVM : INotifyPropertyChanged
    {
        private Notebook selectedNotebook;
        public ObservableCollection<Notebook> Notebooks { get; set; }
        public ObservableCollection<Note> Notes { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
        public NewNotebookCommand NewNotebookCommand { get; set; }
        public EditCommand EditCommand { get; set; }
        public EndEditingCommand EndEditingCommand { get; set; }

        private Visibility isVisible;
        public Visibility IsVisible
        {
            get { return isVisible; }
            set 
            { 
                isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler SelectedNoteChanged;

        public NotesVM()
        {
			NewNoteCommand = new NewNoteCommand(this);
			NewNotebookCommand = new NewNotebookCommand(this);
            EditCommand = new EditCommand(this);
            EndEditingCommand = new EndEditingCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
			Notes = new ObservableCollection<Note>();

            isVisible = Visibility.Collapsed;

            GetNotebooks();
        }

        
        private Note selectedNote;

        public Note SelectedNote
        {
            get { return selectedNote; }
            set 
            {
                selectedNote = value;
                OnPropertyChanged("SelectedNote");
                SelectedNoteChanged?.Invoke(this, new EventArgs());
            }
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

		private async void GetNotebooks()
		{
			var notebooks = (await DatabaseHelper.Read<Notebook>()).Where(n => n.UserId == App.UserId).ToList();
			Notebooks.Clear();

            foreach (var item in notebooks)
            {
				Notebooks.Add(item);
            }
        }

        private async void GetNotes()
        {
            if (SelectedNotebook != null)
            {
                var notes = (await DatabaseHelper.Read<Note>()).Where(n => n.NotebookId == SelectedNotebook.Id).ToList();
                Notes.Clear();

                foreach (var item in notes)
                {
                    Notes.Add(item);
                }
            }
            
        }

        public async Task CreateNotebookAsync()
        {
            Notebook newNotebook = new Notebook
            {
                Name = "Notebook",
                UserId = App.UserId
            };
            //asynchronicznie, bo najpierw czekam aż wstawie w bazę danych, a potem sobie mogę pobrać
            await DatabaseHelper.InsertAsync(newNotebook);
            GetNotebooks();
        }

        public async Task CreateNotesAsync(string notebookId)
        {
            Note newNote = new Note
            {
                Title = $"Note for {DateTime.Now.ToString()}",
                NotebookId = notebookId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await DatabaseHelper.InsertAsync(newNote);
            GetNotes();
        }
        public void StartEditing()
        {
            IsVisible = Visibility.Visible;
        }

        public void EndEditing(Notebook notebook)
        {
            IsVisible = Visibility.Collapsed;
            DatabaseHelper.Update(notebook);
            GetNotebooks();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
