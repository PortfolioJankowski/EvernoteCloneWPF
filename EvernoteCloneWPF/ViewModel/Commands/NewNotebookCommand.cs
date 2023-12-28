using EvernoteCloneWPF.ViewModel.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteCloneWPF.ViewModel.Commands
{
    public class NewNotebookCommand
    {
        public NotesVM VM { get; set; } 
        public NewNotebookCommand(NotesVM vm)
        {
            VM = vm;
        }
    }
}
